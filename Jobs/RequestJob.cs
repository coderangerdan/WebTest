//-----------------------------------------------------------------------
// <copyright file="RequestJob.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Logging;
    using Models;

    public class RequestJob : Job
    {
        public RequestJob( ILogger logger, string server, int timeoutSeconds, RequestDetails request )
        {
            _logger = logger;
            Server = server;
            TimeoutMilliSecs = timeoutSeconds * 1000;
            Request = request;

            if( Request.Verb.ToUpper() == "GET" && Request.Data.Length > 0 )
            {
                if( !Request.Url.Contains( "?" ) )
                {
                    Request.Url += "?";
                }

                Request.Url += Request.Data;
                Request.Data = string.Empty;
            }
        }

        public RequestDetails Request { get; set; }
        private int TimeoutMilliSecs { get; set; }
        private string Server { get; set; }
        private bool Succeeded { get; set; }

        public override bool Execute()
        {
            string log = $"Executing job \"{Request.TestName}\" ... ";

            return CallWebRequest( log );
        }

        public override bool Equals( object obj )
        {
            var rhs = obj as RequestDetails;
            return rhs != null &&
                   Request.TestName == rhs.TestName &&
                   Request.Url == rhs.Url &&
                   Request.Verb == rhs.Verb &&
                   Request.Data == rhs.Data &&
                   Request.ExpectedResponse == rhs.ExpectedResponse &&
                   Request.Headers == rhs.Headers;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Request.TestName}:\t{Request.Verb}\t{Request.Url}";
        }

        private bool CallWebRequest( string log )
        {
            var webRequest = (HttpWebRequest)WebRequest.Create( $"{Server}{Request.Url}" );
            webRequest.Method = Request.Verb;
            webRequest.Timeout = TimeoutMilliSecs;

            // If required by the server, set the credentials
            //request.Credentials = CredentialCache.DefaultCredentials;
            if( Request.Headers != null )
            {
                foreach( var header in Request.Headers )
                {
                    if( header.Name.Equals( "Content-Type" ) )
                    {
                        webRequest.ContentType = header.Value;
                    }
                    else if( header.Name.Equals( "Accept" ) )
                    {
                        webRequest.Accept = header.Value;
                    }
                    else if( header.Name.Equals( "Expect" ) )
                    {
                        webRequest.Expect = header.Value;
                    }
                    else if( header.Name.Equals( "IfModifiedSince" ) )
                    {
                        webRequest.IfModifiedSince = DateTime.Parse( header.Value );
                    }
                    else if( header.Name.Equals( "MediaType" ) )
                    {
                        webRequest.MediaType = header.Value;
                    }
                    else if( header.Name.Equals( "Referer" ) )
                    {
                        webRequest.Referer = header.Value;
                    }
                    else if( header.Name.Equals( "TransferEncoding" ) )
                    {
                        webRequest.TransferEncoding = header.Value;
                    }
                    else if( header.Name.Equals( "UserAgent" ) )
                    {
                        webRequest.UserAgent = header.Value;
                    }
                    else
                    {
                        webRequest.Headers[ header.Name ] = header.Value;
                    }
                }
            }

            if( Request.Data.Length > 0 )
            {
                byte[] postData = Encoding.UTF8.GetBytes( Request.Data );
                webRequest.ContentLength = postData.Length;

                using( var requestStream = webRequest.GetRequestStream() )
                {
                    requestStream.Write( postData, 0, postData.Length );
                }
            }

            try
            {
                using( var response = (HttpWebResponse)webRequest.GetResponse() )
                {
                    return CheckResponse( (int)response.StatusCode, response, log );
                }
            }
            catch( WebException wex )
            {
                using( var r = (HttpWebResponse)wex.Response )
                {
                    return CheckResponse( (int)r.StatusCode, r, log );
                }
            }
            catch( Exception ex )
            {
                log += $"ERROR: Job failed to send the request because {ex.Message}";
                _logger.Error( log );
                return false;
            }
        }

        private bool CheckResponse( int statusCode, HttpWebResponse response, string logMessage )
        {
            if( statusCode == Request.ExpectedResponse.StatusCode )
            {
                //_logger.Info( $"Request returned the expected status code" );

                if( Request.ExpectedResponse.BodyRegEx.Length > 0 )
                {
                    // Get the stream containing content returned by the server.
                    using( var dataStream = response.GetResponseStream() )
                    using( var reader = new StreamReader( dataStream ) )
                    {
                        string responseFromServer = reader.ReadToEnd();

                        var rgx = new Regex( Request.ExpectedResponse.BodyRegEx, RegexOptions.Singleline );
                        if( !rgx.IsMatch( responseFromServer ) )
                        {
                            var responseFromServerTruncated = responseFromServer.Length > 100 ? responseFromServer.Substring( 0, 100 ) + "..." : responseFromServer;
                            responseFromServerTruncated = responseFromServerTruncated.Trim();

                            var rgxFixupForLog = new Regex( "[\r\n]", RegexOptions.Singleline );
                            responseFromServerTruncated = rgxFixupForLog.Replace( responseFromServerTruncated, "\\n" );
                            logMessage += $"ERROR: Job failed because response returned an unexpected body \"{responseFromServerTruncated}\", but expected \"{Request.ExpectedResponse.BodyRegEx}\"";
                            _logger.Error( logMessage );
                            return false;
                        }
                    }
                }
            }
            else
            {
                logMessage += $"ERROR: Job failed because response returned status code \"{(int)response.StatusCode}\", but expected \"{Request.ExpectedResponse.StatusCode}\"";
                _logger.Error( logMessage );
                return false;
            }

            logMessage += $"Success";
            _logger.Info( logMessage );
            return true;
        }


        private readonly ILogger _logger;
    }
}
