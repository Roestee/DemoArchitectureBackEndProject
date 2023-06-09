﻿using Business.Authentication.Validation.FluentValidation;
using Business.Repositories.UserRepository;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Hashing;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.Security.JWT;
using Entities.Dtos;

namespace Business.Authentication
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHandler _tokenHandler;

        public AuthManager(IUserService userService, ITokenHandler tokenHandler)
        {
            _userService = userService;
            _tokenHandler = tokenHandler;
        }

        [ValidationAspect(typeof(AuthValidator))]
        public IResult Register(RegisterAuthDto registerDto)
        {
            var result = BusinessRules.Run(CheckIfEmailExists(registerDto.Email),
                CheckIfImageExtensionsAllow(registerDto.Image.FileName),
                CheckIfImageSizeLessThanOneMb(registerDto.Image.Length));

            if (result != null) return result;

            _userService.Add(registerDto);
            return new SuccessResult("Kullanıcı kaydı başarıyla tamamlandı");
        }

        public IDataResult<Token> Login(LoginAuthDto loginDto)
        {
            var user = _userService.GetByEmail(loginDto.Email);
            var result = HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt);
            var operationClaims = _userService.GetUserOperationClaims(user.Id);
            if (result)
            {
                var token = new Token();
                token = _tokenHandler.CreateToken(user, operationClaims);
                return new SuccessDataResult<Token>(token);
            }

            return new ErrorDataResult<Token>("Kullanıcı maili ya da şifre bilgisi yanlış");
        }

        private IResult CheckIfEmailExists(string email)
        {
            var list = _userService.GetByEmail(email);
            if (list != null) return new ErrorResult("Bu mail adresi daha önce kullanılmış");

            return new SuccessResult();
        }

        private IResult CheckIfImageExtensionsAllow(string fileName)
        {
            var ext = fileName.Substring(fileName.LastIndexOf('.'));
            var extension = ext.ToLower();
            var allowFileExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".gif" };
            if (!allowFileExtensions.Contains(extension))
            {
                return new ErrorResult("Eklediğiniz resim .jpeg, .jpg, .png, .gif türlerinden biri olmalıdır!");
            }
            return new SuccessResult();
        }

        private IResult CheckIfImageSizeLessThanOneMb(long imgSize)
        {
            var imgMbSize = Convert.ToDecimal(imgSize * 0.000001);
            if (imgMbSize > 1) return new ErrorResult("Yüklediğiniz resim boyutu en fazla 1mb olmalıdır");

            return new SuccessResult();
        }
    }
}
