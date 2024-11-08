using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.Models
{
    public class ParentChatViewModel
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public Parent Parent { get; set; }
    }
}
