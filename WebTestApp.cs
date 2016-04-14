//-----------------------------------------------------------------------
// <copyright file="WebTestApp.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using Jobs;
    using Logging;
    using Models;
    using Newtonsoft.Json;
    using Queues;

    public class WebTestApp
    {
        public WebTestApp( ILogger logger, ISettings settings, string[] args )
        {
            _logger = logger;
            _queue = new RequestQueue<Job>( _logger );
            _customThreadPool = new CustomThreadPool( _queue, _logger );
            _customThreadPool.MaxThreads = settings.ThreadCount;

            _runFilePath = args[ 0 ];
            _server = args[ 1 ];
        }

        private string _runFilePath;
        private string _server;

        public bool Execute()
        {
            try
            {
                _logger.Info( $"Web Test starting" );

                //var lastReported = "";
                LoadRequestsFromFile( _runFilePath );
                _customThreadPool.MaxThreads = _runData.ThreadsToUse;
                _logger.Info( $"Using {_customThreadPool.MaxThreads} threads\n" );

                _logger.Info( $"Ensuring web site has started" );
                try
                {
                    var request = WebRequest.Create( _runData.Server );
                    request.Timeout = _runData.StartupTimeout * 1000;

                    using( var response = (HttpWebResponse)request.GetResponse() )
                    {
                        if( response.StatusCode != HttpStatusCode.OK )
                        {
                            _logger.Error( $"ERROR: Failed to start web site {_runData.Server} with {response.StatusCode.ToString()}: {response.StatusDescription}" );
                            return false;
                        }
                    }
                }
                catch( Exception ex )
                {
                    _logger.Error( $"ERROR: Failed to start web site {_runData.Server} because {ex.Message}" );
                    return false;
                }


                _logger.Info( $"\nStarting with {_customThreadPool.Queue.Count} items in the queue\n" );
                _customThreadPool.Process();
                while( !_customThreadPool.IsFinished )
                {
                    //var reporting = $"Queue items still to process: {_customThreadPool.Queue.Count}";
                    //if( lastReported != reporting )
                    //{
                    //    _logger.Info( reporting );
                    //    lastReported = reporting;
                    //}
                    //else
                    {
                        System.Threading.Thread.Sleep( 10 );
                    }
                }

                _logger.Info( $"\nPawa Load Test Completed: {_customThreadPool.Succeeded} Succeeded, {_customThreadPool.Failed} Failed" );
                return _customThreadPool.Failed > 0 ? false : true;
            }
            catch( Exception ex )
            {
                _logger.Fatal( $"Load Test Failed, exception thrown: {ex.Message}", ex );
                return false;
            }
        }


        private void LoadRequestsFromFile( string requestRunFilePath )
        {
            var data = File.ReadAllText( requestRunFilePath );
            if( data != null && data.Length > 0 )
            {
                try
                {
                    _runData = JsonConvert.DeserializeObject<RequestRunDetails>( data );
                }
                catch( JsonException ex )
                {
                    _logger.Error( $"Unable to deserialise JSON in file {requestRunFilePath} because {ex.Message}" );
                    throw ex;
                }

                _runData.Server = _server;

                if( _runData.ImportRequests != null )
                {
                    foreach( var filePath in _runData.ImportRequests )
                    {
                        var fileData = File.ReadAllText( filePath );
                        if( fileData.Length > 0 )
                        {
                            try
                            {
                                var fileRequests = JsonConvert.DeserializeObject<ImportRequestDetails>( fileData );
                                _runData.Requests.AddRange( fileRequests.Requests );
                            }
                            catch( JsonException ex )
                            {
                                _logger.Error( $"Unable to deserialise JSON in file {filePath} because {ex.Message}" );
                                throw ex;
                            }
                        }
                    }
                }


                var repeatedList = new List<RequestDetails>();
                foreach( var request in _runData.Requests )
                {
                    for( var i = 0; i < request.Repeat; i++ )
                    {
                        repeatedList.Add( request );
                    }
                }
                repeatedList.Shuffle();

                foreach( var request in repeatedList )
                {
                    var job = new RequestJob( _logger, _runData.Server, _runData.RequestTimeout, request );
                    _queue.Enqueue( job );
                }
            }
        }


        private RequestRunDetails _runData;
        private readonly ILogger _logger;
        private readonly RequestQueue<Job> _queue;
        private readonly CustomThreadPool _customThreadPool;
    }
}
