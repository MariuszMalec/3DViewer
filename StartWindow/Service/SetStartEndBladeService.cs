using System;
using System.Collections.Generic;

namespace StartWindow.Service
{
    public static class SetStartEndBladeService
    {
        public static string SetTextBox(string catPartFile, string typefile)
        {
            try
            {
                //wypelnij poczatkowe i koncowe
                //wypelnij automatycznie catpart poczatkowej textBox8
                int count = 0;
                List<string> listfiltertext3 = new List<string>(new string[] { });
                string[] filtertext3 = catPartFile.Split(new char[] { '\\', '.' });
                string filterxmltext3 = "";
                int substringslength = filtertext3.Length;
                //MessageBox.Show(substringslength.ToString());
                string modifystartpartname = "";
                foreach (string element in filtertext3)
                {
                    if (element != "CATPart")
                    {
                        //MessageBox.Show(element);
                        if (count != 0)
                        {
                            filterxmltext3 = filterxmltext3 + "\\";
                        }
                        if (count != substringslength - 2)
                        {
                            filterxmltext3 = String.Concat(filterxmltext3, element);
                            //MessageBox.Show(filterxmltext3);
                        }

                        if (count == substringslength - 2)
                        {
                            //zmien nazwe modelu dla lopatki poczatkowej 11XX_-.CATPart
                            modifystartpartname = element.Replace("_", "").Replace("-", "");
                            int nmbletter = modifystartpartname.Length;
                            string newmodifystartpartname = "";
                            for (int i = 0; i <= (nmbletter - 1); i++)
                            {
                                if (i != nmbletter - 3)//trzeci znak od konca podmienic na 1 dla poczatkowej
                                {
                                    //MessageBox.Show(modifystartpartname[i].ToString());
                                    newmodifystartpartname = String.Concat(newmodifystartpartname, modifystartpartname[i].ToString());
                                }
                                else
                                {
                                    if (typefile == "PARTSTART")
                                    {
                                        newmodifystartpartname = String.Concat(newmodifystartpartname, "1");
                                    }
                                    else if (typefile == "XMLSTART")
                                    {
                                        newmodifystartpartname = String.Concat(newmodifystartpartname, "1");
                                    }
                                    else if (typefile == "PARTEND")
                                    {
                                        newmodifystartpartname = String.Concat(newmodifystartpartname, "2");
                                    }
                                    else if (typefile == "XMLEND")
                                    {
                                        newmodifystartpartname = String.Concat(newmodifystartpartname, "2");
                                    }
                                    else
                                    {
                                        newmodifystartpartname = String.Concat(newmodifystartpartname, "ERROR");
                                    }
                                }
                            }

                            if (typefile == "PARTSTART" || typefile == "PARTEND")
                            {
                                newmodifystartpartname = String.Concat(newmodifystartpartname, "_-.CATPart");//dodanie stalej koncowki
                            }
                            else if (typefile == "XMLSTART")
                            {
                                newmodifystartpartname = String.Concat(newmodifystartpartname, "_-_SB_BMD.xml");//dodanie stalej koncowki
                            }
                            else if (typefile == "XMLEND")
                            {
                                newmodifystartpartname = String.Concat(newmodifystartpartname, "_-_EB_BMD.xml");//dodanie stalej koncowki
                            }
                            else
                            {

                            }

                            //nowy filterxmltext3
                            string dircatpart = System.IO.Path.GetDirectoryName(catPartFile);
                            filterxmltext3 = dircatpart + "\\";

                            filterxmltext3 = String.Concat(filterxmltext3, newmodifystartpartname);
                            //MessageBox.Show(filterxmltext3);
                        }

                    }
                    count += 1;
                }
                if (typefile == "PARTSTART")
                {
                    return filterxmltext3;
                }
                else if (typefile == "XMLSTART")
                {
                    return filterxmltext3;
                }
                else if (typefile == "PARTEND")
                {
                    return filterxmltext3;
                }
                else if (typefile == "XMLEND")
                {
                    return filterxmltext3;
                }
                else
                {
                    return "WYBIERZ RECZNIE!";
                }
            }
            catch (Exception e)
            {
                throw new Exception("check function SetTextBox in SetStartEndBladeService", e);
            }
        }
    }
}
