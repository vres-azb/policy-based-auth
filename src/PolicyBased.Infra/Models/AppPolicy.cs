﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PolicyBased.Infra.Models
{
    public partial class AppPolicy
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual Policy Policy { get; set; }
        public virtual Role Role { get; set; }
    }
}