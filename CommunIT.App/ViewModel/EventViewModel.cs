using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Event;
using CommunIT.Shared.Portable.DTOs.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace CommunIT.App.ViewModel
{
    public class EventViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepo;
        private readonly IEventRepository _eventRepo;

        private bool _isEventOwner = false;
        public bool IsEventOwner
        {
            get => _isEventOwner;
            set => SetProperty(ref _isEventOwner, value);
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _eventTitle;
        public string EventTitle
        {
            get => _eventTitle;
            set => SetProperty(ref _eventTitle, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _createdByUserId;
        public string CreatedByUserId
        {
            get => _createdByUserId;
            set => SetProperty(ref _createdByUserId, value);
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private bool _isCanGoEnabled;
        public bool IsCanGoEnabled
        {
            get => _isCanGoEnabled;
            set => SetProperty(ref _isCanGoEnabled, value);
        }

        private bool _isCannotGoEnabled;
        public bool IsCannotGoEnabled
        {
            get => _isCannotGoEnabled;
            set => SetProperty(ref _isCannotGoEnabled, value);
        }

        public ObservableCollection<UserDetailDto> ParticipatingUsers { get; set; }

        public ICommand LoadCommand { get; }
        public ICommand CanGoCommand { get; }
        public ICommand CantGoCommand { get; }

        private string UserId { get; set; }

        public EventViewModel(INavigation navigation, IUserRepository userRepo, IEventRepository eventRepo)
            : base(navigation)
        {
            _userRepo = userRepo;
            _eventRepo = eventRepo;

            UserId = ApplicationData.Current.LocalSettings.Values["Username"] as string;

            ParticipatingUsers = new ObservableCollection<UserDetailDto>();
            LoadCommand = new RelayCommand(async p => await ExecuteLoadCommand(p));
            CanGoCommand = new RelayCommand(async _ => await ExecuteCanGoCommand());
            CantGoCommand = new RelayCommand(async _ => await ExecuteCantGoCommand());
        }

        private async Task ExecuteLoadCommand(object parameter)
        {
            Loading = true;
            if (parameter is EventUpdateDetailDto @event)
            {
                Id = @event.Id;
                Title = @event.Title;
                Description = @event.Description;
                Date = @event.Date;
            }

            UpdateButtonEnabling();

            await UpdateParticipantListAsync();
        }

        private async Task ExecuteCanGoCommand()
        {
            Loading = true;
            var successful = await _eventRepo.AddUserToEventAsync(this.Id, UserId);
            if (successful)
            {
                await UpdateParticipantListAsync();
            }
            Loading = false;
        }

        private async Task ExecuteCantGoCommand()
        {
            Loading = true;
            var successful = await _eventRepo.RemoveUserFromEventAsync(this.Id, UserId);
            if (successful)
            {
                await UpdateParticipantListAsync();
            }
            Loading = false;
        }

        private async Task UpdateParticipantListAsync()
        {
            ParticipatingUsers.Clear();

            var users = await _userRepo.ReadByEventId(this.Id);

            foreach (var user in users)
            {
                ParticipatingUsers.Add(user);
            }

            UpdateButtonEnabling();

            Loading = false;
        }

        private void UpdateButtonEnabling()
        {
            if (ParticipatingUsers.Any(u => u.Username == UserId))
            {
                IsCanGoEnabled = false;
                IsCannotGoEnabled = true;
            }
            else
            {
                IsCanGoEnabled = true;
                IsCannotGoEnabled = false;
            }
        }
    }
}
