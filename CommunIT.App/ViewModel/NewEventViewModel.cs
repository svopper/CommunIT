using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommunIT.App.ViewModel
{
    public class NewEventViewModel : BaseViewModel
    {
        private readonly IEventRepository _eventRepo;

        private int _communityId;
        public int CommunityId
        {
            get => _communityId;
            set => SetProperty(ref _communityId, value);
        }

        public DateTimeOffset MinDate => DateTimeOffset.Now;

        public ICommand LoadCommand { get; }
        public ICommand AddEventCommand { get; }

        public NewEventViewModel(INavigation navigation, IEventRepository eventRepo)
            : base(navigation)
        {
            Title = "Create New Event";
            _eventRepo = eventRepo;

            LoadCommand = new RelayCommand(p => ExecuteLoadCommand(p));
            AddEventCommand = new RelayCommand(async p => await ExecuteAddEventCommand(p));
        }

        public void ExecuteLoadCommand(object parameter)
        {
            if (parameter is int communityId)
            {
                CommunityId = communityId;
            }
        }

        public async Task ExecuteAddEventCommand(object parameter)
        {
            if (parameter is ValueTuple<string, DateTimeOffset?, string> param)
            {
                var dto = new EventCreateDto
                {
                    Title = param.Item1,
                    Date = param.Item2.Value.DateTime,
                    Description = param.Item3,
                    CommunityId = this.CommunityId
                };

                await _eventRepo.CreateAsync(dto);
                Navigation.GoBackCommand.Execute(this.CommunityId);
            }
        }
    }
}
