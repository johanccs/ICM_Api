using System.Threading.Tasks;

namespace AECI.ICM.Application.Interfaces
{
    public interface ILogger
    {
        Task<bool> LogAsync(ILogMessageType message, string url);
    }
}
