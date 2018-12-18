using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.User;
using Windows.Storage;
using Windows.UI.Xaml;

namespace CommunIT.App.ViewModel
{
    public class OfficialUserProfileViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;
        private string _userName;
        private string _displayName;
        private string _universityName;
        private string _bio;
        private DateTime _created;

        public string Username
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        [Required]
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }
        public string UniversityName
        {
            get => _universityName;
            set => SetProperty(ref _universityName, value);
        }
        [DisplayName("Describe your self")]
        public string Bio
        {
            get => _bio;
            set => SetProperty(ref _bio, value);
        }
        [DisplayName("User since:")]
        public DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value);
        }

        public OfficialUserProfileViewModel(INavigation navigation, IUserRepository userRepo) : base(navigation)
        {
            _userRepository = userRepo;
            LoadCommand = new RelayCommand(async p => await ExecuteLoadCommand(p));
        }

        public ICommand LoadCommand { get; }

        private async Task ExecuteLoadCommand(object parameter)
        {
            if (parameter is UserDetailDto dto)
            {
                var userDto = new UserSearchDto { Username = dto.Username };
                var returnedUSerDto = await _userRepository.ReadAsync(userDto);

                DisplayName = returnedUSerDto.DisplayName;
                UniversityName = returnedUSerDto.University;
                Bio = returnedUSerDto.Bio;
                Created = returnedUSerDto.Created;
            }
            
        }
    }
}