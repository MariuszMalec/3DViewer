using BladeMill.BLL.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Markup;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace StartWindow.Service
{
    public static class InputDataXmlService
    {
        public static void CreateInputDataXml(string file, List<InputDataXml> datas)
        {
            var service = new BladeMill.BLL.Services.InputDataXmlService();
            CheckDirectory(file);
            //usuniecie xml declarations from root
            var xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add("", "");
            var serializer = new XmlSerializer(typeof(List<InputDataXml>));
            using (var writer = File.CreateText(file))
            {
                serializer.Serialize(writer, new List<InputDataXml>(datas), xmlnsEmpty);
            }
            //usuniecie roota
            XDocument input = XDocument.Load(file);
            XElement firstChild = input.Root.Elements().First();
            firstChild.Save(file);
        }

        private static void CheckDirectory(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static List<InputDataXml> GetDataFromInputDataXml()
        {
            var service = new BladeMill.BLL.Services.InputDataXmlService();
            return service.GetAllDataFromInputDataXml();
        }

    }
}
