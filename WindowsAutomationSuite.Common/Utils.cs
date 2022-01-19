#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: Utils.cs
// 
#endregion
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;

namespace WindowsAutomationSuite.Common
{
    public static class Utils
    {
        public static string ToTestResult(this bool result)
        {
            return result ? "Passed" : "Failed";
        }
        public static DateTime ParseDate(string v, string format)
        {
            DateTime dt;
            return DateTime.TryParseExact(v, format, null, DateTimeStyles.None, out dt) ? dt : DateTime.MinValue;
        }

        public static bool ExecuteProcess(ProcessStartInfo psi, string stdInput = null, int waitTime = 20000)
        {
            Logger.Log($"Executing Process- '{psi.FileName}', Arguments- '{psi.Arguments}', Input- '{stdInput}', Timeout- {waitTime} ms");
            psi.RedirectStandardInput = stdInput != null;
            psi.RedirectStandardOutput = false;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();
                if (stdInput != null)
                    process.StandardInput.WriteLine(stdInput);

                process.WaitForExit(waitTime);
                return process.HasExited && process.ExitCode == 0;
            }
        }

        /// <summary>
        /// Split data table to chunks of specified size.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> ToChunks<T>(this IEnumerable<T> enumerable, int chunkSize)
        {
            int itemsReturned = 0;
            var list = enumerable.ToList(); // Prevent multiple execution of IEnumerable.
            int count = list.Count;
            while (itemsReturned < count)
            {
                int currentChunkSize = Math.Min(chunkSize, count - itemsReturned);
                yield return list.GetRange(itemsReturned, currentChunkSize);
                itemsReturned += currentChunkSize;
            }
        }

        public static void CreateFile(string target, long sizeKb)
        {
            Logger.Log($"Creating file '{target}' of size {sizeKb}KB.");
            using (var stream = File.Create(target))
            {
                stream.SetLength(sizeKb * 1024);
            }
        }

        public static bool UnzipFile(string filePath, string outputFolder)
        {
            var psi = new ProcessStartInfo { FileName = @"unzip.exe", Arguments = filePath + " -d " + outputFolder };
            return ExecuteProcess(psi);
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, bool overwrite = true)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
        }

        public static string StringifyTime(long ms)
        {
            var timeElapsed = TimeSpan.FromMilliseconds(ms);
            return timeElapsed.ToString();
        }

        public static void SetCreationTimeWithFilter(string targetFolder, string searchPattern, DateTime datetime)
        {
            var fileInfos = new DirectoryInfo(targetFolder).GetFiles(searchPattern, SearchOption.AllDirectories).ToList();
            Logger.Log($"Setting  SetCreationTime to '{datetime}' for files in folder '{targetFolder}' with filter '{searchPattern}', result files[{string.Join(", ", fileInfos.Select(x => x.Name))}]");
            fileInfos.ForEach(file => {
                File.SetCreationTime(file.FullName, datetime);
                File.SetLastWriteTime(file.FullName, datetime);
            });
        }

        public static void SetLastWriteTimeWithFilter(string targetFolder, string searchPattern, DateTime datetime)
        {
            var fileInfos = new DirectoryInfo(targetFolder).GetFiles(searchPattern, SearchOption.AllDirectories).ToList();
            Logger.Log($"Setting  SetLastWriteTime to '{datetime}' for files in folder '{targetFolder}' with filter '{searchPattern}', result files[{string.Join(", ", fileInfos.Select(x => x.Name))}]");
            fileInfos.ForEach(file => File.SetLastWriteTime(file.FullName, datetime));
        }
    }
}
