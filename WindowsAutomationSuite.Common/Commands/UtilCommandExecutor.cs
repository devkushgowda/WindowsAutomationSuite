#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: UtilCommandExecutor.cs
// 
#endregion
using System.Threading;
using System.Diagnostics;

namespace WindowsAutomationSuite.Common.Commands
{
    public class UtilCommandExecutor : ICommandExecutor
    {
        public const string Identifier = "util";
        public bool Run(TestStep testStep)
        {
            switch (testStep.Command)
            {
                case CommandConstants.UTIL_SLEEP:
                    return Sleep(testStep);
                case CommandConstants.UTIL_SYSTEM_COMMAND:
                    return ExecuteSystemCommand(testStep);
                default: throw new TestFailedException($"Invalid Util command type '{testStep.Command}'");
            }

        }

        private bool ExecuteSystemCommand(TestStep testStep)
        {
            var pname = testStep.TryGetValue("pname");
            var args = testStep.HasAttribute("args") ? testStep.TryGetValue("args") : null;
            var timeout = testStep.HasAttribute("timeout") ? int.Parse(testStep.TryGetValue("timeout")) : 30000;
            var input = testStep.HasAttribute("input") ? testStep.TryGetValue("input") : null;
            var psi = new ProcessStartInfo { FileName = pname, Arguments = args };
            return Utils.ExecuteProcess(psi, input, timeout);
        }

        public bool Sleep(TestStep testStep)
        {
            int ms;
            if (int.TryParse(testStep.TryGetValue("ms"), out ms))
            {
                Logger.Log($"Sleeping for {ms} ms.");
                Thread.Sleep(ms);
                return true;
            }
            else
                throw new TestFailedException($"Invalid sleep time '{ms}'");
        }
    }
}
