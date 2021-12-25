using RabbitMQ.Client.Exceptions;
using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace JobsityChat
{
    public static class Bot
    {
        public static void GetQuote(string stockeCode)
        {
            string hostname = ConfigurationManager.AppSettings["hostname"];
            string rabbitUsername = ConfigurationManager.AppSettings["username"];
            string rabbitPassword = ConfigurationManager.AppSettings["password"];
            try
            {
                string url = "https://stooq.com/q/l/?s=" + stockeCode + "&f=sd2t2ohlcv&h&e=csv";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string results = sr.ReadToEnd();
                sr.Close();

                var fields = results.Split(',');
                var price = fields[fields.Length - 2];
                var response = stockeCode.ToUpper() + " quote is $" + price + " per share";

                RabbitMQ.Publish(hostname, rabbitUsername, rabbitPassword, response);
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine("Error while reaching stocks API");
                    Console.WriteLine(reader.ReadToEnd());
                }
                RabbitMQ.Publish(hostname, rabbitUsername, rabbitPassword, "Stock price could not be requested");
            }
            catch (BrokerUnreachableException ex)
            {
                Console.WriteLine("Error while connecting to RabbitMQ server");
                Console.WriteLine(ex.Message);
                RabbitMQ.Publish(hostname, rabbitUsername, rabbitPassword, "Stock price could not be requested");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception");
                Console.WriteLine(ex.Message);
                RabbitMQ.Publish(hostname, rabbitUsername, rabbitPassword, "Stock price could not be requested");
            }
        }
    }
}