using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.Models
{
    public class Idea
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Title must be less than 255 chracters!")]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        public string Docs { get; set; }

        public string State { get; set; }
        public List<String> StateSuggestions => new List<string> { "VIC", "NSW", "QLD", "NT", "WA", "SA", "TAS", "ACT" };
        public List<Comment> Comments { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }


        [ForeignKey("Mission")]
        public int MissionId{ get; set; }
        public Mission Mission { get; set; }  
        
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
