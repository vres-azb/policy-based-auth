﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyBased.Infra.Models
{
    public partial class GetUserPermissionsResult
    {
        public int PolicyId { get; set; }
        public string PolicyName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int PermisionId { get; set; }
        public string PermissionName { get; set; }
        public int UserId { get; set; }
        public int AppId { get; set; }
        public string AppName { get; set; }
    }
}
