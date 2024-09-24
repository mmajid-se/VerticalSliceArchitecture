using Carter;
using MediatR;
using MeesageService.InfraStructure.Context;
using MeesageService.Shared.Behaviour;
using MeesageService.Shared.Constants;
using MeesageService.Shared.Extensions;
using MeesageService.Shared.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Producer.Api.Features.Message.Create;
using System;
using static MeesageService.Features.Message.Get.GetMessage;

namespace MeesageService.Features.Message.Get
{
    public static class GetMessage
    {
        // Query
        public class GetMessageByIdQuery : IRequest<Result<MessageDto>>
        {
            public Guid Id { get; set; }

            public GetMessageByIdQuery(Guid id)
            {
                Id = id;
            }
        }
        public class GetMessageByIdQueryHandler : IRequestHandler<GetMessageByIdQuery, Result<MessageDto>>
        {
            private readonly ApplicationDbContext _context;

            public GetMessageByIdQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Result<MessageDto>> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
            {
                var message = await _context.Message
                 .Where(m => m.Id == request.Id)
                 .Select(m => new MessageDto
                 {
                     Id = m.Id,
                     Value = m.value
                 })
                 .FirstOrDefaultAsync(cancellationToken);

                if (message == null)
                {
                    return Result.Failure(Errors.Validation("ValidationError", "Message value cannot be empty")); // You can take it from json resource file
                }

                return Result<MessageDto>.Success(message);
            }
        }

    }
    public class CreateMessageEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(MessageServicesNames.Get, async (Guid Id, ISender sender) =>
            {
                var query = new GetMessageByIdQuery(Id);
                Result<MessageDto> result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return result.ToProblemDetails();
                }
                return Results.Ok(result.Value);
            });
        }
    }
}
