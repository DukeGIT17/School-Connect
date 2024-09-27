namespace SchoolConnect_DomainLayer.Models
{
    public class LearnerParent
    {
        public long LearnerID { get; set; }
        public long LearnerIdNo { get; set; }
        public Learner Learner { get; set; }

        public long ParentID { get; set; }
        public long ParentIdNo { get; set; }
        public Parent Parent { get; set; }
    }
}
