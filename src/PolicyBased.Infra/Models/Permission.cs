﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PolicyBased.Infra.Models
{
    public partial class Permission
    {
        public Permission()
        {
            AppPolicies = new HashSet<AppPolicy>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AppPolicy> AppPolicies { get; set; }
    }
}