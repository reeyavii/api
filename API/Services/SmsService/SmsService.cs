
using Twilio.Rest.Api.V2010.Account;
using Twilio;
using Twilio.Clients;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.SmsService
{
    public class SmsService : ISmsService
    {

        private static string accountSid = "AC70f55fffbb9ab314617f6d9c139703fa";
        private static string authToken = "d6c94ba860499345b460bd78e9461711";
        

        public Task SendPin(string pin, string phoneNumber)
        {
            var client = new TwilioRestClient(accountSid, authToken);
            var message = MessageResource.Create(
            body: $"Your PIN is {pin}",
            client: client,
            from: new Twilio.Types.PhoneNumber("+19896744744"),
            to: new Twilio.Types.PhoneNumber(phoneNumber));
            return Task.FromResult(message.Sid);
        }
        
    }
}
