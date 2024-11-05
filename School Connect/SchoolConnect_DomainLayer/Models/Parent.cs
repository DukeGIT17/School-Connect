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
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID Number should contain 13 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please specify a proper Emis Number. Only numerical values allowed.")]
        public string IdNo { get; set; }


        /// <summary>
        /// The type of parent, e.g., Mother, Father, or Guardian
        /// </summary>
        [Required(ErrorMessage = "Please provide learner type.")]
        [Display(Name = "Parent Type")]
        [AllowedValues("Mother", "Father", "Guardian", ErrorMessage = "Please enter a valid parent type. (e.g., Mother, Father, Guardian)")]
        public string ParentType { get; set; }

        /// <summary>
        /// The parent's active email address through which they can receive important communication from the school.
        /// </summary>
        [Required(ErrorMessage = "Please provide email address.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please provide phone number.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public long PhoneNumber { get; set; }

        #region Navigation Properties
        /// <summary>
        /// The navigation property that references the bridging LearnerParent class. All of this parent's children can be accessed through it.
        /// </summary>
        public IList<LearnerParent>? Children { get; set; }

        /// <summary>
        /// The navigation property that references the bridging GroupActor class. All groups the parent is associated with can be access through it.
        /// </summary>
        public IList<GroupParent>? GroupsNP { get; set; }

        public IList<Chat>? Chats { get; set; }
        #endregion
    }
}
