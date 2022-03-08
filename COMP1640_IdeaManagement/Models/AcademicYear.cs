using System;

namespace COMP1640_IdeaManagement.Models
{
    public class AcademicYear
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime StartedDate { get; set; }
        public DateTime EndedDate { get; set; }
    }
}
