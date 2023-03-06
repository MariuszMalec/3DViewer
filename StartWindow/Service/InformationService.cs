using BladeMill.BLL.SourceData;
using System;
using System.Windows;
using System.Xml.XPath;
using System.Xml;
using System.IO;

namespace StartWindow.Service
{
    public static class InformationService
    {
        public static void Usebmtemplate(bool? checkBox)
        {
            if (checkBox == true)
            {
                var pathData = new PathDataBase();
                string cleverhome = pathData.GetCleverHome();
                //MessageBox.Show(cleverhome.ToString(),"CLEVERHOME",MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (!cleverhome.Contains("3.17") && !cleverhome.Contains("3.18") && !cleverhome.Contains("3.19") && !cleverhome.Contains("3.2"))
                {
                    MessageBox.Show("Po zakonczeniu liczenia otworz BladeMill i zrob update geometrii", "INFORMACJA", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                MessageBox.Show("Do zakonczenia obliczen nie wykonuj zadnych operacji na komputerze", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static void CheckMaterial(string xmlfile)
        {
            if (File.Exists(xmlfile))
            {
                try
                {
                    //create list	
                    XmlDocument document = new XmlDocument();
                    document.Load(xmlfile);
                    XPathNavigator navigator2 = document.CreateNavigator();
                    XPathNodeIterator nodes2 = navigator2.Select("/BPMManufacturingData/Header/Part/StandardRawMaterial");
                    //
                    string line;
                    while (nodes2.MoveNext())
                    {
                        line = nodes2.Current.GetAttribute("Name", "");
                        if (line.Contains("STT17"))
                        {
                            MessageBox.Show("UWAGA AUSTENIT !!!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("check function checkBladeMaterial", e);
                }
            }
        }

        internal static bool ReplaceExistOrder(string order, bool error)
        {
            if (order != "")
            {
                if (error == false)
                {
                    if (MessageBox.Show("ORDER ZOSTANIE NADPISANY, CZY NA PEWNO KONTYNUOWAC?", "ZAPYTANIE", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
