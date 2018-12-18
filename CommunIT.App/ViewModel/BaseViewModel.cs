﻿using CommunIT.App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _loading = false;
        public bool Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        
        public INavigation Navigation { get; }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = null,
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BaseViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
