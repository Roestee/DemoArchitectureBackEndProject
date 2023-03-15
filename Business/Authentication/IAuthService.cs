using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Result.Abstract;
using Entities.Dtos;

namespace Business.Authentication
{
    public interface IAuthService
    {
        IResult Register(RegisterAuthDto registerDto);
        string Login(LoginAuthDto loginDto);
    }
}
