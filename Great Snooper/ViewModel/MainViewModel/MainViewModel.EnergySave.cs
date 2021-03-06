﻿namespace GreatSnooper.ViewModel
{
    using System;
    using System.Windows;

    using GalaSoft.MvvmLight;

    public partial class MainViewModel : ViewModelBase, IDisposable
    {
        public void EnterEnergySaveMode()
        {
            /*
            this.shouldWindowBeActivated = this.isHidden == false && this.DialogService.GetView().WindowState != WindowState.Minimized;
            this.shouldWindowBeShowed = this.isHidden == false;
            if (this.isHidden == false)
            {
                this.DialogService.GetView().Hide();
                this.isHidden = true;
            }
            */
            this.IsEnergySaveMode = true;
            for (int i = 0; i < this.Servers.Length; i++)
            {
                foreach (var item in this.Servers[i].Channels)
                {
                    ChannelViewModel chvm = item.Value as ChannelViewModel;
                    if (chvm != null)
                    {
                        if (chvm.UserListDG != null)
                        {
                            chvm.UserListDG.ItemsSource = null;
                        }
                        if (chvm.GameListGrid != null)
                        {
                            chvm.GameListGrid.DataContext = null;
                        }
                    }
                }
            }
        }

        public void LeaveEnergySaveMode()
        {
            if (this.isHidden || this.DialogService.GetView().WindowState == WindowState.Minimized)
            {
                return;
            }
            var screenBounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            if (screenBounds.Height == 480 && screenBounds.Width == 640)
            {
                shouldLeaveEnergySaveMode = true;
                return;
            }

            this.shouldLeaveEnergySaveMode = false;
            this.IsEnergySaveMode = false;

            for (int i = 0; i < this.Servers.Length; i++)
            {
                foreach (var item in this.Servers[i].Channels)
                {
                    ChannelViewModel chvm = item.Value as ChannelViewModel;
                    if (chvm != null)
                    {
                        if (chvm.UserListDG != null)
                        {
                            chvm.UserListDG.ItemsSource = chvm.Users;
                            chvm.UserListDG.SetDefaultOrderForGrid();
                        }
                        if (chvm.GameListGrid != null)
                        {
                            chvm.GameListGrid.DataContext = chvm;
                        }
                    }
                    if (item.Value.Joined && item.Value.HiddenMessagesInEnergySaveMode)
                    {
                        item.Value.LoadNewMessages();
                    }
                }
            }
        }
    }
}