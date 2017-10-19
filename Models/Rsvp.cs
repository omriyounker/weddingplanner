using System.ComponentModel.DataAnnotations;
using System;

using System.Collections.Generic;
namespace wedding.Models
{
    public class Rsvp{

        [Key]
        public int RSVPId {get;set;}

        [Required]
        public int UserId {get;set;}
        public User User {get;set;}

        [Required]
        public int WeddingId {get;set;}
        public Wedding Wedding {get;set;}
    }
}