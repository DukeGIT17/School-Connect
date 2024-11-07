using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.Models
{
    public class TeacherChatViewModel
    {
        public IEnumerable<Parent> Parents { get; set; }
        public Teacher Teacher { get; set; }
    }
}
