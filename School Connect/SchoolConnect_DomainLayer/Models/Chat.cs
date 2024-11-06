using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolConnect_DomainLayer.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Cannot send an empty message.")]
        public string Message { get; set; }

        public DateTime TimeSent { get; set; }

        public long SenderId { get; set; }

        public long ReceiverId { get; set; }


        #region Foreign Key Properties
        [ForeignKey(nameof(Teacher))]
        public long TeacherId { get; set; }

        [ForeignKey(nameof(Parent))]
        public long ParentId { get; set; }

        [ForeignKey(nameof(School))]
        public long SchoolID { get; set; }
        #endregion


        #region Navigation Properties
        public Teacher? Teacher { get; set; }

        public Parent? Parent { get; set; }

        public School? School { get; set; }
        #endregion
    }
}
