using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon;
using Amazon.SimpleEmail.Model;
using System.Web;

namespace semenarna_id2 {
    public class Mailer {

        readonly string senderAddress;
        readonly string receiverAddress;
        readonly string htmlBody;
        public Mailer (string sender, string receive, string html) {
            senderAddress = sender;
            receiverAddress = receive;
            htmlBody = html;
        }

        // Replace sender@example.com with your "From" address.
        // This address must be verified with Amazon SES.

        // Replace recipient@example.com with a "To" address. If your account
        // is still in the sandbox, this address must be verified.

        // The configuration set to use for this email. If you do not want to use a
        // configuration set, comment out the following property and the
        // ConfigurationSetName = configSet argument below. 
        static readonly string configSet = "ConfigSet";

        // The subject line for the email.
        static readonly string subject = "Amazon SES test (AWS SDK for .NET)";

        // The email body for recipients with non-HTML email clients.
        static readonly string textBody = "Amazon SES Test (.NET)\r\n"
                                        + "This email was sent through Amazon SES "
                                        + "using the AWS SDK for .NET.";



        

         public void Send() {
            // Replace USWest2 with the AWS Region you're using for Amazon SES.
            // Acceptable values are EUWest1, USEast1, and USWest2.
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUCentral1)) {
                var sendRequest = new SendEmailRequest {
                    Source = senderAddress,
                    Destination = new Destination {
                        ToAddresses =
                        new List<string> { receiverAddress }
                    },
                    Message = new Message {
                        Subject = new Content(subject),
                        Body = new Body {
                            Html = new Content {
                                Charset = "UTF-8",
                                Data = htmlBody
                            },
                            Text = new Content {
                                Charset = "UTF-8",
                                Data = textBody
                            }
                        }
                    },
                };
                try {
                    Console.WriteLine("Sending email using Amazon SES...");
                    var response = client.SendEmailAsync(sendRequest).GetAwaiter().GetResult();
                    Console.WriteLine("The email was sent successfully.");
                }
                catch (Exception ex) {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);

                }
            }

            /*Console.Write("Press any key to continue...");
            Console.ReadKey();*/
        }
    }
}
