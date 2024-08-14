using Ems.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Application.Login
{
    public record LoginCommand(string Email):ICommand<string>; //Or can be pass Password,return jwt here
}
