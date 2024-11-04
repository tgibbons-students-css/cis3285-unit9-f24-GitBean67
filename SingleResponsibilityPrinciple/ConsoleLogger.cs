﻿using System;

using SingleResponsibilityPrinciple.Contracts;

namespace SingleResponsibilityPrinciple
{
    public class ConsoleLogger : ILogger
    {
        public void LogWarning(string message, params object[] args)
        {
            LogMessage("WARN", message, args);
        }

        public void LogInfo(string message, params object[] args)
        {
            LogMessage("INFO", message, args);
        }

        private void LogMessage(string type, string message, params object[] args)
        {
            // Format the message with any additional arguments
            string formattedMessage = string.Format(message, args);

            Console.WriteLine(type + ": " + message, args);

            using (StreamWriter logfile = File.AppendText("log.xml"))
            {
                logfile.WriteLine("<log><type>" + type + "</type><message>" + message + "</message></log> ", args);
            }
        }
    }
}
