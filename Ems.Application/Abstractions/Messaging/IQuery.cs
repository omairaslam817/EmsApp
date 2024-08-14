using Ems.Domain.Shared;
using MediatR;

namespace Ems.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}