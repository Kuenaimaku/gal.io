using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class UserView
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
