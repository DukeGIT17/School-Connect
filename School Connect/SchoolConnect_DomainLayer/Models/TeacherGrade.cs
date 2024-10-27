using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    public class TeacherGrade
    {
        [Key]
        public long TeacherID { get; set; }
        public string StaffNr { get; set; }
        public Teacher? Teacher { get; set; }

        [Key]
        public int ClassID { get; set; }
        public string ClassDesignate { get; set; }
        public SubGrade? Class { get; set; }
    }
}
