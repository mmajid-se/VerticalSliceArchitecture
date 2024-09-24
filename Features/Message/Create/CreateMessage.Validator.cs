using FluentValidation;

namespace MeesageService.Features.Message.Create
{
    public class CreateMessageValidator : AbstractValidator<CreateMessageCommand>
    {
        public CreateMessageValidator()
        {
            RuleFor(x => x.Value).MinimumLength(10);
            RuleFor(x => x.Value).NotNull();
        }
    }
}
