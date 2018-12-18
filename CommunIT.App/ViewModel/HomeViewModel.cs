using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Community;
using CommunIT.Shared.Portable.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace CommunIT.App.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly ICommunityRepository _communityRepository;
        private readonly IEventRepository _eventRepository;

        private CommunityUpdateListViewDto _selectedCommunity;
        public CommunityUpdateListViewDto SelectedCommunity
        {
            get => _selectedCommunity;
            set => SetProperty(ref _selectedCommunity, value, nameof(SelectedCommunity), () =>
            {
                if (SelectedCommunity != null)
                {
                    Navigation.GoToCommunityPageCommand.Execute(SelectedCommunity);
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

        private string _progressRingVisibility = "Visible";
        public string ProgressRingVisibility
        {
            get => _progressRingVisibility;
            set => SetProperty(ref _progressRingVisibility, value);
        }

        public ObservableCollection<CommunityUpdateListViewDto> Favorited { get; set; }
        public ObservableCollection<CommunityUpdateListViewDto> Popular { get; set; }
        public ObservableCollection<EventUpdateDetailDto> Events { get; set; }

        public ICommand LoadCommand { get; }

        public HomeViewModel(INavigation navigation, ICommunityRepository communityRepository, IEventRepository eventRepository)
            : base(navigation)
        {
            _communityRepository = communityRepository;
            _eventRepository = eventRepository;

            Title = "Home";
            Favorited = new ObservableCollection<CommunityUpdateListViewDto>();
            Popular = new ObservableCollection<CommunityUpdateListViewDto>();
            Events = new ObservableCollection<EventUpdateDetailDto>();

            LoadCommand = new RelayCommand(async _ => await ExecuteLoadCommand());
        }

        private async Task ExecuteLoadCommand()
        {
            if (Loading)
            {
                return;
            }
            Loading = true;
            ProgressRingVisibility = "Visible";

            Favorited.Clear();
            Popular.Clear();
            Events.Clear();

            var tempData = ApplicationData.Current.LocalSettings;
            var username = tempData.Values["Username"] as string;

            var favCommunities = await _communityRepository.ReadFavoritedByUserIdAsync(username);
            var popCommunities = await _communityRepository.ReadMostPopularAsync();
            var events = await _eventRepository.ReadByUserIdAsync(username);

            foreach (var c in favCommunities)
            {
                Favorited.Add(c);
            }
            foreach (var c in popCommunities)
            {
                Popular.Add(c);
            }
            foreach (var e in events)
            {
                Events.Add(e);
            }

            ProgressRingVisibility = "Collapsed";
            Loading = false;
        }
    }
}
