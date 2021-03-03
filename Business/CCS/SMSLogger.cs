using System;

namespace Business.CCS
{
    public class SMSLogger : ILogger
    {
        public void Log()
        {
            Console.WriteLine("SMS ile loglama yapildi.");
        }
    }
}
