﻿using System.ComponentModel.DataAnnotations;
using USAApi.Infrastructure;

namespace USA_REST_Api.Models
{
    public class RegisterForm
    {
        [Required]
        [Display(Name = "email", Description = "Email address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        [Display(Name = "password", Description = "Password")]
        [Secret]
        public string Password { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        [Display(Name = "firstName", Description = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        [Display(Name = "lastName", Description = "Last name")]
        public string LastName { get; set; }
    }
}
