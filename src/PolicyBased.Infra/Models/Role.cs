﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PolicyBased.Infra.Models
{
    public partial class Role
    {
        public Role()
        {
            AppPolicies = new HashSet<AppPolicy>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public int PolicyId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Policy Policy { get; set; }
        public virtual ICollection<AppPolicy> AppPolicies { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}