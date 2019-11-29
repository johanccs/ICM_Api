namespace AECI.ICM.Application.Interfaces
{
    public interface INotificationService
    {
        string From { get; set; }
        string To { get; set; }
        string Server { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        string CC { get; set; }
        string AttachmentPath { get; set; }

        void Send();
    }
}
