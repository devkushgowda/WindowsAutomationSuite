#region Author: Kushan Deavarajegowda 2022
// 
// All rights are reserved. Reproduction or transmission in whole or in part, in  
// any form or by any means, electronic, mechanical or otherwise, is prohibited  
// without the prior written consent of the copyright owner. 
//  
// FILENAME: XmlFileHelper.cs
// 
#endregion
using System.Xml;

namespace WindowsAutomationSuite.Common
{
    public class XmlFileHelper
    {
        private readonly string _file;
        private XmlDocument _xmlDocument;

        public XmlDocument Document
        {
            get
            {
                return _xmlDocument;
            }
        }
        public string Name
        {
            get
            {
                return _file;
            }
        }

        public XmlFileHelper(string file)
        {
            _file = file;
        }

        public static XmlFileHelper Load(string file)
        {
            var xmlFileHelper = new XmlFileHelper(file);
            xmlFileHelper.Initialize();
            return xmlFileHelper;
        }

        public void Initialize()
        {
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(_file);
        }

        public bool SetAttributeValue(string xpath, string value)
        {
            var res = false;
            var xmlAttribute = (XmlAttribute)_xmlDocument.SelectSingleNode(xpath);
            if (xmlAttribute != null)
            {
                res = true;
                xmlAttribute.Value = value;
            }
            return res;
        }

        public bool SetNodeValue(string xpath, string value)
        {
            var res = false;
            var xmlElement = (XmlElement)_xmlDocument.SelectSingleNode(xpath);
            if (xmlElement != null)
            {
                res = true;
                xmlElement.InnerText = value;
            }
            return res;
        }

        public string GetAttributeValue(string xpath)
        {
            var xmlAttribute = (XmlAttribute)_xmlDocument.SelectSingleNode(xpath);
            return xmlAttribute?.Value;
        }

        public string GetNodeValue(string xpath)
        {
            var xmlElement = (XmlElement)_xmlDocument.SelectSingleNode(xpath);
            return xmlElement?.InnerText;
        }

        public void Save()
        {
            _xmlDocument.Save(_file);
        }
    }
}
