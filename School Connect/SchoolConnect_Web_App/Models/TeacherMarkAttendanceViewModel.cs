using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.Models
{
    public class TeacherMarkAttendanceViewModel
    {
        public Teacher? Teacher { get; set; }
        public List<Attendance> AttendanceRecords { get; set; }
    }
}
