using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Tag;
using CommunIT.Shared.Portable.DTOs.Community;
using Windows.Storage;
using Windows.UI.Popups;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CommunIT.App.ViewModel
{
    public class CreatePageViewModel : BaseViewModel
    {
        private readonly ITagRepository _tagRepository;
        private readonly ICommunityRepository _communityRepository;

        private string _name;
        private string _description;

        private IEnumerable<TagDetailDto> _selectedBaseTags;
        private IEnumerable<TagDetailDto> _selectedSubTags;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ObservableCollection<TagDetailDto> BaseTags { get; set; }
        public ObservableCollection<TagDetailDto> SubTags { get; set; }
        public ObservableCollection<BaseSubTagDto> BaseSubTags { get; set; }

        public ICommand LoadCommand { get; }
        public CreatePageViewModel(INavigation navigation, ITagRepository tagRepository, ICommunityRepository communityRepository) : base(navigation)
        {
            _tagRepository = tagRepository;
            _communityRepository = communityRepository;

            BaseTags = new ObservableCollection<TagDetailDto>();
            SubTags = new ObservableCollection<TagDetailDto>();
            BaseSubTags = new ObservableCollection<BaseSubTagDto>();

            LoadCommand = new RelayCommand(async _ => await ExecuteLoadCommand());
        }

        private async Task ExecuteLoadCommand()
        {
            var baseTags = await _tagRepository.ReadBaseTags();
            foreach (var baseTag in baseTags)
            {
                BaseTags.Add(baseTag);
            }

            var subTags = await _tagRepository.ReadSubTags();
            var orderedSubtags = subTags.OrderBy(st => st.Name);
            foreach (var subTag in orderedSubtags)
            {
                SubTags.Add(subTag);
            }
        }

        internal async Task SaveDtoData(string inputName, string inputDescription, List<TagDetailDto> selctedBaseTags, List<TagDetailDto> selectedSubTags)
        {
            var tempData = ApplicationData.Current.LocalSettings;
            var username = tempData.Values["Username"] as string;

            var community = new CommunityCreateDto
            {
                Name = inputName,
                Description = inputDescription,
                BaseTagIds = selctedBaseTags.Select(t => t.Id),
                SubTagIds = selectedSubTags.Select(t => t.Id),
                CreatedBy = username
            };

            var createdCommunity = await _communityRepository.CreateAsync(community);

            Navigation.GoToCommunityPageCommand.Execute(createdCommunity);
        }
    }
}
