using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    public class GroupParent
    {
        [ForeignKey(nameof(GroupNP))]
        public int GroupId { get; set; }

        [ForeignKey(nameof(ParentNP))]
        public long? ParentId { get; set; }

        public Group GroupNP { get; set; }
        public Parent? ParentNP { get; set; }
    }
}
