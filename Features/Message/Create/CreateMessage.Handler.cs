using Confluent.Kafka;
using MediatR;
using MeesageService.Data.Interfaces;
using MeesageService.InfraStructure.Context;
using MeesageService.Shared.Behaviour;
using MeesageService.Shared.Enums;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Newtonsoft.Json;
using Producer.Api.Data.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeesageService.Features.Message.Create
{
    public class CreateMessageCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = string.Empty;
    }
    public sealed class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Result<Guid>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPublisherManager _publisherManager;

        public CreateMessageCommandHandler(ApplicationDbContext dbContext, IPublisherManager publisherManager)
        {
            _dbContext = dbContext;
            _publisherManager = publisherManager;
        }

        public async Task<Result<Guid>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Value))
            {
                return Result<Guid>.Failure(Errors.Validation("ValidationError", "Message value cannot be empty"));
            }

            var Message = new Producer.Api.Data.Models.Message
            {
                Id = Guid.NewGuid(),
                value = request.Value
            };

            try
            {
                _dbContext.Message.Add(Message);
                var changesSaved = await _dbContext.SaveChangesAsync(cancellationToken);
                if (changesSaved > 0)
                {
                    var sendingObject = JsonConvert.SerializeObject(Message);
                    await _publisherManager.ProduceMessageAsync(KafkaTopics.MessageTopic, Message.Id.ToString(), JsonConvert.SerializeObject(Message));
                }

                return Result.Success(Message.Id);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return Result<Guid>.Failure(Errors.Failure("DatabaseError", "An error occurred while saving the Message"));
            }
        }

    }
}


