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
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }

        
        public List<IdeaImage> Images { get; set; }

       
        public List<Comment> Comments { get; set; }

        public List<Like> Likes { get; set; }
        public List<Dislike> Dislikes { get; set; }


        [ForeignKey("User")]
        [Display(Name = "User Id")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [ForeignKey("Mission")]
        [Display(Name = "Mission Id")]
        public int MissionId{ get; set; }
        public Mission Mission { get; set; }       

        [ForeignKey("Category")]
        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
