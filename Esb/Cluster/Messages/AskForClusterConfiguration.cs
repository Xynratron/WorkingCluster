using System.Dynamic;
using System.Linq;
using System.Text;

namespace Esb.Cluster.Messages
{
    [ControllerMessage, SingleProcessingMessage]
    public class BroadcastClusterConfigurationMessage
    {
    }
}
