﻿using Ems.Application.Members.GetMemberById;
using Ems.Domain.Shared;
using Gatherly.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Presentation.Controllers
{
    [Route("api/auth")]

    public sealed class AuthController : ApiController
    {

        public AuthController(ISender sender)
       : base(sender)
        {
            //this.user = user;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> Hello(CancellationToken cancellationToken)
        {


            return Ok($"Hello{User.Identity.Name}!");
        }
    }
}
