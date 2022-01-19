#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: DirectoryManager.cs
// 
#endregion

using System;
using System.IO;

namespace WindowsAutomationSuite.Common
{
    public static class TestManager
    {
        private static string _testSuiteDirectory = null;

        public static string TestSuiteDirectory
        {
            get { return _testSuiteDirectory ?? Directory.GetCurrentDirectory(); }
        }

        public static string ReportDirectory
        {
            get { return Path.Combine(TestSuiteDirectory, "Reports"); }
        }

        public static string ResourceDirectory
        {
            get { return Path.Combine(TestSuiteDirectory, "Resources"); }
        }
        public static string TestConfigurationDirectory
        {
            get { return Path.Combine(TestSuiteDirectory, "Tests"); }
        }

        public static void Initialize(string testSuiteDirectory)
        {
            _testSuiteDirectory = testSuiteDirectory;
            if (!Directory.Exists(testSuiteDirectory))
            {
                throw new InvalidOperationException($"Test suite not found in the provided directory '{testSuiteDirectory}'.");
            }
            Directory.SetCurrentDirectory(testSuiteDirectory);
            Directory.CreateDirectory(ReportDirectory);
            Logger.Initialize(testSuiteDirectory);
        }
    }
}
