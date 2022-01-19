#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: TestConfiguration.cs
// 
#endregion
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace WindowsAutomationSuite.Common
{
    public class TestStep
    {
        public TestStep(TestConfiguration testConfiguration, string command, IReadOnlyDictionary<string, string> keyMapDictionary)
        {
            Configuration = testConfiguration;
            Command = command;
            KeyMapDictionary = keyMapDictionary;
        }
        public TestConfiguration Configuration { get; private set; }
        public string Command { get; private set; }
        public IReadOnlyDictionary<string, string> KeyMapDictionary { get; private set; }

        public bool HasAttribute(string key)
        {
            return KeyMapDictionary.ContainsKey(key);
        }

        public string TryGetValue(string key)
        {
            if (KeyMapDictionary.ContainsKey(key))
            {
                return KeyMapDictionary[key];
            }
            else
            {
                throw new TestFailedException("Unable to get mandatory key '" + key + "'");
            }
        }
    }

    public class TestInformation
    {
        public TestInformation(string id, string name, string description)
        {
            TestId = id;
            Name = name;
            Description = description;
        }

        public string TestId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

    }


    public class TestConfiguration
    {

        private XmlFileHelper _xmlFile;

        public TestInformation TestInformation { get; private set; }
        public IReadOnlyList<TestStep> TestSetupSequence { get; private set; }
        public IReadOnlyList<TestStep> TestSequence { get; private set; }
        public IReadOnlyList<TestStep> TestCleanupSequence { get; private set; }
        public ServiceConfigHelper ServiceConfigHelper { get; private set; }
        public void Initialize(string file)
        {
            _xmlFile = XmlFileHelper.Load(file);
            _xmlFile.Save();
            var serviceConfigFile = _xmlFile.GetNodeValue("/DataExportFrameworkTest/TestSetup/ServiceConfiguration/Path") ?? @"G:\Site\dataexportserviceconfiguration.xml";
            var finalDirectory = _xmlFile.GetNodeValue("/DataExportFrameworkTest/TestSetup/ServiceConfiguration/FinalDirectory");

            ServiceConfigHelper = ServiceConfigHelper.Load(serviceConfigFile);

            var testId = _xmlFile.GetNodeValue("/DataExportFrameworkTest/TestInformation/Id") ?? throw new TestFailedException("Error while loading the test configuration " + file + "testId cannot be null.");
            var testName = _xmlFile.GetNodeValue("/DataExportFrameworkTest/TestInformation/Name");
            var testDescription = _xmlFile.GetNodeValue("/DataExportFrameworkTest/TestInformation/Description");
            TestInformation = new TestInformation(testId, testName, testDescription);

            TestSetupSequence = GetTestSteps(_xmlFile.Document, "/DataExportFrameworkTest/TestSetup/Sequence/Step");
            TestSequence = GetTestSteps(_xmlFile.Document, "/DataExportFrameworkTest/Test/Sequence/Step");
            TestCleanupSequence = GetTestSteps(_xmlFile.Document, "/DataExportFrameworkTest/TestCleanup/Sequence/Step");
        }

        private List<TestStep> GetTestSteps(XmlDocument xmlDoc, string xPath)
        {
            var nodes = xmlDoc.SelectNodes(xPath).Cast<XmlElement>();
            var result = nodes.Select(node =>
              {
                  const string commandKey = "command";
                  var map = node.Attributes.Cast<XmlAttribute>()
                    .ToDictionary(attribute => attribute.Name.ToLower(), attribute => attribute.Value);
                  var command = map[commandKey];
                  map.Remove(commandKey);
                  return new TestStep(this, command, map);
              }).ToList();
            return result;
        }

        public static TestConfiguration Load(string file)
        {
            var tc = new TestConfiguration();
            tc.Initialize(file);
            return tc;
        }
    }
}
