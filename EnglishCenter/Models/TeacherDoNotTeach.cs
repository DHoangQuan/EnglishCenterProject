using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishCenter.Models
{
    public class TeacherDoNotTeach
    {
        [StringLength(50)]
        public string PeopleID { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string Active { get; set; }

        public string Password { get; set; }
        public string RoleID { get; set; }

        public string RoleName { get; set; }
    }
}