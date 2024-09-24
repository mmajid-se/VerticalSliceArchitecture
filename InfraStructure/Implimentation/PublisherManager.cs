using Confluent.Kafka;
using MeesageService.Data.Interfaces;
using MeesageService.Shared.Enums;

namespace MeesageService.InfraStructure.Implimentation
{
    public class PublisherManager : IPublisherManager
    {
        private readonly IKafkaSettings _kafkaSettings;


        private readonly ProducerBuilder<string, string> _producerBuilder;
        private readonly IProducer<string, string> _producer;

        public PublisherManager(IKafkaSettings kafkaSettings)
        {
            _kafkaSettings = kafkaSettings;
            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                Acks = Acks.All
            };

            _producerBuilder = new ProducerBuilder<string, string>(producerConfig);
            _producer = _producerBuilder.Build();
        }

        public async Task ProduceMessageAsync(KafkaTopics topic, string key, string value, CancellationToken cancellationToken = default)
        {
            try
            {
                var deliveryResult = await _producer.ProduceAsync(
                    topic.ToString(),
                    new Message<string, string> { Key = key, Value = value },
                    cancellationToken
                );


            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Error occurred: {ex.Error.Reason}");
            }
        }
        public async Task ProduceMessageAsync(string topic, string key, string value, int partition, CancellationToken cancellationToken = default)
        {
            try
            {
                var Message = new Message<string, string>
                {
                    Key = key,
                    Value = value
                };
                var deliveryResult = await _producer.ProduceAsync(new TopicPartition(topic, partition), Message);
                Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'.");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Produce failed: {ex.Error.Reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
