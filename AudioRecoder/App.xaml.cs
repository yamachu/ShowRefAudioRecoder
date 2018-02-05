using Prism;
using Prism.Ioc;
using Prism.Unity;
using Unity;
using Xamarin.Forms;

namespace AudioRecoder
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) {}

        protected override void OnInitialized()
        {
            NavigationService.NavigateAsync($"AudioRecoderPage");
        }

        protected override void RegisterTypes(IContainerRegistry containterRegistry)
        {
            containterRegistry.RegisterForNavigation<AudioRecoderPage>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
