//-----------------------------------------------------------------------
// <copyright file="CustomThreadPool.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest.Queues
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Jobs;
    using Logging;

    public class CustomThreadPool
    {
        public CustomThreadPool( RequestQueue<Job> queue, ILogger logger )
        {
            Queue = queue;
            MaxThreads = 4;

            _logger = logger;
        }

        public int MaxThreads { get; set; }
        public RequestQueue<Job> Queue { get; private set; }

        public int Succeeded { get; set; } = 0;
        public int Failed { get; set; } = 0;

        public bool IsFinished
        {
            get { return _threads.Count == 0; }
        }

        public void Process()
        {
            // start with one thread, it will create more if needed
            this.StartThread();
        }

        private void ThreadProc( object param )
        {
            OnThreadStarted();
            var thread = (Thread)param;

            DequeueItem<Job> jobDetails = null;
            try
            {
                while( true )
                {
                    jobDetails = Queue.Dequeue();

                    //  Only exit the last thread if there are no job details.
                    if( jobDetails == null )
                    {
                        // nothing to do so we might as well exit
                        break;
                    }

//                    if( Queue.Count > 0 )
                    {
                        // should we spawn more threads...
                        // start with one thread and create more if needed
                        lock ( _threads )
                        {
                            if( _threads.Count < MaxThreads )
                            {
                                this.StartThread();
                            }
                        }
                    }

                    try
                    {
                        _logger.Debug( "Executing item on thread {0}", thread.ManagedThreadId );
                        if( jobDetails.Item.Execute() )
                        {
                            Succeeded++;
                        }
                        else
                        {
                            // Job wasn't ready to run so we put back on queue...
                            Failed++;
                            Queue.Detach( jobDetails.Item );
                            //                            Queue.Enqueue( jobDetails.Item );
                        }
                    }
                    catch( Exception ex )
                    {
                        _logger.Error( "Exception occurred for job {0}, with message {1}", jobDetails.Item, ex.Message ); // add to retry list
                        throw ex;
                    }
                }
            }
            catch( Exception ex )
            {
                _logger.Fatal( "Exception occurred in {0}, with message {1}", jobDetails == null ? "thread " + thread.ManagedThreadId.ToString() : jobDetails.Item.ToString(), ex.Message );
            }
            finally
            {
                lock ( _threads )
                {
                    if( _threads.Contains( thread ) )
                    {
                        _logger.Debug( "Thread {0} shutting down", thread.ManagedThreadId );
                        _threads.Remove( thread );
                    }
                }
                OnThreadEnding();
            }
        }


        private void StartThread()
        {
            lock ( _threads )
            {
                var thread = new Thread( this.ThreadProc );
                _threads.Add( thread );
                _logger.Debug( "Created New Thread {0}, starting", thread.ManagedThreadId );

                thread.Start( thread );
            }
        }

        public virtual void OnThreadStarted()
        {
            if( ThreadStarted != null )
            {
                ThreadStarted( this, EventArgs.Empty );
            }
        }

        public virtual void OnThreadEnding()
        {
            if( ThreadEnding != null )
            {
                ThreadEnding( this, EventArgs.Empty );
            }
        }

        public event EventHandler ThreadStarted;
        public event EventHandler ThreadEnding;


        private readonly List<Thread> _threads = new List<Thread>();
        private readonly ILogger _logger;
    }
}