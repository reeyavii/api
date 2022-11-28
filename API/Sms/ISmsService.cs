namespace API.Sms
{
    public interface ISmsService
    {
        Task SendPin(string pin, string phoneNumber);
        Task SendNotice(string phoneNumber);
        Task SendDelinquent(string phoneNumber);
    }
}