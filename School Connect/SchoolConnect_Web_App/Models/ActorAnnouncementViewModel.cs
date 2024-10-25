using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.Models
{
    public class ActorAnnouncementViewModel<T> where T : BaseActor
    {
        public T Actor { get; set; }
        public string? StaffNr { get; set; }
        public Announcement Announcement { get; set; }
    }
}
