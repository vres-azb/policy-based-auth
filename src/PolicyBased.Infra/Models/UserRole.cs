﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PolicyBased.Infra.Models
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}