using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.Models
{
    public class ActorRegistrationViewModel
    {
        public Principal? Principal { get; set; }
        public Teacher? Teacher { get; set; }
        public Parent? Parent { get; set; }
        public Learner? Learner { get; set; }

        public long SchoolID { get; set; }
        public long AdminID { get; set; }
    }
}
