//-----------------------------------------------------------------------
// <copyright file="Settings.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest
{
    using System;
    using System.Configuration;

    public class Settings : ISettings
    {
        public Settings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                if( appSettings.Count == 0 )
                {
                    Console.WriteLine( "ERROR: AppSettings is empty." );
                    throw new ArgumentException( "Config file did not have any settings" );
                }

                ThreadCount = GetAppSettingValueAsInt( "ThreadCount", 1 );
            }
            catch( ConfigurationErrorsException ex )
            {
                Console.WriteLine( "Error reading settings" );
                throw new ArgumentException( "Error reading settings because: " + ex.Message );
            }
        }

        public int ThreadCount { get; private set; }

        private string GetAppSettingValueAsString( string name, string defaultValue = "" )
        {
            var v = ConfigurationManager.AppSettings[ name ];
            return v == null ? defaultValue : v;
        }

        private int GetAppSettingValueAsInt( string name, int defaultValue = 0 )
        {
            var v = ConfigurationManager.AppSettings[ name ];
            return v == null ? defaultValue : Convert.ToInt32( v );
        }
    }
}
