using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace CommunIT.App.ViewModel
{
    public class NewForumViewModel : BaseViewModel
    {
        private readonly IForumRepository _forumRepo;

        private int _communityId;
        public int CommunityId
        {
            get => _communityId;
            set => SetProperty(ref _communityId, value);
        }

        public ICommand LoadCommand { get; }
        public ICommand AddForumCommand { get; }

        public NewForumViewModel(INavigation navigation, IForumRepository forumRepo)
            : base(navigation)
        {
            Title = "Create New Forum";
            _forumRepo = forumRepo;

            LoadCommand = new RelayCommand(p => ExecuteLoadCommand(p));
            AddForumCommand = new RelayCommand(async p => await ExecuteAddForumCommand(p));
        }

        public void ExecuteLoadCommand(object parameter)
        {
            if (parameter is int communityId)
            {
                CommunityId = communityId;
            }
        }

        public async Task ExecuteAddForumCommand(object parameter)
        {
            if (parameter is ValueTuple<string, string> param)
            {
                var dto = new ForumCreateDto
                {
                    Name = param.Item1,
                    Description = param.Item2,
                    CommunityId = this.CommunityId
                };

                await _forumRepo.CreateAsync(dto);
                Navigation.GoBackCommand.Execute(this.CommunityId);
            }
        }
    }
}
