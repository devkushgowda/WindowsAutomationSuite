﻿#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: ICommandExecutor.cs
// 
#endregion
namespace WindowsAutomationSuite.Common.Commands
{
    public interface ICommandExecutor
    {
        bool Run(TestStep testStep);
    }
}
