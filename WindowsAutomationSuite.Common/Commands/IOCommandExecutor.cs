#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: IOCommandExecutor.cs
// 
#endregion
using System;
using System.IO;
using System.Linq;

namespace WindowsAutomationSuite.Common.Commands
{
    public class IOCommandExecutor : ICommandExecutor
    {
        public const string Identifier = "io";
        public bool Run(TestStep testStep)
        {
            switch (testStep.Command)
            {
                case CommandConstants.IO_FILE_COPY_WITHFILTER:
                    {
                        return FileCopy(testStep);
                    }
                case CommandConstants.IO_FOLDER_COPY:
                    {
                        return FolderCopy(testStep);
                    }
                case CommandConstants.IO_FILE_CREATE_SETSIZE:
                    {
                        return CreateFileSetSize(testStep);
                    }
                case CommandConstants.IO_FOLDER_DELETE:
                    {
                        return FolderDelete(testStep);
                    }
                case CommandConstants.IO_FILE_DELETE_WITHFILTER:
                    {
                        return FileDelete(testStep);
                    }
                case CommandConstants.IO_FILE_SETMODTIME_WITHFILTER:
                    {
                        return SetLastModifiedDateTime(testStep);
                    }
                case CommandConstants.IO_UNZIP:
                    {
                        return Unzip(testStep);
                    }
                default: throw new TestFailedException($"Invalid IO command type '{testStep.Command }'");
            }

        }
        public bool CreateFileSetSize(TestStep testStep)
        {
            var target = testStep.TryGetValue("target");
            long sizeKb = long.Parse(testStep.TryGetValue("kbsize"));
            try
            {
                Utils.CreateFile(target, sizeKb);
            }
            catch (Exception e)
            {
                throw new TestFailedException($"Error CreateFileSetSize for file '{target}' of size {sizeKb}KB , Message: '{e.Message}'");
            }
            return true;
        }

        public bool SetLastModifiedDateTime(TestStep testStep)
        {
            var targetPath = testStep.TryGetValue("target");
            var addSeconds = testStep.TryGetValue("addseconds");
            try
            {
                var folder = Path.GetDirectoryName(targetPath);
                var searchPattern = Path.GetFileName(targetPath);
                var addSecondsVal = double.Parse(addSeconds);
                var datetime = DateTime.Now.AddSeconds(addSecondsVal);
                Utils.SetLastWriteTimeWithFilter(folder, searchPattern, datetime);
            }
            catch (Exception e)
            {
                throw new TestFailedException($"Error SetLastWriteTime for filter '{targetPath}', Message: '{e.Message}'");
            }
            return true;
        }

        public bool FileDelete(TestStep testStep)
        {
            var source = testStep.TryGetValue("target");
            try
            {
                var folder = Path.GetDirectoryName(source);
                var searchPattern = Path.GetFileName(source);
                var fileInfos = Directory.GetFiles(folder, searchPattern).ToList();
                Logger.Log($"Deleting files[{string.Join(", ", fileInfos.Select(x => Path.GetFileName(x)))}] from folder '{folder}' with filter '{searchPattern}'.");
                fileInfos.ForEach(filepath =>
                {
                    File.Delete(filepath);
                });
            }
            catch (IOException e)
            {
                throw new TestFailedException($"Error deleting file '{source}', Message: '{e.Message}'");
            }
            return true;
        }

        public bool FolderDelete(TestStep testStep)
        {
            var target = testStep.TryGetValue("target");
            try
            {
                Logger.Log($"Deleting folder '{target}' reccursively.");
                if (Directory.Exists(target))
                    Directory.Delete(target, true);
            }
            catch (IOException e)
            {
                throw new TestFailedException($"Error deleting folder '{target}', Message: '{e.Message}'");
            }
            return true;
        }

        public bool FileCopy(TestStep testStep)
        {
            var source = testStep.TryGetValue("source");
            var target = testStep.TryGetValue("target");
            try
            {
                var folder = Path.GetDirectoryName(source);
                var searchPattern = Path.GetFileName(source);
                var fileInfos = Directory.GetFiles(folder, searchPattern).ToList();
                Directory.CreateDirectory(target);
                Logger.Log($"Copying files[{string.Join(", ", fileInfos.Select(x => Path.GetFileName(x)))}] from folder '{folder}' with filter '{searchPattern}' to '{target}'.");
                fileInfos.ForEach(filepath =>
                {
                    var targetPath = Path.Combine(target, Path.GetFileName(filepath));
                    File.Copy(filepath, targetPath, true);
                });
            }
            catch (IOException e)
            {
                throw new TestFailedException($"Error copying file '{source}' to '{target}', Message: '{e.Message}'");
            }
            return true;
        }

        public bool Unzip(TestStep testStep)
        {
            bool result = false;
            var source = testStep.TryGetValue("source");
            var target = testStep.TryGetValue("target");
            try
            {
                Logger.Log($"Unzipping file '{source}' to '{target}'.");
                result = Utils.UnzipFile(source, target);
            }
            catch (IOException e)
            {
                throw new TestFailedException($"Error unzipping file '{source}' to '{target}', Message: '{e.Message}'");
            }
            return result;
        }

        public bool FolderCopy(TestStep testStep)
        {
            var source = testStep.TryGetValue("source");
            var target = testStep.TryGetValue("target");
            try
            {
                var sourceDirInfo = new DirectoryInfo(source);
                var targetDirInfo = new DirectoryInfo(target);
                targetDirInfo.Create();
                Logger.Log($"Copying folder '{source}' to {target}.");
                Utils.CopyFilesRecursively(sourceDirInfo, targetDirInfo);
            }
            catch (IOException e)
            {
                throw new TestFailedException($"Error copying folder '{source}' to '{target}', Message: '{e.Message}'");
            }
            return true;
        }

    }
}
