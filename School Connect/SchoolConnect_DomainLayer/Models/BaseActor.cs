using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

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

#nullable enable
        /// <summary>
        /// The name of the profile picture stored in the School Connect system.
        /// </summary>
        [Display(Name = "Profile Picture")]
        public string? ProfileImage { get; set; }

        /// <summary>
        /// The file containing the image the user selected for their profile image.
        /// </summary>
        [NotMapped]
        [AllowedExtensions(".jpg", ".jpeg", ".png", ".webp", ".jfif")]
        public IFormFile? ProfileImageFile { get; set; }

        [NotMapped]
        public string? ProfileImageBase64 { get; set; }

        [NotMapped]
        public string? ProfileImageType { get; set; }
#nullable disable

        [Required(ErrorMessage = "Please specify the title.")]
        [AllowedValues("Miss", "Ms", "Mrs", "Mr")]
        public string Title { get; set; }

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
        [Required(ErrorMessage = "Please specify a role.")]
        [AllowedValues("System Admin", "Principal", "Teacher", "Parent", "Learner", ErrorMessage = "Please enter a valid role.")]
        public string Role { get; set; }
    }
}
