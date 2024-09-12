namespace SchoolConnect_DomainLayer.Models
{
    public class LearnerParent
    {
        public long LearnerID { get; set; }
        public Learner Learner { get; set; }

        public long ParentID { get; set; }
        public Parent Parent { get; set; }
    }
}
