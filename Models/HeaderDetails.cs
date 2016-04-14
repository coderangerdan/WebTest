//-----------------------------------------------------------------------
// <copyright file="HeaderDetails.cs" company="CodeRanger.com">
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


    public class HeaderDetails
    {
        [Required]
        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [Required]
        [DataMember( Name = "value" )]
        public string Value { get; set; }
    }
}
