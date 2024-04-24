﻿using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTOs
{
    public class LoginModelDTO
    {
        [Required(ErrorMessage = "User name is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
