using CommunIT.App.View;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CommunIT.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Nav_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (NavigationViewItemBase item in Nav.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "HomePage")
                {
                    Nav.SelectedItem = item;
                    break;
                }
            }
            ContentFrame.Navigate(typeof(HomePage));
        }

        private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
        }

        private void Nav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var itemContent = args.InvokedItem as TextBlock;
            if (itemContent != null)
            {
                switch (itemContent.Tag)
                {
                    case "Nav_Home":
                        ContentFrame.Navigate(typeof(HomePage));
                        break;
                    case "Nav_Profile":
                        ContentFrame.Navigate(typeof(UserProfile));
                        break;
                    case "Nav_Create":
                        ContentFrame.Navigate(typeof(CreatePage));
                        break;
                    case "Nav_Search":
                        ContentFrame.Navigate(typeof(SearchPage));
                        break;
                    case "Nav_Joined":
                        ContentFrame.Navigate(typeof(JoinedPage));
                        break;
                    case "Nav_Admin":
                        ContentFrame.Navigate(typeof(AdministratedPage));
                        break;
                }
            }
            
        }

        private void Nav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (Nav.IsPaneOpen &&
                (Nav.DisplayMode == NavigationViewDisplayMode.Compact ||
                 Nav.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }
    }
}
