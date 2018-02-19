using System;
using AppKit;
using Foundation;
using Prism;
using Prism.Ioc;
using Xamarin.Forms.Platform.MacOS;

namespace AudioRecoder.macOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        NSWindow _window;

        public AppDelegate()
        {
            ObjCRuntime.Runtime.MarshalManagedException += (sender, args) =>
            {
                Console.WriteLine(args.Exception);
            };

            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
            _window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            _window.Title = "AudioRecoder";
        }

        public override NSWindow MainWindow => _window;

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new Forms.App(new macOSInitializer()));

            base.DidFinishLaunching(notification);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        public override bool ApplicationShouldHandleReopen(NSApplication sender, bool hasVisibleWindows)
        {
            if (!hasVisibleWindows && _window != null)
            {
                _window.IsVisible = true;
            }

            return true;
        }
    }

    public class macOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
