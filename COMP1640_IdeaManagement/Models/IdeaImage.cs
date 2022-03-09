using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.Models
{
    public class IdeaImage
    {
        [Key]
        public int Id { get; set; }
       
        public string FileName { get; set; }

        public int IdeaId { get; set; }
        [ForeignKey("IdeaId")]
        public Idea Idea { get; set; }
    }
}
