using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Forum;
using CommunIT.Shared.Portable.DTOs.Thread;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommunIT.App.ViewModel
{
    public class ForumViewModel : BaseViewModel
    {
        private readonly IThreadRepository _threadRepo;

        private bool _isForumOwner;
        public bool IsForumOwner
        {
            get => _isForumOwner;
            set => SetProperty(ref _isForumOwner, value);
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


        private ThreadDetailDto _selectedThread;
        public ThreadDetailDto SelectedThread
        {
            get => _selectedThread;
            set => SetProperty(ref _selectedThread, value, nameof(SelectedThread), () =>
            {
                if (SelectedThread != null)
                {
                    Navigation.GoToThreadPageCommand.Execute(SelectedThread);
                }
            });
        }

        public ObservableCollection<ThreadDetailDto> Threads { get; set; }

        public ICommand LoadCommand { get; }
        
        public ICommand AddNewThreadCommand { get; }

        public ForumViewModel(INavigation navigation, IThreadRepository threadRepo)
            : base(navigation)
        {
            _threadRepo = threadRepo;

            Threads = new ObservableCollection<ThreadDetailDto>();

            LoadCommand = new RelayCommand(async p => await ExecuteLoadCommand(p));
            AddNewThreadCommand = new RelayCommand(_ => ExecuteAddNewThreadCommand());
        }

        public async Task ExecuteLoadCommand(object parameter)
        {
            Loading = true;
            if (parameter is ForumDetailDto forum)
            {
                Id = forum.Id;
                Name = forum.Name;
                Description = forum.Description;
            }

            Threads.Clear();

            var realThreads = await _threadRepo.ReadByForumId(this.Id);

            foreach (var thread in realThreads)
            {
                Threads.Add(thread);
            }
            Loading = false;
        }

        public void ExecuteAddNewThreadCommand()
        {
            Navigation.GoToNewThreadPageCommand.Execute(this.Id);
        }

    }
}
