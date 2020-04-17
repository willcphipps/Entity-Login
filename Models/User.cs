using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginRegistration.Models {
    public class User {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage="You must have a longer name!")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2, ErrorMessage="You must have a longer name!")]
        public string LastName { get; set; }
        [EmailAddress]
        [Required]  
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
    }
}