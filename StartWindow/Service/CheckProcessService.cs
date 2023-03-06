using BladeMill.BLL.Models;
using System.Diagnostics;
using System.Windows;

namespace StartWindow.Service
{
    public static class CheckProcessService
    {
        public static bool IsBladeMill()
        {
            System.Diagnostics.Process[] process2 = System.Diagnostics.Process.GetProcessesByName("Alstom.BladeMill.Gui");
            foreach (System.Diagnostics.Process p in process2)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    MessageBox.Show("ZAMKNIJ BLADEMILLa !!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
            return false;
        }
        public static bool IsCatia()
        {
            Process[] pname = Process.GetProcessesByName("CNEXT");
            if (pname.Length == 0)
            {
                MessageBox.Show("Prosze uruchomic CATIE!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }
        public static bool IsBMTemplate(bool? checkBox, string bmTemplate)
        {
            if (checkBox == true)
            {
                string varpoolztemplata = bmTemplate.Replace(".cbm", "_varpool.xml");
                var varpoolModel = new VarpoolXmlFile(varpoolztemplata);
                var BMTemplate = varpoolModel.BMTemplate;
                if (BMTemplate.Contains("True"))
                {
                    MessageBox.Show("UWAGA! Uzyles nieprawidlowego ordera jako template", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
                return false;
            }
            return false;
        }
        public static bool IsMiddleTol(bool? checkBox, string user, string bpmtype)
        {
            if (checkBox == true)
            {
                if (!user.ToString().Contains("Mariusz Malec"))
                {
                    MessageBox.Show("Program na środek tolerancji jeszcze nie działa", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
                else
                {
                    if (!bpmtype.Contains("RTBMovingBlade") && !bpmtype.Contains("RTBFixedBlade"))
                    {
                        MessageBox.Show("Ten typ łopatki na środek tolerancji jeszcze nie dziala", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool IsOrderThSameAsBmTemplate(bool? checkBox, string order, string bmtemplatefile)
        {
            if (order + ".cbm" == System.IO.Path.GetFileName(bmtemplatefile) && checkBox == true)
            {
                MessageBox.Show("Order nie moze byc taki sam jak BladeMill template!", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }
    }
}
