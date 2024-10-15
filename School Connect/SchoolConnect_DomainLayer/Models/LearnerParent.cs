using System.ComponentModel.DataAnnotations;
using SchoolConnect_DomainLayer.CustomAttributes;

namespace SchoolConnect_DomainLayer.Models
{
    public class LearnerParent
    {
        public long LearnerID { get; set; }

        [Required(ErrorMessage = "Please provide identity number.")]
        [Display(Name = "Learner ID Number")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID Number should contain 13 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please specify a proper Emis Number. Only numerical values allowed.")]
        public string LearnerIdNo { get; set; }

        public Learner? Learner { get; set; }




        public long ParentID { get; set; }

        [Required(ErrorMessage = "Please provide identity number.")]
        [Display(Name = "Parent ID Number")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID Number should contain 13 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please specify a proper Emis Number. Only numerical values allowed.")]
        public string ParentIdNo { get; set; }

        public Parent? Parent { get; set; }
    }
}
