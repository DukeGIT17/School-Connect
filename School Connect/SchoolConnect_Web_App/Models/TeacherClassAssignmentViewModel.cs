using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.Models
{
    public class TeacherClassAssignmentViewModel
    {
        public IEnumerable<Teacher>? Teachers { get; set; }
        public IEnumerable<SubGrade>? Classes { get; set; }
        public string TeacherEmailAddress { get; set; }
        public string ClassDesignate { get; set; }
        public bool MainClass { get; set; }
        public long SchoolId { get; set; }
    }
}
