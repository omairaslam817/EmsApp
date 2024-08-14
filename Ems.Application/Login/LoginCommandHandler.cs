using Ems.Application.Abstractions.Jwt;
using Ems.Application.Abstractions.Messaging;
using Ems.Domain.Entities;
using Ems.Domain.Errors;
using Ems.Domain.Repositories;
using Ems.Domain.Shared;
using Ems.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Application.Login
{
    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IJwtProvider _jwtProvider;

        public LoginCommandHandler(IMemberRepository memberRepository,
            IJwtProvider jwtProvider)
        {
            _memberRepository = memberRepository;
            _jwtProvider = jwtProvider;
        }
        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //Login user here and generate Jwt token here 
            //steps
            //get member
            Result<Email> email = Email.Create(request.Email);

          Member?  member =  await _memberRepository.GetByEmailAsync(email.Value, cancellationToken);

            if (member is null)
            {
                return Result.Failure<string>(DomainErrors.Member.InvalidCredentials); //return same error in case of Password mismatch
            }
            //create jwt 
            string token = _jwtProvider.Generate(member);
            //return jwt
            return token;
        }
    }
}
