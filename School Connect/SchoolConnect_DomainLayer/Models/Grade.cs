using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolConnect_DomainLayer.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide Grade.")]
        [Display(Name = "Grade")]
        [AllowedValues("R", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12")]
        public string GradeDesignate { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(GradeSchoolNP))]
        public long SchoolID { get; set; }
        #endregion

        #region Navigation Properties
        public School? GradeSchoolNP { get; set; }
        public IList<SubGrade> Classes { get; set; }
        #endregion
    }
}
