using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SchoolConnect_DomainLayer.CustomAttributes;

namespace SchoolConnect_DomainLayer.Models
{
    public class Learner : BaseActor
    {
        [Required(ErrorMessage = "Please provide identity number.")]
        [Display(Name = "ID Number")]
        [NumberLength(13, ErrorMessage = "ID Number should contain 13 digits.")]
        public long IdNo { get; set; }

        [Required(ErrorMessage = "Please provide learner class ID")]
        [Display(Name = "Class ID")]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "Please specify a valid class. (E.g., 8D)")]
        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Please specify a valid class. No special or lower case characters allowed.")]
        public string ClassID { get; set; }

        public IEnumerable<string> Subjects { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(LearnerSchoolNP))]
        public long SchoolID { get; set; }
        #endregion

        #region Navigation Properties
        public School LearnerSchoolNP { get; set; }
        public ICollection<LearnerParent> Parents { get; set; }
        #endregion
    }
}
