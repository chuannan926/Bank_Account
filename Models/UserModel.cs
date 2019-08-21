using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Login.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MinLength(2)]
        public string First_Name { get; set; }
        [Required]
        [MinLength(2)]        
        public string Last_Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm{get;set;}
        public double Current_balance { get; set; }

        public List<Transaction> userTransaction {get;set;}
        

        


    }
}





