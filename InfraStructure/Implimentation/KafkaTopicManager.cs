using Confluent.Kafka;
using Confluent.Kafka.Admin;
using MeesageService.Data.Interfaces;

namespace MeesageService.InfraStructure.Implimentation
{
    public class KafkaTopicManager : IKafkaTopicManager
    {
        private readonly AdminClientConfig _adminClientConfig;
        private readonly IKafkaSettings _kafkaSettings;
        public KafkaTopicManager(IKafkaSettings kafkaSettings)
        {
            _kafkaSettings = kafkaSettings;
            _adminClientConfig = new AdminClientConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers
            };
        }

        public async Task<bool> TopicExistsAsync(string topicName)
        {
            using (var adminClient = new AdminClientBuilder(_adminClientConfig).Build())
            {
                try
                {
                    var metadata = await Task.Run(() => adminClient.GetMetadata(TimeSpan.FromSeconds(10)));
                    return metadata?.Topics?.Exists(t => t.Topic == topicName) ?? false;
                }
                catch (Exception ex)
                {
                    _ = ex.Message;
                    Console.WriteLine($"Save exception on ELK or Database.");
                    return false;
                }
            }
        }

        public async Task<string> CreateTopicAsync(string topicName, short? replicationFactor = null, int? numPartitions = null)
        {
            using (var adminClient = new AdminClientBuilder(_adminClientConfig).Build())
            {
                var topicSpecifications = new List<TopicSpecification>
                {
                    new TopicSpecification
                    {
                        Name = topicName,
                        ReplicationFactor = replicationFactor.HasValue ?  replicationFactor.Value : _kafkaSettings.DefaultReplicationFactor ,
                        NumPartitions = numPartitions.HasValue ? numPartitions.Value : _kafkaSettings.DefaultNumPartitions
                    }
                };
                try
                {
                    await adminClient.CreateTopicsAsync(topicSpecifications);
                    return topicName;
                }
                catch (CreateTopicsException e)
                {
                    return $"An error occurred creating topic: {e.Results.FirstOrDefault()?.Error.Reason}";
                }
                catch (Exception ex)
                {
                    // Log exception or handle accordingly
                    return $"An unexpected error occurred: {ex.Message}";
                }
            }
        }
    }
}
