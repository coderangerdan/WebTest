//-----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest.Logging
{
    public interface ILogger
    {
        void Info( string message );
        void Info( string format, params object[] args );

        void Warning( string message );
        void Warning( string format, params object[] args );

        void Error( string message );
        void Error( string format, params object[] args );

        void Fatal( string message );
        void Fatal( string format, params object[] args );

        void Debug( string message );
        void Debug( string format, params object[] args );

        void Trace( string message );
        void Trace( string format, params object[] args );

        string CopyToTemp();
    }
}
