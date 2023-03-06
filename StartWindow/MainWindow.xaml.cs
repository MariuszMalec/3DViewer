using BladeMill.BLL.Services;
using Microsoft.Win32;
using StartWindow.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.OleDb;
using DataTable = System.Data.DataTable;
using System.Collections.Generic;

namespace StartWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string outfile = (@"C:\temp\inputdata.xml");
        UserServiceWithoutDatabase _userService = new UserServiceWithoutDatabase();
        AppXmlConfService _appXmlConfService = new AppXmlConfService();
        InputDataXmlService _inputDataXmlService = new InputDataXmlService();
        public MainData _mainData = new MainData();  
        XMLBmdService _xmlBmdService = new XMLBmdService();
        public static bool mistake = false; //error flag

        public MainWindow()
        {

            InitializeComponent();

            //SetSettings();
        }

        //private void SetSettings()
        //{
        //    //user.Text = data.Getuserprofiledir();
        //    username.Text = _userService.GetUserFirstLastName();

        //    rootengdir.Text = _appXmlConfService.GetRootEngDir();
        //    rootmfgdir.Text = _appXmlConfService.GetRootMfgDir();

        //    //wczytanie danych do okienek z inputdata
        //    SetStartDataFromInputDataFile();
        //}

        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DragMove();
        //    }
        //}

        //private void SetStartDataFromInputDataFile()
        //{
        //    if (File.Exists(outfile))
        //    {
        //        var inputData = _inputDataXmlService.GetDataFromInputDataXml();
        //        order.Text = inputData.infile;
        //        wybranamaszyna.Text = inputData.machine;
        //        machine.Text = inputData.machine;
        //        catpartfile.Text = inputData.catpart;
        //        xmlfile.Text = inputData.xmlpart;
        //        xlsfile.Text = inputData.xlspart;
        //        catpartfilefirstblade.Text = inputData.catpartfirst;
        //        catpartfileendblade.Text = inputData.catpartend;
        //        xmlfilefirstblade.Text = inputData.xmlpartfirst;
        //        xmlfileendblade.Text = inputData.xmlpartend;
        //        bpmtype.Text = inputData.TypeBlade;

        //        runconfiguration.IsChecked = bool.Parse(inputData.runconfiguration);
        //        runbm.IsChecked = bool.Parse(inputData.runbm);
        //        runcmm.IsChecked = bool.Parse(inputData.runcmm);

        //        createstls.IsChecked = bool.Parse(inputData.readxls);
        //        createprerawbox.IsChecked = bool.Parse(inputData.Prerawbox);
        //        raport.IsChecked = bool.Parse(inputData.createraport);
        //        usebmtemplate.IsChecked = bool.Parse(inputData.BMTemplate);
        //        noxls.IsChecked = bool.Parse(inputData.readxls);
        //        middletol.IsChecked = bool.Parse(inputData.middleTol);

        //        wybranemocowanie.Text = inputData.Clampingmethod;
        //        Mocowanieztemplata.Text = inputData.ClampFromTemplate;
        //        pinweling.IsChecked = bool.Parse(inputData.pinwelding);
        //        tb_fig_n.Text = inputData.FIG_N;
        //        clamping.Text = inputData.Clampingmethod;

        //        ShowHideSettings();// chowanie i pokazywanie okienek

        //        if (File.Exists(xmlfile.Text))
        //        {
        //            ShowListViewFromBMDxmlfile(xmlfile.Text);
        //        }
        //    }
        //}

        //private void ShowHideSettings()
        //{
        //    bool newVal1 = (usebmtemplate.IsChecked == false);
        //    if (newVal1)
        //    {
        //        bmtemplatefile.Visibility = Visibility.Hidden;
        //        Button_BMTemplate.Visibility = Visibility.Hidden;
        //        //firstxml.Visibility = Visibility.Hidden;
        //        //secondxml.Visibility = Visibility.Hidden;
        //        Mocowanieztemplata.Visibility = Visibility.Hidden;
        //    }
        //    else
        //    {
        //        //firstxml.Visibility = Visibility.Visible;
        //        //secondxml.Visibility = Visibility.Visible;
        //        Mocowanieztemplata.Visibility = Visibility.Visible;
        //    }
        //    //
        //    bool newVal2 = (noxls.IsChecked == true);
        //    if (newVal2)
        //    {
        //        noxls.IsChecked = false;
        //        wybranemocowanie.Visibility = Visibility.Hidden;
        //        millshroud.Visibility = Visibility.Hidden;
        //        pinweling.Visibility = Visibility.Hidden;
        //        wybierzxls.Visibility = Visibility.Visible;
        //        xlsfile.Visibility = Visibility.Visible;
        //        fig_n.Visibility = Visibility.Hidden;
        //    }
        //    else
        //    {
        //        noxls.IsChecked = true;
        //        //clamping.Visibility = Visibility.Visible;
        //        wybranemocowanie.Visibility = Visibility.Visible;
        //        millshroud.Visibility = Visibility.Visible;
        //        pinweling.Visibility = Visibility.Visible;
        //        wybierzxls.Visibility = Visibility.Hidden;
        //        xlsfile.Visibility = Visibility.Hidden;
        //        fig_n.Visibility = Visibility.Visible;
        //    }
        //    if (bpmtype.Text != "RTBFixedBlade")//schowaj poczatkowe koncowe okienka
        //    {
        //        catpartfilefirstblade.Visibility = Visibility.Hidden;
        //        xmlfilefirstblade.Visibility = Visibility.Hidden;
        //        catpartfileendblade.Visibility = Visibility.Hidden;
        //        xmlfileendblade.Visibility = Visibility.Hidden;
        //        wybierzpartsb.Visibility = Visibility.Hidden;
        //        wybierzxmlsb.Visibility = Visibility.Hidden;
        //        wybierzparteb.Visibility = Visibility.Hidden;
        //        wybierzxmleb.Visibility = Visibility.Hidden;
        //    }
        //    if (bpmtype.Text == "RTBFixedBlade")//pokaz poczatkowe koncowe okienka
        //    {
        //        catpartfilefirstblade.Visibility = Visibility.Visible;
        //        xmlfilefirstblade.Visibility = Visibility.Visible;
        //        catpartfileendblade.Visibility = Visibility.Visible;
        //        xmlfileendblade.Visibility = Visibility.Visible;
        //        wybierzpartsb.Visibility = Visibility.Visible;
        //        wybierzxmlsb.Visibility = Visibility.Visible;
        //        wybierzparteb.Visibility = Visibility.Visible;
        //        wybierzxmleb.Visibility = Visibility.Visible;
        //    }
        //    if (bpmtype.Text != "RTBFixedBlade" && bpmtype.Text != "RTBMovingBlade")//schowaj przycisk template
        //    {
        //        usebmtemplate.IsChecked = false;
        //    }
        //}

        //private void WybierzCatPart_Click(object sender, RoutedEventArgs e)
        //{
        //    pokazdanezbmdfile.Items.Clear();
        //    pokazdanezexcela.Items.Clear();

        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Text files (*.CATPart)|*.CATPart|All files (*.*)|*.*";
        //    openFileDialog.InitialDirectory = rootengdir.Text;
        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        Mouse.OverrideCursor = Cursors.Wait;

        //        _mainData.CatPart = openFileDialog.FileName;
        //        catpartfile.Text = _mainData.CatPart;

        //        // wypelnienie okienek na podstawie wybranego catparta
        //        string dircatpart = Path.GetDirectoryName(catpartfile.Text);
        //        string xmlname = Path.GetFileName(catpartfile.Text).Replace("_-.CATPart", "_-_BMD.xml");
        //        xmlfile.Text = Path.Combine(dircatpart, xmlname);
        //        _mainData.BmdFile = xmlfile.Text;

        //        string xlsname = Path.GetFileName(catpartfile.Text).Replace("_-.CATPart", ".xls");
        //        xlsfile.Text = Path.Combine(dircatpart, xlsname);
        //        _mainData.XlsFile = xlsfile.Text;

        //        SetBpmType();

        //        if (bpmtype.Text.Contains("ITB"))//schowaj przycisk template
        //        {
        //            usebmtemplate.IsChecked = false;
        //        }

        //        if (bpmtype.Text == "RTBFixedBlade" && bpmtype.Text != "unknown")//wypelnij automatycznie catparty i xmle poczatkowej i koncowej
        //        {
        //            ReplaceStartEndFileName("PARTSTART");
        //            ReplaceStartEndFileName("PARTEND");
        //            ReplaceStartEndFileName("XMLSTART");
        //            ReplaceStartEndFileName("XMLEND");
        //        }

        //        //-------------------------------------------------------------------
        //        //pokazanie danych z xmla
        //        //-------------------------------------------------------------------
        //        if (File.Exists(xmlfile.Text))
        //        {
        //            ShowListViewFromBMDxmlfile(xmlfile.Text);
        //        }

        //        //-------------------------------------------------------------------
        //        //pokazanie danych z excela
        //        //-------------------------------------------------------------------
        //        if ((noxls.IsChecked == false && File.Exists(xlsfile.Text)
        //            && bpmtype.Text == "RTBRadialFixedBlade")
        //            || (noxls.IsChecked == false && File.Exists(xlsfile.Text)
        //            && bpmtype.Text == "RTBFixedBlade")
        //            || (noxls.IsChecked == false && File.Exists(xlsfile.Text)
        //            && bpmtype.Text == "RTBMovingBlade")
        //            || (noxls.IsChecked == false && File.Exists(xlsfile.Text)
        //            && bpmtype.Text == "ITBMovingBlade"))
        //        {
        //            ReadDataFromExcel(xlsfile.Text);
        //        }
        //        else if (noxls.IsChecked == false)
        //        {
        //            MessageBox.Show("Brak pliku excel " + xlsfile.Text, "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }

        //    }
        //    Mouse.OverrideCursor = Cursors.Arrow;
        //}

        //void ReplaceStartEndFileName(string typefile)
        //{
        //    try
        //    {
        //        int count = 0;
        //        List<string> listfiltertext3 = new List<string>(new string[] { });
        //        string[] filtertext3 = catpartfile.Text.Split(new char[] { '\\', '.' });
        //        string filterxmltext3 = "";
        //        int substringslength = filtertext3.Length;
        //        //MessageBox.Show(substringslength.ToString());
        //        string modifystartpartname = "";
        //        foreach (string element in filtertext3)
        //        {
        //            if (element != "CATPart")
        //            {
        //                //MessageBox.Show(element);
        //                if (count != 0)
        //                {
        //                    filterxmltext3 = filterxmltext3 + "\\";
        //                }
        //                if (count != substringslength - 2)
        //                {
        //                    filterxmltext3 = String.Concat(filterxmltext3, element);
        //                    //MessageBox.Show(filterxmltext3);
        //                }

        //                if (count == substringslength - 2)
        //                {
        //                    //zmien nazwe modelu dla lopatki poczatkowej 11XX_-.CATPart
        //                    modifystartpartname = element.Replace("_", "").Replace("-", "");
        //                    int nmbletter = modifystartpartname.Length;
        //                    string newmodifystartpartname = "";
        //                    for (int i = 0; i <= (nmbletter - 1); i++)
        //                    {
        //                        if (i != nmbletter - 3)//trzeci znak od konca podmienic na 1 dla poczatkowej
        //                        {
        //                            //MessageBox.Show(modifystartpartname[i].ToString());
        //                            newmodifystartpartname = String.Concat(newmodifystartpartname, modifystartpartname[i].ToString());
        //                        }
        //                        else
        //                        {
        //                            if (typefile == "PARTSTART")
        //                            {
        //                                newmodifystartpartname = String.Concat(newmodifystartpartname, "1");
        //                            }
        //                            else if (typefile == "XMLSTART")
        //                            {
        //                                newmodifystartpartname = String.Concat(newmodifystartpartname, "1");
        //                            }
        //                            else if (typefile == "PARTEND")
        //                            {
        //                                newmodifystartpartname = String.Concat(newmodifystartpartname, "2");
        //                            }
        //                            else if (typefile == "XMLEND")
        //                            {
        //                                newmodifystartpartname = String.Concat(newmodifystartpartname, "2");
        //                            }
        //                            else
        //                            {
        //                                newmodifystartpartname = String.Concat(newmodifystartpartname, "ERROR");
        //                            }
        //                        }
        //                    }

        //                    if (typefile == "PARTSTART" || typefile == "PARTEND")
        //                    {
        //                        newmodifystartpartname = String.Concat(newmodifystartpartname, "_-.CATPart");//dodanie stalej koncowki
        //                    }
        //                    else if (typefile == "XMLSTART")
        //                    {
        //                        newmodifystartpartname = String.Concat(newmodifystartpartname, "_-_SB_BMD.xml");//dodanie stalej koncowki
        //                    }
        //                    else if (typefile == "XMLEND")
        //                    {
        //                        newmodifystartpartname = String.Concat(newmodifystartpartname, "_-_EB_BMD.xml");//dodanie stalej koncowki
        //                    }
        //                    else
        //                    {

        //                    }

        //                    //nowy filterxmltext3
        //                    string dircatpart = System.IO.Path.GetDirectoryName(catpartfile.Text);
        //                    filterxmltext3 = dircatpart + "\\";

        //                    filterxmltext3 = String.Concat(filterxmltext3, newmodifystartpartname);
        //                    //MessageBox.Show(filterxmltext3);
        //                }

        //            }
        //            count += 1;
        //        }
        //        if (typefile == "PARTSTART")
        //        {
        //            catpartfilefirstblade.Text = filterxmltext3;
        //        }
        //        else if (typefile == "XMLSTART")
        //        {
        //            xmlfilefirstblade.Text = filterxmltext3;
        //        }
        //        else if (typefile == "PARTEND")
        //        {
        //            catpartfileendblade.Text = filterxmltext3;
        //        }
        //        else if (typefile == "XMLEND")
        //        {
        //            xmlfileendblade.Text = filterxmltext3;
        //        }
        //        else
        //        {
        //            catpartfilefirstblade.Text = "WYBIERZ RECZNIE!";
        //            xmlfilefirstblade.Text = "WYBIERZ RECZNIE!";
        //            catpartfileendblade.Text = "WYBIERZ RECZNIE!";
        //            xmlfileendblade.Text = "WYBIERZ RECZNIE!";
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("check function replacestartendfilename", e);
        //    }
        //}

        //private void Button_xml_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Text files (*.xml)|*.xml|All files (*.*)|*.*";
        //    openFileDialog.InitialDirectory = rootengdir.Text;
        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        Mouse.OverrideCursor = Cursors.Wait;

        //        _mainData.BmdFile = openFileDialog.FileName;
        //        xmlfile.Text = _mainData.BmdFile;

        //        SetBpmType();

        //        SetVisibility(bpmtype.Text);

        //        if (File.Exists(xmlfile.Text))//pokazanie danych z bmd xml file
        //        {
        //            ShowListViewFromBMDxmlfile(xmlfile.Text);
        //        }

        //        Mouse.OverrideCursor = Cursors.Arrow;
        //    }
        //}

        //private void SetBpmType()
        //{
        //    //sprawdz bpmtype
        //    if (xmlfile.Text.Contains(".xml"))
        //    {
        //        _mainData.BpmType = _xmlBmdService.GetBmdType(_mainData.BmdFile);
        //    }
        //    else
        //    {
        //        _mainData.BpmType = "unknown";
        //    }
        //    bpmtype.Text = _mainData.BpmType;
        //}

        //private void ShowListViewFromBMDxmlfile(string plikbmdxml)
        //{
        //    pokazdanezbmdfile.Items.Clear();
        //    pokazdanezbmdfile.Items.Add("Typ lopatki " + "  | " + _xmlBmdService.GetTypeBlade(plikbmdxml));
        //    pokazdanezbmdfile.Items.Add("Rysunek     " + "  | " + _xmlBmdService.GetDrawing(plikbmdxml));
        //    pokazdanezbmdfile.Items.Add("Projekt       " + "  | " + _xmlBmdService.GetProject(plikbmdxml));
        //    pokazdanezbmdfile.Items.Add("Material     " + "  | " + _xmlBmdService.GetMaterial(plikbmdxml));
        //    pokazdanezbmdfile.Items.Add("Strumien    " + "  | " + _xmlBmdService.GetOrientation(plikbmdxml));
        //    pokazdanezbmdfile.Items.Add("Typ            " + "  | " + _xmlBmdService.GetBmdType(plikbmdxml));
        //    pokazdanezbmdfile.Items.Refresh();
        //}

        //private void SetVisibility(string bpmType)
        //{
        //    if (bpmType == "ITBMovingBlade" || bpmType == "RTBRadialFixedBlade" ||
        //        bpmType == "CDMovingBlade" || bpmType == "CDFixedPlatformBlade")
        //    {
        //        noxls.IsChecked = true;
        //    }
        //    else
        //    {
        //        noxls.IsChecked = false;
        //    }
        //    if (bpmtype.Text != "RTBFixedBlade")//schowaj poczatkowe koncowe okienka
        //    {
        //        catpartfilefirstblade.Visibility = Visibility.Hidden;
        //        xmlfilefirstblade.Visibility = Visibility.Hidden;
        //        catpartfileendblade.Visibility = Visibility.Hidden;
        //        xmlfileendblade.Visibility = Visibility.Hidden;
        //        wybierzpartsb.Visibility = Visibility.Hidden;
        //        wybierzxmlsb.Visibility = Visibility.Hidden;
        //        wybierzparteb.Visibility = Visibility.Hidden;
        //        wybierzxmleb.Visibility = Visibility.Hidden;
        //    }
        //    if (bpmtype.Text == "RTBFixedBlade")//schowaj poczatkowe koncowe okienka
        //    {
        //        catpartfilefirstblade.Visibility = Visibility.Visible;
        //        xmlfilefirstblade.Visibility = Visibility.Visible;
        //        catpartfileendblade.Visibility = Visibility.Visible;
        //        xmlfileendblade.Visibility = Visibility.Visible;
        //        wybierzpartsb.Visibility = Visibility.Visible;
        //        wybierzxmlsb.Visibility = Visibility.Visible;
        //        wybierzparteb.Visibility = Visibility.Visible;
        //        wybierzxmleb.Visibility = Visibility.Visible;
        //    }
        //}

        //private void wybierzxls_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Text files (*.xls)|*.xls|All files (*.*)|*.*";
        //    openFileDialog.InitialDirectory = rootengdir.Text;
        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        Mouse.OverrideCursor = Cursors.Wait;

        //        var excelService = new BladeMillWithExcel.Logic.Services.ExcelService();
        //        excelService.KillSoftware("Excel", true);

        //        _mainData.XlsFile = openFileDialog.FileName;
        //        xlsfile.Text = _mainData.XlsFile;
        //        if ((xlsfile.Text.Contains("CATPart")) || (xlsfile.Text.Contains("xml")) || (xlsfile.Text.Contains("KDT")))
        //        {
        //            MessageBox.Show("You selected wrong XLS file , select again !!", "", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        else
        //        {
        //            //---------------------------------------------------------
        //            //wyswietla wymiary kloca z Excela i pobiera mocowanie								    				
        //            //----------------------------------------------------------
        //            if (bpmtype.Text == "RTBFixedBlade" || bpmtype.Text == "RTBMovingBlade" || bpmtype.Text == "RTBRadialFixedBlade")
        //            {
        //                if (noxls.IsChecked == false)
        //                {
        //                    ReadDataFromExcel(xlsfile.Text);
        //                }
        //            }
        //        }
        //        Mouse.OverrideCursor = Cursors.Arrow;
        //    }
        //}


        //private void ReadDataFromExcel(string path)
        //{
        //    try
        //    {
        //        string openexcel = path;

        //        if (File.Exists(openexcel) && !openexcel.Contains("KDT") && !openexcel.Contains("P.XLS") && !openexcel.Contains("P.xls"))
        //        {
        //            //MessageBox.Show(openexcel);
        //            string POCpath = openexcel;
        //            string POCConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + POCpath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";";

        //            OleDbConnection POCcon = new OleDbConnection(POCConnection);
        //            //--------------------------------------------------------------
        //            //CZYTANIE KOMOREK Z EXCELA Z ZAKLADKI DANE
        //            //--------------------------------------------------------------
        //            OleDbCommand DANEcommand = new OleDbCommand();
        //            DataTable DANEdt = new DataTable();
        //            OleDbDataAdapter DANECommand = new OleDbDataAdapter("select * from [DANE$] ", POCcon);
        //            DANECommand.Fill(DANEdt);
        //            string materialzexcela = "brak";
        //            if (DANEdt.Rows[29][1].ToString().Contains("Gatunek materiału:") && bpmtype.Text == "RTBFixedBlade")
        //            {
        //                materialzexcela = DANEdt.Rows[29][2].ToString();
        //            }
        //            if (DANEdt.Rows[30][1].ToString().Contains("Gatunek materiału:") && bpmtype.Text == "RTBMovingBlade")
        //            {
        //                materialzexcela = DANEdt.Rows[30][2].ToString();
        //            }
        //            if (DANEdt.Rows[26][1].ToString().Contains("Gatunek materiału:") && bpmtype.Text == "RTBRadialFixedBlade")
        //            {
        //                materialzexcela = DANEdt.Rows[26][2].ToString();
        //            }
        //            if (DANEdt.Rows[31][1].ToString().Contains("Gatunek materiału:") && bpmtype.Text == "ITBMovingBlade")
        //            {
        //                materialzexcela = DANEdt.Rows[31][2].ToString();
        //            }
        //            string typlopatki = "brak";
        //            if (DANEdt.Rows[26][1].ToString().Contains("Typ:"))
        //            {
        //                typlopatki = DANEdt.Rows[26][2].ToString();
        //            }
        //            if (DANEdt.Rows[23][1].ToString().Contains("Typ:") && bpmtype.Text == "RTBRadialFixedBlade")
        //            {
        //                typlopatki = DANEdt.Rows[23][2].ToString();
        //            }
        //            //MessageBox.Show("typlopatki", "typlopatki z excela", MessageBoxButton.OK, MessageBoxImage.Information);
        //            //--------------------------------------------------------------
        //            //CZYTANIE KOMOREK Z EXCELA Z ZAKLADKI CNC
        //            //--------------------------------------------------------------
        //            OleDbCommand CNCcommand = new OleDbCommand();
        //            DataTable CNCdt = new DataTable();
        //            OleDbDataAdapter CNCCommand = new OleDbDataAdapter("select * from [CNC$] ", POCcon);
        //            CNCCommand.Fill(CNCdt);
        //            string Project = CNCdt.Rows[4][1].ToString();//TODO Project Name dodaj sprawdzanie polskich liter!

        //            //if (CheckPolishLetter(Project))
        //            //{
        //            //    //MessageBox.Show("UWAGA! usun polskie znaki: " + Project.ToString() + " z komorki projekt ", "", MessageBoxButton.OK, MessageBoxImage.Error);                        
        //            //}

        //            string Avalue = CNCdt.Rows[11][1].ToString();
        //            string Bvalue = CNCdt.Rows[12][1].ToString();
        //            string LJOvalue = "brak";
        //            string Lvalue = "brak";
        //            string Dvalue = "brak";
        //            string Cvalue = "brak";
        //            string Ltol_HEvalue = "brak";
        //            string Utol_HEvalue = "brak";

        //            if (bpmtype.Text == "RTBFixedBlade")
        //            {
        //                Ltol_HEvalue = CNCdt.Rows[42][1].ToString();
        //                Utol_HEvalue = CNCdt.Rows[43][1].ToString();
        //            }

        //            if (CNCdt.Rows[3][1].ToString().Contains("CDMovingBlade"))
        //            {
        //                Lvalue = CNCdt.Rows[13][1].ToString();
        //                Dvalue = CNCdt.Rows[15][1].ToString();
        //                Cvalue = CNCdt.Rows[16][1].ToString();
        //            }
        //            else
        //            {
        //                LJOvalue = CNCdt.Rows[15][1].ToString();
        //                Lvalue = CNCdt.Rows[20][1].ToString();
        //                Dvalue = CNCdt.Rows[21][1].ToString();
        //                Cvalue = CNCdt.Rows[22][1].ToString();
        //            }
        //            string KDKNo = CNCdt.Rows[6][1].ToString();
        //            //
        //            string Zgrz_PIN = "brak";
        //            string GRIP = "brak";
        //            string FNvalue = "brak";
        //            string Obr_band = "brak";
        //            string FIG_BAND = "brak";
        //            string FIG_N = "brak";
        //            tb_fig_n.Text = FIG_N;
        //            string Moc_band = "brak";
        //            if (!CNCdt.Rows[3][1].ToString().Contains("CDMovingBlade"))//z excela
        //            {
        //                if (bpmtype.Text == "ITBMovingBlade")
        //                {
        //                    Zgrz_PIN = CNCdt.Rows[37][1].ToString();
        //                    GRIP = CNCdt.Rows[36][1].ToString();
        //                    FNvalue = CNCdt.Rows[38][1].ToString();
        //                }
        //                else
        //                {
        //                    if (bpmtype.Text == "RTBMovingBlade")
        //                    {
        //                        Zgrz_PIN = CNCdt.Rows[37][1].ToString();
        //                        GRIP = CNCdt.Rows[38][1].ToString();
        //                        FIG_BAND = CNCdt.Rows[39][1].ToString();
        //                        FIG_N = "brak";
        //                        tb_fig_n.Text = FIG_N;
        //                        Obr_band = CNCdt.Rows[36][1].ToString();
        //                        Moc_band = CNCdt.Rows[40][1].ToString();
        //                    }
        //                    else if (bpmtype.Text == "RTBFixedBlade")
        //                    {
        //                        Zgrz_PIN = CNCdt.Rows[37][1].ToString();
        //                        GRIP = CNCdt.Rows[38][1].ToString();
        //                        FIG_BAND = CNCdt.Rows[39][1].ToString();
        //                        FIG_N = CNCdt.Rows[40][1].ToString();
        //                        tb_fig_n.Text = FIG_N;
        //                        Obr_band = CNCdt.Rows[36][1].ToString();
        //                        Moc_band = CNCdt.Rows[41][1].ToString();
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Nie czyta wszystkich danych z excela", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //                    }
        //                }
        //            }
        //            //--------------------------------------------------------------
        //            //WYPELNIENIE LISTVIEW
        //            //--------------------------------------------------------------
        //            pokazdanezexcela.Items.Clear();
        //            pokazdanezexcela.Items.Add("TYP LOP" + " = " + typlopatki);
        //            pokazdanezexcela.Items.Add("KDKNo" + " = " + KDKNo);
        //            pokazdanezexcela.Items.Add("Project" + " = " + Project);
        //            pokazdanezexcela.Items.Add("MATERIAL" + " = " + materialzexcela);
        //            pokazdanezexcela.Items.Add("A" + " = " + Avalue);
        //            pokazdanezexcela.Items.Add("B" + " = " + Bvalue);
        //            pokazdanezexcela.Items.Add("C" + " = " + Cvalue);
        //            pokazdanezexcela.Items.Add("D" + " = " + Dvalue);
        //            pokazdanezexcela.Items.Add("L" + " = " + Lvalue);
        //            //
        //            pokazdanezexcela.Items.Add("FIG_N" + " = " + FIG_N);
        //            pokazdanezexcela.Items.Add("Ltol_HE" + " = " + Ltol_HEvalue);
        //            pokazdanezexcela.Items.Add("Utol_HE" + " = " + Utol_HEvalue);
        //            pokazdanezexcela.Items.Add("FN" + " = " + FNvalue);
        //            pokazdanezexcela.Items.Add("LJO" + " = " + LJOvalue);
        //            pokazdanezexcela.Items.Add("GRIP" + " = " + GRIP);
        //            pokazdanezexcela.Items.Add("Zgrz_PIN" + " = " + Zgrz_PIN);
        //            pokazdanezexcela.Items.Add("Obr_band" + " = " + Obr_band);
        //            pokazdanezexcela.Items.Add("FIG_BAND" + " = " + FIG_BAND);
        //            pokazdanezexcela.Items.Add("Moc_band" + " = " + Moc_band);
        //            //-----------------------------------------------------------
        //            //ostrzezenie o figurze nozki
        //            //-----------------------------------------------------------
        //            if (FIG_N == "F2A")
        //            {
        //                MessageBox.Show("FIGURA F2A , wykonac recznie dodatkowe operacje frezowania czol nozki!", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            if (FIG_N == "F3 (F2A)")
        //            {
        //                MessageBox.Show("FIGURA F3 (F2A) , wykonac recznie dodatkowe operacje frezowania czol nozki!", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            //--------------------------------------------------------------
        //            //PRZY OSIOWYCH BRAK WPISANEJ KOMORKI GRIP!!!
        //            //--------------------------------------------------------------
        //            if (CNCdt.Rows[3][1].ToString().Contains("RTBFIXEDBLADE")
        //                || CNCdt.Rows[3][1].ToString().Contains("RTBMOVINGBLADE"))
        //            {
        //                //--------------------------------------------------------------
        //                //WSTAWIENIE MOCOWANIA
        //                //--------------------------------------------------------------
        //                if (GRIP == "TAK" && Zgrz_PIN == "TAK" && Moc_band == "brak")
        //                {
        //                    //MessageBox.Show("GripPinWelding", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Information);
        //                    clamping.Text = "GripPinWelding";
        //                }
        //                else if (GRIP == "TAK" && Zgrz_PIN == "TAK" && Moc_band == "")
        //                {
        //                    //MessageBox.Show("GripPinWelding", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Information);
        //                    clamping.Text = "GripPinWelding";
        //                }
        //                else if (GRIP == "TAK" && Zgrz_PIN == "NIE" && Moc_band == "brak")
        //                {
        //                    //MessageBox.Show("GripPin", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Information);
        //                    clamping.Text = "GripPin";
        //                }
        //                else if (GRIP == "TAK" && Zgrz_PIN == "NIE" && Moc_band == "")
        //                {
        //                    //MessageBox.Show("GripPin", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Information);
        //                    clamping.Text = "GripPin";
        //                }
        //                else if (GRIP == "TAK" && Zgrz_PIN == "TAK" && Moc_band == "ZABIERAK")
        //                {
        //                    clamping.Text = "GripZabierak";
        //                }
        //                else if (GRIP == "TAK" && Zgrz_PIN == "TAK" && Moc_band == "GRIP")
        //                {
        //                    clamping.Text = "GripGrip";
        //                }
        //                else if (GRIP == "TAK" && Zgrz_PIN == "TAK" && Moc_band == "PIN")
        //                {
        //                    clamping.Text = "GripPinWelding";
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Bledne mocowanie, zglos sie do Mariusza!", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Error);
        //                }
        //            }
        //            else if (CNCdt.Rows[3][1].ToString().Contains("CDMovingBlade"))//na stale
        //            {
        //                clamping.Text = "GripTang";
        //            }
        //            else//RTB Radial
        //            {
        //                clamping.Text = "GripPinWelding";//na stale
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        mistake = true;
        //        MessageBox.Show($"check function czytajplikexcelztechnologia, sprawdz wybrany plik excel! {e.Message}", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void wybierzpartsb_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void wybierzxmlsb_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void wybierzparteb_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void wybierzxmleb_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void noxls_UnChecked(object sender, RoutedEventArgs e)
        //{
        //    bool newVal = (noxls.IsChecked == false);
        //    if (newVal)
        //    {
        //        wybranemocowanie.Visibility = Visibility.Hidden;
        //        millshroud.Visibility = Visibility.Hidden;
        //        pinweling.Visibility = Visibility.Hidden;
        //        wybierzxls.Visibility = Visibility.Visible;
        //        xlsfile.Visibility = Visibility.Visible;
        //        fig_n.Visibility = Visibility.Hidden;
        //    }
        //}
        //private void noxls_Checked(object sender, RoutedEventArgs e)
        //{
        //    bool newVal = (noxls.IsChecked == true);
        //    if (newVal)
        //    {
        //        wybranemocowanie.Visibility = Visibility.Visible;
        //        millshroud.Visibility = Visibility.Visible;
        //        pinweling.Visibility = Visibility.Visible;
        //        wybierzxls.Visibility = Visibility.Hidden;
        //        xlsfile.Visibility = Visibility.Hidden;
        //        fig_n.Visibility = Visibility.Visible;
        //    }
        //}

        //private void usebmtemplate_UnChecked(object sender, RoutedEventArgs e)
        //{
        //    bool newVal = (usebmtemplate.IsChecked == false);
        //    if (newVal)
        //    {
        //        bmtemplatefile.Visibility = Visibility.Hidden;
        //        Button_BMTemplate.Visibility = Visibility.Hidden;
        //        //firstxml.Visibility = Visibility.Hidden;
        //        //secondxml.Visibility = Visibility.Hidden;
        //        Mocowanieztemplata.Visibility = Visibility.Hidden;
        //    }
        //}
        //private void usebmtemplate_Checked(object sender, RoutedEventArgs e)
        //{
        //    bool newVal = (usebmtemplate.IsChecked == true);
        //    if (newVal)
        //    {
        //        bmtemplatefile.Visibility = Visibility.Visible;
        //        Button_BMTemplate.Visibility = Visibility.Visible;
        //        //firstxml.Visibility = Visibility.Visible;
        //        //secondxml.Visibility = Visibility.Visible;
        //        Mocowanieztemplata.Visibility = Visibility.Visible;
        //    }
        //}

        //private void Button_BMTemplate_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void ComboBox_SelectionChanged_FIG_N(object sender, SelectionChangedEventArgs e)
        //{
        //    if (fig_n.SelectedItem != null)
        //    {
        //        tb_fig_n.Text = (fig_n.SelectedItem as ComboBoxItem).Content.ToString();
        //    }
        //}

        //private void Button_StartProcess_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void Button_Przerwij_Click(object sender, RoutedEventArgs e)
        //{
        //    Close();
        //}

        //private void ComboBox_Clamping_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (wybranemocowanie.SelectedItem != null)
        //    {
        //        clamping.Text = (wybranemocowanie.SelectedItem as ComboBoxItem).Content.ToString();
        //    }
        //}

        //private void ComboBox_Machine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (wybranamaszyna.SelectedItem != null)
        //    {
        //        machine.Text = (wybranamaszyna.SelectedItem as ComboBoxItem).Content.ToString();
        //    }
        //}

        //private void Button_catpart_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
