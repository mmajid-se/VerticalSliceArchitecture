using AutoMapper;
using MeesageService.Data.Models;
using MeesageService.Features.Message.Create;

namespace MeesageService.Shared.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateMessageCommand, Message>();
        }
    }
}
