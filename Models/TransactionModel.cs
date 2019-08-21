using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Login.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [Required]
        [Display(Name="Deposit/Withdraw:")]
        [DataType(DataType.Currency)]
        public decimal Amount{get;set;}

        public int UserId{get;set;}

        public User AccountOwner{get;set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;

    }



}