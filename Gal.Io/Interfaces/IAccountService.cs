using Gal.Io.Interfaces.DTOs;
using Gal.Io.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces
{
    public interface IAccountService
    {
        UserDTO Authenticate(LoginDTO login);
        UserDTO Register(RegisterDTO register);
    }
}
