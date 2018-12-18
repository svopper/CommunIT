using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.Shared.Portable.DTOs.User;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace CommunIT.App.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepo;

        private readonly IPublicClientApplication _publicClientApplication;
        private readonly ISettings _settings;

        private bool _signedIn;
        public bool SignedIn
        {
            get => _signedIn;
            set => SetProperty(ref _signedIn, value, nameof(SignedIn), () =>
            {
                if (value)
                {
                    Message = null;
                }
                SignInCommand.OnCanExecuteChanged(this);
                SignOutCommand.OnCanExecuteChanged(this);
            });
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand LoadCommand { get; }
        public RelayCommand SignInCommand { get; }
        public RelayCommand SignOutCommand { get; }

        ApplicationDataContainer tempData;

        public LoginViewModel(INavigation navigation, IUserRepository userRepo, IPublicClientApplication publicClientApplication, ISettings settings)
            : base(navigation)
        {
            Title = "CommunIT";

            _userRepo = userRepo;

            _publicClientApplication = publicClientApplication;
            _settings = settings;

            LoadCommand = new RelayCommand(async _ => await ExecuteLoadCommand());
            SignInCommand = new RelayCommand(async _ => await ExecuteSignInCommand(), _ => !SignedIn);
            SignOutCommand = new RelayCommand(async _ => await ExecuteSignOutCommand(), _ => SignedIn);

            tempData = ApplicationData.Current.LocalSettings;
        }

        private async Task ExecuteLoadCommand()
        {
            if (Loading)
            {
                return;
            }
            Loading = true;

            var account = await GetAccountAsync();

            if (account != null)
            {
                var authResult = await _publicClientApplication.AcquireTokenSilentAsync(_settings.Scopes, account);

                if (authResult != null)
                {
                    SignedIn = true;
                    Username = authResult.Account.Username;
                    tempData.Values["Username"] = Username;
                    await CheckIfUserExists(Username);
                }
            }

            Loading = false;
        }

        private async Task ExecuteSignInCommand()
        {
            AuthenticationResult authenticationResult;
            try
            {
                authenticationResult = await _publicClientApplication.AcquireTokenAsync(_settings.Scopes);
            }
            catch (MsalClientException e)
            {
                Message = e.Message;
                return;
            }
            if (authenticationResult != null)
            {
                SignedIn = true;
                Username = authenticationResult.Account.Username;
                tempData.Values["Username"] = Username;
                await CheckIfUserExists(Username);
            }
        }

        private async Task ExecuteSignOutCommand()
        {
            var account = await GetAccountAsync();

            if (account != null)
            {
                await _publicClientApplication.RemoveAsync(account);

                SignedIn = false;
                Username = null;
            }
        }

        private async Task<IAccount> GetAccountAsync()
        {
            var accounts = await _publicClientApplication.GetAccountsAsync();

            return accounts.FirstOrDefault();
        }

        private async Task CheckIfUserExists(string username)
        {
            var user = await _userRepo.ReadAsync(new UserSearchDto { Username = username });
            if (user is null)
            {
                var createDto = new UserCreateDto
                {
                    Username = username,
                    DisplayName = "New User"
                };

                await _userRepo.CreateAsync(createDto);
            }
        }
    }
}
