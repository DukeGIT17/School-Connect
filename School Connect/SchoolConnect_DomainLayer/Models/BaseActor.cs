using SchoolConnect_DomainLayer.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    /// <summary>
    /// The class containing properties shared by all actors.
    /// </summary>
    public abstract class BaseActor
    {
        /// <summary>
        /// The primary form of identification for all actors within the School Connect system.
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        public long Id { get; set; }

        /// <summary>
        /// The name of the profile picture stored in the School Connect system.
        /// </summary>
        [Display(Name = "Profile Picture")]
        [DataType(DataType.Upload)]
        public string? ProfileImage { get; set; }

        /// <summary>
        /// The name of this actor.
        /// </summary>
        [Required(ErrorMessage = "Please provide name.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name should contain between 3 and 30 characters (Inclusive).")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetical values allowed.")]
        public string Name { get; set; }

        /// <summary>
        /// The surname of this actor.
        /// </summary>
        [Required(ErrorMessage = "Please provide surname.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Surname should contain between 3 and 30 characters (Inclusive).")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetical values allowed.")]
        public string Surname { get; set; }

        /// <summary>
        /// The gender/sex of this actor.
        /// </summary>
        [Required(ErrorMessage = "Please specify gender.")]
        [Gender]
        public string Gender { get; set; }

        /// <summary>
        /// The role of this actor within the School Connect System.
        /// </summary>
        [ValidRoles("System Admin", "Principal", "Teacher", "Parent", "Learner")]
        [Required(ErrorMessage = "Please specify role.")]
        [StringLength(15, ErrorMessage = "Role should not exceed 15 characters.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only alphabetical values allowed.")]
        public string Role { get; set; }
    }
}
