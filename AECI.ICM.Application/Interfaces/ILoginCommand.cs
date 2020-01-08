namespace AECI.ICM.Application.Interfaces
{
    public interface ILoginCommand
    {
        string Username { get; set; }

        string Password { get; set; }

        string Email { get; set; }

        string Site { get; set; }

        string ADUser { get; set; }

        string DisplayName { get; set; }

        string Role { get; set; }

        string SystemStatus { get; set; }
    }
}
