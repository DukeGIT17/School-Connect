using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    public class GroupActor
    {
        [StringLength(15, MinimumLength = 6)]
        public string ActorType { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(GroupNP))]
        public int GroupId { get; set; }

        [ForeignKey(nameof(ParentNP))]
        public long ParentId { get; set; }

        [ForeignKey(nameof(TeacherNP))]
        public long TeacherId { get; set; }
        #endregion

        #region Navigation Properties
        public Group GroupNP { get; set; }
        public Parent? ParentNP { get; set; }
        public Teacher? TeacherNP { get; set; }
        #endregion
    }
}
