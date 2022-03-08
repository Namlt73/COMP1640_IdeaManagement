using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime OpenDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ClosureDate { get; set; }

        [ForeignKey("AcademicYear")]
        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }
    }
}
