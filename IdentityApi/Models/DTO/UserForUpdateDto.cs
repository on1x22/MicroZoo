﻿using System.ComponentModel.DataAnnotations;

namespace MicroZoo.IdentityApi.Models.DTO
{
    public class UserForUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }        
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
