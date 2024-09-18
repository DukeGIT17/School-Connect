using SchoolConnect_DomainLayer.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    public class Parent : BaseActor
    {
        [Required(ErrorMessage = "Please provide identity number.")]
        [Display(Name = "ID Number")]
        [NumberLength(13, ErrorMessage = "ID Number should contain 13 digits.")]
        public long IdNo { get; set; }

        [Required(ErrorMessage = "Please provide learner type.")]
        [Display(Name = "Parent Type")]
        [StringLength(15, MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetical values allowed.")]
        public string ParentType { get; set; }

        [Required(ErrorMessage = "Please provide email address.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        #region Navigation Properties
        public ICollection<LearnerParent> Children { get; set; }
        public ICollection<GroupActor>? GroupsNP { get; set; }
        #endregion
    }
}
