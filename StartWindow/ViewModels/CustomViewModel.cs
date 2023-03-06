using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Prism.Commands;
using StartWindow.Enums;
using StartWindow.Models;
using StartWindow.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BmdXmlFileView = StartWindow.Models.BmdXmlFileView;

namespace StartWindow.ViewModels
{
    public class CustomViewModel: BaseViewModel, ICloseWindows
    {
        public string _inputdata = @"C:\temp\inputdata.xml";
        public List<bool> mistake = new List<bool>() { };

        private string _catPart;                

        private string _bmtemplatefile;

        private string _currentUser;
        public string CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; OnPropertyChanged(nameof(CurrentUser)); }
        }

        private bool _isSelectedRunConfig;
        public bool IsSelectedRunConfigurationCommand
        {
            get { return _isSelectedRunConfig; }
            set
            {
                if (_isSelectedRunConfig == value) return;
                _isSelectedRunConfig = value;

                OnPropertyChanged(nameof(IsSelectedRunConfigurationCommand));
            }
        }

        private bool _isSelectedRunBm;
        public bool IsSelectedRunBmCommand
        {
            get { return _isSelectedRunBm; }
            set
            {
                if (_isSelectedRunBm == value) return;
                _isSelectedRunBm = value;

                OnPropertyChanged(nameof(IsSelectedRunBmCommand));
            }
        }

        private bool _isSelectedRunCmm;
        public bool IsSelectedRunCmmCommand
        {
            get { return _isSelectedRunCmm; }
            set
            {
                if (_isSelectedRunCmm == value) return;
                _isSelectedRunCmm = value;

                OnPropertyChanged(nameof(IsSelectedRunCmmCommand));
            }
        }

        private bool _isSelectedCreateStls;
        public bool IsSelectedCreateStlsCommand
        {
            get { return _isSelectedCreateStls; }
            set
            {
                if (_isSelectedCreateStls == value) return;
                _isSelectedCreateStls = value;

                OnPropertyChanged(nameof(IsSelectedCreateStlsCommand));
            }
        }

        private bool _isSelectedPreRawBox;
        public bool IsSelectedPreRawBoxCommand
        {
            get { return _isSelectedPreRawBox; }
            set
            {
                if (_isSelectedPreRawBox == value) return;
                _isSelectedPreRawBox = value;

                OnPropertyChanged(nameof(IsSelectedPreRawBoxCommand));
            }
        }

        private bool _isSelectedCreateRaport;
        public bool IsSelectedCreateRaportCommand
        {
            get { return _isSelectedCreateRaport; }
            set
            {
                if (_isSelectedCreateRaport == value) return;
                _isSelectedCreateRaport = value;

                OnPropertyChanged(nameof(IsSelectedCreateRaportCommand));
            }
        }

        private bool _isSelectedUseBmTemplate;
        public bool IsSelectedUseBmTemplateCommand
        {
            get { return _isSelectedUseBmTemplate; }
            set
            {
                if (_isSelectedUseBmTemplate == value) return;
                _isSelectedUseBmTemplate = value;

                OnPropertyChanged(nameof(IsSelectedUseBmTemplateCommand));
            }
        }

        private bool _isSelectedNoXls;
        public bool IsSelectedNoXlsCommand
        {
            get { return _isSelectedNoXls; }
            set
            {
                if (_isSelectedNoXls == value) return;
                _isSelectedNoXls = value;

                OnPropertyChanged(nameof(IsSelectedNoXlsCommand));
            }
        }

        private bool _isSelectedMiddleTol;
        public bool IsSelectedMiddleTolCommand
        {
            get { return _isSelectedMiddleTol; }
            set
            {
                if (_isSelectedMiddleTol == value) return;
                _isSelectedMiddleTol = value;

                OnPropertyChanged(nameof(IsSelectedMiddleTolCommand));
            }
        }

        private bool _isSelectedPinwelding;
        public bool IsSelectedPinweldingCommand
        {
            get { return _isSelectedPinwelding; }
            set
            {
                if (_isSelectedPinwelding == value) return;
                _isSelectedPinwelding = value;

                OnPropertyChanged(nameof(IsSelectedPinweldingCommand));
            }
        }

        private bool _isSelectedMillShroud;
        public bool IsSelectedMillShroudCommand
        {
            get { return _isSelectedMillShroud; }
            set
            {
                if (_isSelectedMillShroud == value) return;
                _isSelectedMillShroud = value;

                OnPropertyChanged(nameof(IsSelectedMillShroudCommand));
            }
        }

        private string _startEndFixBladeDataUpdateVisibility;
        public string StartEndFixBladeDataUpdateVisibility
        {
            get => _startEndFixBladeDataUpdateVisibility;
            set
            {
                _startEndFixBladeDataUpdateVisibility = value;
                OnPropertyChanged(nameof(StartEndFixBladeDataUpdateVisibility));
            }
        }

        private string _useBmTemplateUpdateVisibility;
        public string UseBmTemplateUpdateVisibility
        {
            get => _useBmTemplateUpdateVisibility;
            set
            {
                _useBmTemplateUpdateVisibility = value;
                OnPropertyChanged(nameof(UseBmTemplateUpdateVisibility));
            }
        }

        private string _noExcelUpdateVisibility;
        public string NoExcelUpdateVisibility
        {
            get => _noExcelUpdateVisibility;
            set
            {
                _noExcelUpdateVisibility = value;
                OnPropertyChanged(nameof(NoExcelUpdateVisibility));
            }
        }

        private List<string> _machineCategory;
        public List<string> MachineCategory
        {
            get
            {
                return _machineCategory;
            }
            set
            {
                _machineCategory = value;
                OnPropertyChanged(nameof(MachineCategory));
            }
        }

        private List<string> _clampingCategory;
        public List<string> ClampingCategory
        {
            get
            {
                return _clampingCategory;
            }
            set
            {
                _clampingCategory = value;
                OnPropertyChanged(nameof(ClampingCategory));
            }
        }

        private List<BmdXmlFileView> _bmdXmlItems;
        public List<BmdXmlFileView> BmdXmlItems
        {
            get
            {
                return _bmdXmlItems;
            }
            set
            {
                _bmdXmlItems = value;
                OnPropertyChanged(nameof(BmdXmlItems));
            }
        }

        private List<TechnologyXlsFile> _xlsItems;
        public List<TechnologyXlsFile> XlsItems
        {
            get
            {
                return _xlsItems;
            }
            set
            {
                _xlsItems = value;
                OnPropertyChanged(nameof(XlsItems));
            }
        }

        private List<BmdXmlFileView> _bmdXmlTemplateItems;
        public List<BmdXmlFileView> BmdXmlTemplateItems
        {
            get
            {
                return _bmdXmlTemplateItems;
            }
            set
            {
                _bmdXmlTemplateItems = value;
                OnPropertyChanged(nameof(BmdXmlTemplateItems));
            }
        }

        //commands
        public ICommand CatPartCommand { get; }

        public ICommand StartProcessCommand { get; }

        public ICommand CancelProcessCommand { get; }

        public ICommand UseBmTemplateCommand { get; }

        public ICommand NoExcelCommand { get; }

        public ICommand OpenBmTemplateCommand { get; }

        public CustomViewModel()
        {

            BmdXmlItems = new List<BmdXmlFileView>() { };

            BmdXmlTemplateItems = new List<BmdXmlFileView>() { };

            XlsItems = new List<TechnologyXlsFile>() { };

            Settings();

            CatPartCommand = new ViewModelCommand(ExecuteCatPartCommand);

            StartProcessCommand = new ViewModelCommand(ExecuteStartProcessCommand);

            CancelProcessCommand = new ViewModelCommand(ExecuteCancelProcessCommand);

            UseBmTemplateCommand = new ViewModelCommand(ExecuteUseBmTemplateCommand, CanExecuteUseBmTemplateCommand);//TODO ukrycie od razu danych!

            NoExcelCommand = new ViewModelCommand(ExecuteNoExcelCommand, CanExecuteNoExcelCommand);//TODO tutaj wykonuje akcje!

            OpenBmTemplateCommand = new ViewModelCommand(ExecuteOpenBmTemplateCommand);

            ClampingCategory = ClampingService.GetAll();

            MachineCategory = Service.MachineService.GetAll();

        }

        //another version close windows
        private DelegateCommand _closeCommand;

        public DelegateCommand CloseCommand => _closeCommand ?? (_closeCommand = new DelegateCommand(CloseWindow));

        public System.Action Close { get; set;}

        public bool CanClose()
        {
            return true;
        }

        void CloseWindow()
        {
            WriteToInputDataXmlFile();
            var datas = new List<InputDataXml>();
            datas.Add(CurrentDataFromInputXml);
            Service.InputDataXmlService.CreateInputDataXml(_inputdata, datas);
            Close?.Invoke();
        }

        private void ExecuteCancelProcessCommand(object obj)
        {
            WriteToInputDataXmlFile();
            var datas = new List<InputDataXml>();
            datas.Add(CurrentDataFromInputXml);
            Service.InputDataXmlService.CreateInputDataXml(_inputdata, datas);
            App.Current.Shutdown();//TODO to slow!!!!           
        }

        private void WriteToInputDataXmlFile()
        {
            //write checkboxes
            CurrentDataFromInputXml.runconfiguration = IsSelectedRunConfigurationCommand.ToString();
            CurrentDataFromInputXml.runbm = IsSelectedRunBmCommand.ToString();
            CurrentDataFromInputXml.runcmm = IsSelectedRunCmmCommand.ToString();
            CurrentDataFromInputXml.readxls = IsSelectedNoXlsCommand.ToString();
            CurrentDataFromInputXml.Prerawbox = IsSelectedPreRawBoxCommand.ToString();
            CurrentDataFromInputXml.createvcproject = _isSelectedCreateStls.ToString();
            CurrentDataFromInputXml.BMTemplate = IsSelectedUseBmTemplateCommand.ToString();
            CurrentDataFromInputXml.createraport = IsSelectedCreateRaportCommand.ToString();
        }

        private void ExecuteOpenBmTemplateCommand(object obj)
        {

            BmdXmlTemplateItems = new List<BmdXmlFileView>() { };
            BmdXmlTemplateItems.Clear();
            BmdXmlItems = new List<BmdXmlFileView>() { };
            BmdXmlItems.Clear();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.CATPart)|*.cbm|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"C:\Clever\V300\BladeMill\data\RootMfgDir";//TODO how to set here rootengdir.Text
            _bmtemplatefile = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                _bmtemplatefile = openFileDialog.FileName;
                Mouse.OverrideCursor = Cursors.Arrow;
                if (IsSelectedUseBmTemplateCommand == true && (CurrentDataFromInputXml.TypeBlade == "RTBFixedBlade" || CurrentDataFromInputXml.TypeBlade == "RTBMovingBlade"))
                {
                    var bmdService = new BmdXmlService();
                    var templateBmdXml = BmTemplateService.GetBMXmlFromBmTemplate(_bmtemplatefile);
                    CurrentDataFromInputXml.BMTemplateFile = _bmtemplatefile;
                    var templateBladeType = bmdService.GetBmdType(templateBmdXml);

                    var currentBmdXml = CurrentDataFromInputXml.xmlpart;
                    var currentbladeType = bmdService.GetBmdType(currentBmdXml);

                    var templateClamping = CurrentDataFromInputXml.ClampFromTemplate;
                    var currentClamping = CurrentDataFromInputXml.Clampingmethod;

                    //validation
                    if (currentbladeType == templateBladeType && templateClamping == currentClamping)
                    {
                        if (File.Exists(templateBmdXml))
                        {
                            BmdXmlTemplateItems = new List<BmdXmlFileView>() { };
                            BmdXmlTemplateItems.Clear();
                            bmdService.GetAll(templateBmdXml);
                            List<string> showList = GetListParameters(templateBladeType);
                            foreach (var parameter in showList)
                            {
                                BmdXmlTemplateItems.Add(bmdService.GetByName(parameter));
                            };
                        }
                        if (File.Exists(currentBmdXml))
                        {
                            BmdXmlItems = new List<BmdXmlFileView>() { };
                            BmdXmlItems.Clear();
                            bmdService.GetAll(currentBmdXml);
                            List<string> showList = GetListParameters(currentbladeType);
                            foreach (var parameter in showList)
                            {
                                BmdXmlItems.Add(bmdService.GetByName(parameter));
                            };
                        }
                        //TODO kolorowanie na czerwono
                        if (BmdXmlItems.Count() == BmdXmlTemplateItems.Count())
                        {
                            for (int i = 0; i < BmdXmlItems.Count; i++)
                            {
                                if (BmdXmlItems[i] != null)
                                {
                                    if (BmdXmlItems[i].Value.ToString() != BmdXmlTemplateItems[i].Value.ToString())
                                    {
                                        BmdXmlItems[i].Passing = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (currentClamping != templateClamping)
                            MessageBox.Show("Nie uzywaj templata, rozne typy mocowan!!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Error);
                        if (currentbladeType != templateBladeType)
                            MessageBox.Show("Nie uzywaj templata, rozne typy lopatek!!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    OnPropertyChanged(nameof(CurrentDataFromInputXml));
                }
            }
        }

        private static List<string> GetListParameters(string bladeType)
        {
            var showList = new List<string>();
            if (bladeType == "RTBFixedBlade")
            {
                showList = new List<string>() {
                            "Drawing",
                            "Typ lopatki",
                            "Material",
                            "Strumien",
                            "PAFORM",
                            "BPA",
                            "FDFORM",
                            "FNFORM",
                            "FZFORM",
                            "RBN",
                            "RBZ",
                            "RE",
                            "HE"
                            };
            }
            if (bladeType == "RTBMovingBlade")
            {
                showList = new List<string>() {
                            "Drawing",
                            "Typ lopatki",
                            "Material",
                            "Strumien",
                            "PAFORM",
                            "FDFORM",
                            "FNFORM",
                            "FZFORM",
                            "HHU",
                            "HH",
                            "RSU",
                            "RS",
                            "RSA",
                            "RSB",
                            "RSC",
                            "RSK",
                            "RBN",
                            "RBZ"
                            };
            }
            return showList;
        }

        private bool CanExecuteUseBmTemplateCommand(object obj)
        {
            //Ukrycie danych
            if (IsSelectedUseBmTemplateCommand == false)
            {
                UseBmTemplateUpdateVisibility = EnumVisibility.Hidden.ToString();
            }
            if (IsSelectedUseBmTemplateCommand == true)
            {
                UseBmTemplateUpdateVisibility = EnumVisibility.Visible.ToString();
            }
            return true;//TODO tutaj dajac false mozna zablokowac wybor!
        }

        private bool CanExecuteNoExcelCommand(object obj)
        {
            if (IsSelectedNoXlsCommand == false)
            {
                NoExcelUpdateVisibility = EnumVisibility.Hidden.ToString();
            }
            if (IsSelectedNoXlsCommand == true)
            {
                NoExcelUpdateVisibility = EnumVisibility.Visible.ToString();
            }
            return true;//TODO tutaj dajac false mozna zablokowac wybor!
        }

        private void ExecuteNoExcelCommand(object obj)
        {
            //Ukrycie danych
            if (IsSelectedNoXlsCommand == false)
            {
                NoExcelUpdateVisibility = EnumVisibility.Hidden.ToString();
            }
            if (IsSelectedNoXlsCommand == true)
            {
                NoExcelUpdateVisibility = EnumVisibility.Visible.ToString();
            }
        }

        private void ExecuteUseBmTemplateCommand(object obj)
        {
            //Ukrycie danych
            if (IsSelectedUseBmTemplateCommand == false)
            {
                UseBmTemplateUpdateVisibility = EnumVisibility.Hidden.ToString();
            }
            if (IsSelectedUseBmTemplateCommand == true)
            {
                UseBmTemplateUpdateVisibility = EnumVisibility.Visible.ToString();
            }
        }

        private void ExecuteStartProcessCommand(object obj)
        {
            //--------------- SPRAWDZANIE --------------------
            Service.ExcelService.KillExcel();

            mistake.Clear();
            //WALIDACJE
            mistake.Add(Service.CheckProcessService.IsCatia());
            mistake.Add(Service.CheckProcessService.IsBladeMill());
            mistake.Add(Service.CheckProcessService.IsBMTemplate(IsSelectedUseBmTemplateCommand, CurrentDataFromInputXml.BMTemplateFile));

            var userService = new UserServiceWithoutDatabase();
            var userName = userService.GetUserFirstLastName();
            mistake.Add(Service.CheckProcessService.IsMiddleTol(IsSelectedMiddleTolCommand, userName, CurrentDataFromInputXml.TypeBlade));
            mistake.Add(Service.CheckProcessService.IsOrderThSameAsBmTemplate(IsSelectedUseBmTemplateCommand, CurrentDataFromInputXml.infile, CurrentDataFromInputXml.BMTemplateFile));
            mistake.Add(Service.ValidateService.BmdXml(CurrentDataFromInputXml.xmlpart, CurrentDataFromInputXml.TypeBlade));
            mistake.Add(Service.ValidateService.Xls(CurrentDataFromInputXml.xlspart, IsSelectedNoXlsCommand));
            mistake.Add(Service.ValidateService.CheckAutomatedProcess(CurrentDataFromInputXml.machine, CurrentDataFromInputXml.TypeBlade));
            mistake.Add(Service.ValidateService.CheckRadialBlade(CurrentDataFromInputXml.TypeBlade, IsSelectedRunCmmCommand));
            mistake.Add(Service.ValidateService.CantCalculateMillAndCmm(IsSelectedRunBmCommand, IsSelectedRunCmmCommand));
            mistake.Add(Service.ValidateService.MachninesNotSupported(CurrentDataFromInputXml.machine));
            mistake.Add(Service.ValidateService.TypeOfBladeNotSupported(CurrentDataFromInputXml.TypeBlade, CurrentDataFromInputXml.machine));
            mistake.Add(Service.ValidateService.NotPassClampingSystem(CurrentDataFromInputXml.machine, CurrentDataFromInputXml.Clampingmethod,
                CurrentDataFromInputXml.ClampFromTemplate, IsSelectedUseBmTemplateCommand));
            mistake.Add(Service.ValidateService.SelectCorrectClamping(CurrentDataFromInputXml.Clampingmethod, CurrentDataFromInputXml.TypeBlade,
                IsSelectedNoXlsCommand, IsSelectedPreRawBoxCommand));
            mistake.Add(Service.ValidateService.CheckIfNotExistFile(CurrentDataFromInputXml.catpart));
            mistake.Add(Service.ValidateService.CheckIfNotExistFile(CurrentDataFromInputXml.xmlpart));
            mistake.Add(Service.ValidateService.CheckIfNotExistFile(CurrentDataFromInputXml.xmlpart));
            mistake.Add(Service.ValidateService.CheckIfNotExistFile(CurrentDataFromInputXml.catpartfirst));
            mistake.Add(Service.ValidateService.CheckIfNotExistFile(CurrentDataFromInputXml.catpartend));
            mistake.Add(Service.ValidateService.CheckIfNotExistFile(CurrentDataFromInputXml.xmlpartfirst));
            mistake.Add(Service.ValidateService.CheckIfNotExistFile(CurrentDataFromInputXml.xmlpartend));
            mistake.Add(Service.ValidateService.SelectCorrectCatPart(CurrentDataFromInputXml.catpart, CurrentDataFromInputXml.xlspart, CurrentDataFromInputXml.TypeBlade,
                CurrentDataFromInputXml.catpartfirst, CurrentDataFromInputXml.catpartend, CurrentDataFromInputXml.xmlpartfirst, CurrentDataFromInputXml.xmlpartend));
            mistake.Add(Service.ValidateService.CheckCorrectOrderName(CurrentDataFromInputXml.infile, CurrentDataFromInputXml.machine,
                IsSelectedRunBmCommand, IsSelectedRunCmmCommand));

            //get errors
            var error = mistake.Any(e => e == true);
            if (error)
            {
                MessageBox.Show("Sa bledy!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //INFORMACJE
            Service.InformationService.Usebmtemplate(IsSelectedUseBmTemplateCommand);
            Service.InformationService.CheckMaterial(CurrentDataFromInputXml.xmlpart);
            bool startProcess = Service.InformationService.ReplaceExistOrder(CurrentDataFromInputXml.infile, error);
            if (startProcess && error == false)
            {
                WriteToInputDataXmlFile();
                var datas = new List<InputDataXml>();
                datas.Add(CurrentDataFromInputXml);
                Service.InputDataXmlService.CreateInputDataXml(_inputdata, datas);
                App.Current.Shutdown();
            }
        }

        private void Settings()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            var userService = new UserServiceWithoutDatabase();
            CurrentUser = userService.GetUserFirstLastName();

            IsSelectedRunConfigurationCommand = bool.Parse(CurrentDataFromInputXml.runconfiguration);
            IsSelectedRunBmCommand = bool.Parse(CurrentDataFromInputXml.runbm);
            IsSelectedRunCmmCommand = bool.Parse(CurrentDataFromInputXml.runcmm);
            IsSelectedCreateStlsCommand = true;
            IsSelectedPreRawBoxCommand = bool.Parse(CurrentDataFromInputXml.Prerawbox);
            IsSelectedCreateRaportCommand = bool.Parse(CurrentDataFromInputXml.createraport);
            IsSelectedUseBmTemplateCommand = bool.Parse(CurrentDataFromInputXml.BMTemplate);
            IsSelectedNoXlsCommand = bool.Parse(CurrentDataFromInputXml.readxls) ? true : false;//TODO trzeba zmienic na odwrot historycznie
            IsSelectedMiddleTolCommand = bool.Parse(CurrentDataFromInputXml.middleTol);
            bool flag = false;
            IsSelectedPinweldingCommand = bool.TryParse(CurrentDataFromInputXml.pinwelding, out flag);
            IsSelectedMillShroudCommand = bool.TryParse(CurrentDataFromInputXml.millshroud, out flag);

            FillBmdXmlListView();
            //FillFromExcelListView();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void FillBmdXmlListView()
        {
            if (File.Exists(CurrentDataFromInputXml.xmlpart))
            {
                BmdXmlItems.Clear();
                var currentXml = CurrentDataFromInputXml.xmlpart;
                if (File.Exists(currentXml))
                {
                    BmdXmlItems = new List<BmdXmlFileView>() { };
                    BmdXmlItems.Clear();
                    var bmdService = new BmdXmlService();
                    var bmdList = bmdService.GetAll(currentXml);
                    var bladeType = bmdService.GetBmdType(currentXml);
                    var showList = new List<string>();
                    showList = new List<string>() {
                            "Typ lopatki",
                            "Drawing",
                            "Project",
                            "Material",
                            "Strumien",
                            "Typ"
                            };
                    foreach (var parameter in showList)
                    {
                        BmdXmlItems.Add(bmdService.GetByName(parameter));
                    };
                }
            }
        }

        private void FillFromExcelListView()
        {
            if (System.IO.File.Exists(CurrentDataFromInputXml.xlspart))
            {
                XlsItems.Clear();
                var currentExcel = CurrentDataFromInputXml.xlspart;
                var bladeType = CurrentDataFromInputXml.TypeBlade;
                if (File.Exists(currentExcel))
                {
                    var excelList = Service.ExcelService.GetAll(currentExcel, bladeType);
                    var showList = new List<string>();
                    showList = new List<string>() {
                            "TYP LOP",
                            "KDKNo",
                            "ProjectName",
                            "Material",
                            "BladeOrientation",
                            "BladeType",
                            "Moc_band",
                            "A",
                            "B",
                            "C",
                            "D",
                            "L",
                            "FIG_N",
                            "Ltol_HE",
                            "Utol_HE",
                            "FN",
                            "GRIP",
                            "Zgrz_PIN",
                            "Obr_band",
                            "FIG_BAND"                            
                            };
                    if (excelList != null)
                    {
                        foreach (var parameter in showList)
                        {
                            XlsItems.Add(Service.ExcelService.GetByName(parameter));
                        };
                    }
                }
            }
        }

        private void ExecuteCatPartCommand(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.CATPart)|*.CATPart|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"C:\Clever\V300\BladeMill\data\RootEngDir";//TODO how to set here rootengdir.Text
            
            _catPart = string.Empty;            

            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                _catPart = openFileDialog.FileName;                               
            }

            CurrentDataFromInputXml.catpart = _catPart;

            // wypelnienie okienek na podstawie wybranego catparta
            if (_catPart != string.Empty)
            {
                string dircatpart = Path.GetDirectoryName(_catPart);
                string xmlname = Path.GetFileName(_catPart).Replace("_-.CATPart", "_-_BMD.xml");
                CurrentDataFromInputXml.xmlpart = Path.Combine(dircatpart, xmlname);
                //Set type of blade
                var bmdService = new XMLBmdService();
                CurrentDataFromInputXml.TypeBlade = bmdService.GetBmdType(CurrentDataFromInputXml.xmlpart);

                string xlsname = Path.GetFileName(_catPart).Replace("_-.CATPart", ".xls");
                CurrentDataFromInputXml.xlspart = Path.Combine(dircatpart, xlsname);

                //Ukrycie danych
                if (CurrentDataFromInputXml.TypeBlade != "RTBFixedBlade")
                {
                    StartEndFixBladeDataUpdateVisibility = "Hidden";
                }
                else
                {
                    StartEndFixBladeDataUpdateVisibility = "Visible";
                }

                //---------------------------------
                //Wypelnij poczatkowe i koncowe pliki
                //---------------------------------
                if (CurrentDataFromInputXml.TypeBlade == "RTBFixedBlade" && CurrentDataFromInputXml.TypeBlade != "unknown")
                {
                    CurrentDataFromInputXml.catpartfirst = SetStartEndBladeService.SetTextBox(_catPart, "PARTSTART");
                    CurrentDataFromInputXml.catpartend = SetStartEndBladeService.SetTextBox(_catPart, "PARTEND");
                    CurrentDataFromInputXml.xmlpartfirst = SetStartEndBladeService.SetTextBox(_catPart, "XMLSTART");
                    CurrentDataFromInputXml.xmlpartend = SetStartEndBladeService.SetTextBox(_catPart, "XMLEND");
                }

                BmdXmlItems = new List<BmdXmlFileView>() { };
                FillBmdXmlListView();
                XlsItems = new List<TechnologyXlsFile>() { };
                FillFromExcelListView();

                //Set datas from technology
                CurrentDataFromInputXml.Clampingmethod = ExcelService.GetCurrentClamping(XlsItems);

                OnPropertyChanged(nameof(CurrentDataFromInputXml));

                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }

}
