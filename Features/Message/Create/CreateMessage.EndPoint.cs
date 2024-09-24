using Carter;
using MediatR;
using MeesageService.Shared.Behaviour;
using MeesageService.Shared.Constants;
using MeesageService.Shared.Extensions;

namespace MeesageService.Features.Message.Create
{
    public class CreateMessageEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(MessageServicesNames.Create, async (CreateMessageCommand request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(request);
                if (!result.IsSuccess)
                {
                    return result.ToProblemDetails();
                }
                return Results.Ok(result.Value);
            });
        }
    }
}
