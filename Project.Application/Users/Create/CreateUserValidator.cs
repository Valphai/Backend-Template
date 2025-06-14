﻿using FluentValidation;

namespace Project.Application.Users.Create;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.RoleIds).NotEmpty().NotNull();
        RuleFor(x => x.Email).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(3).MaximumLength(100);
    }
}