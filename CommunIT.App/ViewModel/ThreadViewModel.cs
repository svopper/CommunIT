using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.Comment;
using CommunIT.Shared.Portable.DTOs.Thread;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;

namespace CommunIT.App.ViewModel
{
    public class ThreadViewModel : BaseViewModel
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IThreadRepository _threadRepo;

        private bool _isThreadOwner = false;
        public bool IsThreadOwner
        {
            get => _isThreadOwner;
            set => SetProperty(ref _isThreadOwner, value);
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _threadTitle;
        public string ThreadTitle
        {
            get => _threadTitle;
            set => SetProperty(ref _threadTitle, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private string _createdByUserId;
        public string CreatedByUserId
        {
            get => _createdByUserId;
            set => SetProperty(ref _createdByUserId, value);
        }

        private string _newComment;
        public string NewComment
        {
            get => _newComment;
            set => SetProperty(ref _newComment, value);
        }

        private DateTime _created;
        public DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value);
        }

        public ICommand AddNewCommentCommand
        {
            get
            {
                return new RelayCommand(async _ => await AddNewComment(this.NewComment));
            }
        }

        public ObservableCollection<CommentDetailDto> Comments { get; set; }
        public ICommand LoadCommand { get; }
        public ICommand DeleteThreadCommand { get; }
        
        public ThreadViewModel(INavigation navigation, ICommentRepository commentRepo, IThreadRepository threadRepo)
            : base(navigation)
        {
            _commentRepo = commentRepo;
            _threadRepo = threadRepo;

            Comments = new ObservableCollection<CommentDetailDto>();

            LoadCommand = new RelayCommand(async p => await ExecuteLoadCommand(p));
            DeleteThreadCommand = new RelayCommand(async _ => await ExecuteDeleteThreadCommand());
            
        }
        

        public async Task ExecuteLoadCommand(object parameter)
        {
            if (parameter is ThreadDetailDto thread)
            {
                Id = thread.Id;
                ThreadTitle = thread.Title;
                Content = thread.Content;
                Created = thread.Created;
                CreatedByUserId = thread.CreatedByUserId;

                var tempData = ApplicationData.Current.LocalSettings;
                var username = tempData.Values["Username"] as string;

                IsThreadOwner = false;
                if (CreatedByUserId == username)
                {
                    IsThreadOwner = true;
                }
            }
            
            await UpdateComments();
        }

        public async Task UpdateComments()
        {
            Loading = true;
            Comments.Clear();

            var comments = await _commentRepo.ReadByThreadAsync(this.Id);            

            foreach (var comment in comments)
            {
                Comments.Add(comment);
            }

            Loading = false;
        }

        public async Task AddNewComment(string comment)
        {
            var tempData = ApplicationData.Current.LocalSettings;
            var username = tempData.Values["Username"] as string;
            var commentDto = new CommentCreateUpdateDto
            {
                Content = comment,
                ThreadId = this.Id,
                UserId = username
            };

            await _commentRepo.CreateAsync(commentDto);
            await UpdateComments();
        }

        public async Task ExecuteDeleteThreadCommand()
        {
            await _threadRepo.DeleteAsync(this.Id);
            Navigation.GoBackCommand.Execute(null);
        }
    }
}