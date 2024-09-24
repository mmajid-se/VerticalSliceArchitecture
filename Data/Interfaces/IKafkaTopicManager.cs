namespace MeesageService.Data.Interfaces
{
    public interface IKafkaTopicManager
    {
        Task<bool> TopicExistsAsync(string topicName);
        Task<string> CreateTopicAsync(string topicName, short? replicationFactor = null, int? numPartitions = null);

    }
}
