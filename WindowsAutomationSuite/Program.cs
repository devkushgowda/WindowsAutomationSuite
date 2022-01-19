#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: Program.cs
// 
#endregion
using System;
using System.IO;
using System.Linq;
using WindowsAutomationSuite.Common;

namespace WindowsAutomationSuite
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                TestManager.Initialize(args[0]);
                try
                {
                    BatchTestRunner batchTestRunner = new BatchTestRunner();
                    var testFiles = Directory.GetFiles(TestManager.TestConfigurationDirectory, "deft*.xml").ToList();
                    batchTestRunner.Initialize(testFiles);
                    batchTestRunner.Run();
                    Logger.Log("\n" + batchTestRunner.GetReport());
                }
                catch (Exception e)
                {
                    Logger.Log("\n" + e.Message);
                }
            }
            else
            {
                Console.WriteLine($"Invalid arguments.\n" +
                                  $"Usage: TestApplication.exe <TestSuiteDirectory>");
            }
        }
    }
}
