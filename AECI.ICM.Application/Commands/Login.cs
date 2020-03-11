using AECI.ICM.Application.Interfaces;
using AECI.ICM.Shared.Interfaces;

namespace AECI.ICM.Application.Commands
{
    public static class Login
    {
        public static class V1
        {
            public class Login:ILoginCommand
            {
                public string Username { get; set; }

                public string Password { get; set; }

                public string Email { get; set; }

                public string Site { get; set; }

                public string ADUser { get; set; }

                public string DisplayName { get; set; }

                public string Role { get; set; }

                public string SystemStatus { get; set; }

                public string Region { get; set; }

                public bool IsGM { get; set; }
            }
        }
    }
}
