using System.ComponentModel.DataAnnotations;
using SchoolConnect_DomainLayer.CustomAttributes;

namespace SchoolConnect_DomainLayer.Models
{
    public class LearnerParent
    {
        public long LearnerID { get; set; }

        [Required(ErrorMessage = "Please provide identity number.")]
        [Display(Name = "Learner ID Number")]
        [NumberLength(13, ErrorMessage = "ID Number should contain 13 digits.")]
        public long LearnerIdNo { get; set; }

        public Learner? Learner { get; set; }




        public long ParentID { get; set; }

        [Required(ErrorMessage = "Please provide identity number.")]
        [Display(Name = "Parent ID Number")]
        [NumberLength(13, ErrorMessage = "ID Number should contain 13 digits.")]
        public long ParentIdNo { get; set; }

        public Parent? Parent { get; set; }
    }
}
