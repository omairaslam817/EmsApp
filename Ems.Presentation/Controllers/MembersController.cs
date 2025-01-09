using Ems.Application.Members.CreateMember;
using Ems.Application.Members.UpdateMember;
using Ems.Application.Members.GetMemberById;
using Ems.Domain.Shared;
using Gatherly.Presentation.Abstractions;
using Ems.Presentation.Contracts.Members;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Ems.Application.Login;
using System.Net;
using System.Text.Json;
using Ems.Infrastructure.Authentication;
using Ems.Domain.Enums;

namespace Gatherly.Presentation.Controllers;

[Route("api/members")]
public sealed class MembersController : ApiController
{
    private readonly IDistributedCache _cache;

    public MembersController(ISender sender, IDistributedCache cache)
        : base(sender)
    {
        _cache = cache;
    }

    //[HasPermission(Permission.ReadMember)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMemberById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            const string cachePrefix = "Member_";
            string cacheKey = $"{cachePrefix}{id}";

            // Try to retrieve the data from the cache
            var cachedMember = await _cache.GetStringAsync(cacheKey,cancellationToken);
            if (!string.IsNullOrEmpty(cachedMember))
            {
                // Deserialize and return cached data
                var cachedResponse = JsonSerializer.Deserialize<MemberResponse>(cachedMember);
                return Ok(cachedResponse);
            }

            // If not cached, retrieve data from the query handler
            var query = new GetMemberByIdQuery(id);
            Result<MemberResponse> response = await Sender.Send(query, cancellationToken);

            if (!response.IsSuccess)
            {
                return NotFound(response.Error);
            }

            // Cache the response with an expiration time
            var cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Adjust cache duration as needed
            };
            var serializedResponse = JsonSerializer.Serialize(response.Value);
            await _cache.SetStringAsync(cacheKey, serializedResponse, cacheEntryOptions);

            return Ok(response.Value);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginMember(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email);

        Result<string> tokenResult = await Sender.Send(
            command,
            cancellationToken);

        if (tokenResult.IsFailure)
        {
            return HandleFailure(tokenResult);
        }

        return Ok(tokenResult.Value);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterMember(
        [FromBody] RegisterMemberRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateMemberCommand(
            request.Email,
            request.FirstName,
            request.LastName);

        Result<Guid> result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
            nameof(GetMemberById),
            new { id = result.Value },
            result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMember(
        Guid id,
        [FromBody] UpdateMemberRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMemberCommand(
            id,
            request.FirstName,
            request.LastName);

        Result result = await Sender.Send(
            command,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }
}
