using System;
using System.ComponentModel.DataAnnotations;


namespace LoginRegistration.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string LoginPassword {get;set;}
    }
}