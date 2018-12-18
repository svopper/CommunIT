using CommunIT.App.Models;
using CommunIT.App.Models.Repositories;
using CommunIT.App.View;
using CommunIT.App.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CommunIT.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        private readonly Lazy<IServiceProvider> _provider;

        public IServiceProvider Container => _provider.Value;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            _provider = new Lazy<IServiceProvider>(() => ConfigureServices());
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(LoginPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private bool On_BackRequested()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                return true;
            }
            return false;
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var settings = new Settings();

            var publicClientApplication = new PublicClientApplication(settings.ClientId, $"https://login.microsoftonline.com/{settings.TenantId}");

            var handler = new BearerTokenHttpClientHandler(publicClientApplication, settings);

            var httpClient = new HttpClient(handler) { BaseAddress = settings.BackendUrl };

            services.AddSingleton<ISettings>(settings);
            services.AddSingleton<IPublicClientApplication>(publicClientApplication);
            services.AddSingleton(httpClient);
            services.AddScoped<INavigation>(_ => new Navigation(Window.Current.Content as Frame));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommunityRepository, CommunityRepository>();
            services.AddScoped<IForumRepository, ForumRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IThreadRepository, ThreadRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddTransient<JoinedViewModel>();
            services.AddTransient<AdministratedViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<CommunityViewModel>();
            services.AddTransient<ForumViewModel>();
            services.AddTransient<ThreadViewModel>();
            services.AddTransient<BaseViewModel>();
            services.AddTransient<NewThreadViewModel>();
            services.AddTransient<NewForumViewModel>();
            services.AddTransient<NewEventViewModel>();
            services.AddTransient<EventViewModel>();
            services.AddTransient<SearchViewModel>();
            services.AddTransient<UserProfileViewModel>();
            services.AddTransient<OfficialUserProfileViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<CreatePageViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
