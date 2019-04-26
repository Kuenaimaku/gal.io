using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Gal.Io.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [MaxLength(16),MinLength(3),Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public string Hash { get; set; }
        public virtual List<Match> Matches { get; set; }
    }
}
