using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Community;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CommunIT.App.ViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly ICommunityRepository _communityRepo;

        public ObservableCollection<CommunityUpdateListViewDto> Communities { get; set; }
        public ObservableCollection<CommunityUpdateListViewDto> TempCommunities { get; set; }

        public ICommand LoadCommand { get; }

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

        public SearchViewModel(INavigation navigation, ICommunityRepository communityRepo)
            :base(navigation)
        {
            _communityRepo = communityRepo;

            Communities = new ObservableCollection<CommunityUpdateListViewDto>();
            TempCommunities = new ObservableCollection<CommunityUpdateListViewDto>();

            LoadCommand = new RelayCommand(async _ => await ExecuteLoadCommand());
        }

        private async Task ExecuteLoadCommand()
        {
            var communities = await _communityRepo.ReadAsListViewAsync();

            Communities.Clear();
            TempCommunities.Clear();
            foreach (var community in communities)
            {
                Communities.Add(community);
                TempCommunities.Add(community);
            }
        }

        public void SearchBox_TextChanged(TextBox sender, RoutedEventArgs args)
        {
            if (sender.Text.Length == 0)
            {
                TempCommunities.Clear();
                foreach (var community in Communities)
                {
                    TempCommunities.Add(community);
                }
            }
            else 
            {
                var tmp = Communities.Where(c => c.Name.ToLower().Contains(sender.Text.ToLower()));
                TempCommunities.Clear();
                foreach (var community in tmp)
                {
                    TempCommunities.Add(community);
                }
            }
        }
    }
}
