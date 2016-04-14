//-----------------------------------------------------------------------
// <copyright file="LoggerConsoleShim.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest.Logging
{
    using System;

    public class LoggerConsoleShim : ILogger
    {
        public void Info( string message )
        {
            Console.Out.WriteLine( message );
        }

        public void Info( string format, params object[] args )
        {
            Console.Out.WriteLine( format, args );
        }

        public void Warning( string message )
        {
            Console.Error.WriteLine( message );
        }

        public void Warning( string format, params object[] args )
        {
            Console.Error.WriteLine( format, args );
        }

        public void Error( string message )
        {
            Console.Error.WriteLine( message );
        }

        public void Error( string format, params object[] args )
        {
            Console.Error.WriteLine( format, args );
        }

        public void Fatal( string message )
        {
            Console.Error.WriteLine( message );
        }

        public void Fatal( string format, params object[] args )
        {
            Console.Error.WriteLine( format, args );
        }

        public void Debug( string message )
        {
            Trace( message );
        }

        public void Debug( string format, params object[] args )
        {
            Trace( format, args );
        }

        public void Trace( string message )
        {
            System.Diagnostics.Debug.WriteLine( message );
        }

        public void Trace( string format, params object[] args )
        {
            System.Diagnostics.Debug.WriteLine( format, args );
        }

        public string CopyToTemp()
        {
            return null;
        }
    }
}
