using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.XPath;

namespace StartWindow.Service
{
    public static class ValidateService
    {
        public static bool CheckAutomatedProcess(string machine, string bpmtype)
        {
            if (machine == "HM_HSTM_300HD_SIM840D" && !bpmtype.Contains("RTB"))
            {
                MessageBox.Show("Dla tej maszyny automat jeszcze nie dziala", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        public static bool BmdXml(string xmlFile, string bpmtype)
        {
            if (!xmlFile.Contains(".xml"))
            {
                MessageBox.Show("Bledny xml , wybierz go ponownie ", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            if (xmlFile.Contains(".xml") && bpmtype == "RTBFixedBlade" && File.Exists(xmlFile))//check xml file
            {
                return checkxmlfile(xmlFile);
            }
            if (xmlFile.Contains(".xml") && bpmtype == "RTBMovingBlade" && File.Exists(xmlFile))//check xml file
            {
                return checkxmlfile(xmlFile);
            }
            return false;
        }

        private static bool checkxmlfile(string xmlfile)//sprawdza czy nie wybrano XMLa dla pocztkowej i koncowej
        {
            try
            {
                //create list	
                XmlDocument document = new XmlDocument();
                document.Load(xmlfile);
                XPathNavigator navigator2 = document.CreateNavigator();
                XPathNodeIterator nodes2 = navigator2.Select("/BPMManufacturingData/Header/Part");
                //
                string line;
                while (nodes2.MoveNext())
                {
                    line = nodes2.Current.GetAttribute("Name", "");
                    if (line.StartsWith("A"))
                    {
                        MessageBox.Show("UWAGA  !!! BLEDNY XML ", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return true;
                    }
                    if (line.StartsWith("E"))
                    {
                        MessageBox.Show("UWAGA  !!! BLEDNY XML ", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("check function checkxmlfile", e);
            }
        }

        public static bool Xls(string xlsFile, bool? checkBox)
        {
            if (!xlsFile.Contains(".xls") && checkBox == false)
            {
                MessageBox.Show("Bledny xls , wybierz go ponownie ", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        public static bool CheckRadialBlade(string bpmtype, bool? runcmm)
        {
            if (bpmtype == "RTBRadialFixedBlade" && runcmm == true)
            {
                MessageBox.Show("Pomiaru osiowej nie wykonujemy", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            return false;
        }

        public static bool CantCalculateMillAndCmm(bool runBm, bool runCmm)
        {
            if (runBm == true && runCmm == true)
            {
                MessageBox.Show("Nie mozna jednoczesnie liczyc obrobki i pomiaru!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                Serilog.Log.Error("Nie mozna jednoczesnie liczyc obrobki i pomiaru!");
                return true;
            }
            return false;
        }

        internal static bool MachninesNotSupported(string machine)
        {
            if (machine == "DMU60P_HEIDENHAIN" || machine == "CHIRON_FZ" || machine == "SH_NX155_OSAI_8600" || machine == "FADAL")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        internal static bool TypeOfBladeNotSupported(string typeBlade, string machine)
        {
            if (typeBlade == "ITBMovingBlade" && machine == "SH_HX151_24_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna dla tego typu lopatki nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (typeBlade == "CDMovingBlade" && machine == "SH_HX151_24_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna dla tego typu lopatki nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (typeBlade == "CDMovingBlade" && machine == "HURON_EX20_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna dla tego typu lopatki nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (typeBlade == "ITBFixedPlatformBlade" && machine == "SH_HX151_24_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna dla tego typu lopatki nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (typeBlade == "CDFixedPlatformBlade" && machine == "SH_HX151_24_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna dla tego typu lopatki nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (typeBlade == "CDFixedPlatformBlade" && machine == "HURON_EX20_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna dla tego typu lopatki nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (typeBlade == "CDFixedPlatformBlade" && machine == "SH_HX151_24_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna dla tego typu lopatki nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else { }
            return false;
        }

        internal static bool NotPassClampingSystem(string machine, string clampingmethod, string clampingFromTemplate, bool useBmTemplate)
        {
            if (useBmTemplate == true)
            {
                if (clampingmethod != clampingFromTemplate.Replace("&", ""))
                {
                    if (MessageBox.Show("Mocowanie w templacie jest inne, CZY NA PEWNO KONTYNUOWAC?", "ZAPYTANIE", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        return true;
                    }
                }
                if (clampingFromTemplate == "Mocowanie z templata")
                {
                    MessageBox.Show("Wybierz template, aby kontynuowac!", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
            return false;
        }

        internal static bool SelectCorrectClamping(string clampingmethod, string typeBlade, bool isSelectedNoXlsCommand, bool isSelectedPreRawBoxCommand)
        {
            if (typeBlade == "ITBMovingBlade" || typeBlade == "ITBFixedPlatformBlade" || typeBlade == "CDFixedPlatformBlade")
            {
                if (isSelectedNoXlsCommand == true)
                {
                    if (typeBlade == "Wybierz mocowanie")
                    {
                        MessageBox.Show("Wybierz mocowanie!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return true;
                    }
                    else
                    {
                        if (!typeBlade.Contains("RTB") && clampingmethod == "GripGrip")
                        {
                            MessageBox.Show("This clamping not supported yet!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                            return true;
                        }
                        if (!typeBlade.Contains("RTB") && clampingmethod == "GripZabierak")
                        {
                            MessageBox.Show("This clamping not supported yet!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                            return true;
                        }
                        if (typeBlade == "CDFixedPlatformBlade" && clampingmethod == "GripPin" && isSelectedNoXlsCommand == true)
                        {
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            return true;
                        }
                        if (typeBlade == "CDFixedPlatformBlade" && clampingmethod == "GripPinWelding" && isSelectedNoXlsCommand == true)
                        {
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            return true;
                        }
                        if (typeBlade == "CDFixedPlatformBlade" && clampingmethod == "DovetailPinCenterBox" && isSelectedNoXlsCommand == true)
                        {
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            return true;
                        }
                        if (typeBlade == "CDFixedPlatformBlade" && clampingmethod == "GripTang" && isSelectedNoXlsCommand == true)
                        {
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            return true;
                        }
                        if (typeBlade == "ITBMovingBlade" && clampingmethod == "DovetailPinCenterBox" && isSelectedNoXlsCommand == true)
                        {
                            return true;
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        if (typeBlade == "ITBMovingBlade" && clampingmethod == "GripTang" && isSelectedNoXlsCommand == true)
                        {
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding, DovetailPin, GripPin or GripPinWelding", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            return true;               
                        }

                        if (typeBlade == "ITBFixedPlatformBlade" && clampingmethod == "GripPin" && isSelectedNoXlsCommand == true)
                        {
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            return true;
                        }
                        if (typeBlade == "ITBFixedPlatformBlade" && clampingmethod == "GripPinWelding" && isSelectedNoXlsCommand == true)
                        {
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            return true;
                        }
                        if (typeBlade == "ITBFixedPlatformBlade" && clampingmethod == "DovetailPinCenterBox" && isSelectedNoXlsCommand == true)
                        {
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            return true;
                        }
                        if (typeBlade == "ITBFixedPlatformBlade" && clampingmethod == "GripTang" && isSelectedNoXlsCommand == true)
                        {
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            return true;
                        }
                    }
                }
                else
                {
                    if (typeBlade == "CDFixedPlatformBlade" && isSelectedPreRawBoxCommand == false)
                    {
                        MessageBox.Show("Wybierz przygotowke (only ITB fix and CD fix)!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    if (typeBlade == "CDFixedPlatformBlade" && isSelectedPreRawBoxCommand == true)
                    {
                        MessageBox.Show("Wybierz plik excel z technologia (tylko do zapisu danych do technologi) i nastepnie wybierz BRAK XLSa!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;       
                    }
                    if (typeBlade == "ITBFixedPlatformBlade" && isSelectedPreRawBoxCommand == false)
                    {
                        MessageBox.Show("Wybierz przygotowke (only ITB fix and CD fix)!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;                       
                    }
                    if (typeBlade == "ITBFixedPlatformBlade" && isSelectedPreRawBoxCommand == true)
                    {
                        MessageBox.Show("Wybierz plik excel z technologia (tylko do zapisu danych do technologi) i nastepnie wybierz BRAK XLSa!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool CheckIfNotExistFile(string file)
        {
            if (!File.Exists(file))
            {
                MessageBox.Show($"File {file} doesn't exist!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        internal static bool SelectCorrectCatPart(string catPart, string xlsPart, string typeOfBlade, string firstPart, string endPart, string firstXml, string endXml)
        {
            if (!File.Exists(catPart))
            {
                return true;
            }
            if (File.Exists(catPart))
            {
                int count = 0;
                List<string> listfiltertext = new List<string>(new string[] { });
                string[] filtertext = catPart.Split(new char[] { '\\', '.' });
                string nazwalopatki = "";
                int substringslength = filtertext.Length;
                //MessageBox.Show(substringslength.ToString());
                string modifystartpartname = "";
                foreach (string element in filtertext)
                {
                    if (element != "CATPart")
                    {
                        //MessageBox.Show(element);
                        if (count == substringslength - 2)
                        {
                            nazwalopatki = element;
                        }
                    }
                    count += 1;
                }
                modifystartpartname = nazwalopatki.Replace("_", "").Replace("-", "");
                //MessageBox.Show(modifystartpartname);
                if (!catPart.Contains(modifystartpartname) || !catPart.Contains(modifystartpartname) || !xlsPart.Contains(modifystartpartname))
                {
                    if (MessageBox.Show("Sprawdz wybor stopnia w okienkach, nie pasuja nazwy, CZY NA PEWNO KONTYNUOWAC?", "ZAPYTANIE", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        return true;
                    }
                }

                //sprawdzenie czy nie wybrano nie tego samego rzedu w okienkach dla lopatki zamkowej
                if (typeOfBlade == "RTBFixedBlade")
                {
                    string stopien = "";
                    string rysunekbezpozycji = "";
                    string rysunek = System.IO.Path.GetFileName(catPart).Replace("_-.CATPart", "");
                    int dlugosrysunku = rysunek.Length;
                    stopien = rysunek.Remove(0, dlugosrysunku - 2);
                    rysunekbezpozycji = rysunek.Remove(dlugosrysunku - 3, 3);
                    string rysunekpoczatkowej = rysunekbezpozycji + "1" + stopien;
                    //MessageBox.Show(rysunekpoczatkowej);
                    if (!firstPart.Contains(rysunekpoczatkowej) || !firstXml.Contains(rysunekpoczatkowej))
                    {
                        MessageBox.Show("Sprawdz wybor stopnia dla lopatki poczatkowej w okienkach", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return true;
                    }
                    string rysunekkoncowej = rysunekbezpozycji + "2" + stopien;
                    //MessageBox.Show(rysunekkoncowej);
                    if (!endPart.Contains(rysunekkoncowej) || !endXml.Contains(rysunekkoncowej))
                    {                        
                        MessageBox.Show("Sprawdz wybor stopnia dla lopatki koncowej w okienkach", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool CheckCorrectOrderName(string order, string machine, bool runbm, bool runcmm)
        {
            if (runbm == true && runcmm == false)
            {
                if (machine == "HM_HSTM_500M_SIM840D")
                {
                    if (!order.StartsWith("C"))
                    {
                        MessageBox.Show("INFO! brak przedrostka C w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    if (order.Length > 7 || order.Length < 7)
                    {
                        MessageBox.Show("INFO! nieprawidlowa ilość znaków w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
                else if (machine == "HM_HSTM_300_SIM840D")
                {
                    if (!order.StartsWith("A") && !order.StartsWith("D"))
                    {
                        MessageBox.Show("INFO! brak przedrostka A lub D w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    if (order.Length > 7 || order.Length < 7)
                    {
                        MessageBox.Show("INFO! nieprawidlowa ilość znaków w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
                if (machine == "HM_HSTM_500_SIM840D")
                {
                    if (!order.StartsWith("B"))
                    {
                        MessageBox.Show("INFO! brak przedrostka B w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    if (order.Length > 7 || order.Length < 7)
                    {
                        MessageBox.Show("INFO! nieprawidlowa ilość znaków w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
                else if (machine == "HM_HSTM_300HD_SIM840D")
                {
                    if (!order.StartsWith("D"))
                    {
                        MessageBox.Show("INFO! brak przedrostka D w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    if (order.Length > 7 || order.Length < 7)
                    {
                        MessageBox.Show("INFO! nieprawidlowa ilość znaków w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
                if (machine == "HURON_EX20_SIM840D")
                {
                    if (order.Length > 5 || order.Length < 5)
                    {
                        MessageBox.Show("UWAGA! bledna ilosc znaków w nazwie ordera, musi być 5 znaków", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return true;
                    }
                }
                if (machine == "SH_HX151_24_SIM840D")
                {
                    if (order.Length > 5 || order.Length < 5)
                    {
                        MessageBox.Show("UWAGA! bledna ilosc znaków w nazwie ordera, musi być 5 znaków", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
