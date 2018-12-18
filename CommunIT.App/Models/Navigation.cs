using CommunIT.App.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace CommunIT.App.Models
{
    public class Navigation : INavigation
    {
        private readonly Frame _frame;

        public Navigation(Frame frame)
        {
            _frame = frame;

            GoBackCommand = new RelayCommand(p => { if (_frame.CanGoBack) { _frame.GoBack(); } });
            GoToHomePageCommand = new RelayCommand(_ => _frame.Navigate(typeof(HomePage)));
            GoToAdministratedPageCommand = new RelayCommand(_ => _frame.Navigate(typeof(AdministratedPage)));
            GoToJoinedPageCommand = new RelayCommand(_ => _frame.Navigate(typeof(JoinedPage)));
            GoToCommunityPageCommand = new RelayCommand(p => _frame.Navigate(typeof(CommunityPage), p));
            GoToMainPageCommand = new RelayCommand(_ => _frame.Navigate(typeof(MainPage)));
            GoToForumPageCommand = new RelayCommand(p => _frame.Navigate(typeof(ForumPage), p));
            GoToEventPageCommand = new RelayCommand(p => _frame.Navigate(typeof(EventPage), p));
            GoToThreadPageCommand = new RelayCommand(p => _frame.Navigate(typeof(ThreadPage), p));
            GoToNewThreadPageCommand = new RelayCommand(p => _frame.Navigate(typeof(NewThreadPage), p));
            GoToPublicUserProfilePageCommand = new RelayCommand(p => _frame.Navigate(typeof(OfficialUserProfile), p));
        }

        public ICommand GoBackCommand { get; }

        public ICommand GoToHomePageCommand { get; }

        public ICommand GoToAdministratedPageCommand { get; }

        public ICommand GoToJoinedPageCommand { get; }

        public ICommand GoToCommunityPageCommand { get; }

        public ICommand GoToMainPageCommand { get; }

        public ICommand GoToForumPageCommand { get; }

        public ICommand GoToEventPageCommand { get; }

        public ICommand GoToThreadPageCommand { get; }

        public ICommand GoToNewThreadPageCommand { get; }

        public ICommand GoToPublicUserProfilePageCommand { get; }
    }
}
