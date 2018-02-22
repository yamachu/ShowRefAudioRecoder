using Prism;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;

namespace AudioRecoder.Forms
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
            containterRegistry.RegisterForNavigation<Views.AudioRecoderPage, Core.ViewModels.AudioRecoderPageViewModel>();

            containterRegistry.RegisterSingleton<Core.Services.IRecorderService, Core.Services.RecorderService>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(Core.ViewModelTypeResolver.Resolve);
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
