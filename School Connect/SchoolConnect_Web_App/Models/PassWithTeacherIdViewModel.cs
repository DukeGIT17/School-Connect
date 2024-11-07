namespace SchoolConnect_Web_App.Models
{
    public class PassWithTeacherIdViewModel<T> where T : class
    {
        public long TeacherId { get; set; }
        public T ActualModel { get; set; }
    }
}
