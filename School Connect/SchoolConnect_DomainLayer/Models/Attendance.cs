using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolConnect_DomainLayer.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        public bool MondayDate { get; set; }

        public bool TuesdayDate { get; set; }

        public bool WednesdayDate { get; set; }

        public bool ThursdayDate { get; set; }

        public bool FridayDate { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(LearnerNP))]
        public long LearnerId { get; set; }

        [ForeignKey(nameof(TeacherNP))]
        public long TeacherId { get; set; }
        #endregion

        #region Navigation Properties
        public Teacher TeacherNP { get; set; }
        public Learner LearnerNP { get; set; }
        #endregion
    }
}
