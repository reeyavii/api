namespace API.Sms
{
    public interface ISmsService
    {
        Task SendPin(string pin, string phoneNumber);
    }
}