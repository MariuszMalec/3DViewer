using StartWindow.Enums;
using StartWindow.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace StartWindow.Service
{
    public static class ExcelService
    {
        private static List<TechnologyXlsFile> _allCnc;
        public static void KillExcel()
        {
            var excelService = new BladeMillWithExcel.Logic.Services.ExcelService();
            excelService.KillSoftware("Excel", true);
        }

        public static List<TechnologyXlsFile> GetAll(string excelFile, string bpmtype)
        {
            if (File.Exists(excelFile) && !excelFile.Contains("KDT") && !excelFile.Contains("P.XLS") && !excelFile.Contains("P.xls"))
            {
                bool pass = true;
                _allCnc = new List<TechnologyXlsFile>();
                string POCConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFile + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";";
                OleDbConnection POCcon = new OleDbConnection(POCConnection);
                OleDbCommand DANEcommand = new OleDbCommand();
                System.Data.DataTable CNCdt = new System.Data.DataTable();
                //czytanie z zakladki CNC
                OleDbDataAdapter CNCCommand = new OleDbDataAdapter("select * from [CNC$] ", POCcon);
                CNCCommand.Fill(CNCdt);
                for (int i = 3; i < 50; i++)
                {
                    try
                    {
                        string name = CNCdt.Rows[i][0].ToString();
                        string value = CNCdt.Rows[i][1].ToString();
                        _allCnc.Add(new TechnologyXlsFile(i, name, value, pass));
                    }
                    catch {}
                }
                //czytanie z zakladki DANE
                System.Data.DataTable DANEdt = new System.Data.DataTable();
                OleDbDataAdapter DANECommand = new OleDbDataAdapter("select * from [DANE$] ", POCcon);
                DANECommand.Fill(DANEdt);
                string materialzexcela = "brak";
                if (DANEdt.Rows[29][1].ToString().Contains("Gatunek materiału:") && bpmtype == "RTBFixedBlade")
                {
                    materialzexcela = DANEdt.Rows[29][2].ToString();
                    if (materialzexcela == "A")
                        pass = false;
                    _allCnc.Add(new TechnologyXlsFile(1, "Material", materialzexcela, pass));
                }
                if (DANEdt.Rows[30][1].ToString().Contains("Gatunek materiału:") && bpmtype == "RTBMovingBlade")
                {
                    materialzexcela = DANEdt.Rows[30][2].ToString();
                    if (materialzexcela == "A")
                        pass = false;
                    _allCnc.Add(new TechnologyXlsFile(1, "Material", materialzexcela, pass));
                }
                if (DANEdt.Rows[26][1].ToString().Contains("Gatunek materiału:") && bpmtype == "RTBRadialFixedBlade")
                {
                    materialzexcela = DANEdt.Rows[26][2].ToString();
                    if (materialzexcela == "A")
                        pass = false;
                    _allCnc.Add(new TechnologyXlsFile(1, "Material", materialzexcela, pass));
                }
                if (DANEdt.Rows[31][1].ToString().Contains("Gatunek materiału:") && bpmtype == "ITBMovingBlade")
                {
                    materialzexcela = DANEdt.Rows[31][2].ToString();
                    if (materialzexcela == "A")
                        pass = false;
                    _allCnc.Add(new TechnologyXlsFile(1, "Material", materialzexcela, pass));
                }
                pass = true;
                string typlopatki = "brak";
                if (DANEdt.Rows[26][1].ToString().Contains("Typ:"))
                {
                    typlopatki = DANEdt.Rows[26][2].ToString();
                    _allCnc.Add(new TechnologyXlsFile(1, "TYP LOP", typlopatki, pass));
                }
                if (DANEdt.Rows[23][1].ToString().Contains("Typ:") && bpmtype == "RTBRadialFixedBlade")
                {
                    typlopatki = DANEdt.Rows[23][2].ToString();
                    _allCnc.Add(new TechnologyXlsFile(1, "TYP LOP", typlopatki, pass));
                }
                KillExcel();
                return _allCnc;
            }
            return default(List<TechnologyXlsFile>);
        }

        public static TechnologyXlsFile GetByName(string name)
        {
            return _allCnc.ToList().Where(p => p.Name == name).Select(p => p).FirstOrDefault();
        }

        public static string? GetCurrentClamping(List<TechnologyXlsFile> xlsItems)
        {
            var clamping = xlsItems.Where(x => x.Name == "Moc_band").Select(x => x.Value).FirstOrDefault();
            if (clamping == null)
                return EnumClamping.GripZabierak.ToString();
            if (clamping == "ZABIERAK")
                return EnumClamping.GripZabierak.ToString();
            if (clamping == "Grip")
                return EnumClamping.GripGrip.ToString();
            return clamping;
        }
    }
}