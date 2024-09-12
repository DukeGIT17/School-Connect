using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolConnect_DomainLayer.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        public string WorkDone { get; set; }

        public DateTime Date { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(TeacherNP))]
        public long TeacherId { get; set; }

        [ForeignKey(nameof(LearnerNP))]
        public long LearnerId { get; set; }
        #endregion

        #region Navigation Properties
        public Teacher TeacherNP { get; set; }
        public Learner LearnerNP { get; set; }
        #endregion
    }
}
