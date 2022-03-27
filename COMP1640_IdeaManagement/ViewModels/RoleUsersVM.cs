using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.ViewModels
{
    public class RoleUsersVM
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<IdentityUser> Members { get; set; }
        public IEnumerable<IdentityUser> NonMembers { get; set; }
    }
}
