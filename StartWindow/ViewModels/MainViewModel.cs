using BladeMill.BLL.Models;
using Microsoft.Win32;
using StartWindow.Views;
using System;
using System.Windows.Input;

namespace StartWindow.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _catPart;

        private ViewModelBase _currentChildView;

        public ViewModelBase CurrentChildView
        {
            get { return _currentChildView; }
            set { _currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); }
        }

        //commands
        public ICommand CatPartCommand { get; }
        public ICommand HomeViewCommand { get; }

        public MainViewModel()
        {

            CatPartCommand = new ViewModelCommand(ExecuteCatPartCommand);
            HomeViewCommand = new ViewModelCommand(ExecuteHomeViewCommand);

            //Default view
            //ExecuteHomeViewCommand(null);
        }

        private void ExecuteCatPartCommand(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.CATPart)|*.CATPart|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"C:\Clever\V300\BladeMill\data\RootMfgDir\A999999";
            _catPart = string.Empty;

            if (openFileDialog.ShowDialog() == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;

                _catPart = openFileDialog.FileName;

                Mouse.OverrideCursor = Cursors.Arrow;
            }

            CurrentChildView = new BaseViewModel();

        }

        private void ExecuteHomeViewCommand(object obj)
        {
            CurrentChildView = new BaseViewModel();
        }

    }
}
