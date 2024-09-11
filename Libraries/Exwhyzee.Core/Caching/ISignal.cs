using Microsoft.Extensions.Primitives;

namespace Exwhyzee.Caching
{
    public interface ISignal
    {
        IChangeToken GetToken(string key);

        void SignalToken(string key);
    }
}
