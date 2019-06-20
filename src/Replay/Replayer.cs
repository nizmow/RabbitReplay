using System.Threading;
using System.Threading.Tasks;
using RabbitReplay.Shared;

namespace RabbitReplay.Replay
{
    public class Replayer : IProgramVerb
    {
        public Task<int> Run(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
