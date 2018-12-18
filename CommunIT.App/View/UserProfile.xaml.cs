
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CommunIT.App.ViewModel;
using Microsoft.Extensions.DependencyInjection;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CommunIT.App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserProfile : Page
    {
        private readonly UserProfileViewModel _vm;
        public UserProfile()
        {
            this.InitializeComponent();
            DataContext = _vm = (Application.Current as App).Container.GetRequiredService<UserProfileViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _vm.LoadCommand.Execute(e.Parameter);

            base.OnNavigatedTo(e);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var inputDisplayName = DisplayNameBox.Text;
            var inputUniversity = UniversityBox.Text;
            var inputDescription = BioBox.Text;
            await _vm.SaveDtoData(inputDisplayName, inputUniversity, inputDescription);
            
        }
    }
}
