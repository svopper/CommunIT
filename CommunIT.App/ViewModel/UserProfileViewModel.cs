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
    public class UserProfileViewModel : BaseViewModel
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

        public UserProfileViewModel(INavigation navigation, IUserRepository userRepo) : base(navigation)
        {
            _userRepository = userRepo;
            LoadCommand = new RelayCommand(async _ => await ExecuteLoadCommand());
        }
    
        public ICommand LoadCommand { get; }

        private async Task ExecuteLoadCommand()
        {
            Loading = true;

            var TempData = ApplicationData.Current.LocalSettings;
            var usernamestring = TempData.Values["Username"] as string;
            var userDto = new UserSearchDto { Username = usernamestring };
            var returnedUSerDto = await _userRepository.ReadAsync(userDto);

            Username = returnedUSerDto.Username;
            DisplayName = returnedUSerDto.DisplayName;
            UniversityName = returnedUSerDto.University;
            Bio = returnedUSerDto.Bio;
            Created = returnedUSerDto.Created;

            Loading = false;
        }

        public async Task SaveDtoData(string displayName, string universityName, string description)
        {
            var tempData = ApplicationData.Current.LocalSettings;
            var userNameString = tempData.Values["Username"] as string;
            var userDTO = new UserDetailDto
            {
                Username = userNameString,
                DisplayName = displayName,
                University = universityName,
                Bio = description,
                Created = Created
            };
            await _userRepository.UpdateAsync(userDTO);

            await ExecuteLoadCommand();
            
        }
    }
}
