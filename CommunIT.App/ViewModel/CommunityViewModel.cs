using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Community;
using CommunIT.Shared.Portable.DTOs.Event;
using CommunIT.Shared.Portable.DTOs.Forum;
using CommunIT.Shared.Portable.DTOs.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace CommunIT.App.ViewModel
{
    public class CommunityViewModel : BaseViewModel
    {
        private readonly ICommunityRepository _communityRepo;
        private readonly IForumRepository _forumRepo;
        private readonly IEventRepository _eventRepo;
        private readonly IUserRepository _userRepo;

        private bool _loadingForums = false;
        public bool LoadingForums
        {
            get => _loadingForums;
            set => SetProperty(ref _loadingForums, value);
        }

        private bool _loadingEvents = false;
        public bool LoadingEvents
        {
            get => _loadingEvents;
            set => SetProperty(ref _loadingEvents, value);
        }

        private bool _loadingUsers = false;
        public bool LoadingUsers
        {
            get => _loadingUsers;
            set => SetProperty(ref _loadingUsers, value);
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _currentUser;
        public string CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        private bool _isAdmin = false;
        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }

        private bool _isJoined = false;
        public bool IsJoined
        {
            get => _isJoined;
            set => SetProperty(ref _isJoined, value);
        }

        private bool _isNotJoined = false;
        public bool IsNotJoined
        {
            get => _isNotJoined;
            set => SetProperty(ref _isNotJoined, value);
        }

        private ForumDetailDto _selectedForum;
        public ForumDetailDto SelectedForum
        {
            get => _selectedForum;
            set => SetProperty(ref _selectedForum, value, nameof(SelectedForum), () =>
            {
                if (SelectedForum != null)
                {
                    Navigation.GoToForumPageCommand.Execute(SelectedForum);
                }
            });
        }

        private EventUpdateDetailDto _selectedEvent;
        public EventUpdateDetailDto SelectedEvent
        {
            get => _selectedEvent;
            set => SetProperty(ref _selectedEvent, value, nameof(SelectedEvent), () =>
            {
                if (SelectedEvent != null)
                {
                    Navigation.GoToEventPageCommand.Execute(SelectedEvent);
                }
            });
        }

        private UserDetailDto _selectedUser;
        public UserDetailDto SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value, nameof(SelectedUser), () =>
            {
                if (SelectedUser != null)
                {
                    Navigation.GoToPublicUserProfilePageCommand.Execute(SelectedUser);
                }
            });
        }

        public ObservableCollection<ForumDetailDto> Forums { get; set; }
        public ObservableCollection<EventUpdateDetailDto> Events { get; set; }
        public ObservableCollection<UserDetailDto> Users { get; set; }

        public ICommand LoadCommand { get; }
        public ICommand JoinCommunityCommand { get; }
        public ICommand AddToFavoritesCommand { get; }
        public ICommand AddNewEventCommand { get; }
        public ICommand AddNewForumCommand { get; }

        public CommunityViewModel(INavigation navigation, ICommunityRepository communityRepo, IForumRepository forumRepo, IEventRepository eventRepo, IUserRepository userRepo)
            : base(navigation)
        {
            _communityRepo = communityRepo;
            _forumRepo = forumRepo;
            _eventRepo = eventRepo;
            _userRepo = userRepo;
            
            Forums = new ObservableCollection<ForumDetailDto>();
            Events = new ObservableCollection<EventUpdateDetailDto>();
            Users = new ObservableCollection<UserDetailDto>();

            CurrentUser = ApplicationData.Current.LocalSettings.Values["Username"] as string;

            LoadCommand = new RelayCommand(async p => await ExecuteLoadCommand(p));
            JoinCommunityCommand = new RelayCommand(async _ => await ExecuteJoinCommunityCommand());
            AddToFavoritesCommand = new RelayCommand(async _ => await ExecuteAddToFavoritesCommand());
        }

        public async Task ExecuteJoinCommunityCommand()
        {
            await _communityRepo.JoinCommunity(Id, CurrentUser);
            await LoadPage();
        }

        public async Task ExecuteAddToFavoritesCommand()
        {
            await _communityRepo.UserAddCommunityToFavorites(this.Id, this.CurrentUser);
        }

        public async Task LoadPage()
        {
            var tempData = ApplicationData.Current.LocalSettings;
            CurrentUser = tempData.Values["Username"] as string;

            IsJoined = await _communityRepo.IsUserInCommunityAsync(Id, CurrentUser);

            if (IsJoined)
            {
                IsNotJoined = !IsJoined;

                IsAdmin = await _userRepo.IsUserAdminInCommunity(CurrentUser, this.Id);

                Forums.Clear();

                var forums = await _forumRepo.ReadByCommunityId(this.Id);

                foreach (var forum in forums)
                {
                    Forums.Add(forum);
                }
                LoadingForums = false;

                Events.Clear();

                var events = await _eventRepo.ReadByCommunityIdAsync(this.Id);

                foreach (var @event in events)
                {
                    Events.Add(@event);
                }

                var usersInCommunity = await _userRepo.ReadByCommunityId(this.Id);
                foreach (var @users in usersInCommunity)
                {
                    Users.Add(users);
                }

                LoadingEvents = false;
            }

            IsNotJoined = !IsJoined;
        }

        private async Task ExecuteLoadCommand(object parameter)
        {
            LoadingForums = true;
            LoadingEvents = true;
            if (parameter is CommunityDetailDto community1)
            {
                Name = community1.Name;
                Id = community1.Id;
                Description = community1.Description;

            }
            else if (parameter is CommunityUpdateListViewDto community2)
            {
                Name = community2.Name;
                Id = community2.Id;
                Description = community2.Description;
            }

            await LoadPage();
        }
    }
}
