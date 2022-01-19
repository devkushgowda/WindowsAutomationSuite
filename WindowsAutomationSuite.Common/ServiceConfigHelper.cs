#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: ServiceConfigHelper.cs
// 
#endregion
using System;

namespace WindowsAutomationSuite.Common
{
    public class ServiceConfigHelper
    {
        private readonly string _file;
        private XmlFileHelper _xmlFile;

        public XmlFileHelper XmlFileHelper
        {
            get
            {
                return _xmlFile;
            }
        }

        public ServiceConfigHelper(string file)
        {
            _file = file;
        }

        public void Initialize()
        {
            _xmlFile = XmlFileHelper.Load(_file);
        }

        public static ServiceConfigHelper Load(string file)
        {
            var serviceConfigHelper = new ServiceConfigHelper(file);
            serviceConfigHelper.Initialize();
            return serviceConfigHelper;
        }

        public void Save()
        {
            _xmlFile.Save();
        }


        #region MaxDataCacheSizeInGBXPath
        public string GetMaxDataCacheSizeInGBXPath()
        {
            return "/DataExport/MaxDataCacheSizeInGB";
        }
        public string GetMaxDataCollectionInDays()
        {
            return _xmlFile.GetNodeValue(GetMaxDataCacheSizeInGBXPath());
        }
        public bool SetMaxDataCollectionInDays(string value)
        {
            return _xmlFile.SetNodeValue(GetMaxDataCacheSizeInGBXPath(), value);
        }
        #endregion

        #region MaxDataCacheSizeInGB
        public string GetMaxDataCollectionInDaysXPath()
        {
            return "/DataExport/MaxDataCollectionInDays";
        }

        public string GetMaxDataCacheSizeInGB()
        {
            return _xmlFile.GetNodeValue(GetMaxDataCollectionInDaysXPath());
        }

        public bool SetMaxDataCacheSizeInGB(string value)
        {
            return _xmlFile.SetNodeValue(GetMaxDataCollectionInDaysXPath(), value);
        }
        #endregion

        #region CleanUpDataOlderThanDays
        public string GetCleanUpDataOlderThanDaysXPath()
        {
            return "/DataExport/CleanUpDataOlderThanDays";
        }

        public string GetCleanUpDataOlderThanDays()
        {
            return _xmlFile.GetNodeValue(GetCleanUpDataOlderThanDaysXPath());
        }

        public bool SetCleanUpDataOlderThanDays(string value)
        {
            return _xmlFile.SetNodeValue(GetCleanUpDataOlderThanDaysXPath(), value);
        }

        #endregion

        #region FinalDirectory
        public string GetFinalDirectoryXPath()
        {
            return "/DataExport/FinalDirectory";
        }

        public string GetFinalDirectory()
        {
            return _xmlFile.GetNodeValue(GetFinalDirectoryXPath());
        }

        public bool SetFinalDirectory(string value)
        {
            return _xmlFile.SetNodeValue(GetFinalDirectoryXPath(), value);
        }

        #endregion


        #region CacheDirectory
        public string GetCacheDirectoryXPath()
        {
            return "/DataExport/CacheDirectory";
        }

        public string GetCacheDirectory()
        {
            return _xmlFile.GetNodeValue(GetCacheDirectoryXPath());
        }

        public bool SetCacheDirectory(string value)
        {
            return _xmlFile.SetNodeValue(GetCacheDirectoryXPath(), value);
        }

        #endregion

        #region FileTransferProviderType
        public string GetFileTransferProviderTypeXPath()
        {
            return "/DataExport/FileTransferProviderType";
        }

        public string GetFileTransferProviderType()
        {
            return _xmlFile.GetNodeValue(GetFileTransferProviderTypeXPath());
        }

        public bool SetFileTransferProviderType(string value)
        {
            return _xmlFile.SetNodeValue(GetFileTransferProviderTypeXPath(), value);
        }

        #endregion

        #region ZipFile
        public string GetZipFileXPath()
        {
            return "/DataExport/ZipFile";
        }

        public string GetZipFile()
        {
            return _xmlFile.GetNodeValue(GetFileTransferProviderTypeXPath());
        }

        public bool SetZipFile(string value)
        {
            return _xmlFile.SetNodeValue(GetFileTransferProviderTypeXPath(), value);
        }

        #endregion
        #region IsEnabled
        public string GetIsEnabledXPath()
        {
            return "/DataExport/IsEnabled";
        }

        public string GetIsEnabled()
        {
            return _xmlFile.GetNodeValue(GetIsEnabledXPath());
        }

        public string GetEnabledDateTimeXPath()
        {
            return "/DataExport/EnabledDateTime";
        }

        public bool SetIsEnabled(string value)
        {
            _xmlFile.SetNodeValue(GetEnabledDateTimeXPath(), DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            return _xmlFile.SetNodeValue(GetIsEnabledXPath(), value);
        }

        #endregion

        #region RepeatInterval

        public string GetRepeatIntervalXPath(string category)
        {
            return string.Format("/DataExport/CategoryList/Category[@Name=\"{0}\"]/IntervalInMinutes/@Repeat", category);
        }

        public string GetRepeatInterval(string category)
        {
            return _xmlFile.GetAttributeValue(GetRepeatIntervalXPath(category));
        }

        public bool SetRepeatInterval(string category, string value)
        {
            return _xmlFile.SetAttributeValue(GetRepeatIntervalXPath(category), value);
        }
        #endregion

    }
}
