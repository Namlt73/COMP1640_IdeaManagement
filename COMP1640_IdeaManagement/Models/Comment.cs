﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
        public int UserId { get; set; }
        public IdentityUser User { get; set; }
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
    }
}