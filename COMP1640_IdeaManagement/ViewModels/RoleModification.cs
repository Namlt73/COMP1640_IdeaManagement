using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.ViewModels
{
    public class RoleModification
    {
        [Required]
        public string RoleName { get; set; }

        public string RoleId { get; set; }

        public string[] AddIds { get; set; }

        public string[] DeleteIds { get; set; }
    }
}
