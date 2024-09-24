using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesageService.Shared.Enums
{
    public enum AccountStatus
    {
        Pending,
        Approved,
        Blocked
    }

    public enum KafkaTopics
    {
        OrderTopic,
        MessageTopic,
        InventoryTopic
    }

    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3
    }
}
