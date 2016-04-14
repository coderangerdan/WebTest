//-----------------------------------------------------------------------
// <copyright file="RequestRunDetails.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;


    public class RequestRunDetails
    {
        [DataMember( Name = "server" )]
        public string Server { get; set; }

        [DataMember( Name = "threadsToUse" )]
        public int ThreadsToUse { get; set; } = 1;

        [DataMember( Name = "startupTimeout" )]
        public int StartupTimeout { get; set; } = 120;

        [DataMember( Name = "requestTimeout" )]
        public int RequestTimeout { get; set; } = 120;

        [DataMember( Name = "requests" )]
        public List<RequestDetails> Requests { get; set; }

        [DataMember( Name = "importRequests" )]
        public List<string> ImportRequests { get; set; }
    }
}
