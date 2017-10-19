using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace wedding.Models
{
    public class User{

        [Key]
        public int UserId {get;set;}

        [Required]
        [MinLength(2)]
        public string FirstName {get;set;}


        [Required]
        [MinLength(2)]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(8)]
        public string Password {get;set;}

        
        public List<Rsvp> Going {get;set;}

        public User(){
            Going = new List<Rsvp>();
            
        }


    }

}