using Microsoft.Extensions.Logging;
using AndroidNetMauiPoC.Service;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace AndroidNetMauiPoC
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            //LoadConfiguration();

            builder.Services.AddMauiBlazorWebView();

#if ANDROID
            builder.Services.AddSingleton<IServiceUnderTest, Platforms.Android.AndrodServiceUnderTest>();
#endif

#if WINDOWS
            builder.Services.AddSingleton<IServiceUnderTest, Platforms.Windows.WindowsServiceUnderTest>();
#endif

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }


    }
}
