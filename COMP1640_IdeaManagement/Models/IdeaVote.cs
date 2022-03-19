using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.Models
{
    public class IdeaVote
    {
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int IdeaId { get; set; }
        public Idea Idea { get; set; }

        public bool? IsLike { get; set; }

        public bool? IsDislike { get; set; }
    }
}
