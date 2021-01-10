using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public int? ParentRoleId { get; set; }
        public int? ChildRoleId { get; set; }
    }
}
