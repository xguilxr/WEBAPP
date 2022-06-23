using Bat.PortalDeCargas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Bat.PortalDeCargas.Domain.Entities
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string UserEmail { get; set; }
        public UserType UserType { get; set; }         
        public string Password { get; set; }
    }
}
