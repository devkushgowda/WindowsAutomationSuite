#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: CommandFactory.cs
// 
#endregion
using System;
using System.Linq;
using System.Collections.Generic;

namespace WindowsAutomationSuite.Common.Commands
{
    public static class CommandFactory
    {
        /// <summary>
        /// Register all the command executors in this dictionary.
        /// </summary>
        private readonly static Dictionary<string, ICommandExecutor> commandDictionary = new Dictionary<string, ICommandExecutor>()
        {
            { UtilCommandExecutor.Identifier, new UtilCommandExecutor() },
            { IOCommandExecutor.Identifier, new IOCommandExecutor() },
        };

        /// <summary>
        /// Provides command executor based on the command alias.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandExecutor CommandExecutorFactory(string command)
        {
            var ce = commandDictionary.FirstOrDefault(kv =>
                command.StartsWith(kv.Key, StringComparison.InvariantCultureIgnoreCase));
            if (ce.Value == null)
            {
                throw new TestFailedException("Command '" + command + "' not defined.");
            }
            return ce.Value;
        }

        /// <summary>
        /// Execute the test step.
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool Run(TestStep ts)
        {
            var commandExecutor = CommandExecutorFactory(ts.Command);
            return commandExecutor.Run(ts);
        }
    }
}
