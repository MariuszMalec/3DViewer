using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using BladeMill.BLL.Services;

namespace StartWindow.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        public string _inputdata = @"C:\temp\inputdata.xml";
        private InputDataXml _currentDataFromInputXml { get; set; }
        public InputDataXml CurrentDataFromInputXml
        {
            get { return _currentDataFromInputXml; }
            set { _currentDataFromInputXml = value; OnPropertyChanged(nameof(CurrentDataFromInputXml)); }
        }
        private AppXmlConfDirectories _currentAppXmlConfDirectories { get; set; }
        public AppXmlConfDirectories CurrentAppXmlConfDirectories
        {
            get { return _currentAppXmlConfDirectories; }
            set { _currentAppXmlConfDirectories = value; OnPropertyChanged(nameof(CurrentAppXmlConfDirectories)); }
        }

        public BaseViewModel()
        {
            CurrentDataFromInputXml = new InputDataXml();
            LoadData();

        }
        private void LoadData()
        {
            CurrentAppXmlConfDirectories = new AppXmlConfDirectories();//TODO set rootengdir and rootmfgdir

            var inputDataXml = _inputdata;
            var inputDataXmlService = new XMLInputFileService();
            var xmlService = new XmlService(inputDataXmlService);
            
            CurrentDataFromInputXml.catpart = xmlService.GetFromFileValue(inputDataXml, InputXmlEnum.catpart.ToString());
            CurrentDataFromInputXml.xmlpart = xmlService.GetFromFileValue(inputDataXml, InputXmlEnum.xmlpart.ToString());
            CurrentDataFromInputXml.xlspart = xmlService.GetFromFileValue(inputDataXml, InputXmlEnum.xlspart.ToString());
            //
            BladeMill.BLL.Services.InputDataXmlService _inputDataXmlService = new BladeMill.BLL.Services.InputDataXmlService();
            var inputData = _inputDataXmlService.GetDataFromInputDataXml();
            CurrentDataFromInputXml.BMTemplateFile = inputData.BMTemplateFile;
            CurrentDataFromInputXml.BMTemplate = inputData.BMTemplate;
            CurrentDataFromInputXml.catpartfirst = inputData.catpartfirst;
            CurrentDataFromInputXml.catpartend = inputData.catpartend;
            CurrentDataFromInputXml.xmlpartfirst = inputData.xmlpartfirst;
            CurrentDataFromInputXml.xmlpartend = inputData.xmlpartend;
            CurrentDataFromInputXml.ClampFromTemplate = inputData.ClampFromTemplate;
            CurrentDataFromInputXml.Clampingmethod = inputData.Clampingmethod;
            CurrentDataFromInputXml.clickcancel = inputData.clickcancel;
            CurrentDataFromInputXml.createraport = inputData.createraport;
            CurrentDataFromInputXml.createvcproject = inputData.createvcproject;
            CurrentDataFromInputXml.FIG_N = inputData.FIG_N;
            CurrentDataFromInputXml.machine = inputData.machine;
            CurrentDataFromInputXml.middleTol = inputData.middleTol;
            CurrentDataFromInputXml.runconfiguration = inputData.runconfiguration;
            CurrentDataFromInputXml.runbm = inputData.runbm;
            CurrentDataFromInputXml.runcmm = inputData.runcmm;
            CurrentDataFromInputXml.readxls = inputData.readxls;
            CurrentDataFromInputXml.TypeBlade = inputData.TypeBlade;
            CurrentDataFromInputXml.TypeOfProcess = inputData.TypeOfProcess;
            CurrentDataFromInputXml.infile = inputData.infile;
            CurrentDataFromInputXml.Prerawbox = inputData.Prerawbox;
            CurrentDataFromInputXml.outfile = _inputdata;
            CurrentDataFromInputXml.pinwelding = inputData.pinwelding;
            CurrentDataFromInputXml.millshroud = inputData.millshroud;
            CurrentDataFromInputXml.selectlanguage = "pol";//TODO fix value
            CurrentDataFromInputXml.RootMfgDir = inputData.RootMfgDir;
            CurrentDataFromInputXml.IsXML = "True";//TODO fix value
            CurrentDataFromInputXml.admin = "False";//TODO fix value
        }
    }
}
