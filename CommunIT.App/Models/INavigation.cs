using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommunIT.App.Models
{
    public interface INavigation
    {
        ICommand GoBackCommand { get; }
        ICommand GoToHomePageCommand { get; }
        ICommand GoToAdministratedPageCommand { get; }
        ICommand GoToJoinedPageCommand { get; }
        ICommand GoToCommunityPageCommand { get; }
        ICommand GoToMainPageCommand { get; }
        ICommand GoToForumPageCommand { get; }
        ICommand GoToEventPageCommand { get; }
        ICommand GoToThreadPageCommand { get; }
        ICommand GoToNewThreadPageCommand { get; }
        ICommand GoToPublicUserProfilePageCommand { get; }
    }
}
