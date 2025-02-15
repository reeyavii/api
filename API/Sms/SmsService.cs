﻿using Twilio.Rest.Api.V2010.Account;
using Twilio;
using Twilio.Clients;
using Microsoft.AspNetCore.Mvc;

namespace API.Sms
{
    public class SmsService : ISmsService
    {

        private static string accountSid = "AC1679dc7dd92f88f8b952e4f1cb554e15";
        private static string authToken = "49f78267e4f0f4b55256efb0025f6637";
        private static string fromNumber = "+13855264985";



        public Task SendPin(string pin, string phoneNumber)
        {
            var client = new TwilioRestClient(accountSid, authToken);
            var message = MessageResource.Create(
            body: $"Your PIN is {pin}",
            client: client,
            from: new Twilio.Types.PhoneNumber(fromNumber),
            to: new Twilio.Types.PhoneNumber(phoneNumber));
            return Task.FromResult(message.Sid);
        }
        public Task SendNotice(string phoneNumber )
        {
            var currentDate = DateTime.Now;
            var month = currentDate.ToString("MMMM");
            var client = new TwilioRestClient(accountSid, authToken);
            var message = MessageResource.Create(
            body: $"Good day, This is a reminder that your upcoming rent bill is due on 10th of {month}.Kindly pay beforehand to avoid delinquency.",
            client: client,
            from: new Twilio.Types.PhoneNumber(fromNumber),
            to: new Twilio.Types.PhoneNumber(phoneNumber));
            return Task.FromResult(message.Sid);
        }
        public Task SendDelinquent(string phoneNumber)
        {
            var currentDate = DateTime.Now;
            var month = currentDate.ToString("MMMM");
            var client = new TwilioRestClient(accountSid, authToken);
            var message = MessageResource.Create(
            body: $"Good day, Your account has received a notice of delinquency. This is a reminder that your monthly payment was due on 10th of {month} exactly 3 months today.Per record of this Office, you have incurred an amount on your leased stall.Further, Municipal Ordinance no. 2011-01, Section X Paragraph 3, clearly states that the Lessor reserves the right to summarily eject the Lessee of the stall who incurred three (3) months arrears on Stall RENTS.", 
            client: client,
            from: new Twilio.Types.PhoneNumber(fromNumber),
            to: new Twilio.Types.PhoneNumber(phoneNumber));
            return Task.FromResult(message.Sid);
        }
    }
}


