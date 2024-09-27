using SchoolConnect_DomainLayer.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    /// <summary>
    /// A class that represents a single Parent entity.
    /// </summary>
    public class Parent : BaseActor
    {

        /// <summary>
        /// The parent's unique national ID number.
        /// </summary>
        [Required(ErrorMessage = "Please provide identity number.")]
        [Display(Name = "ID Number")]
        [NumberLength(13, ErrorMessage = "ID Number should contain 13 digits.")]
        public long IdNo { get; set; }


        /// <summary>
        /// The type of parent, e.g., Mother, Father, or Guardian
        /// </summary>
        [Required(ErrorMessage = "Please provide learner type.")]
        [Display(Name = "Parent Type")]
        [StringLength(15, MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetical values allowed.")]
        public string ParentType { get; set; }

        /// <summary>
        /// The parent's active email address through which they can receive important communication from the school.
        /// </summary>
        [Required(ErrorMessage = "Please provide email address.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        #region Navigation Properties
        /// <summary>
        /// The navigation property that references the bridging LearnerParent class. All of this parent's children can be accessed through it.
        /// </summary>
        public ICollection<LearnerParent> Children { get; set; }

        /// <summary>
        /// The navigation property that references the bridging GroupActor class. All groups the parent is associated with can be access throug it.
        /// </summary>
        public ICollection<GroupActor>? GroupsNP { get; set; }
        #endregion
    }
}
