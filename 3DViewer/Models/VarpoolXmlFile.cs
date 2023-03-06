using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace _3DViewer.Models
{
    public class VarpoolXmlFile
    {
        public string FileClampingRawBox { get; set; }
        public string BMTemplate { get; set; }
        public string ClampMethod { get; set; }
        public string DFA { get; set; }
        public string OrderName { get; set; }
        public string BMTemplateFile { get; set; }
        public string HROH { get; set; }
        public string BROH { get; set; }
        public string LROH { get; set; }
        public string FIG_N { get; set; }
        public string BladeMaterial { get; set; }
        public string MfgSystem { get; set; }
        public string ProjectNameAndPassword { get; set; }
        public string BladeOrientation { get; set; }
        public string middleTol { get; set; }
        public string VCprojectTemplate { get; set; }
        public string VarpoolFile { get; set; }
        public string Ltol_HE { get; set; }
        public string Utol_HE { get; set; }
        public VarpoolXmlFile(string varpoolFile)
        {
            VarpoolFile = varpoolFile;
            FileClampingRawBox = GetFromFileValue("FileClampingRawBox");
            BMTemplate = GetFromFileValue("BMTemplate");
            DFA = GetFromFileValue("DFA");
            OrderName = GetFromFileValue("OrderName");
            BMTemplateFile = GetFromFileValue("BMTemplateFile");
            HROH = GetFromFileValue("HROH");
            BROH = GetFromFileValue("BROH");
            LROH = GetFromFileValue("LROH");
            FIG_N = GetFromFileValue("FIG_N");
            BladeMaterial = GetFromFileValue("BladeMaterial");
            ClampMethod = GetFromFileValue("ClampMethod");
            MfgSystem = GetFromFileValue("MfgSystem");
            ProjectNameAndPassword = GetFromFileValue("ProjectNameAndPassword");
            BladeOrientation = GetFromFileValue("BladeOrientation");
            middleTol = GetFromFileValue("middleTol");
            VCprojectTemplate = GetFromFileValue("VCprojectTemplate");
            Utol_HE = GetFromFileValue("Utol_HE");
            Ltol_HE = GetFromFileValue("Ltol_HE");
        }
        private string GetFromFileValue(string findtext)
        {
            try
            {
                string Value = string.Empty;
                if (System.IO.File.Exists(VarpoolFile))
                {
                    List<string> listvarpoolNames = new List<string>(new string[] { });
                    List<string> listvarpoolValues = new List<string>(new string[] { });
                    XmlDocument doc = new XmlDocument();
                    doc.Load(VarpoolFile);
                    XPathNavigator navigator = doc.CreateNavigator();
                    XPathNodeIterator nodes = navigator.Select("/VarPool/Overview");
                    string Name = "";
                    XPathNodeIterator nodesName = navigator.Select("/VarPool/Var/Name");
                    foreach (XPathNavigator oCurrent in nodesName)
                    {
                        Name = oCurrent.InnerXml;//Name
                        listvarpoolNames.Add(Name);
                    }
                    XPathNodeIterator nodesValue = navigator.Select("/VarPool/Var/Value");
                    foreach (XPathNavigator oCurrent in nodesValue)
                    {
                        Value = oCurrent.InnerXml;//Name
                        listvarpoolValues.Add(Value);
                    }
                    Value = string.Empty;
                    int count = 0;
                    foreach (string element in listvarpoolNames)
                    {
                        //if(element.Contains(textfind))
                        if (element == findtext)
                        {
                            Value = listvarpoolValues[count].ToString().Replace(" ", "");
                        }
                        count++;
                    }
                    return $"{Value.Replace("\n", "").Replace("\r", "").Replace("\t", "")}";
                }
                return $"{Value}";
            }
            catch (Exception e)
            {
                throw new Exception("check function GetFromXmlFileValue", e);
            }
        }

    }
}
