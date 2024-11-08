using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.Models
{
    public class ParentModelViewModel<T> where T : class
    {
        public Parent Parent { get; set; }
        public T ActualModel { get; set; }
    }
}
