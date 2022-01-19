#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: BatchTestRunner.cs
// 
#endregion
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace WindowsAutomationSuite.Common
{
    public class BatchTestRunner
    {
        private List<string> _testFilesList;
        private List<TestRunner> _testRunnersList;
        public void Initialize(List<string> testFilesList)
        {
            if (testFilesList.Count == 0)
            {
                throw new TestFailedException("Test configuration cannot be empty.");
            }
            _testFilesList = testFilesList;
            _testFilesList.Sort();
            _testRunnersList = new List<TestRunner>();
            Logger.Log("Scanning for test files...");
            testFilesList.ForEach(file =>
            {
                var tr = TestRunner.Create(file);
                var dupObject = _testRunnersList.FirstOrDefault(x =>
                    x.Configuration.TestInformation.TestId.Equals(tr.Configuration.TestInformation.TestId,
                        StringComparison.InvariantCultureIgnoreCase));
                if (dupObject != null)
                {
                    throw new TestFailedException("Duplicate test id '" + tr.Configuration.TestInformation.TestId + "' recognised in '" + dupObject.File + "' and '" + tr.File + "'");
                }
                Logger.Log($"{Path.GetFileName(tr.File)} test added.");
                _testRunnersList.Add(tr);
            });

            Logger.Log("Total " + _testRunnersList.Count + " tests detected.\n");
        }

        public void Run()
        {
            _testRunnersList.ForEach(tr =>
            {
                tr.Run();
            });
        }

        public string GetReport()
        {
            var sb = new StringBuilder();
            var stars = new string('*', 150);
            sb.AppendLine();
            _testRunnersList.ForEach(tr =>
            {
                sb.AppendLine(stars);
                sb.AppendLine();
                sb.AppendLine(tr.GetReport());
                sb.AppendLine();
            });
            sb.AppendLine();
            sb.AppendLine(stars);
            sb.AppendLine();
            sb.AppendLine($"Total result\t: {(_testRunnersList.All(x => x.Result).ToTestResult())}");
            var totalTimeInMs = _testRunnersList.Select(x => x.TotalTestTimeElapsedMilliSeconds).Sum();
            sb.AppendLine($"Total time\t: {Utils.StringifyTime(totalTimeInMs)}");
            sb.AppendLine();
            sb.AppendLine(stars);
            return sb.ToString();
        }
    }
}
