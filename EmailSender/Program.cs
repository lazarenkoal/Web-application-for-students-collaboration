using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

class Program
{
    static void Main(string[] args)
    {
        var client = new SmtpClient("smtp.yandex.ru", 465)
        {
            Credentials = new NetworkCredential("kondrashka1313@yandex.ru", "YxZpQ10YO65mAPsh"),
            EnableSsl = true
        };
        client.Send("kondrashka1313@yandex.ru", "lazarenko.ale@gmail.com", "test", "testbody");
        Console.WriteLine("Sent");
        Console.ReadLine();
    }
}
