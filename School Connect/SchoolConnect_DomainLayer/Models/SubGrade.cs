using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolConnect_DomainLayer.Models
{
    public class SubGrade
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Class Designate is required")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "Please provide a proper class designate. e.g., 8A")]
        [RegularExpression(@"^[0-9A-Z]+$", ErrorMessage = "Please specify a valid class. No special or lower case characters allowed.")]
        [Display(Name = "Class")]
        public string ClassDesignate { get; set; }

        public IList<string> SubjectsTaught { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(MainTeacher))]
        public long? MainTeacherId { get; set; }

        [ForeignKey(nameof(Grade))]
        public int GradeId { get; set; }
        #endregion

        #region Navigation Properties
        public IList<Learner>? Learners { get; set; }
        public Teacher? MainTeacher { get; set; }
        public IList<TeacherGrade>? Teachers { get; set; }
        public Grade? Grade { get; set; }
        #endregion
    }
}
