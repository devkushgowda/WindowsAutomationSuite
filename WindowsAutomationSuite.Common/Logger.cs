#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: Logger.cs
// 
#endregion
using System;
using System.IO;

namespace WindowsAutomationSuite.Common
{
    public static class Logger
    {
        private static StreamWriter _sw = null;
        public static void Log(string message)
        {
            var logMessage = $"{DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]")} {message}";
            if (_sw != null)
                _sw.WriteLine(logMessage);
            Console.WriteLine(logMessage);
        }

        public static void SetTitle(string title)
        {
            Console.Title = title;
        }

        public static void Initialize(string testSuiteDirectory)
        {
            _sw = new StreamWriter(new FileStream($"test_report{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.log", FileMode.Create)) { AutoFlush = true };
        }
    }
}
