using _3DViewer.Models;
using BladeMill.BLL.SourceData;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
//using Excel = Microsoft.Office.Interop.Excel;

namespace Display3DModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Path to the model file
        public static string outfile = (@"C:\temp\inputdata.xml"); //XML file to read
        public static string xmloutfile = (@"C:\temp\inputdata.xml"); //XML file to read
        public static string stlnamewithpath = (@"C:\temp\..."); //XML initial file path
        public static bool isexistrootengdirnet = true;
        public static string NetCatiaexeexist = (@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\task\Catia0.exe");
        public static string LocalCatiaexeexist = (@"C:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\task\Catia0.exe");
        public static ModelVisual3D device3D = new ModelVisual3D();
        public static ModelVisual3D device3D_next = new ModelVisual3D();
        //public static string RootEngDir = "";//GetRootEngDir();
        //public static string RootMfgDir = "";// GetRootMfgDir();
        public static string StlPart = (@"C:\temp\0000.CATPART"); //STL file
        public static string CatPartFile = (@"C:\temp\0000.CATPART"); //CATPART file
        public static string XmlFile = (@"C:\temp\0000.xml"); //XML file
        public static string XlsFile = (@"C:\temp\0000.xls"); //XLS file
        public static string Machine = (""); //obrabiarka
        public static string Order = (@"C:\temp\0000.cbm"); //CBM file
        public static bool RunConfiguration = false; //wystartowac konfiguracje w Cati
        public static bool RunBM = false; //wystartowac BMa
        public static bool RunCMM = false; //wystartowac CMMa
        public static string BpmType = (""); //typ lopatki
        public static bool CreateSTLs = false; //tworzenie stlow dla vericuta
        public static bool CreatePrerawbox = false; //tworzenie stlow dla vericuta
        public static bool Raport = false; //tworzenie stlow dla vericuta
        public static bool Usebmtemplate = false; //tworzenie stlow dla vericuta
        public static bool Noxls = false; //tworzenie stlow dla vericuta
        public static string Clamping = ("GripPinWelding"); //mocowanie
        public static bool PinWelding = false; //zgrzany nit
        public static bool MillShroud = false; //frezowanie bandaza
        public static string BmTemplatefile = (""); //mocowanie
        public static string Catpartfilefirstblade = (@"C:\temp\0000.xls");
        public static string Xmlfilefirstblade = (@"C:\temp\0000.xls");
        public static string Catpartfileendblade = (@"C:\temp\0000.xls");
        public static string Xmlfileendblade = (@"C:\temp\0000.xls");
        public static List<string> globstrlist = new List<string>(new string[] { });
        public static bool zakladkacnc = false;
        public static bool mistake = false; //error flag
        public static bool readxls = false;
        public static bool polishletter = false; //error flag
        public static bool clickcancel = false; //error flag
        //public static string User = ""; //user
        //public static string userprofile = ""; //user
        //public static string cleverhome = (@"C:\Users");
        //public static string Applicationconffile = (@"C:\Users\212517683\AppData\Roaming\BladeMill"); //Application file
        public static int Czas = 0;
        public static bool MiddleTol = false;
        public static string drugibmdxmlplik = "";
        public static string airfoiltype = "";
        public static bool admin = false;
        public static List<string> danezpierwszegoxmla = new List<string>(new string[] { });
        public static List<string> danezdrugiegoxmla = new List<string>(new string[] { });
        //public static string NetRootEngDir = @"U:\clever\V300\BladeMill\data\RootEngDir";       
        
        public static List<string> listalini = new List<string>(new string[] { });
        public static string katalogzprogramami = (@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\task");
        public static bool OneDrive = false;
        public static string onedrivedir = (@"C:\Users\212517683\OneDrive - General Electric International, Inc");
        public static string mocowanieztempleta = "";
        public static bool noweczytanieexcel = true;

        //jak schowac przycisk close
        // Prep stuff needed to remove close button on window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public PathDataBase pathDataBase = new PathDataBase();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += ToolWindow_Loaded;//usuniecie przycisku close

            Serilog.Log.Information("Start app");

            //firstxml.ItemContainerGenerator.StatusChanged += new EventHandler(ContainerStatusChanged);//nie dziala jak chcialem
            //----------------------------------------------------------------------
            //zabicie excela
            //----------------------------------------------------------------------
            zabijguwno("Excel", false);
            //----------------------------------------------------------------------
            // ustawienia dla uzytkowanika
            //----------------------------------------------------------------------
            user.Text = "brk profilu";
            username.Text = "Anomnim";
            //MessageBox.Show(Environment.UserName, "User", MessageBoxButton.OK, MessageBoxImage.Information);
            //----------------------------------------------------------------------
            //uzytkownicy tylko informacja
            //----------------------------------------------------------------------
            //212736611 Patryk
            //212510479 Piotr
            //212537861 Marian
            //212538300 Michal
            //212540098 Zbyszek
            //212556351 Wisniewski
            //212736611 Kuba
            //----------------------------------------------------------------------
            //USTAWIENIE SCIEZKI ONEDRIVE
            //----------------------------------------------------------------------
            onedrivedir = System.IO.Path.Combine(@"C:\Users", Environment.UserName, @"General Electric International, Inc\Sekcja Technologiczna T1 - Clever");
            if (Directory.Exists(onedrivedir))
            {
                OneDrive = true;
                isonedrive.IsChecked = true;
            }
            if (user.Text.Contains("212517683"))//admin
            {
                Admin.IsChecked = false;
                admin = false;
                //----------------------------------------------------------------------
                //zabicie Catii notoryczne bledy podczas liczenia nie wiem dlaczego
                //----------------------------------------------------------------------
                //zabijguwno("CNEXT", false);
                //
                //----------------------------------------------------------------------
                //Otworz Catie
                //----------------------------------------------------------------------
                //uruchomCatieR28();
            }
            //----------------------------------------------------------------------
            //wez dane z Applicationconffile
            //----------------------------------------------------------------------
            bmversion.Text = "brak BM";
            rootengdir.Text = "brak rootengdir";
            rootmfgdir.Text = "brak rootmfgdir";
            //----------------------------------------------------------------------
            //sprawdzenie czy jest katalog RootEngDir z konfiguracji BMa
            //----------------------------------------------------------------------
            if (!Directory.Exists(rootengdir.Text))
            {
                MessageBox.Show("Brak katalogu RootEngDir, zglos sie do Mariusza!", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Information);
                isexistrootengdirnet = false;
            }

            //sprawdzenie czy jest dysk S
            if (!Directory.Exists(pathDataBase.GetDirBladeMillScripts()))
            {
                MessageBox.Show("Brak dysku sieciowego S", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                isexistrootengdirnet = false;
            }

            order.Text = "test";
            Button_model3d.Visibility = Visibility.Hidden;
            Button_rootengdir.Visibility = Visibility.Hidden;
            firstxml.Visibility = Visibility.Hidden;
            secondxml.Visibility = Visibility.Hidden;

            if (System.IO.File.Exists(outfile))
            {
                //-------------------------------------------------------------------
                //wczytanie stla z xmla
                //-------------------------------------------------------------------
                loadSTLFromXML();
                //-------------------------------------------------------------------
                //tworzenie stla przy uzyciu catia0.exe
                //-------------------------------------------------------------------
                //tworzenieSTLa();
                //-------------------------------------------------------------------
                //pokazanie modelu 3d
                //-------------------------------------------------------------------
                //-------------------------------------------------------------------
                // Wypelnienie okienek
                //-------------------------------------------------------------------
                loadXML();
                //-------------------------------------------------------------------
                // wczytanie danych z okienek wypelnionych
                //-------------------------------------------------------------------
                czytaniedanychzokienkek();
                //-------------------------------------------------------------------
                // chowanie i pokazywanie okienek
                //-------------------------------------------------------------------
                bool newVal1 = (usebmtemplate.IsChecked == false);
                if (newVal1)
                {
                    bmtemplatefile.Visibility = Visibility.Hidden;
                    Button_BMTemplate.Visibility = Visibility.Hidden;
                    firstxml.Visibility = Visibility.Hidden;
                    secondxml.Visibility = Visibility.Hidden;
                    Mocowanieztemplata.Visibility = Visibility.Hidden;
                }
                else
                {
                    firstxml.Visibility = Visibility.Visible;
                    secondxml.Visibility = Visibility.Visible;
                    Mocowanieztemplata.Visibility = Visibility.Visible;
                }
                //
                bool newVal2 = (noxls.IsChecked == true);
                if (newVal2)
                {
                    noxls.IsChecked = false;
                    //clamping.Visibility = Visibility.Hidden;
                    wybranemocowanie.Visibility = Visibility.Hidden;
                    millshroud.Visibility = Visibility.Hidden;
                    pinweling.Visibility = Visibility.Hidden;
                    wybierzxls.Visibility = Visibility.Visible;
                    xlsfile.Visibility = Visibility.Visible;
                    fig_n.Visibility = Visibility.Hidden;
                }
                else
                {
                    noxls.IsChecked = true;
                    //clamping.Visibility = Visibility.Visible;
                    wybranemocowanie.Visibility = Visibility.Visible;
                    millshroud.Visibility = Visibility.Visible;
                    pinweling.Visibility = Visibility.Visible;
                    wybierzxls.Visibility = Visibility.Hidden;
                    xlsfile.Visibility = Visibility.Hidden;
                    fig_n.Visibility = Visibility.Visible;
                }
                if (bpmtype.Text != "RTBFixedBlade")//schowaj poczatkowe koncowe okienka
                {
                    catpartfilefirstblade.Visibility = Visibility.Hidden;
                    xmlfilefirstblade.Visibility = Visibility.Hidden;
                    catpartfileendblade.Visibility = Visibility.Hidden;
                    xmlfileendblade.Visibility = Visibility.Hidden;
                    wybierzpartsb.Visibility = Visibility.Hidden;
                    wybierzxmlsb.Visibility = Visibility.Hidden;
                    wybierzparteb.Visibility = Visibility.Hidden;
                    wybierzxmleb.Visibility = Visibility.Hidden;
                }
                if (bpmtype.Text != "RTBFixedBlade" && bpmtype.Text != "RTBMovingBlade")//schowaj przycisk template
                {
                    usebmtemplate.IsChecked = false;
                }
                //-------------------------------------------------------------------
                //pokazanie danych z bmd xml file
                //-------------------------------------------------------------------
                showListViewFromBMDxmlfile(xmlfile.Text);
                //-------------------------------------------------------------------
                //-------------------------------------------------------------------
            }
            else
            {
                MessageBox.Show("Brak pliku " + outfile, "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        void uruchomCatieR28()
        {
            listalini.Clear();
            bool catiaisiopen = false;
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("CNEXT");
            foreach (System.Diagnostics.Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    catiaisiopen = true;
                }
            }
            if (catiaisiopen == false)
            {
                Process myProcess = new Process();
                try
                {
                    listalini.Add(@"C:\WINDOWS\system32\cscript.exe C:\Apps\Catia\Start_cmd\Start_CatiaV5R28P2.vbs");
                    string destFile = System.IO.Path.Combine(katalogzprogramami, "Programiki_Launcher.bat");
                    if (OneDrive)
                    {
                        destFile = System.IO.Path.Combine(onedrivedir, "000_Repository", "EXE", "Programiki_Launcher.bat");
                    }
                    if (System.IO.File.Exists(destFile))
                    {
                        System.IO.File.Delete(destFile);
                    }
                    System.IO.File.WriteAllLines(destFile, listalini);

                    if (System.IO.File.Exists(destFile))
                    {
                        string program = "Programiki_Launcher.bat";
                        string[] paths = { onedrivedir, "000_Repository", "EXE", program };
                        string[] paths1 = { katalogzprogramami, program };
                        string fullPath = System.IO.Path.Combine(paths);
                        string fullPath1 = System.IO.Path.Combine(paths1);
                        if (!OneDrive)
                        {
                            myProcess.StartInfo.FileName = fullPath1;
                            myProcess.Start();
                        }
                        else
                        {
                            myProcess.StartInfo.FileName = fullPath;
                            myProcess.Start();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Brak pliku bat zglos sie do Mariusza M.", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "UWAGA! ZGLOS SIE DO MARIUSZA", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        private void czytaniedanychzokienkek()
        {
            selectedTypeOfProcess.Text = tb_TypeOfProcess.Text;
            rootengdir.Text = rootengdir.Text;
            stlpart.Text = StlPart;
            catpartfile.Text = CatPartFile;
            xmlfile.Text = XmlFile;
            xlsfile.Text = XlsFile;
            order.Text = Order;
            machine.Text = Machine;
            wybranamaszyna.Text = Machine;
            clamping.Text = Clamping;
            wybranemocowanie.Text = Clamping;
            bmtemplatefile.Text = BmTemplatefile;

            catpartfilefirstblade.Text = Catpartfilefirstblade;
            xmlfilefirstblade.Text = Xmlfilefirstblade;
            catpartfileendblade.Text = Catpartfileendblade;
            xmlfileendblade.Text = Xmlfileendblade;

            runconfiguration.IsChecked = RunConfiguration;
            runbm.IsChecked = RunBM;
            runcmm.IsChecked = RunCMM;
            createstls.IsChecked = CreateSTLs;
            createprerawbox.IsChecked = CreatePrerawbox;
            raport.IsChecked = Raport;
            usebmtemplate.IsChecked = Usebmtemplate;
            noxls.IsChecked = Noxls;
            pinweling.IsChecked = PinWelding;
            millshroud.IsChecked = MillShroud;
            middletol.IsChecked = MiddleTol;

            if (System.IO.File.Exists(xmlfile.Text))
            {
                showelementfromxml("Type", "/BPMManufacturingData/BladeTopology/MainFunctionElement", "BPMTYP", xmlfile.Text);
            }
            else
            {
                BpmType = "unknown";
            }
            bpmtype.Text = BpmType;
        }
        private void zabijguwno(string soft, bool zabijbezpytania)
        {
            //ZABIJ GUWNO
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName(soft);

            if (soft == "CNEXT")
            {
                soft = "CatieR28";
            }

            foreach (System.Diagnostics.Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    try
                    {
                        if (zabijbezpytania == true)
                        {
                            p.Kill();
                        }
                        else
                        {
                            if (MessageBox.Show("Program musi zamknac " + soft + ",  wiec stracisz niezapisane dane w excelu!, CZY KONTYNUOWAC OBLICZENIA AUTOMATU?", "UWAGA!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                            {
                                p.Kill();
                            }
                            else
                            {
                                //Application.Exit();
                                Environment.Exit(1);
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        List<string> createlistfromsinglestructure(string element, string navigator, string newname, string xmlfile)
        {
            try
            {
                //create list	
                XmlDocument document = new XmlDocument();
                document.Load(xmlfile);
                XPathNavigator navigator2 = document.CreateNavigator();
                XPathNodeIterator nodes2 = navigator2.Select(navigator);
                string line;
                foreach (XPathNavigator oCurrent in nodes2)
                {
                    line = newname;
                    globstrlist.Add(line);
                    line = oCurrent.SelectSingleNode(element).Value;
                    globstrlist.Add(line);
                }
                return globstrlist;
            }
            catch (Exception e)
            {
                throw new Exception("check function createlistfromsinglestructure", e);
                //MessageBox.Show("blad w xmlu", "", MessageBoxButton.OK, MessageBoxImage.Error);
                //return globstrlist;
            }
        }
        List<string> createlistfromstructure(string element, string navigator, string newname, string xmlfile)
        {
            try
            {
                //create list	
                XmlDocument document = new XmlDocument();
                document.Load(xmlfile);
                XPathNavigator navigator2 = document.CreateNavigator();
                XPathNodeIterator nodes2 = navigator2.Select(navigator);
                //
                string line;
                while (nodes2.MoveNext())
                {
                    line = newname;
                    globstrlist.Add(line);
                    line = nodes2.Current.GetAttribute(element, "");
                    globstrlist.Add(line);
                    if (line.Contains("STT17"))
                    {
                        //MessageBox.Show("UWAGA AUSTENIT !!!","",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                return globstrlist;
            }
            catch (Exception e)
            {
                throw new Exception("check function createlistfromstructure", e);
                //MessageBox.Show("blad w xmlu", "", MessageBoxButton.OK, MessageBoxImage.Error);
                //return globstrlist;
            }
        }


        void showListViewFromBMDxmlfile(string plikbmdxml)
        {
            pokazdanezbmdfile.Items.Clear();
            globstrlist.Clear();
            if (File.Exists(xmlfile.Text) && xmlfile.Text.Contains(".xml"))
            {
                createlistfromstructure("Name", "/BPMManufacturingData/Header/Part", "Typ lop   ", xmlfile.Text);
                createlistfromstructure("ID", "/BPMManufacturingData/Header/Part", "Rysunek ", xmlfile.Text);
                createlistfromstructure("Project", "/BPMManufacturingData/Header/Part", "Project   ", xmlfile.Text);
                createlistfromstructure("Name", "/BPMManufacturingData/Header/Part/StandardRawMaterial", "Material  ", xmlfile.Text);
                createlistfromsinglestructure("BladeOrientation", "/BPMManufacturingData/BladeTopology", "Strumien ", xmlfile.Text);
            }
            int count = 0;
            foreach (string element in globstrlist)
            {
                try
                {
                    //MessageBox.Show(globstrlist[count].ToString() + " = " + globstrlist[count + 1].ToString(), "", MessageBoxButton.OK, MessageBoxImage.Information);
                    pokazdanezbmdfile.Items.Add(globstrlist[count] + " | " + globstrlist[count + 1]);
                    pokazdanezbmdfile.Items.Add(globstrlist[count + 2] + " | " + globstrlist[count + 3]);
                    count += 4;
                }
                catch
                {
                    break;
                }
            }
            pokazdanezbmdfile.Items.Add("Typ       " + "  | " + bpmtype.Text);
            pokazdanezbmdfile.Items.Refresh();
        }

        void loadXML()
        {
            try
            {
                if (System.IO.File.Exists(outfile))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(outfile);

                    XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/DANE");
                    foreach (XmlNode node in nodeList)
                    {
                        CatPartFile = node.SelectSingleNode("catpart").InnerText;
                        XmlFile = node.SelectSingleNode("xmlpart").InnerText;
                        XlsFile = node.SelectSingleNode("xlspart").InnerText;
                        Machine = node.SelectSingleNode("machine").InnerText;
                        /*
                        //proba naprawy wyboru maszyny przy None w wybranym mocowaniu
                        if (node.SelectSingleNode("Clampingmethod").InnerText == "None")
                        {
                            Clamping = "GripPinWelding";
                        }
                        else
                        {
                            Clamping = node.SelectSingleNode("Clampingmethod").InnerText;
                        }
                        */
                        Clamping = node.SelectSingleNode("Clampingmethod").InnerText;
                        Order = node.SelectSingleNode("infile").InnerText;
                        BmTemplatefile = node.SelectSingleNode("BMTemplateFile").InnerText;

                        Catpartfilefirstblade = node.SelectSingleNode("catpartfirst").InnerText;
                        Xmlfilefirstblade = node.SelectSingleNode("xmlpartfirst").InnerText;
                        Catpartfileendblade = node.SelectSingleNode("catpartend").InnerText;
                        Xmlfileendblade = node.SelectSingleNode("xmlpartend").InnerText;

                        if (node.SelectSingleNode("ClampFromTemplate") != null)
                        {
                            Mocowanieztemplata.Text = node.SelectSingleNode("ClampFromTemplate").InnerText.Replace("&", "");
                        }

                        if (node.SelectSingleNode("FIG_N") != null)
                        {
                            tb_fig_n.Text = node.SelectSingleNode("FIG_N").InnerText;
                        }

                        if (node.SelectSingleNode("TypeOfProcess") != null)
                        {
                            tb_TypeOfProcess.Text = node.SelectSingleNode("TypeOfProcess").InnerText;
                        }

                        if (node.SelectSingleNode("runconfiguration").InnerText == "True")
                        {
                            RunConfiguration = true;
                        }
                        if (node.SelectSingleNode("runbm").InnerText == "True")
                        {
                            RunBM = true;
                        }
                        if (node.SelectSingleNode("runcmm").InnerText == "True")
                        {
                            RunCMM = true;
                        }
                        if (node.SelectSingleNode("createvcproject").InnerText == "True")
                        {
                            CreateSTLs = true;
                        }

                        if (node.SelectSingleNode("Prerawbox").InnerText == "True")
                        {
                            CreatePrerawbox = true;
                        }
                        if (node.SelectSingleNode("createraport").InnerText == "True")
                        {
                            Raport = true;
                        }
                        if (node.SelectSingleNode("BMTemplate").InnerText == "True")
                        {
                            Usebmtemplate = true;
                        }
                        if (node.SelectSingleNode("readxls").InnerText == "True")
                        {
                            Noxls = true;
                        }
                        if (node.SelectSingleNode("pinwelding").InnerText == "True")
                        {
                            PinWelding = true;
                        }
                        if (node.SelectSingleNode("millshroud").InnerText == "True")
                        {
                            MillShroud = true;
                        }
                        if (node.SelectSingleNode("middleTol").InnerText == "True")
                        {
                            MiddleTol = true;
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Brak pliku wejsciowego xml!", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch// (Exception e)
            {
                //throw new Exception("PLEASE! DELETE C:\\temp\\inputdata.xml", e);
                MessageBox.Show("ERROR IN FUNCION loadXML", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        void showelementfromxml(string element, string navigator, string newname, string xmlfile)
        {

            try
            {
                //create list

                XmlDocument document = new XmlDocument();
                document.Load(xmlfile);
                XPathNavigator navigator2 = document.CreateNavigator();
                XPathNodeIterator nodes2 = navigator2.Select(navigator);
                //
                string line;
                while (nodes2.MoveNext())
                {
                    line = newname;
                    //MessageBox.Show(line);
                    line = nodes2.Current.GetAttribute(element, "");
                    BpmType = (line);
                }

                //return globstrlist;
            }
            catch// (Exception e)
            {
                //throw new Exception("check function showelementfromxml", e);
                MessageBox.Show("You selected wrong XML file, so select again or file doesn't exist!! look showelementfromxml", "", MessageBoxButton.OK, MessageBoxImage.Error);
                //Close();
            }
        }

        /// <summary>
        /// Display 3D Model
        /// </summary>
        /// <param name="model">Path to the Model file</param>
        /// <returns>3D Model Content</returns>
        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                //Adding a gesture here
                viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);

                //Import 3D model file
                ModelImporter import = new ModelImporter();

                //Load the 3D model file
                device = import.Load(model);
            }
            catch (Exception e)
            {
                // Handle exception in case can not file 3D model
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }

        private Model3D Display3d_next(string model)
        {
            Model3D device = null;
            try
            {
                //Adding a gesture here
                viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);

                device3D.Children.Clear();
                device3D_next.Children.Clear();
                viewPort3d.Children.Remove(device3D);
                viewPort3d.Children.Remove(device3D_next);

                //Import 3D model file
                ModelImporter import = new ModelImporter();

                //Load the 3D model file
                device = import.Load(model);

            }
            catch (Exception e)
            {
                // Handle exception in case can not file 3D model
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //-------------------------------------------------
            //zapis xmla dla automatu
            //-------------------------------------------------
            createXML(xmloutfile);

            mistake = false;

            //*************************************************************************
            // SPRAWDZANIE
            //*************************************************************************

            //if (noxls.IsChecked == false && polishletter == true)//podczas wczytania danych z excela moze pojawic sie blad!
            //{
            //    MessageBox.Show("UWAGA! usun polskie znaki z komorki projekt w pliku xls technologi", "", MessageBoxButton.OK, MessageBoxImage.Error);
            //    mistake = true;
            //}

            //-------------------------------------------------
            // sprawdzenie czy nie jest liczony templete z templeta
            //-------------------------------------------------
            if (usebmtemplate.IsChecked == true)
            {               
                string varpoolztemplata = bmtemplatefile.Text.Replace(".cbm", "_varpool.xml");
                var varpoolModel = new VarpoolXmlFile(varpoolztemplata);
                var BMTemplate = varpoolModel.BMTemplate;
                if (BMTemplate.Contains("True"))
                {
                    MessageBox.Show("UWAGA! Uzyles nieprawidlowego ordera jako template, template nie moze byc zrobiony z templata!, wybierz oryginalny order!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                }
            }
            //-------------------------------------------------
            //DLA MASZYNY HSTM500M NIE DZIALAJA INNE LOPATKI NIZ RTB (TO WSTAWIC gdy bedzie juz dzialac hstm500m) 
            //-------------------------------------------------
            if (machine.Text == "HM_HSTM_500M_SIM840D")
            {
                if (bpmtype.Text != "")
                {
                    //MessageBox.Show("Wersja testowa wybranej maszyny, jesli bedzie cos zle zglos sie do Mariusza M.", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            //-------------------------------------------------
            //sprawdzenie czy na srodek narazie tylko u mnie dziala
            //po wdrozeniu HD wykasowac to... && !user.Text.Contains("212517683")
            //-------------------------------------------------
            if (middletol.IsChecked == true)
            {
                if (!user.Text.ToString().Contains("212517683"))
                {
                    MessageBox.Show("Program na środek tolerancji jeszcze nie działa", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                }
                else
                {
                    if (!bpmtype.Text.Contains("RTBMovingBlade") && !bpmtype.Text.Contains("RTBFixedBlade"))
                    {
                        MessageBox.Show("Ten typ łopatki na środek tolerancji jeszcze nie dziala", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                        mistake = true;
                    }
                }
            }
            //order ten sam jak template nie moze byc!
            if (order.Text + ".cbm" == System.IO.Path.GetFileName(bmtemplatefile.Text) && usebmtemplate.IsChecked == true)
            {
                MessageBox.Show("Order nie moze byc taki sam jak BladeMill template!", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }

            //sprawdzenie confa lokalnego i sieciowego
            if (rootengdir.Text.Contains("C:") && catpartfile.Text.Contains("U:"))
            {
                MessageBox.Show("Nie pasuje konfiguracja Blademila, wybierz Catpart ponownie", "INFORMACJA", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            //sprawdzenie czy dobre rozrzezenia w oknach
            if (!xmlfile.Text.Contains(".xml"))
            {
                MessageBox.Show("Bledny xml , wybierz go ponownie ", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            //sprawdzenie czy dobre rozrzezenia w oknach
            if (!xlsfile.Text.Contains(".xls") && noxls.IsChecked == false)
            {
                MessageBox.Show("Bledny xls , wybierz go ponownie ", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            //-------------------------------------------------
            //UWAGI JAK DZIALAC GDY WYBRANO TEMPLATE
            //-------------------------------------------------
            if (usebmtemplate.IsChecked == true)
            {
                string cleverhome = pathDataBase.GetCleverHome();
                //MessageBox.Show(cleverhome.ToString(),"CLEVERHOME",MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (!cleverhome.Contains("3.17") && !cleverhome.Contains("3.18") && !cleverhome.Contains("3.19") && !cleverhome.Contains("3.2"))
                {
                    MessageBox.Show("Po zakonczeniu liczenia otworz BladeMill i zrob update geometrii", "INFORMACJA", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                MessageBox.Show("Do zakonczenia obliczen nie wykonuj zadnych operacji na komputerze", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            //-------------------------------------------------
            //UWAGA WYBRANY TEMPLATE MA PRZECIWNY STRUMIEN
            //-------------------------------------------------
            if (usebmtemplate.IsChecked == true)
            {
                if (danezpierwszegoxmla.Count == danezdrugiegoxmla.Count && danezpierwszegoxmla.Count > 0 && danezdrugiegoxmla.Count > 0)
                {
                    string strumien1 = "";
                    string strumien2 = "";
                    foreach (string item in danezdrugiegoxmla)
                    {
                        //MessageBox.Show(item, "danezdrugiegoxmla", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (item.Contains("Strumien"))
                        {
                            strumien2 = item;
                            //MessageBox.Show(item, "danezdrugiegoxmla", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    foreach (string item in danezpierwszegoxmla)
                    {
                        if (item.Contains("Strumien"))
                        {
                            strumien1 = item;
                        }
                    }
                    if (strumien1 != strumien2)
                    {
                        if (MessageBox.Show("Template ma inny strumien , CZY KONTYNUOWAC OBLICZENIA AUTOMATU?", "UWAGA!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                        }
                        else
                        {
                            mistake = true;
                        }
                    }
                }
            }

            //--------------SPRAWDZANIE CZY NIE WYBRANO XMLA DLA POCZTKOWEJ LUB KONCOWEJ-----------------
            if (xmlfile.Text.Contains(".xml") && bpmtype.Text == "RTBFixedBlade" && File.Exists(xmlfile.Text))//check xml file
            {
                //MessageBox.Show("checking xml file","",MessageBoxButtons.OK, MessageBoxIcon.Information);
                checkxmlfile(xmlfile.Text);
            }
            if (xmlfile.Text.Contains(".xml") && bpmtype.Text == "RTBMovingBlade" && File.Exists(xmlfile.Text))//check xml file
            {
                //MessageBox.Show("checking xml file","",MessageBoxButtons.OK, MessageBoxIcon.Information);
                checkxmlfile(xmlfile.Text);
            }
            //-----------------OSTRZEZENIE PRZED AUSTENITEM----------------
            //check bladematerial
            //-------------------------------------------------
            if ((xmlfile.Text != "") && (xmlfile.Text.Contains(".xml")) && File.Exists(xmlfile.Text))
            {
                checkBladeMaterial(xmlfile.Text);
            }
            //*************************************************************************
            //DLA MASZYNY HSTM300HD NIE DZIALA AUTOMATYZACJA
            //*************************************************************************
            //po wdrozeniu HD wykasowac to... && !user.Text.Contains("212517683")
            if (machine.Text == "HM_HSTM_300HD_SIM840D" && !bpmtype.Text.Contains("RTB"))
            {
                MessageBox.Show("Dla tej maszyny automat jeszcze nie dziala", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            if (machine.Text == "HM_HSTM_300HD_SIM840D")
            {
                //MessageBox.Show("Dla tej maszyny automat jeszcze nie dziala", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                //mistake = true;
            }

            //*************************************************************************
            //sprawdzenie czy to osiowa do pomiaru
            //*************************************************************************
            if (bpmtype.Text == "RTBRadialFixedBlade" && runcmm.IsChecked == true)
            {
                MessageBox.Show("Pomiaru osiowej nie wykonujemy", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
                mistake = true;
            }
            //*************************************************************************
            // TYP LOPATKI NIE MOZE DZIALAC GDY WLACZONY TEMPLATE
            //*************************************************************************
            if (usebmtemplate.IsChecked == true && bpmtype.Text != "RTBMovingBlade" && bpmtype.Text != "RTBFixedBlade")
            {
                MessageBox.Show("TEN TYP LOPATKI NIE DZIALA Z TEMPLETEM !!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            //*************************************************************************
            //SPRAWDZENIE CZY WLACZONY JEST BLADEMILL
            //*************************************************************************
            System.Diagnostics.Process[] process2 = System.Diagnostics.Process.GetProcessesByName("Alstom.BladeMill.Gui");
            foreach (System.Diagnostics.Process p in process2)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    MessageBox.Show("ZAMKNIJ BLADEMILLa !!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                }
            }
            //*************************************************************************
            //sprawdza czy Catia jest uruchomiona
            //*************************************************************************
            Process[] pname = Process.GetProcessesByName("CNEXT");
            if (pname.Length == 0)
            {
                MessageBox.Show("Prosze uruchomic CATIE!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }

            //*************************************************************************
            //sprawdza czy wybrano prawidlowy plik XLS
            //*************************************************************************
            if (noxls.IsChecked != true)
            {
                checknamexls();
            }
            //*************************************************************************
            //sprawdza czy nie wybrano jednoczesnie liczenia obrobki i pomiaru
            //*************************************************************************
            if (runbm.IsChecked == true && runcmm.IsChecked == true)
            {
                MessageBox.Show("Nie mozna jednoczesnie liczyc obrobki i pomiaru!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                Serilog.Log.Error("Nie mozna jednoczesnie liczyc obrobki i pomiaru!");
                mistake = true;
            }

            //*************************************************************************
            //************************SPRAWDZANIE TYPU LOAPTKI WG DOBORU MASZYNY*************************
            //*************************************************************************
            if (machine.Text == "DMU60P_HEIDENHAIN" || machine.Text == "CHIRON_FZ" || machine.Text == "SH_NX155_OSAI_8600")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            if (bpmtype.Text == "ITBMovingBlade" && machine.Text == "SH_HX151_24_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            else if (bpmtype.Text == "CDMovingBlade" && machine.Text == "SH_HX151_24_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            else if (bpmtype.Text == "CDMovingBlade" && machine.Text == "HURON_EX20_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            else if (bpmtype.Text == "CDMovingBlade" && machine.Text == "FADAL")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            else if (bpmtype.Text == "ITBFixedPlatformBlade" && machine.Text == "SH_HX151_24_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            else if (bpmtype.Text == "ITBFixedPlatformBlade" && machine.Text == "FADAL")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            else if (bpmtype.Text == "CDFixedPlatformBlade" && machine.Text == "SH_HX151_24_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            else if (bpmtype.Text == "CDFixedPlatformBlade" && machine.Text == "HURON_EX20_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            else if (bpmtype.Text == "CDFixedPlatformBlade" && machine.Text == "SH_HX151_24_SIM840D")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            else if (bpmtype.Text == "CDFixedPlatformBlade" && machine.Text == "FADAL")
            {
                MessageBox.Show("Wybrana maszyna nie wspierana, wybierz inna!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                mistake = true;
            }
            else
            {
            }
            //-------------------------------------------------
            //BLOKOWANIE GDY NIE PASUJA MOCOWANIA! 
            //-------------------------------------------------
            if (usebmtemplate.IsChecked == true)
            {
                if (clamping.Text != Mocowanieztemplata.Text.Replace("&", ""))
                {
                    if (MessageBox.Show("Mocowanie w templacie jest inne, CZY NA PEWNO KONTYNUOWAC?", "ZAPYTANIE", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        mistake = true;
                    }
                }
                if (Mocowanieztemplata.Text == "Mocowanie z templata")
                {
                    MessageBox.Show("Wybierz template, aby kontynuowac!", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                }
            }
            if (machine.Text == "HURON_EX20_SIM840D" && clamping.Text == "GripGrip")
            {
                //mistake = true;
                //MessageBox.Show("This clamping not supported yet for this HURON machine!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (machine.Text == "HURON_EX20_SIM840D" && clamping.Text == "GripZabierak")
            {
                //mistake = true;
                //MessageBox.Show("This clamping not supported yet for this HURON machine!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            //*************************************************************************
            //************************SPRAWDZANIE MOCOWANIA*************************
            //*************************************************************************
            if (bpmtype.Text == "ITBMovingBlade")
            {
                if (clamping.Text == "TextBox" && noxls.IsChecked == true)
                {
                    MessageBox.Show("Wybierz mocowanie recznie!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                }
            }

            if (bpmtype.Text == "ITBMovingBlade" || bpmtype.Text == "ITBFixedPlatformBlade" || bpmtype.Text == "CDFixedPlatformBlade")
            {
                if (noxls.IsChecked == true)
                {
                    if (clamping.Text == "Wybierz mocowanie")
                    {
                        MessageBox.Show("Wybierz mocowanie!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        mistake = true;
                    }
                    else
                    {
                        if (!bpmtype.Text.Contains("RTB") && clamping.Text == "GripGrip")
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        if (!bpmtype.Text.Contains("RTB") && clamping.Text == "GripZabierak")
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        if (bpmtype.Text == "CDFixedPlatformBlade" && clamping.Text == "GripPin" && noxls.IsChecked == true)
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        if (bpmtype.Text == "CDFixedPlatformBlade" && clamping.Text == "GripPinWelding" && noxls.IsChecked == true)
                        {
                            mistake = true;
                            //MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin","",MessageBoxButtons.OK, MessageBoxIcon.Error);
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        if (bpmtype.Text == "CDFixedPlatformBlade" && clamping.Text == "DovetailPinCenterBox" && noxls.IsChecked == true)
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        if (bpmtype.Text == "CDFixedPlatformBlade" && clamping.Text == "GripTang" && noxls.IsChecked == true)
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        if (bpmtype.Text == "ITBMovingBlade" && clamping.Text == "GripPin" && noxls.IsChecked == true)
                        {
                            mistake = false;
                            //MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin","",MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //MessageBox.Show("Ten rodzaj mocowania jest testowany!","",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (bpmtype.Text == "ITBMovingBlade" && clamping.Text == "GripPinWelding" && noxls.IsChecked == true)
                        {
                            mistake = false;
                            //MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin","",MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //MessageBox.Show("Ten rodzaj mocowania jest testowany!","",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (bpmtype.Text == "ITBMovingBlade" && clamping.Text == "DovetailPinCenterBox" && noxls.IsChecked == true)
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        if (bpmtype.Text == "ITBMovingBlade" && clamping.Text == "GripTang" && noxls.IsChecked == true)
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding, DovetailPin, GripPin or GripPinWelding", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        if (bpmtype.Text == "ITBFixedPlatformBlade" && clamping.Text == "GripPin" && noxls.IsChecked == true)
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        if (bpmtype.Text == "ITBFixedPlatformBlade" && clamping.Text == "GripPinWelding" && noxls.IsChecked == true)
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        if (bpmtype.Text == "ITBFixedPlatformBlade" && clamping.Text == "DovetailPinCenterBox" && noxls.IsChecked == true)
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        if (bpmtype.Text == "ITBFixedPlatformBlade" && clamping.Text == "GripTang" && noxls.IsChecked == true)
                        {
                            mistake = true;
                            MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    if (bpmtype.Text == "CDFixedPlatformBlade" && createprerawbox.IsChecked == false)
                    {
                        mistake = true;
                        //MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin","",MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show("Wybierz przygotowke (only ITB fix and CD fix)!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if (bpmtype.Text == "CDFixedPlatformBlade" && createprerawbox.IsChecked == true)
                    {
                        mistake = true;
                        //MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin","",MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show("Wybierz plik excel z technologia (tylko do zapisu danych do technologi) i nastepnie wybierz BRAK XLSa!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if (bpmtype.Text == "ITBFixedPlatformBlade" && createprerawbox.IsChecked == false)
                    {
                        mistake = true;
                        //MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin","",MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show("Wybierz przygotowke (only ITB fix and CD fix)!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if (bpmtype.Text == "ITBFixedPlatformBlade" && createprerawbox.IsChecked == true)
                    {
                        mistake = true;
                        //MessageBox.Show("This clamping not supported yet! Select DovetailPinWelding or DovetailPin","",MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show("Wybierz plik excel z technologia (tylko do zapisu danych do technologi) i nastepnie wybierz BRAK XLSa!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            //*************************************************************************
            //************************SPRAWDZANIE CZY ISTNIJA PLIKI*************************
            //*************************************************************************
            if (!File.Exists(catpartfile.Text))
            {
                mistake = true;
                MessageBox.Show("CATPART file doesn't exist!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (!File.Exists(catpartfilefirstblade.Text) && bpmtype.Text == "RTBFixedBlade")
            {
                mistake = true;
                MessageBox.Show("START CATPART file doesn't exist!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (!File.Exists(catpartfileendblade.Text) && bpmtype.Text == "RTBFixedBlade")
            {
                mistake = true;
                MessageBox.Show("END CATPART file doesn't exist!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (!File.Exists(xmlfile.Text))
            {
                mistake = true;
                MessageBox.Show("XML file doesn't exist!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (!File.Exists(xmlfilefirstblade.Text) && bpmtype.Text == "RTBFixedBlade")
            {
                mistake = true;
                MessageBox.Show("START XML file doesn't exist!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (!File.Exists(xmlfileendblade.Text) && bpmtype.Text == "RTBFixedBlade")
            {
                mistake = true;
                MessageBox.Show("END XML file doesn't exist!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (!File.Exists(xlsfile.Text) && (noxls.IsChecked == false))
            {
                mistake = true;
                MessageBox.Show("XLS plik nie istnieje! jesli go brakuje a pomimo tego chcesz kontynuowac wybierz opcje brakXLSa", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (order.Text == "")
            {
                MessageBox.Show("Wpisz order!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                mistake = true;
            }
            if (machine.Text == "")
            {
                MessageBox.Show("Please select machine!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                mistake = true;
            }
            if (catpartfile.Text == (""))
            {
                MessageBox.Show("Please select catpartfile!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                mistake = true;
            }
            if (xmlfile.Text == (""))
            {
                MessageBox.Show("Please select xmlfile!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                mistake = true;
            }
            if (noxls.IsChecked == false && xlsfile.Text == "")
            {
                MessageBox.Show("Please select xlsfile!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                mistake = true;
            }
            if (noxls.IsChecked == true)
            {
                if (clamping.Text == "" || clamping.Text == "None")
                {
                    MessageBox.Show("Please select clamping!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    mistake = true;
                }
            }
            if (bpmtype.Text == "CDMovingBlade" && clamping.Text != "GripTang" && noxls.IsChecked == true)
            {
                mistake = true;
                MessageBox.Show("This clamping not supported yet! Select GripTang clamping", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (usebmtemplate.IsChecked == true && bmtemplatefile.Text == "")
            {
                MessageBox.Show("Please select BM Template!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                mistake = true;
            }
            //*************************************************************************
            //osiowa nie dziala z xlsem, testy u mnie dziala
            //po wdrozeniu wykasowac to... && !user.Text.Contains("212517683")
            //*************************************************************************
            if (bpmtype.Text == "RTBRadialFixedBlade" && noxls.IsChecked == false)// && !user.Text.Contains("212517683") )
            {
                /*
                //MessageBox.Show("Please select manuall type of clamping!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBox.Show("Wybierz recznie mocowanie\n" +
                                "dla lopatki osiowej dane z excela nie sa wczytywane\n" +
                                "", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                mistake = true;
                */
            }

            //*************************************************************************
            //sprawdzenie czy nie wybrano nie tego samego rzedu w okienkach dla zwyklej lopatki
            //*************************************************************************
            //wyciagnij nazwe lopatki z catparta
            if (File.Exists(catpartfile.Text))
            {
                int count = 0;
                List<string> listfiltertext = new List<string>(new string[] { });
                string[] filtertext = catpartfile.Text.Split(new char[] { '\\', '.' });
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
                if (!catpartfile.Text.Contains(modifystartpartname) || !xmlfile.Text.Contains(modifystartpartname) || !xlsfile.Text.Contains(modifystartpartname))
                {
                    //mistake = true;
                    //MessageBox.Show("Sprawdz wybor stopnia w okienkach, nie pasuja nazwy ", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (mistake == false)
                    {
                        if (MessageBox.Show("Sprawdz wybor stopnia w okienkach, nie pasuja nazwy, CZY NA PEWNO KONTYNUOWAC?", "ZAPYTANIE", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        {
                            mistake = true;
                            //clickcancel = true;
                        }
                    }
                }

                //sprawdzenie czy nie wybrano nie tego samego rzedu w okienkach dla lopatki zamkowej
                if (bpmtype.Text == "RTBFixedBlade")
                {
                    string stopien = "";
                    string rysunekbezpozycji = "";
                    string rysunek = System.IO.Path.GetFileName(catpartfile.Text).Replace("_-.CATPart", "");
                    int dlugosrysunku = rysunek.Length;
                    stopien = rysunek.Remove(0, dlugosrysunku - 2);
                    rysunekbezpozycji = rysunek.Remove(dlugosrysunku - 3, 3);
                    string rysunekpoczatkowej = rysunekbezpozycji + "1" + stopien;
                    //MessageBox.Show(rysunekpoczatkowej);
                    if (!catpartfilefirstblade.Text.Contains(rysunekpoczatkowej) || !xmlfilefirstblade.Text.Contains(rysunekpoczatkowej))
                    {
                        mistake = true;
                        MessageBox.Show("Sprawdz wybor stopnia dla lopatki poczatkowej w okienkach", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    string rysunekkoncowej = rysunekbezpozycji + "2" + stopien;
                    //MessageBox.Show(rysunekkoncowej);
                    if (!catpartfileendblade.Text.Contains(rysunekkoncowej) || !xmlfileendblade.Text.Contains(rysunekkoncowej))
                    {
                        mistake = true;
                        MessageBox.Show("Sprawdz wybor stopnia dla lopatki koncowej w okienkach", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            //-------------------------------------------------
            //sprawdzanie poprawnosci wpisanych nazw orderow
            //-------------------------------------------------
            if (runbm.IsChecked == true && runcmm.IsChecked == false)
            {
                if (machine.Text == "HM_HSTM_500M_SIM840D")
                {
                    if (!order.Text.StartsWith("C"))
                    {
                        MessageBox.Show("INFO! brak przedrostka C w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        mistake = true;
                    }
                    if (order.Text.Length > 7 || order.Text.Length < 7)
                    {
                        MessageBox.Show("INFO! nieprawidlowa ilość znaków w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        mistake = true;
                    }
                }
                else if (machine.Text == "HM_HSTM_300_SIM840D")
                {
                    if (!order.Text.StartsWith("A") && !order.Text.StartsWith("D"))
                    {
                        MessageBox.Show("INFO! brak przedrostka A lub D w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        mistake = true;
                    }
                    if (order.Text.Length > 7 || order.Text.Length < 7)
                    {
                        MessageBox.Show("INFO! nieprawidlowa ilość znaków w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        mistake = true;
                    }
                }
                if (machine.Text == "HM_HSTM_500_SIM840D")
                {
                    if (!order.Text.StartsWith("B"))
                    {
                        MessageBox.Show("INFO! brak przedrostka B w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        mistake = true;
                    }
                    if (order.Text.Length > 7 || order.Text.Length < 7)
                    {
                        MessageBox.Show("INFO! nieprawidlowa ilość znaków w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        mistake = true;
                    }
                }
                else if (machine.Text == "HM_HSTM_300HD_SIM840D")
                {
                    if (!order.Text.StartsWith("D"))
                    {
                        MessageBox.Show("INFO! brak przedrostka D w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        mistake = true;
                    }
                    if (order.Text.Length > 7 || order.Text.Length < 7)
                    {
                        MessageBox.Show("INFO! nieprawidlowa ilość znaków w nazwie ordera", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        mistake = true;
                    }
                }
                if (machine.Text == "HURON_EX20_SIM840D")
                {
                    if (order.Text.Length > 5 || order.Text.Length < 5)
                    {
                        MessageBox.Show("UWAGA! bledna ilosc znaków w nazwie ordera, musi być 5 znaków", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                        mistake = true;
                    }
                }
                if (machine.Text == "SH_HX151_24_SIM840D")
                {
                    if (order.Text.Length > 5 || order.Text.Length < 5)
                    {
                        MessageBox.Show("UWAGA! bledna ilosc znaków w nazwie ordera, musi być 5 znaków", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                        mistake = true;
                    }
                }
                else{}
            }

            //*************************************************************************
            //************************CZY NADPISAC ORDER*************************
            //*************************************************************************
            //checking if exist directory than stop!
            if (order.Text != "")
            {
                //MessageBox.Show(GetRootMfgDir());
                string checkingdirectory = System.IO.Path.Combine(rootmfgdir.Text, order.Text);//USTAWIENIE SCIEZKI DLA ORDEROW
                //MessageBox.Show(checkingdirectory);
                if (!System.IO.Directory.Exists(checkingdirectory))
                {
                    //MessageBox.Show("Directory doesn't exist!!!");
                }
                else
                {
                    //MessageBox.Show("Directory exist");
                    if (mistake == false)
                    {
                        if (MessageBox.Show("ORDER ZOSTANIE NADPISANY, CZY NA PEWNO KONTYNUOWAC?", "ZAPYTANIE", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        {
                            mistake = true;
                            //clickcancel = true;
                        }
                    }
                }
            }

            //MessageBox.Show(mistake.ToString(), "bledy", MessageBoxButton.OK, MessageBoxImage.Information);
            if (mistake == false)
            {
                //MessageBox.Show("You save file here " + outfile);
                this.Close();
            }

            zabijguwno("Excel", true);

        }

        void checknamexls()
        {
            try
            {
                //wyciagnij nazwe lopatki z catparta
                int count = 0;
                List<string> listfiltertext = new List<string>(new string[] { });
                string[] filtertext = catpartfile.Text.Split(new char[] { '\\', '.' });
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

                //wyciagnij nazwe kopatki z okna xlsa
                count = 0;
                listfiltertext.Clear();
                filtertext = xlsfile.Text.Split(new char[] { '\\', '.' });
                nazwalopatki = "";
                substringslength = filtertext.Length;
                //MessageBox.Show(substringslength.ToString());
                string modifystartxlsname = "";
                foreach (string element in filtertext)
                {
                    if (element != "XLS")
                    {
                        //MessageBox.Show(element);
                        if (count == substringslength - 2)
                        {
                            nazwalopatki = element;
                        }
                    }
                    count += 1;
                }

                modifystartxlsname = nazwalopatki.Replace("_", "").Replace("-", "");
                //MessageBox.Show(modifystartxlsname);

                if (!modifystartxlsname.Contains(modifystartpartname))//(modifystartpartname != modifystartxlsname)
                {
                    MessageBox.Show("Wybrano nieprawidlowy plik XLS, wybierz ponownie!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                }

            }
            catch (Exception e)
            {
                throw new Exception("check function checknamexls", e);
            }
        }

        void checkBladeMaterial(string xmlfile)

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

        static bool checkxmlfile(string xmlfile)//sprawdza czy nie wybrano XMLa dla pocztkowej i koncowej
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
                        mistake = true;
                    }
                    if (line.StartsWith("E"))
                    {
                        MessageBox.Show("UWAGA  !!! BLEDNY XML ", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        mistake = true;
                    }
                }
                return mistake;
            }
            catch (Exception e)
            {
                throw new Exception("check function checkxmlfile", e);
            }
        }

        void loadSTLFromXML()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(outfile);

                XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/DANE");
                foreach (XmlNode node in nodeList)
                {
                    stlpart.Text = node.SelectSingleNode("catpart").InnerText;
                    stlnamewithpath = stlpart.Text;
                    stlnamewithpath = stlnamewithpath.Replace(".CATPart", ".stl");
                    StlPart = stlnamewithpath;
                }
            }
            catch// (Exception e)
            {
                //throw new Exception("PLEASE! DELETE C:\\temp\\inputdata.xml", e);
                MessageBox.Show("ERROR IN FUNCION loadXML", "", MessageBoxButton.OK, MessageBoxImage.Information);
                //System.Windows.Forms.Application.Exit();
            }
        }

        private delegate void UpdateProgressBarDelegate(
                System.Windows.DependencyProperty dp, Object value);

        void show3DmodelfromTextBox(string filestl)
        {
            try
            {

                //Configure the ProgressBar
                timeexcel.Minimum = 0;
                timeexcel.Maximum = Czas;//short.MaxValue;
                timeexcel.Value = 0;
                //Stores the value of the ProgressBar
                double value = 0;
                //Create a new instance of our ProgressBar Delegate that points
                // to the ProgressBar's SetValue method.
                UpdateProgressBarDelegate updatePbDelegate =
                    new UpdateProgressBarDelegate(timeexcel.SetValue);
                //Tight Loop: Loop until the ProgressBar.Value reaches the max
                do
                {
                    value += 1;

                    //System.Threading.Thread.Sleep(Czas * 2000);
                    czas.Text = Czas.ToString() + " sek.";

                    /*Update the Value of the ProgressBar:
                        1) Pass the "updatePbDelegate" delegate
                           that points to the ProgressBar1.SetValue method
                        2) Set the DispatcherPriority to "Background"
                        3) Pass an Object() Array containing the property
                           to update (ProgressBar.ValueProperty) and the new value */
                    Dispatcher.Invoke(updatePbDelegate,
                        System.Windows.Threading.DispatcherPriority.Background,
                        new object[] { ProgressBar.ValueProperty, value });
                }
                while (timeexcel.Value != timeexcel.Maximum);

                //MessageBox.Show("stl = " + stlnamewithpath, "", MessageBoxButton.OK, MessageBoxImage.Information);
                if (File.Exists(filestl))
                {
                    //ModelVisual3D device3D_next = new ModelVisual3D();
                    device3D_next.Content = Display3d_next(filestl);
                    // Add to view port
                    viewPort3d.Children.Add(device3D_next);
                    viewPort3d.ZoomExtents();
                }
            }
            catch (Exception e)
            {
                //throw new Exception("PLEASE! DELETE C:\\temp\\inputdata.xml", e);
                MessageBox.Show("ERROR IN FUNCION show3DmodelfromTextBox " + e, "", MessageBoxButton.OK, MessageBoxImage.Information);
                //System.Windows.Forms.Application.Exit();
            }
        }

        void tworzenieSTLa(string plikstl)
        {
            try
            {
                if (File.Exists(xmloutfile))
                {
                    createXML(xmloutfile);
                }
                if (!File.Exists(plikstl))
                {
                    if (File.Exists(NetCatiaexeexist))
                    {
                        Process.Start("S:\\clever\\V300\\BladeMill\\BladeMillServer\\BladeMillScripts\\Process\\task\\Catia0.exe");
                        isexistrootengdirnet = true;
                    }
                    else if (File.Exists(LocalCatiaexeexist))
                    {
                        Process.Start("C:\\clever\\V300\\BladeMill\\BladeMillServer\\BladeMillScripts\\Process\\task\\Catia0.exe");
                        isexistrootengdirnet = false;
                    }
                    else if (OneDrive == true)
                    {
                        string fullPath = Path.Combine(onedrivedir, @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\task\Catia0.exe");
                        //MessageBox.Show(fullPath);                        
                        if (File.Exists(fullPath))
                        {
                            Process.Start(fullPath);
                        }
                        isexistrootengdirnet = false;
                        OneDrive = true;
                    }
                    else
                    {
                        MessageBox.Show("Brak programu Catia0.exe, wgraj go tutaj: \n" +
                                        LocalCatiaexeexist, "UWAGA", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception e)
            { MessageBox.Show("ERROR IN FUNCION tworzenieSTLa " + e, "", MessageBoxButton.OK, MessageBoxImage.Information); }
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ComboBox_SelectionChanged_Clamp(object sender, SelectionChangedEventArgs e)
        {
            if (wybranemocowanie.SelectedItem != null)
            {
                clamping.Text = (wybranemocowanie.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void ComboBox_SelectionChanged_machine(object sender, SelectionChangedEventArgs e)
        {
            if (wybranamaszyna.SelectedItem != null)
            {
                machine.Text = (wybranamaszyna.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void ComboBox_SelectionChanged_TypeOfProcess(object sender, SelectionChangedEventArgs e)
        {
            if (selectedTypeOfProcess.SelectedItem != null)
            {
                tb_TypeOfProcess.Text = (selectedTypeOfProcess.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }
        private void ComboBox_SelectionChanged_FIG_N(object sender, SelectionChangedEventArgs e)
        {
            if (fig_n.SelectedItem != null)
            {
                tb_fig_n.Text = (fig_n.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            clickcancel = true;
            createXML(xmloutfile);
            Close();
        }

        private void noxls_UnChecked(object sender, RoutedEventArgs e)
        {
            bool newVal = (noxls.IsChecked == false);
            if (newVal)
            {
                //clamping.Visibility = Visibility.Hidden;
                wybranemocowanie.Visibility = Visibility.Hidden;
                millshroud.Visibility = Visibility.Hidden;
                pinweling.Visibility = Visibility.Hidden;
                wybierzxls.Visibility = Visibility.Visible;
                xlsfile.Visibility = Visibility.Visible;
                fig_n.Visibility = Visibility.Hidden;
            }
        }
        private void noxls_Checked(object sender, RoutedEventArgs e)
        {
            bool newVal = (noxls.IsChecked == true);
            if (newVal)
            {
                //clamping.Visibility = Visibility.Visible;
                wybranemocowanie.Visibility = Visibility.Visible;
                millshroud.Visibility = Visibility.Visible;
                pinweling.Visibility = Visibility.Visible;
                wybierzxls.Visibility = Visibility.Hidden;
                xlsfile.Visibility = Visibility.Hidden;
                fig_n.Visibility = Visibility.Visible;
            }
        }
        private void usebmtemplate_UnChecked(object sender, RoutedEventArgs e)
        {
            bool newVal = (usebmtemplate.IsChecked == false);
            if (newVal)
            {
                bmtemplatefile.Visibility = Visibility.Hidden;
                Button_BMTemplate.Visibility = Visibility.Hidden;
                firstxml.Visibility = Visibility.Hidden;
                secondxml.Visibility = Visibility.Hidden;
                Mocowanieztemplata.Visibility = Visibility.Hidden;
            }
        }
        private void usebmtemplate_Checked(object sender, RoutedEventArgs e)
        {
            bool newVal = (usebmtemplate.IsChecked == true);
            if (newVal)
            {
                bmtemplatefile.Visibility = Visibility.Visible;
                Button_BMTemplate.Visibility = Visibility.Visible;
                firstxml.Visibility = Visibility.Visible;
                secondxml.Visibility = Visibility.Visible;
                Mocowanieztemplata.Visibility = Visibility.Visible;
            }
        }
        void replacestartendfilename(string typefile)
        {
            try
            {
                //wypelnij poczatkowe i koncowe
                //wypelnij automatycznie catpart poczatkowej textBox8
                int count = 0;
                List<string> listfiltertext3 = new List<string>(new string[] { });
                string[] filtertext3 = catpartfile.Text.Split(new char[] { '\\', '.' });
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
                            string dircatpart = System.IO.Path.GetDirectoryName(catpartfile.Text);
                            filterxmltext3 = dircatpart + "\\";

                            filterxmltext3 = String.Concat(filterxmltext3, newmodifystartpartname);
                            //MessageBox.Show(filterxmltext3);
                        }

                    }
                    count += 1;
                }
                if (typefile == "PARTSTART")
                {
                    catpartfilefirstblade.Text = filterxmltext3;
                }
                else if (typefile == "XMLSTART")
                {
                    xmlfilefirstblade.Text = filterxmltext3;
                }
                else if (typefile == "PARTEND")
                {
                    catpartfileendblade.Text = filterxmltext3;
                }
                else if (typefile == "XMLEND")
                {
                    xmlfileendblade.Text = filterxmltext3;
                }
                else
                {
                    catpartfilefirstblade.Text = "WYBIERZ RECZNIE!";
                    xmlfilefirstblade.Text = "WYBIERZ RECZNIE!";
                    catpartfileendblade.Text = "WYBIERZ RECZNIE!";
                    xmlfileendblade.Text = "WYBIERZ RECZNIE!";
                }

            }
            catch (Exception e)
            {
                throw new Exception("check function replacestartendfilename", e);
            }
        }

        void czytajplikexcelztechnologia(string path)
        {
            try
            {
                string openexcel = path;

                if (File.Exists(openexcel) && !openexcel.Contains("KDT") && !openexcel.Contains("P.XLS") && !openexcel.Contains("P.xls"))
                {
                    //MessageBox.Show(openexcel);
                    string POCpath = openexcel;
                    string POCConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + POCpath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";";
                    OleDbConnection POCcon = new OleDbConnection(POCConnection);
                    //--------------------------------------------------------------
                    //CZYTANIE KOMOREK Z EXCELA Z ZAKLADKI DANE
                    //--------------------------------------------------------------
                    OleDbCommand DANEcommand = new OleDbCommand();
                    DataTable DANEdt = new DataTable();
                    OleDbDataAdapter DANECommand = new OleDbDataAdapter("select * from [DANE$] ", POCcon);
                    DANECommand.Fill(DANEdt);
                    string materialzexcela = "brak";
                    if (DANEdt.Rows[29][1].ToString().Contains("Gatunek materiału:") && bpmtype.Text == "RTBFixedBlade")
                    {
                        materialzexcela = DANEdt.Rows[29][2].ToString();
                    }
                    if (DANEdt.Rows[30][1].ToString().Contains("Gatunek materiału:") && bpmtype.Text == "RTBMovingBlade")
                    {
                        materialzexcela = DANEdt.Rows[30][2].ToString();
                    }
                    if (DANEdt.Rows[26][1].ToString().Contains("Gatunek materiału:") && bpmtype.Text == "RTBRadialFixedBlade")
                    {
                        materialzexcela = DANEdt.Rows[26][2].ToString();
                    }
                    if (DANEdt.Rows[31][1].ToString().Contains("Gatunek materiału:") && bpmtype.Text == "ITBMovingBlade")
                    {
                        materialzexcela = DANEdt.Rows[31][2].ToString();
                    }
                    string typlopatki = "brak";
                    if (DANEdt.Rows[26][1].ToString().Contains("Typ:"))
                    {
                        typlopatki = DANEdt.Rows[26][2].ToString();
                    }
                    if (DANEdt.Rows[23][1].ToString().Contains("Typ:") && bpmtype.Text == "RTBRadialFixedBlade")
                    {
                        typlopatki = DANEdt.Rows[23][2].ToString();
                    }
                    //MessageBox.Show("typlopatki", "typlopatki z excela", MessageBoxButton.OK, MessageBoxImage.Information);
                    //--------------------------------------------------------------
                    //CZYTANIE KOMOREK Z EXCELA Z ZAKLADKI CNC
                    //--------------------------------------------------------------
                    OleDbCommand CNCcommand = new OleDbCommand();
                    DataTable CNCdt = new DataTable();
                    OleDbDataAdapter CNCCommand = new OleDbDataAdapter("select * from [CNC$] ", POCcon);
                    CNCCommand.Fill(CNCdt);
                    string Project = CNCdt.Rows[4][1].ToString();//TODO Project Name dodaj sprawdzanie polskich liter!

                    if (CheckPolishLetter(Project))
                    {
                        //MessageBox.Show("UWAGA! usun polskie znaki: " + Project.ToString() + " z komorki projekt ", "", MessageBoxButton.OK, MessageBoxImage.Error);                        
                    }

                    string Avalue = CNCdt.Rows[11][1].ToString();
                    string Bvalue = CNCdt.Rows[12][1].ToString();
                    string LJOvalue = "brak";
                    string Lvalue = "brak";
                    string Dvalue = "brak";
                    string Cvalue = "brak";
                    string Ltol_HEvalue = "brak";
                    string Utol_HEvalue = "brak";

                    if (bpmtype.Text == "RTBFixedBlade")
                    {
                        Ltol_HEvalue = CNCdt.Rows[42][1].ToString();
                        Utol_HEvalue = CNCdt.Rows[43][1].ToString();
                    }

                    if (CNCdt.Rows[3][1].ToString().Contains("CDMovingBlade"))
                    {
                        Lvalue = CNCdt.Rows[13][1].ToString();
                        Dvalue = CNCdt.Rows[15][1].ToString();
                        Cvalue = CNCdt.Rows[16][1].ToString();
                    }
                    else
                    {
                        LJOvalue = CNCdt.Rows[15][1].ToString();
                        Lvalue = CNCdt.Rows[20][1].ToString();
                        Dvalue = CNCdt.Rows[21][1].ToString();
                        Cvalue = CNCdt.Rows[22][1].ToString();
                    }
                    string KDKNo = CNCdt.Rows[6][1].ToString();
                    //
                    string Zgrz_PIN = "brak";
                    string GRIP = "brak";
                    string FNvalue = "brak";
                    string Obr_band = "brak";
                    string FIG_BAND = "brak";
                    string FIG_N = "brak";
                    tb_fig_n.Text = FIG_N;
                    string Moc_band = "brak";
                    if (!CNCdt.Rows[3][1].ToString().Contains("CDMovingBlade"))//z excela
                    {
                        if (bpmtype.Text == "ITBMovingBlade")
                        {
                            Zgrz_PIN = CNCdt.Rows[37][1].ToString();
                            GRIP = CNCdt.Rows[36][1].ToString();
                            FNvalue = CNCdt.Rows[38][1].ToString();
                        }
                        else
                        {
                            if (bpmtype.Text == "RTBMovingBlade")
                            {
                                Zgrz_PIN = CNCdt.Rows[37][1].ToString();
                                GRIP = CNCdt.Rows[38][1].ToString();
                                FIG_BAND = CNCdt.Rows[39][1].ToString();
                                FIG_N = "brak";
                                tb_fig_n.Text = FIG_N;
                                Obr_band = CNCdt.Rows[36][1].ToString();
                                Moc_band = CNCdt.Rows[40][1].ToString();
                            }
                            else if (bpmtype.Text == "RTBFixedBlade")
                            {
                                Zgrz_PIN = CNCdt.Rows[37][1].ToString();
                                GRIP = CNCdt.Rows[38][1].ToString();
                                FIG_BAND = CNCdt.Rows[39][1].ToString();
                                FIG_N = CNCdt.Rows[40][1].ToString();
                                tb_fig_n.Text = FIG_N;
                                Obr_band = CNCdt.Rows[36][1].ToString();
                                Moc_band = CNCdt.Rows[41][1].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Nie czyta wszystkich danych z excela", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    //--------------------------------------------------------------
                    //WYPELNIENIE LISTVIEW
                    //--------------------------------------------------------------
                    pokazdanezexcela.Items.Clear();
                    pokazdanezexcela.Items.Add("TYP LOP" + " = " + typlopatki);
                    pokazdanezexcela.Items.Add("KDKNo" + " = " + KDKNo);
                    pokazdanezexcela.Items.Add("Project" + " = " + Project);
                    pokazdanezexcela.Items.Add("MATERIAL" + " = " + materialzexcela);
                    pokazdanezexcela.Items.Add("A" + " = " + Avalue);
                    pokazdanezexcela.Items.Add("B" + " = " + Bvalue);
                    pokazdanezexcela.Items.Add("C" + " = " + Cvalue);
                    pokazdanezexcela.Items.Add("D" + " = " + Dvalue);
                    pokazdanezexcela.Items.Add("L" + " = " + Lvalue);
                    //
                    pokazdanezexcela.Items.Add("FIG_N" + " = " + FIG_N);
                    pokazdanezexcela.Items.Add("Ltol_HE" + " = " + Ltol_HEvalue);
                    pokazdanezexcela.Items.Add("Utol_HE" + " = " + Utol_HEvalue);
                    pokazdanezexcela.Items.Add("FN" + " = " + FNvalue);
                    pokazdanezexcela.Items.Add("LJO" + " = " + LJOvalue);
                    pokazdanezexcela.Items.Add("GRIP" + " = " + GRIP);
                    pokazdanezexcela.Items.Add("Zgrz_PIN" + " = " + Zgrz_PIN);
                    pokazdanezexcela.Items.Add("Obr_band" + " = " + Obr_band);
                    pokazdanezexcela.Items.Add("FIG_BAND" + " = " + FIG_BAND);
                    pokazdanezexcela.Items.Add("Moc_band" + " = " + Moc_band);
                    //-----------------------------------------------------------
                    //ostrzezenie o figurze nozki
                    //-----------------------------------------------------------
                    if (FIG_N == "F2A")
                    {
                        MessageBox.Show("FIGURA F2A , wykonac recznie dodatkowe operacje frezowania czol nozki!", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if (FIG_N == "F3 (F2A)")
                    {
                        MessageBox.Show("FIGURA F3 (F2A) , wykonac recznie dodatkowe operacje frezowania czol nozki!", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //--------------------------------------------------------------
                    //PRZY OSIOWYCH BRAK WPISANEJ KOMORKI GRIP!!!
                    //--------------------------------------------------------------
                    if (CNCdt.Rows[3][1].ToString().Contains("RTBFIXEDBLADE")
                        || CNCdt.Rows[3][1].ToString().Contains("RTBMOVINGBLADE"))
                    {
                        //--------------------------------------------------------------
                        //WSTAWIENIE MOCOWANIA
                        //--------------------------------------------------------------
                        if (GRIP == "TAK" && Zgrz_PIN == "TAK" && Moc_band == "brak")
                        {
                            //MessageBox.Show("GripPinWelding", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Information);
                            clamping.Text = "GripPinWelding";
                        }
                        else if (GRIP == "TAK" && Zgrz_PIN == "TAK" && Moc_band == "")
                        {
                            //MessageBox.Show("GripPinWelding", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Information);
                            clamping.Text = "GripPinWelding";
                        }
                        else if (GRIP == "TAK" && Zgrz_PIN == "NIE" && Moc_band == "brak")
                        {
                            //MessageBox.Show("GripPin", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Information);
                            clamping.Text = "GripPin";
                        }
                        else if (GRIP == "TAK" && Zgrz_PIN == "NIE" && Moc_band == "")
                        {
                            //MessageBox.Show("GripPin", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Information);
                            clamping.Text = "GripPin";
                        }
                        else if (GRIP == "TAK" && Zgrz_PIN == "TAK" && Moc_band == "ZABIERAK")
                        {
                            clamping.Text = "GripZabierak";
                        }
                        else if (GRIP == "TAK" && Zgrz_PIN == "TAK" && Moc_band == "GRIP")
                        {
                            clamping.Text = "GripGrip";
                        }
                        else if (GRIP == "TAK" && Zgrz_PIN == "TAK" && Moc_band == "PIN")
                        {
                            clamping.Text = "GripPinWelding";
                        }
                        else
                        {
                            MessageBox.Show("Bledne mocowanie, zglos sie do Mariusza!", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else if (CNCdt.Rows[3][1].ToString().Contains("CDMovingBlade"))//na stale
                    {
                        clamping.Text = "GripTang";
                    }
                    else//RTB Radial
                    {
                        clamping.Text = "GripPinWelding";//na stale
                    }


                }
            }
            catch (Exception e)
            {
                mistake = true;
                //throw new Exception("check function czytajplikexcelztechnologia", e);
                MessageBox.Show("check function czytajplikexcelztechnologia, sprawdz wybrany plik excel!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void showListViewDimensionMaterialFromexcel(string path)
        {
            MessageBox.Show("Brak aplikacji excel, nie wczytano danych", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
            //try
            //{
            //    pokazdanezexcela.Items.Clear();
            //    timeexcel.Value = 0;
            //    timeexcel.Minimum = 0;
            //    timeexcel.Maximum = 100;
            //    Excel.Application appl = new Excel.Application();
            //    Excel.Workbook workbook = appl.Workbooks.Open(path);
            //    Excel.Sheets excelSheets = workbook.Worksheets;
            //    string currentSheet = "CNC";
            //    timeexcel.Value++;
            //    System.Threading.Thread.Sleep(100);
            //    bool zakladkadane = false;
            //    string materialzexcela = "";
            //    string typlopatki = "";
            //    //--------------------------------------------------------------
            //    //OSTRZEZENIE PRZED BRAKIEM ZAKLADKI CNC!
            //    //--------------------------------------------------------------
            //    int numSheets = workbook.Sheets.Count;
            //    for (int sheetNum = 1; sheetNum < numSheets + 1; sheetNum++)
            //    {
            //        Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[sheetNum];
            //        //MessageBox.Show(sheet.Name,"",MessageBoxButton.OK, MessageBoxImage.Information);		
            //        if (sheet.Name == "CNC")
            //        {
            //            zakladkacnc = true;
            //        }
            //    }
            //    //--------------------------------------------------------------
            //    //OSTRZEZENIE PRZED BRAKIEM ZAKLADKI DANE!
            //    //--------------------------------------------------------------
            //    for (int sheetNum = 1; sheetNum < numSheets + 1; sheetNum++)
            //    {
            //        Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[sheetNum];
            //        //MessageBox.Show(sheet.Name,"",MessageBoxButton.OK, MessageBoxImage.Information);		
            //        if (sheet.Name == "DANE")
            //        {
            //            zakladkadane = true;
            //        }
            //    }

            //    //--------------------------------------------------------------
            //    //WYCIAGNIECIE DANYCH Z ZAKLADKI DANE = > MATERIAL
            //    //--------------------------------------------------------------
            //    currentSheet = "DANE";
            //    if (zakladkadane == true)
            //    {
            //        Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(currentSheet);
            //        //--------------------------------------------------------------
            //        //CZYTANIE KOMOREK Z ECXELA
            //        //--------------------------------------------------------------
            //        var MATERIAL = (Excel.Range)excelWorksheet.Cells[32, 3];
            //        var TYPLOPATKI = (Excel.Range)excelWorksheet.Cells[28, 3];
            //        if (bpmtype.Text == "RTBMovingBlade")
            //        {
            //            MATERIAL = (Excel.Range)excelWorksheet.Cells[32, 3];
            //            TYPLOPATKI = (Excel.Range)excelWorksheet.Cells[28, 3];
            //        }
            //        else if (bpmtype.Text == "RTBFixedBlade")
            //        {
            //            MATERIAL = (Excel.Range)excelWorksheet.Cells[31, 3];
            //            TYPLOPATKI = (Excel.Range)excelWorksheet.Cells[28, 3];
            //        }
            //        else if (bpmtype.Text == "ITBFixedPlatformBlade")
            //        {
            //            MATERIAL = (Excel.Range)excelWorksheet.Cells[27, 3];
            //            TYPLOPATKI = (Excel.Range)excelWorksheet.Cells[25, 3];
            //        }
            //        else if (bpmtype.Text == "CDFixedPlatformBlade")
            //        {
            //            MATERIAL = (Excel.Range)excelWorksheet.Cells[27, 3];
            //            TYPLOPATKI = (Excel.Range)excelWorksheet.Cells[25, 3];
            //        }
            //        else if (bpmtype.Text == "CDMovingBlade")
            //        {
            //            MATERIAL = (Excel.Range)excelWorksheet.Cells[33, 3];
            //            TYPLOPATKI = (Excel.Range)excelWorksheet.Cells[28, 3];
            //        }
            //        else if (bpmtype.Text == "RTBRadialFixedBlade")
            //        {
            //            MATERIAL = (Excel.Range)excelWorksheet.Cells[28, 3];
            //            TYPLOPATKI = (Excel.Range)excelWorksheet.Cells[25, 3];
            //        }
            //        else
            //        {
            //            MATERIAL = (Excel.Range)excelWorksheet.Cells[33, 3];
            //            TYPLOPATKI = (Excel.Range)excelWorksheet.Cells[26, 3];
            //        }

            //        try//wyjatek dla Radka u niego jest 32 dla kierownicy RTBe ale nie dziala wyzej?
            //        {
            //            materialzexcela = MATERIAL.Value2.ToString();
            //        }
            //        catch
            //        {
            //            MATERIAL = (Excel.Range)excelWorksheet.Cells[32, 3];
            //            materialzexcela = MATERIAL.Value2.ToString();
            //        }
            //        typlopatki = TYPLOPATKI.Value2.ToString();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Plik excel nieprawidlowy brak zakladki DANE, wybierz BRAK XLSa!!!", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
            //        mistake = true;
            //    }
            //    //--------------------------------------------------------------
            //    //WYCIAGNIECIE DANYCH Z ZAKLADKI CNC
            //    //--------------------------------------------------------------
            //    currentSheet = "CNC";
            //    if (zakladkacnc == true)
            //    {
            //        Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(currentSheet);
            //        //--------------------------------------------------------------
            //        //OSTRZEZENIE PRZED POLSKIMI LITERAMI
            //        //--------------------------------------------------------------
            //        var Project = (Excel.Range)excelWorksheet.Cells[6, 1];//Project
            //        var Projectvalue = (Excel.Range)excelWorksheet.Cells[6, 2];//Projectvalue
            //        //MessageBox.Show(Project.Value2 + " = " + Projectvalue.Value2);

            //        char[] polishchars = { 'ę', 'ó', 'ą', 'ś', 'ł', 'ż', 'ź', 'ć', 'ń', 'Ę', 'Ó', 'Ą', 'Ś', 'Ł', 'Ż', 'Ź', 'Ć', 'Ń' };

            //        foreach (char element in polishchars)
            //        {
            //            if (Projectvalue.Value2.ToString().Contains(element.ToString()))
            //            {
            //                MessageBox.Show("UWAGA! usun polski znak: " + element.ToString() + " z komorki projekt ", "", MessageBoxButton.OK, MessageBoxImage.Error);
            //                polishletter = true;
            //            }
            //        }
            //        //--------------------------------------------------------------
            //        //CZYTANIE KOMOREK Z ECXELA
            //        //--------------------------------------------------------------
            //        var GRIP = (Excel.Range)excelWorksheet.Cells[39, 2];
            //        var Zgrz_PIN = (Excel.Range)excelWorksheet.Cells[40, 2];
            //        var FIG_BAND = (Excel.Range)excelWorksheet.Cells[41, 2];
            //        var FIG_N = (Excel.Range)excelWorksheet.Cells[42, 2];
            //        var Avalue = (Excel.Range)excelWorksheet.Cells[13, 2];//Avalue
            //        var Bvalue = (Excel.Range)excelWorksheet.Cells[14, 2];//Bvalue
            //        var Lvalue = (Excel.Range)excelWorksheet.Cells[22, 2];//Lvalue
            //        var Obr_band = (Excel.Range)excelWorksheet.Cells[38, 2];
            //        if (bpmtype.Text == "ITBMovingBlade")
            //        {
            //            GRIP = (Excel.Range)excelWorksheet.Cells[38, 2];
            //            Zgrz_PIN = (Excel.Range)excelWorksheet.Cells[39, 2];
            //        }
            //        else
            //        {
            //            GRIP = (Excel.Range)excelWorksheet.Cells[39, 2];
            //            Zgrz_PIN = (Excel.Range)excelWorksheet.Cells[40, 2];
            //        }
            //        var LJO = (Excel.Range)excelWorksheet.Cells[17, 2];
            //        var D = (Excel.Range)excelWorksheet.Cells[23, 2];
            //        var C = (Excel.Range)excelWorksheet.Cells[24, 2];
            //        var KDKNo = (Excel.Range)excelWorksheet.Cells[8, 2];
            //        var FN = (Excel.Range)excelWorksheet.Cells[40, 2];
            //        //--------------------------------------------------------------
            //        //WYPELNIENIE LISTVIEW
            //        //--------------------------------------------------------------
            //        pokazdanezexcela.Items.Clear();
            //        pokazdanezexcela.Items.Add("TYP LOP" + " = " + typlopatki);
            //        pokazdanezexcela.Items.Add("KDKNo" + " = " + KDKNo.Value2.ToString());
            //        pokazdanezexcela.Items.Add("Project" + " = " + Projectvalue.Value2.ToString());
            //        pokazdanezexcela.Items.Add("MATERIAL" + " = " + materialzexcela);
            //        pokazdanezexcela.Items.Add("A" + " = " + Avalue.Value2.ToString());
            //        pokazdanezexcela.Items.Add("B" + " = " + Bvalue.Value2.ToString());
            //        pokazdanezexcela.Items.Add("C" + " = " + C.Value2.ToString());
            //        pokazdanezexcela.Items.Add("D" + " = " + D.Value2.ToString());
            //        pokazdanezexcela.Items.Add("L" + " = " + Lvalue.Value2.ToString());
            //        try
            //        {
            //            pokazdanezexcela.Items.Add("FN" + " = " + FN.Value2.ToString());
            //        }
            //        catch { }
            //        try
            //        {
            //            pokazdanezexcela.Items.Add("LJO" + " = " + LJO.Value2.ToString());
            //        }
            //        catch { }
            //        try
            //        {
            //            pokazdanezexcela.Items.Add("GRIP" + " = " + GRIP.Value2.ToString());
            //        }
            //        catch { }
            //        pokazdanezexcela.Items.Add("Zgrz_PIN" + " = " + Zgrz_PIN.Value2.ToString());
            //        if (bpmtype.Text == "ITBMovingBlade")
            //        { }
            //        else
            //        {
            //            try
            //            {
            //                pokazdanezexcela.Items.Add("Obr_band" + " = " + Obr_band.Value2.ToString());
            //            }
            //            catch
            //            {
            //                pokazdanezexcela.Items.Add("Obr_band" + " = " + "PUSTO");
            //            }
            //            try
            //            {
            //                pokazdanezexcela.Items.Add("FIG_BAND" + " = " + FIG_BAND.Value2.ToString());
            //            }
            //            catch
            //            {
            //                pokazdanezexcela.Items.Add("FIG_BAND" + " = " + "PUSTO");
            //            }
            //            try
            //            {
            //                pokazdanezexcela.Items.Add("FIG_N" + " = " + FIG_N.Value2.ToString());
            //                if (FIG_N.Value2.ToString() == "F2A")
            //                {
            //                    MessageBox.Show("FIGURA F2A , wykonac recznie dodatkowe operacje frezowania czol nozki!", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
            //                }
            //                if (FIG_N.Value2.ToString() == "F3 (F2A)")
            //                {
            //                    MessageBox.Show("FIGURA F3 (F2A) , wykonac recznie dodatkowe operacje frezowania czol nozki!", "UWAGA", MessageBoxButton.OK, MessageBoxImage.Information);
            //                }
            //            }
            //            catch
            //            {
            //                pokazdanezexcela.Items.Add("FIG_N" + " = " + "PUSTO");
            //            }
            //        }
            //        //--------------------------------------------------------------
            //        //PRZY OSIOWYCH BRAK WPISANEJ KOMORKI GRIP!!!
            //        //--------------------------------------------------------------
            //        if (bpmtype.Text != "RTBRadialFixedBlade")
            //        {
            //            //--------------------------------------------------------------
            //            //WSTAWIENIE MOCOWANIA
            //            //--------------------------------------------------------------
            //            if (GRIP.Value2.ToString() == "TAK" && Zgrz_PIN.Value2.ToString() == "TAK")
            //            {
            //                //MessageBox.Show("GripPinWelding", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Information);
            //                clamping.Text = "GripPinWelding";
            //            }
            //            else if (GRIP.Value2.ToString() == "TAK" && Zgrz_PIN.Value2.ToString() == "NIE")
            //            {
            //                //MessageBox.Show("GripPin", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Information);
            //                clamping.Text = "GripPin";
            //            }
            //            else
            //            {
            //                MessageBox.Show("Bledne mocowanie, zglos sie do Mariusza!", "Mocowanie", MessageBoxButton.OK, MessageBoxImage.Error);
            //            }
            //        }
            //        else
            //        {
            //            clamping.Text = "GripPinWelding";
            //        }

            //        //-------------------------------------------------
            //        //blad formatu brak linkowania
            //        //-------------------------------------------------
            //        if (Avalue.Value2.ToString() == "-2146826265")//problem with linking in office365 jak to poprawic!!!
            //        {
            //            MessageBox.Show("Blad formatu komorki #Ref! -2146826265 , problem z linkowaniem plikow z kopiuj odpowiednie pliki", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
            //            mistake = true;
            //        }

            //    }
            //    else
            //    {
            //        MessageBox.Show("Plik excel nieprawidlowy brak zakladki CNC, wybierz BRAK XLSa!!!", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
            //        mistake = true;
            //    }
            //    workbook.Close(0);
            //    appl.Quit();

            //    zabijguwno("Excel", true);

            //}
            //catch (Exception e)
            //{
            //    throw new Exception("check function showListViewDimensionMaterialFromexcel", e);
            //}
        }

        string convertBooltoString(bool pytanie)
        {
            string tekst = "None";
            if (pytanie == true)
            {
                tekst = "True";
            }
            else
            {
                tekst = "False";
            }
            return tekst;
        }

        void createXML(string xmlplik)
        {
            try
            {
                //wczytywanie do reprezenatcji dummy tylko gdy RTB kierownica
                if (bpmtype.Text != "RTBFixedBlade" && bpmtype.Text != "")
                {
                    catpartfilefirstblade.Text = "";
                    catpartfileendblade.Text = "";
                    xmlfilefirstblade.Text = "";
                    xmlfileendblade.Text = "";
                }

                if (Clamping == "TextBox")
                {
                    clamping.Text = "";
                }
                if (noxls.IsChecked == false)
                {
                    readxls = true;
                }
                else
                {
                    readxls = false;
                }
                //podmiana nazwy maszyny gdy HD (tylko lepsze rozroznienie)
                if (machine.Text == "HM_HSTM_500HD_SIM840D")
                {
                    machine.Text = "HM_HSTM_500_SIM840D";
                }

                //podmiana mocowania na stale w gripie dla osiowej gdy wybrano excel
                if (bpmtype.Text == "RTBRadialFixedBlade" && noxls.IsChecked == false)
                {
                    clamping.Text = "GripPinWelding";
                }

                //dopisanie admina
                if (Admin.IsChecked == true)
                {
                    admin = true;
                    //MessageBox.Show("Tutaj admin", "Czy admin tutaj", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                if (Admin.IsChecked == false)
                {
                    admin = false;
                    //MessageBox.Show("Tutaj ktos inny", "Czy admin tutaj", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                XDocument doc = new XDocument(
                new XElement("DANE",
                         new XElement("machine", machine.Text),
                         new XElement("outfile", outfile),
                         new XElement("catpart", catpartfile.Text),
                         new XElement("xmlpart", xmlfile.Text),
                         new XElement("xlspart", xlsfile.Text),
                         new XElement("catpartfirst", catpartfilefirstblade.Text),
                         new XElement("catpartend", catpartfileendblade.Text),
                         new XElement("xmlpartfirst", xmlfilefirstblade.Text),
                         new XElement("xmlpartend", xmlfileendblade.Text),
                         new XElement("Clampingmethod", clamping.Text),
                         new XElement("pinwelding", pinweling.IsChecked.ToString()),
                         new XElement("millshroud", millshroud.IsChecked.ToString()),
                         new XElement("readxls", convertBooltoString(readxls)),
                         new XElement("runconfiguration", runconfiguration.IsChecked.ToString()),
                         new XElement("runbm", runbm.IsChecked.ToString()),
                         new XElement("runcmm", runcmm.IsChecked.ToString()),
                         new XElement("createvcproject", createstls.IsChecked.ToString()),
                         new XElement("selectlanguage", "pol"),
                         new XElement("Prerawbox", createprerawbox.IsChecked.ToString()),
                         new XElement("createraport", raport.IsChecked.ToString()),
                         new XElement("RootMfgDir", rootmfgdir.Text),
                         new XElement("clickcancel", convertBooltoString(clickcancel)),
                         new XElement("BMTemplate", usebmtemplate.IsChecked.ToString()),
                         new XElement("BMTemplateFile", bmtemplatefile.Text),
                         new XElement("IsXML", "True"),
                         new XElement("TypeBlade", bpmtype.Text),
                         new XElement("middleTol", middletol.IsChecked.ToString()),
                         new XElement("admin", admin.ToString()),
                         new XElement("ClampFromTemplate", Mocowanieztemplata.Text),
                         new XElement("FIG_N", tb_fig_n.Text),
                         new XElement("infile", order.Text),
                         new XElement("TypeOfProcess", tb_TypeOfProcess.Text)));
                doc.Save(xmlplik);
            }
            catch (Exception e)
            {
                throw new Exception("check function createXML", e);
            }
        }

        private void WybierzCatPart_Click(object sender, RoutedEventArgs e)
        {
            viewPort3d.Children.Remove(device3D_next);
            //---------------------------------
            // resetowanie okienek z danymi
            //---
            pokazdanezbmdfile.Items.Clear();
            pokazdanezexcela.Items.Clear();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.CATPart)|*.CATPart|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = rootengdir.Text;
            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                //---------------------------------
                // set catpart
                //---------------------------------
                CatPartFile = openFileDialog.FileName;
                catpartfile.Text = CatPartFile;
                //---------------------------------
                // set stlpart
                //---------------------------------
                stlpart.Text = catpartfile.Text.Replace(".CATPart", ".stl");
                StlPart = stlpart.Text;
                //---------------------------------
                // wypelnienie okienek na podstawie wybranego catparta
                //---------------------------------
                //zrob xml z catparta
                string dircatpart = System.IO.Path.GetDirectoryName(catpartfile.Text);
                string xmlname = System.IO.Path.GetFileName(catpartfile.Text).Replace("_-.CATPart", "_-_BMD.xml");
                string nowyxml = System.IO.Path.Combine(dircatpart, xmlname);
                xmlfile.Text = nowyxml;
                //zrob xls z catparta
                string xlsname = System.IO.Path.GetFileName(catpartfile.Text).Replace("_-.CATPart", ".xls");
                string nowyxls = System.IO.Path.Combine(dircatpart, xlsname);
                xlsfile.Text = nowyxls;
                //MessageBox.Show(xlsfile.Text, "xlsfile.Text", MessageBoxButton.OK, MessageBoxImage.Information);
                //set Bpmtype
                if (System.IO.File.Exists(xmlfile.Text))
                {
                    showelementfromxml("Type", "/BPMManufacturingData/BladeTopology/MainFunctionElement", "BPMTYP", xmlfile.Text);
                }
                else
                {
                    BpmType = "unknown";
                }
                bpmtype.Text = BpmType;
                //---------------------------------
                // aktualizuj template
                //---------------------------------
                //MessageBox.Show(bpmtype.Text, "bpmtype", MessageBoxButton.OK, MessageBoxImage.Information);
                if (bpmtype.Text.Contains("ITB"))//schowaj przycisk template
                {
                    usebmtemplate.IsChecked = false;
                }
                //---------------------------------
                //poczatkowe i koncowe pliki
                //---------------------------------
                if (bpmtype.Text == "RTBFixedBlade" && bpmtype.Text != "unknown")//wypelnij automatycznie catparty i xmle poczatkowej i koncowej
                {
                    replacestartendfilename("PARTSTART");
                    replacestartendfilename("PARTEND");
                    replacestartendfilename("XMLSTART");
                    replacestartendfilename("XMLEND");
                }
                //-------------------------------------------------------------------
                //pokazanie ukrycie okienek
                //-------------------------------------------------------------------
                if (bpmtype.Text != "RTBFixedBlade")//schowaj poczatkowe koncowe okienka
                {
                    catpartfilefirstblade.Visibility = Visibility.Hidden;
                    xmlfilefirstblade.Visibility = Visibility.Hidden;
                    catpartfileendblade.Visibility = Visibility.Hidden;
                    xmlfileendblade.Visibility = Visibility.Hidden;
                    wybierzpartsb.Visibility = Visibility.Hidden;
                    wybierzxmlsb.Visibility = Visibility.Hidden;
                    wybierzparteb.Visibility = Visibility.Hidden;
                    wybierzxmleb.Visibility = Visibility.Hidden;
                }
                if (bpmtype.Text == "RTBFixedBlade")//pokaz poczatkowe koncowe okienka
                {
                    catpartfilefirstblade.Visibility = Visibility.Visible;
                    xmlfilefirstblade.Visibility = Visibility.Visible;
                    catpartfileendblade.Visibility = Visibility.Visible;
                    xmlfileendblade.Visibility = Visibility.Visible;
                    wybierzpartsb.Visibility = Visibility.Visible;
                    wybierzxmlsb.Visibility = Visibility.Visible;
                    wybierzparteb.Visibility = Visibility.Visible;
                    wybierzxmleb.Visibility = Visibility.Visible;
                }
                if (bpmtype.Text == "ITBFixedPlatformBlade")//pokaz poczatkowe koncowe okienka
                {
                    noxls.IsChecked = true;
                }
                //-------------------------------------------------------------------
                //pokazanie danych z bmd xml file
                //-------------------------------------------------------------------
                if (System.IO.File.Exists(xmlfile.Text))
                {
                    showListViewFromBMDxmlfile(xmlfile.Text);
                }
                //-------------------------------------------------------------------
                //pokazanie danych z excela
                //-------------------------------------------------------------------
                if ((noxls.IsChecked == false && System.IO.File.Exists(xlsfile.Text)
                    && bpmtype.Text == "RTBRadialFixedBlade")
                    || (noxls.IsChecked == false && System.IO.File.Exists(xlsfile.Text)
                    && bpmtype.Text == "RTBFixedBlade")
                    || (noxls.IsChecked == false && System.IO.File.Exists(xlsfile.Text)
                    && bpmtype.Text == "RTBMovingBlade")
                    || (noxls.IsChecked == false && System.IO.File.Exists(xlsfile.Text)
                    && bpmtype.Text == "ITBMovingBlade"))
                {
                    if (noweczytanieexcel == false)
                    {
                        showListViewDimensionMaterialFromexcel(xlsfile.Text);
                    }
                    else
                    {
                        czytajplikexcelztechnologia(xlsfile.Text);
                    }
                }
                else if (noxls.IsChecked == false)
                {
                    MessageBox.Show("Brak pliku excel " + xlsfile.Text, "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //---------------------------------
                //wypelnij listy do porownania z templatem
                //---------------------------------
                if (usebmtemplate.IsChecked == true && (bpmtype.Text == "RTBFixedBlade" || bpmtype.Text == "RTBMovingBlade"))
                {
                    wyciagnijdrugixml(bmtemplatefile.Text);
                    zroblistyzxmlow(xmlfile.Text, drugibmdxmlplik);
                    foreach (string item in danezdrugiegoxmla)//sprawdzenie
                    {
                        //MessageBox.Show(item, "inne", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //zaznaczenie kolorami roznic
                    if (danezpierwszegoxmla.Count == danezdrugiegoxmla.Count)
                    {
                        if (danezpierwszegoxmla.Count > 0 && danezdrugiegoxmla.Count > 0)
                        {
                            firstxml.ItemsSource = wypelnijpierwszylistview(danezpierwszegoxmla, danezdrugiegoxmla);
                            secondxml.ItemsSource = wypelnijdrugilistview(danezpierwszegoxmla, danezdrugiegoxmla);
                        }
                    }
                }
                else
                {
                    firstxml.Visibility = Visibility.Hidden;
                    secondxml.Visibility = Visibility.Hidden;
                }
                //---------------------------------
                // zmien obrazek 3D
                //---------------------------------
                //show3DmodelfromTextBox(StlPart);
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                //pokazmodel3D();
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }


        }

        private void Button_xml_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = rootengdir.Text;
            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                //---------------------------------
                // set xml
                //---------------------------------
                XmlFile = openFileDialog.FileName;
                xmlfile.Text = XmlFile;

                if (xmlfile.Text.Contains("xml"))
                {
                    showelementfromxml("Type", "/BPMManufacturingData/BladeTopology/MainFunctionElement", "BPMTYP", xmlfile.Text);
                }
                else
                {
                    MessageBox.Show("You selected wrong XML file!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                bpmtype.Text = BpmType;
                if (bpmtype.Text == "ITBMovingBlade" || bpmtype.Text == "RTBRadialFixedBlade" || bpmtype.Text == "CDMovingBlade" || bpmtype.Text == "CDFixedPlatformBlade")
                {
                    noxls.IsChecked = true;
                }
                else
                {
                    noxls.IsChecked = false;
                }
                //-------------------------------------------------------------------
                //pokazanie ukrycie okienek
                //-------------------------------------------------------------------
                if (bpmtype.Text != "RTBFixedBlade")//schowaj poczatkowe koncowe okienka
                {
                    catpartfilefirstblade.Visibility = Visibility.Hidden;
                    xmlfilefirstblade.Visibility = Visibility.Hidden;
                    catpartfileendblade.Visibility = Visibility.Hidden;
                    xmlfileendblade.Visibility = Visibility.Hidden;
                    wybierzpartsb.Visibility = Visibility.Hidden;
                    wybierzxmlsb.Visibility = Visibility.Hidden;
                    wybierzparteb.Visibility = Visibility.Hidden;
                    wybierzxmleb.Visibility = Visibility.Hidden;
                }
                if (bpmtype.Text == "RTBFixedBlade")//schowaj poczatkowe koncowe okienka
                {
                    catpartfilefirstblade.Visibility = Visibility.Visible;
                    xmlfilefirstblade.Visibility = Visibility.Visible;
                    catpartfileendblade.Visibility = Visibility.Visible;
                    xmlfileendblade.Visibility = Visibility.Visible;
                    wybierzpartsb.Visibility = Visibility.Visible;
                    wybierzxmlsb.Visibility = Visibility.Visible;
                    wybierzparteb.Visibility = Visibility.Visible;
                    wybierzxmleb.Visibility = Visibility.Visible;
                }
                //-------------------------------------------------------------------
                //pokazanie danych z bmd xml file
                //-------------------------------------------------------------------
                if (System.IO.File.Exists(xmlfile.Text))
                {
                    showListViewFromBMDxmlfile(xmlfile.Text);
                }

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

            }
        }

        private void wybierzxls_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = rootengdir.Text;
            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                zabijguwno("Excel", true);
                //---------------------------------
                // set xml
                //---------------------------------
                XlsFile = openFileDialog.FileName;
                xlsfile.Text = XlsFile;
                if ((xlsfile.Text.Contains("CATPart")) || (xlsfile.Text.Contains("xml")) || (xlsfile.Text.Contains("KDT")))
                {
                    MessageBox.Show("You selected wrong XLS file , select again !!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    //---------------------------------------------------------
                    //wyswietla wymiary kloca z Excela i pobiera mocowanie								    				
                    //----------------------------------------------------------
                    if (bpmtype.Text == "RTBFixedBlade" || bpmtype.Text == "RTBMovingBlade" || bpmtype.Text == "RTBRadialFixedBlade")
                    {
                        if (noxls.IsChecked == false)
                        {
                            //MessageBox.Show("Show dimensions of material","",MessageBoxButton.OK, MessageBoxImage.Information);
                            if (noweczytanieexcel == false)
                            {
                                showListViewDimensionMaterialFromexcel(xlsfile.Text);
                            }
                            else
                            {
                                czytajplikexcelztechnologia(xlsfile.Text);
                            }
                        }
                    }
                    else if (bpmtype.Text == "ITBMovingBlade")
                    {
                        //MessageBox.Show("Show dimensions of material","",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (noweczytanieexcel == false)
                        {
                            showListViewDimensionMaterialFromexcel(xlsfile.Text);
                        }
                        else
                        {
                            czytajplikexcelztechnologia(xlsfile.Text);
                        }
                    }
                    else if (bpmtype.Text == "CDMovingBlade")
                    {
                        if (noweczytanieexcel == true)
                        {
                            czytajplikexcelztechnologia(xlsfile.Text);
                        }
                    }
                    else if (bpmtype.Text == "ITBFixedPlatformBlade")
                    {
                        //MessageBox.Show("brak zakladki CNC!!","",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    { }
                }
                zabijguwno("Excel", true);
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void wybierzpartsb_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.CATPart)|*.CATPart|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = rootengdir.Text;
            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                //---------------------------------
                // set catpart
                //---------------------------------
                Catpartfilefirstblade = openFileDialog.FileName;
                catpartfilefirstblade.Text = Catpartfilefirstblade;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void wybierzparteb_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.CATPart)|*.CATPart|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = rootengdir.Text;
            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                //---------------------------------
                // set catpart
                //---------------------------------
                Catpartfileendblade = openFileDialog.FileName;
                catpartfileendblade.Text = Catpartfileendblade;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void wybierzxmlsb_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = rootengdir.Text;
            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                //---------------------------------
                // set xml
                //---------------------------------
                Xmlfilefirstblade = openFileDialog.FileName;
                xmlfilefirstblade.Text = Xmlfilefirstblade;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void wybierzxmleb_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = rootengdir.Text;
            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                //---------------------------------
                // set xml
                //---------------------------------
                Xmlfileendblade = openFileDialog.FileName;
                xmlfileendblade.Text = Xmlfileendblade;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }
        private ArrayList wypelnijpierwszylistview(List<string> lista1, List<string> lista2)
        {
            ArrayList itemsList = new ArrayList();
            for (int i = 0; i < danezpierwszegoxmla.Count; i++)
            {
                if (danezpierwszegoxmla[i].ToString() != danezdrugiegoxmla[i].ToString())
                {
                    itemsList.Add(new ListViewItem { Content = danezpierwszegoxmla[i], Foreground = Brushes.Red });
                }
                else
                {
                    itemsList.Add(new ListViewItem { Content = danezpierwszegoxmla[i], Foreground = Brushes.Blue });
                }
            }
            return itemsList;
        }
        private ArrayList wypelnijdrugilistview(List<string> lista1, List<string> lista2)
        {
            ArrayList itemsList = new ArrayList();
            for (int i = 0; i < danezdrugiegoxmla.Count; i++)
            {
                if (danezpierwszegoxmla[i].ToString() != danezdrugiegoxmla[i].ToString())
                {
                    itemsList.Add(new ListViewItem { Content = danezdrugiegoxmla[i], Foreground = Brushes.Red });
                }
                else
                {
                    itemsList.Add(new ListViewItem { Content = danezdrugiegoxmla[i], Foreground = Brushes.Blue });
                }
            }
            return itemsList;
        }
        private void Button_BMTemplate_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.cbm)|*.cbm|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = rootmfgdir.Text;
            firstxml.Visibility = Visibility.Visible;
            secondxml.Visibility = Visibility.Visible;
            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                BmTemplatefile = openFileDialog.FileName;
                bmtemplatefile.Text = BmTemplatefile;

                if (usebmtemplate.IsChecked == true && (bpmtype.Text == "RTBFixedBlade" || bpmtype.Text == "RTBMovingBlade"))
                {
                    //---------------------------------
                    //proba z listview
                    //---------------------------------
                    wyciagnijdrugixml(bmtemplatefile.Text);
                    zroblistyzxmlow(xmlfile.Text, drugibmdxmlplik);
                    foreach (string item in danezdrugiegoxmla)//sprawdzenie
                    {
                        //MessageBox.Show(item, "inne", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //zaznaczenie kolorami roznic
                    if (danezpierwszegoxmla.Count == danezdrugiegoxmla.Count)
                    {
                        if (danezpierwszegoxmla.Count > 0 && danezdrugiegoxmla.Count > 0)
                        {
                            firstxml.ItemsSource = wypelnijpierwszylistview(danezpierwszegoxmla, danezdrugiegoxmla);
                            secondxml.ItemsSource = wypelnijdrugilistview(danezpierwszegoxmla, danezdrugiegoxmla);
                        }
                    }
                    //wycignij mocowanie z templata
                    string varpoolztemplata = bmtemplatefile.Text.Replace(".cbm", "_varpool.xml");
                    if (File.Exists(varpoolztemplata))
                    {
                        //MessageBox.Show(varpoolztemplata);
                        XmlDocument document = new XmlDocument();
                        document.Load(varpoolztemplata);
                        //
                        //Display all the "STL File" from VCproject XML file
                        XmlNodeList elemList = document.GetElementsByTagName("Var");
                        //MessageBox.Show(elemList.Count.ToString());
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            var element = elemList[i].SelectNodes("Value");
                            var firstElement = element.Item(0);
                            if (firstElement.InnerText.Contains("&"))
                            {
                                //MessageBox.Show(firstElement.InnerText.ToString());
                                mocowanieztempleta = firstElement.InnerText;
                                Mocowanieztemplata.Text = mocowanieztempleta.Replace("&", "");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    firstxml.Visibility = Visibility.Hidden;
                    secondxml.Visibility = Visibility.Hidden;
                    Mocowanieztemplata.Visibility = Visibility.Hidden;
                }
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }
        private void ContainerStatusChanged(object sender, EventArgs e)//to nie dziala jak chcialem????
        {
            if (firstxml.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                foreach (var item in firstxml.Items)
                {
                    //????
                }
                for (int i = 0; i < firstxml.Items.Count; i++)
                {
                    if (firstxml.Items[i].ToString() != secondxml.Items[i].ToString())
                    {
                        //firstxml.Items[i] = Brushes.Green;//podmienia nazwe na jakies #???????????
                        //firstxml.Items[i] = "! " + firstxml.Items[i];
                    }
                }
            }
        }
        public class PassConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (value as string)?.Contains("Pass");
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
        public class FailConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (value as string)?.Contains("Fail");
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (File.Exists(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\HTML\3DViewer\Help3Dviewer.html"))
            {
                Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\HTML\3DViewer\Help3Dviewer.html");
            }
            else
            {
                if (OneDrive == true)
                {
                    //MessageBox.Show(System.IO.Path.Combine(onedrivedir, @"Clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\HTML\3DViewer\Help3Dviewer.html"));
                    Process.Start(System.IO.Path.Combine(onedrivedir, @"Clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process", @"HTML", @"3DViewer", @"Help3Dviewer.html"));
                }
                else
                {
                    Process.Start(@"C:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\HTML\3DViewer\Help3Dviewer.html");
                }
            }

        }

        static bool checkifprocessis(string program)
        {
            bool jestprogram = false;

            Process[] processes = Process.GetProcesses();
            foreach (Process proces in processes)
            {
                if (proces.ProcessName == program)
                {
                    jestprogram = true;
                    break;
                }
            }
            return jestprogram;
        }

        void pokazmodel3D()
        {
            viewPort3d.Children.Remove(device3D_next);
            tworzenieSTLa(stlpart.Text);
            //sprawdzenie czy catia0.exe skonczyla
            do//dopoki dziala ctaia0.exe czekaj!!
            {
                //pokazdanezexcela.Items.Add(checkifprocessis("Catia0"));
            }
            while (checkifprocessis("Catia0"));
            show3DmodelfromTextBox(stlpart.Text);
        }

        private void button_pokazmodel_Click(object sender, RoutedEventArgs e)
        {
            firstxml.Visibility = Visibility.Hidden;
            secondxml.Visibility = Visibility.Hidden;
            //*************************************************************************
            //sprawdza czy Catia jest uruchomiona
            //*************************************************************************
            Process[] pname = Process.GetProcessesByName("CNEXT");
            if (pname.Length == 0)
            {
                MessageBox.Show("Prosze uruchomic CATIE!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Mouse.OverrideCursor = Cursors.Wait;
                pokazmodel3D();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        void nie_ruszaj()
        {
            MessageBox.Show("NIE KLIKAJ!!!!!!", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wpisz order", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_2(object sender, KeyEventArgs e)
        {

        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                //MessageBox.Show("klikneles F1", "", MessageBoxButton.OK, MessageBoxImage.Information);
                //tutorial
                if (File.Exists(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\HTML\3DViewer\Help3Dviewer.html"))
                {
                    Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\HTML\3DViewer\Help3Dviewer.html");
                }
                else
                {
                    Process.Start(@"C:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\HTML\3DViewer\Help3Dviewer.html");
                }
            }

            if (e.Key == Key.Escape)
            {
                //MessageBox.Show("klikneles ESC", "", MessageBoxButton.OK, MessageBoxImage.Information);
                //Button_Click_2();
                Przerwij.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }

            if (e.Key == Key.Enter)
            {
                //MessageBox.Show("klikneles Enter", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Button_startprocess.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }

        }

        List<string> createlistof(string navigator, string atribute1, string atribute2, string prefix, string infile)
        {
            try
            {
                //create list
                XmlDocument document = new XmlDocument();
                document.Load(infile);
                XPathNavigator navigator1 = document.CreateNavigator();
                XPathNodeIterator nodes1 = navigator1.Select(navigator);
                string line;
                while (nodes1.MoveNext())
                {
                    if (prefix != "")
                    {
                        line = nodes1.Current.GetAttribute(atribute1, "") + prefix;
                        globstrlist.Add(line);
                        line = nodes1.Current.GetAttribute(atribute2, "");
                        globstrlist.Add(line);
                    }
                    else
                    {
                        line = nodes1.Current.GetAttribute(atribute1, "");
                        globstrlist.Add(line);
                        line = nodes1.Current.GetAttribute(atribute2, "");
                        globstrlist.Add(line);
                    }
                }
                return globstrlist;
            }
            catch (Exception e)
            {
                throw new Exception("ERROR in XML file, check file or function createlistof", e);
            }
        }
        static string readelmentfromlist(string element)
        {

            string stringToCheck = element;
            string result = "";
            string name = globstrlist.Find(item => item == stringToCheck);
            int nominalvalue = globstrlist.IndexOf(stringToCheck);

            if (name == element)
            {
                result = globstrlist[nominalvalue + 1];
            }
            else
            {
                result = "-";
            }

            if (result == "")
            {
                MessageBox.Show("This parameter doesn't exist", "", MessageBoxButton.OK, MessageBoxImage.Information);
                result = "-";
                //Application.Exit();
            }
            return result;

        }

        void wyciagnijdrugixml(string cbmtemplatedir)
        {
            //szukanie xmla w templacie
            string dirbmtemplatefile = System.IO.Path.GetDirectoryName(bmtemplatefile.Text);
            if (Directory.Exists(dirbmtemplatefile))
            {
                string[] files = Directory.GetFiles(dirbmtemplatefile, "*BMD.xml", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    if (name.Contains("BMD.xml"))
                    {
                        //MessageBox.Show(name.ToString(), "BMD.xml", MessageBoxButton.OK, MessageBoxImage.Information);
                        drugibmdxmlplik = System.IO.Path.Combine(dirbmtemplatefile, name);
                    }
                }
            }
        }

        void zroblistyzxmlow(string plikbmdxml1, string plikbmdxml2)
        {
            //---------------------------------------------------------------------
            //dane z pierwszego xmla 
            //---------------------------------------------------------------------
            globstrlist.Clear();
            //MessageBox.Show(plikbmdxml.ToString(), "plikbmdxml", MessageBoxButton.OK, MessageBoxImage.Information); 
            if (File.Exists(plikbmdxml1) && plikbmdxml1.Contains(".xml"))
            {
                createlistfromstructure("Name", "/BPMManufacturingData/Header/Part", "Typ_lop", plikbmdxml1);
                createlistfromstructure("ID", "/BPMManufacturingData/Header/Part", "Rysunek", plikbmdxml1);
                createlistfromstructure("Name", "/BPMManufacturingData/Header/Part/StandardRawMaterial", "Material", plikbmdxml1);
                createlistfromsinglestructure("BladeOrientation", "/BPMManufacturingData/BladeTopology", "Strumien", plikbmdxml1);
                createlistof("/BPMManufacturingData/Quality/Dimensions/LengthDimension", "Name", "NominalValue", "", plikbmdxml1);
                createlistof("/BPMManufacturingData/Quality/Dimensions/RadiusDimension", "Name", "NominalValue", "", plikbmdxml1);
                createlistof("/BPMManufacturingData/Quality/Dimensions/AngleDimension", "Name", "NominalValue", "", plikbmdxml1);
                createlistof("/BPMManufacturingData/Quality/Dimensions/DiameterDimension", "Name", "NominalValue", "", plikbmdxml1);
                createlistof("/BPMManufacturingData/BladeTopology/MainFunctionElement/FunctionElement/ControlPara", "Name", "Value", "", plikbmdxml1);
                createlistfromstructure("Type", "/BPMManufacturingData/BladeTopology/MainFunctionElement", "BPMTYP", plikbmdxml1);
            }

            //MessageBox.Show(getairfoiltype(readelmentfromlist("Typ_lop")), "Pioro", MessageBoxButton.OK, MessageBoxImage.Information);
            //MessageBox.Show((readelmentfromlist("BPMTYP")), "BPMTYP", MessageBoxButton.OK, MessageBoxImage.Information);

            danezpierwszegoxmla.Clear();
            //wypelnienie lsity dla pierwszego xmla
            if (globstrlist.Count > 0)
            {
                //RTB KIEROWNICA
                if (readelmentfromlist("BPMTYP") == "RTBFixedBlade")
                {
                    danezpierwszegoxmla.Add("Rysunek" + "=" + readelmentfromlist("Rysunek"));
                    danezpierwszegoxmla.Add("Material" + "=" + readelmentfromlist("Material"));
                    danezpierwszegoxmla.Add("TypPiora" + "=" + getairfoiltype(readelmentfromlist("Typ_lop")));
                    danezpierwszegoxmla.Add("Strumien" + "=" + readelmentfromlist("Strumien"));
                    danezpierwszegoxmla.Add("PAFORM" + "=" + readelmentfromlist("PAFORM"));
                    danezpierwszegoxmla.Add("BPA" + "=" + readelmentfromlist("BPA"));
                    danezpierwszegoxmla.Add("FDFORM" + "=" + readelmentfromlist("FDFORM"));
                    danezpierwszegoxmla.Add("FNFORM" + "=" + readelmentfromlist("FNFORM"));
                    danezpierwszegoxmla.Add("FZFORM" + "=" + readelmentfromlist("FZFORM"));
                    danezpierwszegoxmla.Add("PartName" + "=" + readelmentfromlist("Typ_lop"));
                    danezpierwszegoxmla.Add("RBN" + "=" + readelmentfromlist("RBN"));
                    danezpierwszegoxmla.Add("RBZ" + "=" + readelmentfromlist("RBZ"));
                    danezpierwszegoxmla.Add("RE" + "=" + readelmentfromlist("RE"));
                    danezpierwszegoxmla.Add("HE" + "=" + readelmentfromlist("HE"));
                }
                //RTB WIRNIK
                if (readelmentfromlist("BPMTYP") == "RTBMovingBlade")
                {
                    danezpierwszegoxmla.Add("Rysunek" + "=" + readelmentfromlist("Rysunek"));
                    danezpierwszegoxmla.Add("Material" + "=" + readelmentfromlist("Material"));
                    danezpierwszegoxmla.Add("TypPiora" + "=" + getairfoiltype(readelmentfromlist("Typ_lop")));
                    danezpierwszegoxmla.Add("Strumien" + "=" + readelmentfromlist("Strumien"));
                    danezpierwszegoxmla.Add("PAFORM" + "=" + readelmentfromlist("PAFORM"));
                    danezpierwszegoxmla.Add("FDFORM" + "=" + readelmentfromlist("FDFORM"));
                    danezpierwszegoxmla.Add("FNFORM" + "=" + readelmentfromlist("FNFORM"));
                    danezpierwszegoxmla.Add("FZFORM" + "=" + readelmentfromlist("FZFORM"));
                    danezpierwszegoxmla.Add("HHU" + "=" + readelmentfromlist("HHU"));
                    danezpierwszegoxmla.Add("HH" + "=" + readelmentfromlist("HH"));
                    danezpierwszegoxmla.Add("RSU" + "=" + readelmentfromlist("RSU"));
                    danezpierwszegoxmla.Add("RS" + "=" + readelmentfromlist("RS"));
                    danezpierwszegoxmla.Add("RSA" + "=" + readelmentfromlist("RSA"));
                    danezpierwszegoxmla.Add("RSB" + "=" + readelmentfromlist("RSB"));
                    danezpierwszegoxmla.Add("RSC" + "=" + readelmentfromlist("RSC"));
                    danezpierwszegoxmla.Add("RSK" + "=" + readelmentfromlist("RSK"));
                    danezpierwszegoxmla.Add("RBN" + "=" + readelmentfromlist("RBN"));
                    danezpierwszegoxmla.Add("RBZ" + "=" + readelmentfromlist("RBZ"));
                }
            }
            //---------------------------------------------------------------------
            //dane z drugiego xmla templata
            //---------------------------------------------------------------------
            globstrlist.Clear();
            //MessageBox.Show(plikbmdxml.ToString(), "plikbmdxml", MessageBoxButton.OK, MessageBoxImage.Information); 
            if (File.Exists(plikbmdxml2) && plikbmdxml2.Contains(".xml"))
            {
                createlistfromstructure("Name", "/BPMManufacturingData/Header/Part", "Typ_lop", plikbmdxml2);
                createlistfromstructure("ID", "/BPMManufacturingData/Header/Part", "Rysunek", plikbmdxml2);
                createlistfromstructure("Name", "/BPMManufacturingData/Header/Part/StandardRawMaterial", "Material", plikbmdxml2);
                createlistfromsinglestructure("BladeOrientation", "/BPMManufacturingData/BladeTopology", "Strumien", plikbmdxml2);
                createlistof("/BPMManufacturingData/Quality/Dimensions/LengthDimension", "Name", "NominalValue", "", plikbmdxml2);
                createlistof("/BPMManufacturingData/Quality/Dimensions/RadiusDimension", "Name", "NominalValue", "", plikbmdxml2);
                createlistof("/BPMManufacturingData/Quality/Dimensions/AngleDimension", "Name", "NominalValue", "", plikbmdxml2);
                createlistof("/BPMManufacturingData/Quality/Dimensions/DiameterDimension", "Name", "NominalValue", "", plikbmdxml2);
                createlistof("/BPMManufacturingData/BladeTopology/MainFunctionElement/FunctionElement/ControlPara", "Name", "Value", "", plikbmdxml2);
                createlistfromstructure("Type", "/BPMManufacturingData/BladeTopology/MainFunctionElement", "BPMTYP", plikbmdxml2);
            }

            //MessageBox.Show(readelmentfromlist("Typ_lop"), "Typ_lop", MessageBoxButton.OK, MessageBoxImage.Information);
            //MessageBox.Show(getairfoiltype(readelmentfromlist("Typ_lop")), "Pioro", MessageBoxButton.OK, MessageBoxImage.Information);

            danezdrugiegoxmla.Clear();
            //wypelnienie lsity dla pierwszego xmla
            if (globstrlist.Count > 0)
            {
                //RTB KIEROWNICA
                if (readelmentfromlist("BPMTYP") == "RTBFixedBlade")
                {
                    danezdrugiegoxmla.Add("Rysunek" + "=" + readelmentfromlist("Rysunek"));
                    danezdrugiegoxmla.Add("Material" + "=" + readelmentfromlist("Material"));
                    danezdrugiegoxmla.Add("TypPiora" + "=" + getairfoiltype(readelmentfromlist("Typ_lop")));
                    danezdrugiegoxmla.Add("Strumien" + "=" + readelmentfromlist("Strumien"));
                    danezdrugiegoxmla.Add("PAFORM" + "=" + readelmentfromlist("PAFORM"));
                    danezdrugiegoxmla.Add("BPA" + "=" + readelmentfromlist("BPA"));
                    danezdrugiegoxmla.Add("FDFORM" + "=" + readelmentfromlist("FDFORM"));
                    danezdrugiegoxmla.Add("FNFORM" + "=" + readelmentfromlist("FNFORM"));
                    danezdrugiegoxmla.Add("FZFORM" + "=" + readelmentfromlist("FZFORM"));
                    danezdrugiegoxmla.Add("PartName" + "=" + readelmentfromlist("Typ_lop"));
                    danezdrugiegoxmla.Add("RBN" + "=" + readelmentfromlist("RBN"));
                    danezdrugiegoxmla.Add("RBZ" + "=" + readelmentfromlist("RBZ"));
                    danezdrugiegoxmla.Add("RE" + "=" + readelmentfromlist("RE"));
                    danezdrugiegoxmla.Add("HE" + "=" + readelmentfromlist("HE"));
                }
                //RTB WIRNIK
                if (readelmentfromlist("BPMTYP") == "RTBMovingBlade")
                {
                    danezdrugiegoxmla.Add("Rysunek" + "=" + readelmentfromlist("Rysunek"));
                    danezdrugiegoxmla.Add("Material" + "=" + readelmentfromlist("Material"));
                    danezdrugiegoxmla.Add("TypPiora" + "=" + getairfoiltype(readelmentfromlist("Typ_lop")));
                    danezdrugiegoxmla.Add("Strumien" + "=" + readelmentfromlist("Strumien"));
                    danezdrugiegoxmla.Add("PAFORM" + "=" + readelmentfromlist("PAFORM"));
                    danezdrugiegoxmla.Add("FDFORM" + "=" + readelmentfromlist("FDFORM"));
                    danezdrugiegoxmla.Add("FNFORM" + "=" + readelmentfromlist("FNFORM"));
                    danezdrugiegoxmla.Add("FZFORM" + "=" + readelmentfromlist("FZFORM"));
                    danezdrugiegoxmla.Add("HHU" + "=" + readelmentfromlist("HHU"));
                    danezdrugiegoxmla.Add("HH" + "=" + readelmentfromlist("HH"));
                    danezdrugiegoxmla.Add("RSU" + "=" + readelmentfromlist("RSU"));
                    danezdrugiegoxmla.Add("RS" + "=" + readelmentfromlist("RS"));
                    danezdrugiegoxmla.Add("RSA" + "=" + readelmentfromlist("RSA"));
                    danezdrugiegoxmla.Add("RSB" + "=" + readelmentfromlist("RSB"));
                    danezdrugiegoxmla.Add("RSC" + "=" + readelmentfromlist("RSC"));
                    danezdrugiegoxmla.Add("RSK" + "=" + readelmentfromlist("RSK"));
                    danezdrugiegoxmla.Add("RBN" + "=" + readelmentfromlist("RBN"));
                    danezdrugiegoxmla.Add("RBZ" + "=" + readelmentfromlist("RBZ"));
                }
            }

        }

        static string getairfoiltype(string typ)
        {
            airfoiltype = typ;
            string[] filtertext = airfoiltype.Split(new char[] { '-' });//HDT8020-48/H2F020L RH 1 / RCCD9035-50/R2F035R RH10
            foreach (string element in filtertext)
            {
                if (element.Contains("80"))
                {
                    airfoiltype = "8000";
                }
                if (element.Contains("90"))
                {
                    airfoiltype = "9000";
                }
            }
            return airfoiltype;
        }

        private void firstxml_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            zabijguwno("mspaint", true);
            zabijguwno("PaintStudio.View", true);
            zabijguwno("Microsoft.Photos", true);
            if (firstxml.SelectedItem != null)
            {
                //MessageBox.Show(firstxml.SelectedItem.ToString());
                if (firstxml.SelectedItem.ToString().Contains("RSU=") || firstxml.SelectedItem.ToString().Contains("HH="))
                {
                    if (isexistrootengdirnet == true)
                    {
                        Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\HH_RSU.png");
                    }
                    else
                    {
                        if (OneDrive == true)
                        {
                            Process.Start(System.IO.Path.Combine(onedrivedir, @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\HH_RSU.png"));
                        }
                        else
                        {
                            Process.Start(@"c:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\HH_RSU.png");
                        }
                    }
                }
                if (firstxml.SelectedItem.ToString().Contains("RS=") || firstxml.SelectedItem.ToString().Contains("HHU="))
                {
                    if (isexistrootengdirnet == true)
                    {
                        Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\RS_HHU.png");
                    }
                    else
                    {
                        if (OneDrive == true)
                        {
                            Process.Start(System.IO.Path.Combine(onedrivedir, @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\RS_HHU.png"));
                        }
                        else
                        {
                            Process.Start(@"c:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\RS_HHU.png");
                        }
                    }
                }
                if (firstxml.SelectedItem.ToString().Contains("HE=") || firstxml.SelectedItem.ToString().Contains("RE="))
                {
                    if (isexistrootengdirnet == true)
                    {
                        Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\HE_RE.png");
                    }
                    else
                    {
                        if (OneDrive == true)
                        {
                            Process.Start(System.IO.Path.Combine(onedrivedir, @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\HE_RE.png"));
                        }
                        else
                        {
                            Process.Start(@"c:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\HE_RE.png");
                        }
                    }
                }
                if (firstxml.SelectedItem.ToString().Contains("RSB=") || firstxml.SelectedItem.ToString().Contains("RSC="))
                {
                    if (isexistrootengdirnet == true)
                    {
                        Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\RSB_RSC.png");
                    }
                    else
                    {
                        if (OneDrive == true)
                        {
                            Process.Start(System.IO.Path.Combine(onedrivedir, @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\RSB_RSC.png"));
                        }
                        else
                        {
                            Process.Start(@"c:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\RSB_RSC.png");
                        }
                    }
                }
                if (firstxml.SelectedItem.ToString().Contains("BPA=") || firstxml.SelectedItem.ToString().Contains("PAFORM="))
                {
                    if (isexistrootengdirnet == true)
                    {
                        if (bpmtype.Text == "RTBFixedBlade")
                        {
                            Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\BPA_PAFORM_RTBfix.png");
                        }
                        else
                        {
                            if (OneDrive == true)
                            {
                                Process.Start(System.IO.Path.Combine(onedrivedir, @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\BPA_PAFORM_RTBfix.png"));
                            }
                            else
                            {
                                Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\PAFORMy_RTBmov.png");
                            }
                        }
                    }
                    else
                    {
                        if (bpmtype.Text == "RTBFixedBlade")
                        {
                            Process.Start(@"C:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\BPA_PAFORM_RTBfix.png");
                        }
                        else
                        {
                            if (OneDrive == true)
                            {
                                Process.Start(System.IO.Path.Combine(onedrivedir, @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\BPA_PAFORM_RTBfix.png"));
                            }
                            else
                            {
                                Process.Start(@"C:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\PAFORMy_RTBmov.png");
                            }
                        }
                    }
                }
                if (firstxml.SelectedItem.ToString().Contains("FDFORM=") || firstxml.SelectedItem.ToString().Contains("FNFORM="))
                {
                    if (isexistrootengdirnet == true)
                    {
                        Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\FDFORM_FNFORM.png");
                    }
                    else
                    {
                        if (OneDrive == true)
                        {
                            Process.Start(System.IO.Path.Combine(onedrivedir, @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\FDFORM_FNFORM.png"));
                        }
                        else
                        {
                            Process.Start(@"c:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\FDFORM_FNFORM.png");
                        }
                    }
                }
                if (firstxml.SelectedItem.ToString().Contains("RBZ") || firstxml.SelectedItem.ToString().Contains("RBN="))
                {
                    if (isexistrootengdirnet == true)
                    {
                        Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\RBZ_RBN.png");
                    }
                    else
                    {
                        if (OneDrive == true)
                        {
                            Process.Start(System.IO.Path.Combine(onedrivedir, @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\RBZ_RBN.png"));
                        }
                        else
                        {
                            Process.Start(@"c:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\RBZ_RBN.png");
                        }
                    }
                }
                if (firstxml.SelectedItem.ToString().Contains("FZFORM"))
                {
                    if (isexistrootengdirnet == true)
                    {
                        Process.Start(@"U:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\FZFORM.png");
                    }
                    else
                    {
                        if (OneDrive == true)
                        {
                            Process.Start(System.IO.Path.Combine(onedrivedir, @"clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\FZFORM.png"));
                        }
                        else
                        {
                            Process.Start(@"c:\clever\V300\BladeMill\BladeMillServer\BladeMillScripts\Process\GIFs\FZFORM.png");
                        }
                    }
                }
            }
        }

        private bool CheckPolishLetter(string text)
        {
            polishletter = false;
            char[] polishchars = { 'ę', 'ó', 'ą', 'ś', 'ł', 'ż', 'ź', 'ć', 'ń', 'Ę', 'Ó', 'Ą', 'Ś', 'Ł', 'Ż', 'Ź', 'Ć', 'Ń' };
            foreach (char element in polishchars)
            {
                if (text.Contains(element.ToString()))
                {
                    polishletter = true;
                    return polishletter;
                }
            }
            return polishletter;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("2023-01-18 dodanie TypeOfProcess\n" +
                            "2022-03-29 dodanie Ltol_HE, Utol_HE i resize\n" +
                            "2022-03-21 ostrzezenie o blednym templacie\n" +
                            "2022-02-17 odblokowanie nazwy ordera gdy pomiar\n" +
                            "2022-01-26 dodanie nowych mocowan dla Hurona\n" +
                            "2021-11-30 dodanie o polskich znakach w pojekcie\n" +
                            "2021-09-30 dodanie dysku S\n" +
                            "2021-08-30 dodanie FIG_N\n" +
                            "2021-07-26 dodanie BM 3.20\n" +
                            "2021-06-29 dodanie ostrzezenia o templacie lewa-prawa\n" +
                            "2021-05-24 zmiana uwagi o roznych templatach\n" +
                            "2021-04-07 zabezpieczenie do mocowan nowych\n" +
                            "2021-03-17 poprawienie czytania mocowania dla RTBwir\n" +
                            "2021-03-04 dodanie sprawdzenia mocowania z templata\n" +
                            "2021-02-19 dodano Moc_band\n" +
                            "2021-01-29 poprawa czytania mocowania z excela\n" +
                            "2021-01-15 dodanie mocowania GripZabierak\n" +
                            "2020-12-09 dodanie przycisku ENTER i ESC\n" +
                            "2020-11-25 dodanie FN\n" +
                            "2020-11-20 dodanie admina\n" +
                            "2020-11-04 dodanie mocowania GripGrip\n" +
                            "2020-10-21 dodanie czytania excela dla osiowej\n" +
                            "2020-10-19 gdy brak excela nie czyta go\n" +
                            "2020-10-13 dodanie okienek gdy uzywamy szablonu BM\n" +
                            "2020-09-25 zmiany w okienkach Dane z xml i excel\n" +
                            "2020-09-23 dodanie HSTM300HD, dziala tylko u admina\n" +
                            "2020-08-19 dodanie middletol\n" +
                            "2020-08-18 poprawienie wyboru mocowania dla osiowej\n" +
                            "2020-06-17 dodanie okna 3D\n" +
                            "2020-06-08 stworzenie programu\n" +
                            "", "Zmiany (tylko informacja)", MessageBoxButton.OK, MessageBoxImage.Information);
            //dodac tutaj tutorial
        }

        private void runconfiguration_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}