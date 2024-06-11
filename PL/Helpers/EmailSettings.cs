using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Net.Mail;

namespace PL.Helpers
{
    public static class EmailSettings
    {

        public static void SendEmail(Email email)
        {
            //determine Email Server
            var client = new SmtpClient("smtp.gmail.com",587 );
            //Configuration
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("alaaali6101999@gmail.com", "brkuxtjddrkmbvad");//sender
            client.Send("alaaali6101999@gmail.com",email.To,email.Subject,email.Body);


        }
    }
}
