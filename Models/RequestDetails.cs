//-----------------------------------------------------------------------
// <copyright file="RequestDetails.cs" company="CodeRanger.com">
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


    public class RequestDetails
    {
        /*
			"testName":"Testing valid request",
			"url":"/api/2/exports/queue/count/",
			"verb":"POST",
			"data": "channelId=28&platformid=17&platformExportid=104",
			"headers": [
				{"name":"Content-Type", "value":"application/json"},
				{"name":"Authorization", "value":"Basic ZGFuQGVicy50djphY2QxNTE4MWFhNDg0Njg0YWMwMzU5MDMyMTFmNGZlMWQ4ZjllNjliYmIzNmUzNGUyNmI5MTgzYmU4ZTVkYTU4"}
			],
			"expectedResponse": {
				"statusCode":"200",
				"bodyRegEx":"{ \"sendCount\": [0-9]+, \"waitCount\": [0-9]+, \"archiveCount\": [0-9]+ }"
			}
        */
        [Required]
        [DataMember( Name = "testName" )]
        public string TestName { get; set; }

        [Required]
        [DataMember( Name = "url" )]
        public string Url { get; set; }

        [Required]
        [DataMember( Name = "verb" )]
        public string Verb { get; set; } = "GET";

        [DataMember( Name = "data" )]
        public string Data { get; set; } = string.Empty;

        [DataMember( Name = "headers" )]
        public List<HeaderDetails> Headers { get; set; }

        [DataMember( Name = "repeat" )]
        public int Repeat { get; set; } = 1;

        [Required]
        [DataMember( Name = "expectedResponse" )]
        public ResponseDetails ExpectedResponse { get; set; }
    }
}
