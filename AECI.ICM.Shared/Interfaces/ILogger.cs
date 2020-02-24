using System.Threading.Tasks;

namespace AECI.ICM.Shared.Interfaces
{
    public interface ILogger
    {
        Task<bool> LogAsync(
            ILogMessageType message,
            string src = "API",
            string url = "");
    }
}
