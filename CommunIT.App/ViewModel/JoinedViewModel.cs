using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Community;
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
    public class JoinedViewModel : BaseViewModel
    {
        private readonly ICommunityRepository _repository;

        private CommunityUpdateListViewDto _selected;
        public CommunityUpdateListViewDto Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value, nameof(Selected), () =>
            {
                if (Selected != null)
                {
                    Navigation.GoToCommunityPageCommand.Execute(Selected);
                }
            });
        }

        public ObservableCollection<CommunityUpdateListViewDto> Items { get; set; }
        
        public ICommand LoadCommand { get; }

        public JoinedViewModel(INavigation navigation, ICommunityRepository repository)
            : base(navigation)
        {
            _repository = repository;

            Title = "Joined Communities";
            Items = new ObservableCollection<CommunityUpdateListViewDto>();

            LoadCommand = new RelayCommand(async _ => await ExecuteLoadCommand());
        }

        private async Task ExecuteLoadCommand()
        {
            if (Loading)
            {
                return;
            }
            Loading = true;

            Items.Clear();

            var tempData = ApplicationData.Current.LocalSettings;
            var username = tempData.Values["Username"] as string;

            var communities2 = await _repository.ReadByUserIdAsync(username);

            var communities = new List<CommunityDetailDto> { new CommunityDetailDto { Id = 1, Description = "Stuff and memes about Denmark", Name = "Denmark" } };

            foreach (var community in communities2)
            {
                Items.Add(community);
            }

            Loading = false;
        }
    }
}
