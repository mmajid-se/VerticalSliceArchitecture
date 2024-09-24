using MeesageService.Data.Interfaces;

namespace MeesageService.Shared.ViewModels
{
    public class KafkaSettings : IKafkaSettings
    {
        public const string Key = "Kafka";
        public string BootstrapServers { get; set; } = string.Empty;
        public short DefaultReplicationFactor { get; set; }
        public int DefaultNumPartitions { get; set; }
        public List<string> Topics { get; set; } = new();
    }
}
