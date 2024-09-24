using Microsoft.AspNetCore.Mvc;

namespace MeesageService.Shared.ViewModels
{
    public class MessageDto
    {
        [FromRoute]
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}
