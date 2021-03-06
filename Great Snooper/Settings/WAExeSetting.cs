﻿namespace GreatSnooper.Settings
{
    using System.IO;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using GreatSnooper.Helpers;

    using Microsoft.Win32;

    class WAExeSetting : AbstractSetting
    {
        private string _path;

        public WAExeSetting(string settingName, string text)
            : base(settingName, text)
        {
            this._path = SettingsHelper.Load<string>(settingName);
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (_path != value)
                {
                    _path = value;
                    SettingsHelper.Save(this.SettingName, _path);
                }
            }
        }

        public ICommand WAExeCommand
        {
            get
            {
                return new RelayCommand(WAExe);
            }
        }

        private void WAExe()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Worms Armageddon Exe (*.exe)|*.exe";
            if (File.Exists(this.Path))
            {
                dlg.InitialDirectory = new FileInfo(this.Path).Directory.FullName;
            }

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = dlg.ShowDialog();

            // Get the selected file name
            if (result.HasValue && result.Value)
            {
                this.Path = dlg.FileName;
                RaisePropertyChanged("Path");
            }
        }
    }
}