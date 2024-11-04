using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolConnect_DomainLayer.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(LearnerNP))]
        public long LearnerId { get; set; }

        [ForeignKey(nameof(TeacherNP))]
        public long TeacherId { get; set; }

        [ForeignKey(nameof(Class))]
        public int ClassID { get; set; }

        [ForeignKey(nameof(School))]
        public long SchoolID { get; set; }
        #endregion

        #region Navigation Properties
        public Teacher? TeacherNP { get; set; }
        public Learner? LearnerNP { get; set; }
        public SubGrade? Class { get; set; }
        public School? School { get; set; }
        #endregion
    }
}
