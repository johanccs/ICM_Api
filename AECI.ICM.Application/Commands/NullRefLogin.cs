using AECI.ICM.Application.Interfaces;

namespace AECI.ICM.Api.ViewModels
{
    public class NullRefLogin : ILoginCommand
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Site { get; set; }

        public string ADUser { get; set; }

        public string DisplayName { get; set; }

        public string Role { get; set; }

        public string SystemStatus { get; set; }
    }
}
