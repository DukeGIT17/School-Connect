namespace SchoolConnect_DomainLayer.Models
{
    public class GroupTeacher
    {
        public int GroupId { get; set; }
        public Group GroupNP { get; set; }

        public long TeacherId { get; set; }
        public Teacher TeacherNP { get; set; }

    }
}
