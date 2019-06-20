using System.Threading;
using System.Threading.Tasks;

namespace RabbitReplay.Shared
{
    public interface IProgramVerb
    {
        Task<int> Run(CancellationToken cancellationToken);
    }
}
