using BladeMill.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml;
using System.IO;
using StartWindow.Models;

namespace StartWindow.Service
{
    public class BmdXmlService
    {
        private List<BmdXmlFileView> _bmdxmlParameters = new List<BmdXmlFileView>() { };

        public List<BmdXmlFileView> GetAll(string bmdFile)
        {
            _bmdxmlParameters.Clear();
            if (File.Exists(bmdFile) && bmdFile.Contains(".xml"))
            {
                var pass = true;
                var typeOfBlade = GetBmdType(bmdFile);
                _bmdxmlParameters.Add(new BmdXmlFileView(1, "Typ lopatki", GetTypeBlade(bmdFile), pass));
                _bmdxmlParameters.Add(new BmdXmlFileView(1, "Drawing", GetDrawing(bmdFile), pass));
                _bmdxmlParameters.Add(new BmdXmlFileView(1, "Project", GetProject(bmdFile), pass));
                _bmdxmlParameters.Add(new BmdXmlFileView(1, "Material", GetMaterial(bmdFile), pass));
                _bmdxmlParameters.Add(new BmdXmlFileView(1, "Strumien", GetOrientation(bmdFile), pass));
                _bmdxmlParameters.Add(new BmdXmlFileView(1, "Typ", typeOfBlade, pass));
                AddParameters(bmdFile);
            }
            return _bmdxmlParameters;
        }

        public BmdXmlFileView GetByName(string name)
        {
            return _bmdxmlParameters.ToList().Where(p => p.Name == name).Select(p => p).FirstOrDefault();
        }

        public List<BmdXmlFileView> AddParameters(string bmdFile)
        {
            if (File.Exists(bmdFile) && bmdFile.Contains(".xml"))
            {
                AddToList("/BPMManufacturingData/Quality/Dimensions/LengthDimension", "Name", "NominalValue", "", bmdFile);
                AddToList("/BPMManufacturingData/Quality/Dimensions/RadiusDimension", "Name", "NominalValue", "", bmdFile);
                AddToList("/BPMManufacturingData/Quality/Dimensions/AngleDimension", "Name", "NominalValue", "", bmdFile);
                AddToList("/BPMManufacturingData/Quality/Dimensions/DiameterDimension", "Name", "NominalValue", "", bmdFile);
                AddToList("/BPMManufacturingData/BladeTopology/MainFunctionElement/FunctionElement/ControlPara", "Name", "Value", "", bmdFile);
            }
            return _bmdxmlParameters;
        }

        private List<BmdXmlFileView> AddToList(string navigator, string atribute1, string atribute2, string prefix, string bmdFile)
        {
            if (File.Exists(bmdFile))
            {
                XmlDocument document = new XmlDocument();
                document.Load(bmdFile);
                XPathNavigator navigator1 = document.CreateNavigator();
                XPathNodeIterator nodes1 = navigator1.Select(navigator);
                string name;
                string value;
                bool pass = true;
                while (nodes1.MoveNext())
                {
                    if (prefix != "")
                    {
                        name = nodes1.Current.GetAttribute(atribute1, "") + prefix;
                        value = nodes1.Current.GetAttribute(atribute2, "");
                        if (value.Contains("ST12T"))
                            pass = true;
                        _bmdxmlParameters.Add(new BmdXmlFileView(1, name, value, pass));
                    }
                    else
                    {
                        name = nodes1.Current.GetAttribute(atribute1, "");
                        value = nodes1.Current.GetAttribute(atribute2, "");
                        if (value.Contains("ST12T"))
                            pass = true;
                        _bmdxmlParameters.Add(new BmdXmlFileView(1, name, value, pass));
                    }
                }
            }
            return _bmdxmlParameters;
        }

        public string GetFromFileValue(string bmdXmlFile, string findtext)
        {
            if (File.Exists(bmdXmlFile))
            {
            }
            return string.Empty;
        }
        public string GetOrientation(string bmdXmlFile)
        {
            if (File.Exists(bmdXmlFile))
            {
                return GetSingleElement("BladeOrientation", "/BPMManufacturingData/BladeTopology", "Strumien", bmdXmlFile);
            }
            return "Unknown";
        }
        public string GetMaterial(string bmdXmlFile)
        {
            if (File.Exists(bmdXmlFile))
            {
                return GetElement("Name", "/BPMManufacturingData/Header/Part/StandardRawMaterial", "Material", bmdXmlFile);
            }
            return "Unknown";
        }
        public string GetDrawing(string bmdXmlFile)
        {
            if (File.Exists(bmdXmlFile))
            {
                return GetElement("ID", "/BPMManufacturingData/Header/Part", "Rysunek", bmdXmlFile);
            }
            return "Unknown";
        }
        public string GetProject(string bmdXmlFile)
        {
            if (File.Exists(bmdXmlFile))
            {
                return GetElement("Project", "/BPMManufacturingData/Header/Part", "Project", bmdXmlFile);
            }
            return "Unknown";
        }
        public string GetBmdType(string bmdXmlFile)
        {
            if (File.Exists(bmdXmlFile))
            {
                return GetElement("Type", "/BPMManufacturingData/BladeTopology/MainFunctionElement", "BPMTYP", bmdXmlFile);
            }
            return "Unknown";
        }
        public string GetTypeBlade(string bmdXmlFile)
        {
            if (File.Exists(bmdXmlFile))
            {
                return GetElement("Name", "/BPMManufacturingData/Header/Part", "TypLopatki", bmdXmlFile);
            }
            return "Unknown";
        }
        private string GetElement(string element, string navigator, string newname, string xmlfile)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(xmlfile);
                XPathNavigator navigator2 = document.CreateNavigator();
                XPathNodeIterator nodes2 = navigator2.Select(navigator);
                //
                string line;
                while (nodes2.MoveNext())
                {
                    line = newname;
                    line = nodes2.Current.GetAttribute(element, "");
                    newname = (line);
                    return newname;
                }
                return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }
        private string GetSingleElement(string element, string navigator, string newname, string xmlfile)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(xmlfile);
                XPathNavigator navigator2 = document.CreateNavigator();
                XPathNodeIterator nodes2 = navigator2.Select(navigator);
                string line;
                foreach (XPathNavigator oCurrent in nodes2)
                {
                    line = newname;
                    line = oCurrent.SelectSingleNode(element).Value;
                    newname = (line);
                    return newname;
                }
                return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}
