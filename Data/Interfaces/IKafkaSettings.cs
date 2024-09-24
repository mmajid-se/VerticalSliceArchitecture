namespace MeesageService.Data.Interfaces
{
    public interface IKafkaSettings
    {
        string BootstrapServers { get; set; }
        short DefaultReplicationFactor { get; set; }
        int DefaultNumPartitions { get; set; }
        List<string> Topics { get; set; }
    }
}
