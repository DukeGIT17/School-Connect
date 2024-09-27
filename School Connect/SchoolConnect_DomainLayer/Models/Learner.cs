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
        /// A default constructor meant to temporarily populate a newly created learner entity with dummy school details until the school in which they
        /// are meant to be registered is found.
        /// </summary>
        public Learner()
        {
            LearnerSchoolNP = new School
            {
                EmisNumber = 15985654,
                Logo = "Default Pic",
                Name = "Dummy School",
                DateRegistered = DateTime.Now,
                Type = "High",
                SystemAdminId = 1,
                SchoolAddress = new Address
                {
                    Street = "1234",
                    Suburb = "Dummy Address",
                    City = "Dummy City",
                    PostalCode = 1234,
                    Province = "Dummy Province"
                },
            };

            var parent = new Parent
            {
                Name = "Dummy Value"
            };
            
            var learner = new Learner
            {
                Name = "Dummy Value"
            };

            Parents!.Add(new LearnerParent
            {
                Parent = parent,
                Learner = learner
            });
        }

        /// <summary>
        /// The learner's unique national ID number.
        /// </summary>
        [Required(ErrorMessage = "Please provide identity number.")]
        [Display(Name = "ID Number")]
        [NumberLength(13, ErrorMessage = "ID Number should contain 13 digits.")]
        public long IdNo { get; set; }

        /// <summary>
        /// The learner's class ID or designation.
        /// </summary>
        [Required(ErrorMessage = "Please provide learner class ID")]
        [Display(Name = "Class ID")]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "Please specify a valid class. (E.g., 8D)")]
        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Please specify a valid class. No special or lower case characters allowed.")]
        public string ClassID { get; set; }

        /// <summary>
        /// A collection of all the subjects this learner is undertaking.
        /// </summary>
        public IEnumerable<string> Subjects { get; set; }

        #region Foreign Key Properties
        /// <summary>
        /// The foreign key referencing the school in which this learner is registered.
        /// </summary>
        [ForeignKey(nameof(LearnerSchoolNP))]
        public long SchoolID { get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// A navigation property referencing the school this learner is associated with.
        /// </summary>
        public School LearnerSchoolNP { get; set; }

        /// <summary>
        /// A navigation property referencing the bridging LearnerParent class. All of this learner's parents can be accessed through it.
        /// </summary>
        public ICollection<LearnerParent> Parents { get; set; }
        #endregion
    }
}
