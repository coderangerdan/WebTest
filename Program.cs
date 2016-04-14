//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest
{
    using System;
    using Logging;

    internal class Program
    {
        public enum ExitCode
        {
            Success = 0,
            Error = 1
        }

        private static int Main( string[] args )
        {
            var logger = new LoggerConsoleShim();
            var settings = new Settings();

            var retCode = ExitCode.Success;
            try
            {
                var app = new WebTestApp( logger, settings, args );
                retCode = app.Execute() ? ExitCode.Success : ExitCode.Error;
            }
            catch( Exception ex )
            {
                logger.Error( $"ERROR: Failed because {ex.Message}" );
                retCode = ExitCode.Error;
            }

            if( retCode == ExitCode.Success )
                logger.Info( "Success, all done!" );
            else
                logger.Error( "Failed, some requests were in error" );

#if DEBUG
            Console.ReadKey();
#endif

            return (int)retCode;
        }
    }
}
