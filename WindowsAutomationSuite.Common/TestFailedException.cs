#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: TestFailedException.cs
// 
#endregion
using System;

namespace WindowsAutomationSuite.Common
{
    public class TestFailedException : Exception
    {
        public TestFailedException(string message) : base(message)
        {

        }
    }
}
