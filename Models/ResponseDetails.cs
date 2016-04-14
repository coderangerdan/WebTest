//-----------------------------------------------------------------------
// <copyright file="ResponseDetails.cs" company="CodeRanger.com">
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


    public class ResponseDetails
    {
        [DataMember( Name = "statusCode" )]
        public int StatusCode { get; set; } = 200;

        [DataMember( Name = "bodyRegEx" )]
        public string BodyRegEx { get; set; } = string.Empty;
    }
}
