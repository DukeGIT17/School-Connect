﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        
        public IList<string> GroupMemberIDs { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Group names should contain between 50 and 3 characters.")]
        public string GroupName { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(GroupSchoolNP))]
        public long SchoolID { get; set; }
        #endregion

        #region Navigation Properties
        public School? GroupSchoolNP { get; set; }
        public IList<GroupParent> GroupParentNP { get; set; }
        public IList<GroupTeacher> GroupTeacherNP { get; set; }
        #endregion
    }
}
