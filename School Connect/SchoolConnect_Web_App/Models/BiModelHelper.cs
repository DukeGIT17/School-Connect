using SchoolConnect_DomainLayer.CustomAttributes;
using SchoolConnect_DomainLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_Web_App.Models
{
    public class BiModelHelper
    {
        public SysAdmin Admin { get; set; }

		[Required(ErrorMessage = "School Emis Number is required.")]
		[Display(Name = "EMIS Number")]
		[NumberLength(8, ErrorMessage = "School Emis Number should have exactly 8 digits.")]
		public long EmisNumber { get; set; }
	}
}
