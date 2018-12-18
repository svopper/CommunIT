using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace CommunIT.App.ViewModel
{
    public class NewThreadViewModel : BaseViewModel
    {
        private readonly IThreadRepository _threadRepo;

        private int _forumId;
        public int ForumId
        {
            get => _forumId;
            set => SetProperty(ref _forumId, value);
        }

        public ICommand LoadCommand { get; }
        public ICommand AddThreadCommand { get; }

        public NewThreadViewModel(INavigation navigation, IThreadRepository threadRepo)
            : base(navigation)
        {
            Title = "Create New Thread";
            _threadRepo = threadRepo;

            LoadCommand = new RelayCommand(p => ExecuteLoadCommand(p));
            AddThreadCommand = new RelayCommand(async p => await ExecuteAddThreadCommand(p));
        }

        public void ExecuteLoadCommand(object parameter)
        {
            if (parameter is int forumId)
            {
                ForumId = forumId;
            }
        }

        public async Task ExecuteAddThreadCommand(object parameter)
        {
            var p = (ValueTuple<string, string>)parameter;
            if (parameter is ValueTuple<string,string> param)
            {
             
                var tempData = ApplicationData.Current.LocalSettings;
                var username = tempData.Values["Username"] as string;

                var dto = new ThreadCreateDto
                {
                    Title = param.Item1,
                    Content = param.Item2,
                    UserId = username,
                    ForumId = this.ForumId
                };
                
                await _threadRepo.CreateAsync(dto);
                Navigation.GoBackCommand.Execute(this.ForumId);
            }
        }
    }
}
