using System.ComponentModel.DataAnnotations;

namespace Login.Models
{ 
    public class LoginUser{
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]        
        public string Password { get; set; }


    }
}