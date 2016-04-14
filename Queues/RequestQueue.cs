//-----------------------------------------------------------------------
// <copyright file="PrioritisedQueue.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest.Queues
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Logging;

    public class RequestQueue<T>
    {
        public RequestQueue( ILogger logger )
        {
            _logger = logger;
        }

        public int Count
        {
            get
            {
                lock( _queuedItems )
                {
                    return _queuedItems.Count;
                }
            }
        }

        public void Enqueue( T item )
        {
            lock( _queuedItems )
            {
                _logger.Debug( $"Enqueuing job \"{item.ToString()}\"" );
                if( _filesInQueue.Contains( item ) )
                {
                    _logger.Debug( $"Found duplicate item {item.ToString()} whilst enqueuing, ignoring" );
                    return;
                }

                _queuedItems.Enqueue( item );
                _filesInQueue.Add( item );
            }
        }

        public DequeueItem<T> Dequeue()
        {
            var dequeueItem = default( T );

            lock( _queuedItems )
            {
                if( _queuedItems.Count > 0 )
                {
                    dequeueItem = _queuedItems.Dequeue();
                }
            }

            return dequeueItem == null ? null : new DequeueItem<T> { Item = dequeueItem };
        }

        public void Detach( T item )
        {
            lock( _queuedItems )
            {
                _filesInQueue.Remove( item );
            }
        }


        private readonly ILogger _logger;
        private readonly System.Collections.Generic.Queue<T> _queuedItems = new System.Collections.Generic.Queue<T>();
        private readonly HashSet<T> _filesInQueue = new HashSet<T>();
    }

    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed." )]
    public class DequeueItem<T>
    {
        public T Item { get; set; }
    }
}