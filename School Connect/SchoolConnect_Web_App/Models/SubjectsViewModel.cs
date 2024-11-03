namespace SchoolConnect_Web_App.Models
{
    public class SubjectsViewModel
    {
        public List<string> Subjects { get; set; }
        public long SchoolId { get; set; }
        public long AdminId { get; set; }
        public string? Destination { get; set; }
        public string? NewClasses { get; set; }
    }
}
