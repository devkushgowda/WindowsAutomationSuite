#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: TestRunner.cs
// 
#endregion
using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using WindowsAutomationSuite.Common.Commands;

namespace WindowsAutomationSuite.Common
{
    public class TestRunner
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private long _testSetupTimeElapsed;
        private long _testTimeElapsed;
        private long _testCleanupTimeElapsed;

        private bool _testResult;
        private string _message = "Test completed successfully";
        private string _file;
        public TestConfiguration Configuration { get; private set; }
        public string Message
        {
            get
            {
                return _message;
            }
        }
        public bool Result
        {
            get
            {
                return _testResult;
            }
        }

        public string File
        {
            get
            {
                return _file;
            }
        }

        public long TestSetupTimeElapsedMilliSeconds
        {
            get
            {
                return _testSetupTimeElapsed;
            }
        }

        public long TestTimeElapsedMilliSeconds
        {
            get
            {
                return _testTimeElapsed;
            }
        }
        public long TestCleanupElapsedMilliSeconds
        {
            get
            {
                return _testCleanupTimeElapsed;
            }
        }

        public long TotalTestTimeElapsedMilliSeconds
        {
            get
            {
                return _testSetupTimeElapsed + _testTimeElapsed + _testCleanupTimeElapsed;
            }
        }

        public TestRunner(string file)
        {
            _file = file;
        }
        public void Initialize()
        {
            Configuration = TestConfiguration.Load(_file);
        }
        public static TestRunner Create(string file)
        {
            TestRunner tr = null;
            try
            {
                tr = new TestRunner(file);
                tr.Initialize();
            }
            catch (Exception e)
            {
                throw new TestFailedException("TestRunner Initialize failed. Message: " + e.Message);
            }
            return tr;
        }

        public bool Run()
        {
            Logger.Log("Started test '" + Configuration.TestInformation.Name + "'");
            Logger.SetTitle($"Executing test {Configuration.TestInformation.Name } ...");
            _stopwatch.Start();
            if (_testResult = Run("TS", Configuration.TestSetupSequence))
            {
                _testSetupTimeElapsed = _stopwatch.ElapsedMilliseconds;
                _stopwatch.Restart();

                //Run actual test
                _testResult = Run("T", Configuration.TestSequence);
                _testTimeElapsed = _stopwatch.ElapsedMilliseconds;

                //Run cleanup
                _stopwatch.Restart();
                _testResult = Run("TC", Configuration.TestCleanupSequence) && _testResult;
                _testCleanupTimeElapsed = _stopwatch.ElapsedMilliseconds;
                _stopwatch.Reset();
            }
            Logger.Log("Finished(" + _testResult.ToTestResult() + ") test '" + Configuration.TestInformation.Name + "'\n\n\n");
            return _testResult;
        }
        private bool Run(string logScope, IReadOnlyList<TestStep> testSteps)
        {
            Logger.Log(logScope + ": Started.");

            bool result = true;
            for (int i = 0; i < testSteps.Count; i++)
            {
                var step = logScope + (i + 1);
                try
                {
                    Logger.Log(step + ": Started");
                    var commandResult = CommandFactory.Run(testSteps[i]);
                    Logger.Log(step + ": Finished(" + commandResult.ToTestResult() + ")");
                    if (!commandResult)
                    {
                        _message = step + ": Failed, aborting test.";
                        result = false;
                        break;
                    }
                }
                catch (Exception e)
                {
                    _message = step + ": Failed -" + e.Message;
                    result = false;
                    break;
                }
            }
            Logger.Log(logScope + ": Finished(" + result.ToTestResult() + ")\n");
            return result;
        }

        public string GetReport()
        {
            Logger.SetTitle($"Test completed, Result : {_testResult.ToTestResult()}");
            var sb = new StringBuilder();
            sb.AppendLine("Id\t\t: " + Configuration.TestInformation.TestId + "");
            sb.AppendLine("Name\t\t: " + Configuration.TestInformation.Name);
            sb.AppendLine("Description\t: " + Configuration.TestInformation.Description);
            sb.AppendLine("Result\t\t: " + _testResult.ToTestResult());
            sb.AppendLine("Message\t\t: " + Message);
            sb.AppendLine("Setup time\t: " + Utils.StringifyTime(TestSetupTimeElapsedMilliSeconds));
            sb.AppendLine("Test time\t: " + Utils.StringifyTime(TestTimeElapsedMilliSeconds));
            sb.AppendLine("Cleanup time\t: " + Utils.StringifyTime(TestCleanupElapsedMilliSeconds));
            sb.AppendLine("Total time\t: " + Utils.StringifyTime(TotalTestTimeElapsedMilliSeconds));
            return sb.ToString();
        }
    }
}
