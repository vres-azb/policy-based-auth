using System;
using System.Collections.Generic;
using System.Linq;

namespace PolicyBased.Infra.Dtos
{
    /// <summary>
    /// Models a permission
    /// </summary>
    public class Permission
    {
        public string Name { get; set; }
        public int Id { get; set; } = default!;

        public bool IsSelected { get; set; } = default!;
        
        public List<Role> Roles { get; set; } = default!;
    }
}