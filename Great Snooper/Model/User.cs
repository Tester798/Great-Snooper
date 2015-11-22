﻿using GalaSoft.MvvmLight;
using GreatSnooper.Helpers;
using GreatSnooper.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GreatSnooper.Model
{
    [DebuggerDisplay("{Name}")]
    public class User : ObservableObject, IComparable
    {
        #region Enums
        public enum Status { Online, Offline, Unknown }
        #endregion

        #region Members
        private string _name;
        private string _clan;
        private bool _isBanned = false;
        private TusAccount _tusAccount;
        private Status _onlineStatus = Status.Unknown;
        private UserGroup _group = GlobalManager.DefaultGroup;
        private string _clientName;
        private bool? _usingGreatSnooper;
        private bool? _usingGreatSnooper2;
        private bool? _canConversation;
        public Rank _rank;
        public Country _country;
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public string Clan
        {
            get
            {
                if (TusAccount != null)
                    return TusAccount.Clan;
                else
                    return _clan;
            }
            set
            {
                if (_clan != value)
                {
                    _clan = value;
                    RaisePropertyChanged("Clan");
                }
            }
        }
        public Rank Rank
        {
            get
            {
                if (TusAccount != null)
                    return TusAccount.Rank;
                else
                    return _rank;
            }
            set
            {
                if (_rank != value)
                {
                    _rank = value;
                    RaisePropertyChanged("Rank");
                }
            }
        }
        public Country Country
        {
            get
            {
                if (TusAccount != null)
                    return TusAccount.Country;
                else
                    return _country;
            }
            set
            {
                if (_country != value)
                {
                    _country = value;
                    RaisePropertyChanged("Country");
                }
            }
        }
        public TusAccount TusAccount
        {
            get { return _tusAccount; }
            set
            {
                if (_tusAccount != value)
                {
                    _tusAccount = value;
                    RaisePropertyChanged("Clan");
                    RaisePropertyChanged("Rank");
                    RaisePropertyChanged("Country");
                    RaisePropertyChanged("TusAccount");
                }
            }
        }
        public Status OnlineStatus
        {
            get { return _onlineStatus; }
            set
            {
                if (_onlineStatus != value)
                {
                    _onlineStatus = value;
                    if (value != Status.Online)
                    {
                        // Reset client info
                        TusAccount = null;
                        ClientName = "";
                    }
                    RaisePropertyChanged("OnlineStatus");
                }
            }
        }
        public bool IsBanned
        {
            get { return _isBanned; }
            set
            {
                if (_isBanned != value)
                {
                    _isBanned = value;
                    RaisePropertyChanged("IsBanned");
                }
            }
        }
        public UserGroup Group
        {
            get { return _group; }
            set
            {
                if (_group != value)
                {
                    if (value != null)
                        _group = value;
                    else
                        _group = GlobalManager.DefaultGroup;

                    // Refresh sorting
                    foreach (var chvm in Channels)
                    {
                        if (chvm.Joined)
                        {
                            chvm.Users.Remove(this);
                            chvm.Users.Add(this);
                        }
                    }

                    RaisePropertyChanged("Group");
                }
            }
        }
        public string ClientName
        {
            get { return _clientName; }
            set
            {
                if (_clientName != value)
                {
                    _clientName = value;
                    _usingGreatSnooper = null;
                    _canConversation = null;
                }
            }
        }
        public bool UsingGreatSnooper
        {
            get
            {
                if (_usingGreatSnooper.HasValue)
                    return _usingGreatSnooper.Value;
                _usingGreatSnooper = ClientName.StartsWith("Great Snooper", StringComparison.OrdinalIgnoreCase);
                return _usingGreatSnooper.Value;
            }
        }
        public bool CanConversation
        {
            get
            {
                if (_canConversation.HasValue)
                    return _canConversation.Value;

                if (!UsingGreatSnooper)
                    _canConversation = false;
                else
                {
                    // Great snooper v1.4
                    string gsVersion = ClientName.Substring(15);
                    _canConversation = Math.Sign(gsVersion.CompareTo("1.4")) != -1;
                }
                return _canConversation.Value;
            }
        }
        public bool UsingGreatSnooper2
        {
            get
            {
                if (_usingGreatSnooper2.HasValue)
                    return _usingGreatSnooper2.Value;

                if (!UsingGreatSnooper)
                    _usingGreatSnooper2 = false;
                else
                {
                    // Great snooper v1.4
                    string gsVersion = ClientName.Substring(15);
                    _usingGreatSnooper2 = Math.Sign(gsVersion.CompareTo("2.0")) != -1;
                }
                return _usingGreatSnooper2.Value;
            }
        }
        public HashSet<ChannelViewModel> Channels { get; private set; }
        public HashSet<PMChannelViewModel> PMChannels { get; private set; }
        public List<ChannelViewModel> AddToChannel { get; private set; }
        #endregion

        public User(string name, string clan = "")
        {
            this._name = name;
            this._clan = clan;
            this.Channels = new HashSet<ChannelViewModel>();
            this.PMChannels = new HashSet<PMChannelViewModel>();
            this.AddToChannel = new List<ChannelViewModel>();
            UserGroup group;
            if (UserGroups.Users.TryGetValue(name, out group))
                this._group = group;
            else
                this._group = GlobalManager.DefaultGroup;
        }

        #region IComparable
        public int CompareTo(object obj)
        {
            var o = obj as User;
            return StringComparer.OrdinalIgnoreCase.Compare(this.Name, o.Name);
        }
        #endregion

        // To use this object with string.Join(",", List<User>);
        public override string ToString()
        {
            return this.Name;
        }
    }
}