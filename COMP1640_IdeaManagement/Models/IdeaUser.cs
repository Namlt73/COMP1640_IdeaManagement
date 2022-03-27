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
<<<<<<< HEAD
        
=======
       
>>>>>>> 3d50767e27d789d425dd51f43fff3d4a5399f4c5
    }
}
