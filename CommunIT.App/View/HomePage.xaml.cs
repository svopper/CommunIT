using CommunIT.App.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CommunIT.App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private readonly HomeViewModel _vm;

        public HomePage()
        {
            this.InitializeComponent();

            DataContext = _vm = (Application.Current as App).Container.GetRequiredService<HomeViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _vm.LoadCommand.Execute(e.Parameter);

            base.OnNavigatedTo(e);
        }
    }
}
