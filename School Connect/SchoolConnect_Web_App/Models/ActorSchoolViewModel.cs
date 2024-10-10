using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.Models
{
    public class ActorSchoolViewModel<T> where T : BaseActor
    {
        public T Actor { get; set; }

		public School? School { get; set; }
	}
}
