using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CommunIT.App.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using CommunIT.Shared.Portable.DTOs.Tag;
using System.Collections.Generic;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CommunIT.App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatePage : Page
    {
        private readonly CreatePageViewModel _vm;

        public CreatePage()
        {
            this.InitializeComponent();
            DataContext = _vm = (Application.Current as App).Container.GetRequiredService<CreatePageViewModel>();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _vm.LoadCommand.Execute(e.Parameter);

            base.OnNavigatedTo(e);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var inputName = Name.Text;
            var inputDescription = Description.Text;
            var selectedBaseTags = GetSelectedBaseTags();
            var selectedSubTags = GetSelectedSubTags();
            
            await _vm.SaveDtoData(inputName, inputDescription, selectedBaseTags, selectedSubTags);
        }

        private List<TagDetailDto> GetSelectedBaseTags()
        {
            var selectedBaseTags = new List<TagDetailDto>();
            foreach (var baseTag in BaseTagList.SelectedItems)
            {
                selectedBaseTags.Add(baseTag as TagDetailDto);
            }

            return selectedBaseTags;
        }

        private List<TagDetailDto> GetSelectedSubTags()
        {
            var selectedSubTags = new List<TagDetailDto>();
            foreach (var subTag in SubTagList.SelectedItems)
            {
                selectedSubTags.Add(subTag as TagDetailDto);
            }

            return selectedSubTags;
        }

        private void BaseTagList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BaseTagList.SelectedItems.Count > 3)
            {
                BaseTagRestriction.Visibility = Visibility.Visible;
                SaveButton.IsEnabled = false;
            } 
            else if (BaseTagList.SelectedItems.Count <= 3)
            {
                BaseTagRestriction.Visibility = Visibility.Collapsed;
                SaveButton.IsEnabled = true;
            }
        }

        private void SubTagList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubTagList.SelectedItems.Count > 6)
            {
                SubTagRestriction.Visibility = Visibility.Visible;
                SaveButton.IsEnabled = false;
            }
            else if (SubTagList.SelectedItems.Count <= 6)
            {
                SubTagRestriction.Visibility = Visibility.Collapsed;
                SaveButton.IsEnabled = true;
            }
        }
    }
    
}
