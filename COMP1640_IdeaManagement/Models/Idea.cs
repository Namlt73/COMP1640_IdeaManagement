using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.Models
{
    public class Idea
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
        public int MyProperty { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        public string Docs { get; set; }
        public int UserId { get; set; }
        public IdentityUser User { get; set; }
        public int AcademicId { get; set; }
        public Academic Academic { get; set; }
        public List<Comment> Comments { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
