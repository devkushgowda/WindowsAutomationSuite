namespace IOTDataTransferTestFramework.Common
{
    public static class LoggerExtension
    {
        public static void Log(this TestRunner obj, string message)
        {
            Logger.Log("[" + obj.Configuration.TestInformation.TestId + "] " + message);
        }
        public static void Log(this BatchTestRunner obj, string message)
        {
            Logger.Log(message);
        }
    }
}
