using Amazon;
using System.Collections.Generic;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

using System;
using System.Threading.Tasks;
using Btf.Utilities.Configuration;
using Microsoft.Extensions.Configuration;

namespace Btf.Services.EmailService
{

    public class EmailService : IEmailService
    {
        private string _accessID;
        private string _accessKey;
        private IConfiguration _configuration;

        private string _verificationCodeMessage;

        private string _senderAddress;


        public EmailService(IConfiguration configuration)
        {

            this._configuration = configuration;
        }

        private string GetEmailVerificationTemplate(EmailConfig config)
        {
            var msg = "<html>" +
            "<head></head>" +
            "<body>" +
            "<p> Hi {0},</p>" +
            "<p>This email address is being used to signup for a VeeMed account. " +
            "If you initiated the account signup, " +
            "please verify your email address by clicking on the link and entering the verification code shown below:" +
            "<br/><br/>" +
            "<a href='" + config.EmailVerificationAddressWeb + "' > Verify your email address</a>:" +
            "<br/>" +
            "<span>Your code:<b> {1}</b></span> " +
            "<p>If you did not create the account, please ignore this email and do not give this code to anyone.</p>" +
            "<p>Thank you, <br> VeeMed Accounts Team</p>" +
            "</body>" +
            "</html>";

            return msg;

        }

        private string GetChangePasswordTemplate(string emailVerificationAddress)
        {
            var msg = "<html>" +
            "<head></head>" +
            "<body>" +
            "<p> Hi {0},</p>" +
            "<p>Please change your password by clicking on the link and entering the verification code shown below:" +
            "<br/><br/>" +
            "<a href='" + emailVerificationAddress + "/#/reset/completion' > Change Your Password</a>:" +
            "<br/>" +
            "<span>Your code:<b> {1}</b></span> " +
            "<p>If you did not request for change password, please contact our support immediately.</p>" +
            "<p>Thank you, <br> VeeMed Accounts Team</p>" +
            "</body>" +
            "</html>";

            return msg;

        }

        public async Task<Boolean> Send(string to, string subject, string[] body, string userType = "veedoc", bool isEmailVerification = true)
        {
            var awsConfig = new AmazonSimpleEmailServiceConfig();
            awsConfig.RegionEndpoint = RegionEndpoint.USEast1;

            var config = new EmailConfig();
            _configuration.Bind("EmailService", config);

            _senderAddress = config.SenderAddress;
            _accessID = config.AccessId;
            _accessKey = config.AccessKey;

            if (isEmailVerification)
                _verificationCodeMessage = GetEmailVerificationTemplate(config);
            else
            {
                if (userType.ToLower() == "veedoc")
                {
                    _verificationCodeMessage = GetChangePasswordTemplate(config.EmailVerificationAddressWeb);
                }
                else if(userType.ToLower() == "veeportal")
                {
                    _verificationCodeMessage = GetChangePasswordTemplate(config.EmailVerificationAddressAdmin);
                }
            }

            var client = new AmazonSimpleEmailServiceClient(_accessID, _accessKey, awsConfig);

            var sendRequest = new SendEmailRequest
            {
                Source = _senderAddress,
                Destination = new Destination
                {
                    ToAddresses =
                        new List<string> { to }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = String.Format(_verificationCodeMessage, body[0], body[1])
                        }
                    }
                },
            };

            try
            {
                Console.WriteLine("Sending email using Amazon SES...");
                var response = await client.SendEmailAsync(sendRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
            }
            return true;
        }

        public async Task SenduserRegistrationInfo(string to, string subject, string[] parameters)
        {
            var awsConfig = new AmazonSimpleEmailServiceConfig();
            awsConfig.RegionEndpoint = RegionEndpoint.USEast1;

            var config = new EmailConfig();
            _configuration.Bind("EmailService", config);

            _senderAddress = config.SenderAddress;
            _accessID = config.AccessId;
            _accessKey = config.AccessKey;

            var msg = "<html>" +
           "<head></head>" +
           "<body>" +
           "<p> Hi {0},</p>" +
           "<p>Your VeeMed account has been created successfully by VeeMed Admin." +
           "<br/><br/>Please set your password by clicking on the link and entering the verification code shown below:" +
           "<br/><br/>" +
           "<a href='" + config.EmailVerificationAddressWeb + "/#/reset/completion' > Set Your Password</a>:" +
           "<br/>" +
           "<span>Your code:<b> {1}</b></span> " +
           "<p>Please contact our support, if you have not requested for an account.</p>" +
           "<p>Thank you, <br> VeeMed Accounts Team</p>" +
           "</body>" +
           "</html>";

            _verificationCodeMessage = msg;

            var client = new AmazonSimpleEmailServiceClient(_accessID, _accessKey, awsConfig);

            var sendRequest = new SendEmailRequest
            {
                Source = _senderAddress,
                Destination = new Destination
                {
                    ToAddresses =
                        new List<string> { to }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = String.Format(_verificationCodeMessage, parameters[0], parameters[1])
                        }
                    }
                },
            };

            try
            {
                Console.WriteLine("Sending email using Amazon SES...");
                var response = await client.SendEmailAsync(sendRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
            }
        }

        public async Task SendRadiologyRequest(string to, string subject, string[] parameters)
        {
            var awsConfig = new AmazonSimpleEmailServiceConfig();
            awsConfig.RegionEndpoint = RegionEndpoint.USEast1;

            var config = new EmailConfig();
            _configuration.Bind("EmailService", config);

            _senderAddress = config.SenderAddress;
            _accessID = config.AccessId;
            _accessKey = config.AccessKey;

            var msg = "<html>" +
           "<head></head>" +
           "<body>" +
           "<p> Hi {0},</p>" +
           "<p>A new Radiology request has been submitted from {1} with urgency: {2}" +
           "<br/><br/>Please log in to your VeeDoc account to take action." +
           "<br/><br/>" +
           "<a href='" + config.EmailVerificationAddressWeb + "' > VeeDoc </a>:" +
           "<br/>" +
           "<p>Thank you, <br> VeeMed Team</p>" +
           "</body>" +
           "</html>";

           // _verificationCodeMessage = msg; why this statement is required?

            var client = new AmazonSimpleEmailServiceClient(_accessID, _accessKey, awsConfig);

            var sendRequest = new SendEmailRequest
            {
                Source = _senderAddress,
                Destination = new Destination
                {
                    ToAddresses =
                        new List<string> { to }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = String.Format(msg, parameters[0], parameters[1], parameters[2])
                        }
                    }
                },
            };

            try
            {
                Console.WriteLine("Sending email using Amazon SES...");
                var response = await client.SendEmailAsync(sendRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
            }
        }


        public async Task SendSupervisorRadiologyRequest(string to, string subject, string[] parameters)
        {
            var awsConfig = new AmazonSimpleEmailServiceConfig();
            awsConfig.RegionEndpoint = RegionEndpoint.USEast1;

            var config = new EmailConfig();
            _configuration.Bind("EmailService", config);

            _senderAddress = config.SenderAddress;
            _accessID = config.AccessId;
            _accessKey = config.AccessKey;

            var msg = "<html>" +
           "<head></head>" +
           "<body>" +
           "<p> Hi {0},</p>" +
           "<p>This is an alert about a radiology request that was requested from {1} at {2}.<br/>" +
           "None of the radiologists have responded so far. Your attention is required.</p>" +
           "<br/><br/>Please log in to your VeePortal account to take action." +
           "<br/><br/>" +
           "<a href='" + config.EmailVerificationAddressAdmin + "' > VeePortal </a>:" +
           "<br/>" +
           "<p>Thank you, <br> VeeMed Team</p>" +
           "</body>" +
           "</html>";

            // _verificationCodeMessage = msg; why this statement is required?

            var client = new AmazonSimpleEmailServiceClient(_accessID, _accessKey, awsConfig);

            var sendRequest = new SendEmailRequest
            {
                Source = _senderAddress,
                Destination = new Destination
                {
                    ToAddresses =
                        new List<string> { to }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = String.Format(msg, parameters[0], parameters[1], parameters[2])
                        }
                    }
                },
            };

            try
            {
                Console.WriteLine("Sending email using Amazon SES...");
                var response = await client.SendEmailAsync(sendRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
            }
        }

    }
}
