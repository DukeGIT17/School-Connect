using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.Models
{
    public class GradeClassesVIewModel
    {
        public string ClassesToAdd { get; set; }
        public long SchoolId { get; set; }
        public IEnumerable<SubGrade>? Classes { get; set; }
    }
}
