using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SchoolConnect_DomainLayer.CustomAttributes;

namespace SchoolConnect_DomainLayer.Models
{
    /// <summary>
    /// A class that represents a single Learner entity.
    /// </summary>
    public class Learner : BaseActor
    {
        /// <summary>
        /// The learner's unique national ID number.
        /// </summary>
        [Required(ErrorMessage = "Please provide identity number.")]
        [Display(Name = "ID Number")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID Number should contain 13 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please specify a proper Emis Number. Only numerical values allowed.")]
        public string IdNo { get; set; }

        /// <summary>
        /// The learner's class ID or designation.
        /// </summary>
        [Required(ErrorMessage = "Please provide learner class ID")]
        [Display(Name = "Class Code")]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "Please specify a valid class. (E.g., 8D)")]
        [RegularExpression(@"^[0-9A-Z]+$", ErrorMessage = "Please specify a valid class. No special or lower case characters allowed.")]
        public string ClassCode { get; set; }

        #region Foreign Key Properties
        /// <summary>
        /// The foreign key referencing the school in which this learner is registered.
        /// </summary>
        [ForeignKey(nameof(LearnerSchoolNP))]
        public long SchoolID { get; set; }

        [ForeignKey(nameof(Class))]
        public int ClassID { get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// A navigation property referencing the school this learner is associated with.
        /// </summary>
        public School? LearnerSchoolNP { get; set; }

        /// <summary>
        /// A navigation property referencing the bridging LearnerParent class. All of this learner's parents can be accessed through it.
        /// </summary>
        [MaxItems(2)]
        public IList<LearnerParent> Parents { get; set; }

        public SubGrade? Class { get; set; }
        #endregion
    }
}
