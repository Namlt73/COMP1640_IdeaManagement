using COMP1640_IdeaManagement.Helpper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.Models
{
    public class IdeaUser
    {
        public int ? IdeaId { get; set; }
        public virtual Idea Idea { get; set; }

        public int ? CommentId { get; set; }
        public virtual Comment Comment { get; set; }
        public Vote Vote { get; set; }
    }
}
