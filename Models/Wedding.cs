using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace wedding.Models
{
    public class Wedding{

        [Key]
        public int WeddingId {get;set;}

        [Required]
        [MinLength(2)]
        public string Wedder1 {get;set;}

        [Required]
        [MinLength(2)]
        public string Wedder2 {get;set;}

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date {get;set;}

        [Required]
        public int Creator {get;set;}
       

        [Required]
        public string Address {get;set;}

        public List<Rsvp> Coming {get;set;}
        
        public Wedding(){  
            Coming = new List<Rsvp>();         
        }


    }

}