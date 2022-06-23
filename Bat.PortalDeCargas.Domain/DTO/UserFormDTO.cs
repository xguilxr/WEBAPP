using Bat.PortalDeCargas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class UserFormDTO
    {
        public  int UserId { get; set; }
        public string UserName { get; set; }        
        public  string UserEmail { get; set; }
        public  UserType UserType { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
