namespace StartWindow.ViewModels
{
    internal interface ICloseWindows
    {
        System.Action Close { get; set; }
        bool CanClose();
    }
}