namespace API.Services.SmsService
{
    public interface ISmsService
    {
        Task SendPin(string pin, string phoneNumber);
    }
}
