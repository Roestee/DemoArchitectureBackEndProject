﻿using FluentValidation;
using Entities.Concrete;

namespace Business.Repositories.UserRepository.Validation.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Kullanıcı adı boş olamaz");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Mail adresi boş olamaz");
            RuleFor(p => p.Email).EmailAddress().WithMessage("Geçerli bir mail adresi yazın");
            RuleFor(p => p.ImageUrl).NotEmpty().WithMessage("Kullanıcı resmi boş olamaz");
        }
    }
}
