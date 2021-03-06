﻿//-----------------------------------------------------------------------
// <copyright file="ImportRequestDetails.cs" company="CodeRanger.com">
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


    public class ImportRequestDetails
    {
        [Required]
        [DataMember( Name = "requests" )]
        public List<RequestDetails> Requests { get; set; }
    }
}
