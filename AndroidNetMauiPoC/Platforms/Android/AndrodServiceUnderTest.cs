using AndroidNetMauiPoC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSEnvironment = Android.OS.Environment;

namespace AndroidNetMauiPoC.Platforms.Android
{
    internal class AndrodServiceUnderTest : IServiceUnderTest
    {
        public async Task ExecuteAsync()
        {
            await ExtractConfigFileIfMissingAsync("appsettings.json");
        }

        private  string GetRootDirectory()
        {
            // By default, get the current directory
            string appFolder = Directory.GetCurrentDirectory();

#if ANDROID
            appFolder = OSEnvironment.GetExternalStoragePublicDirectory(OSEnvironment.DirectoryDocuments)!.AbsolutePath;
            appFolder = Path.Combine(appFolder, "DavisPoC");
#endif
            return appFolder;
        }

        private  async Task ExtractConfigFileIfMissingAsync(string filename)
        {
            try
            {
                string externalPath = GetRootDirectory();
                string routeToFile = Path.Combine(externalPath, filename);
                if (!File.Exists(routeToFile))
                {
                    await RequestStoragePermissionsAsync();

                    if (await global::Microsoft.Maui.Storage.FileSystem.AppPackageFileExistsAsync(filename))
                    {
                        using var stream = await global::Microsoft.Maui.Storage.FileSystem.OpenAppPackageFileAsync(filename);
                        using var reader = new StreamReader(stream);

                        // Ensure the directory exists
                        Directory.CreateDirectory(externalPath);

                        // Write the content to the external path
                        File.WriteAllText(routeToFile, reader.ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }


        private  async Task<bool> RequestStoragePermissionsAsync()
        {
            var statusWrite = await Permissions.RequestAsync<Permissions.StorageWrite>();

            return statusWrite == PermissionStatus.Granted;
        }

        public async Task<string> GetAppSettings()
        {
            string externalPath = GetRootDirectory();
            string routeToFile = Path.Combine(externalPath, "appsettings.json");

            return await File.ReadAllTextAsync(routeToFile);
        }
    }
}
