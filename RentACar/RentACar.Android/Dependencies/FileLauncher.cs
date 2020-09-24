using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Android.Net;
using Uri = Android.Net.Uri;
using RentACar.Dependencies;
using Xamarin.Forms;
using RentACar.Droid.Dependencies;
using Application = Android.App.Application;

[assembly: Dependency(typeof(FileLauncher))]
namespace RentACar.Droid.Dependencies
{
    public class FileLauncher : IFileLauncher
    {
        private readonly string directory;
        private readonly string _newPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

        public FileLauncher()
        {
            //directory = Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, Environment.DirectoryDownloads);
            //Directory.CreateDirectory(directory);

            Directory.CreateDirectory(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
            directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);

        }

        protected virtual void ShowFileFromLocalPath(string path, bool delete = false)
        {
            Java.IO.File f = new Java.IO.File(path);

            Uri uri;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                uri = Uri.Parse("content://" + path);
            }
            else
            {
                Java.IO.File file = new Java.IO.File(path);
                file.SetReadable(true);
                //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
                uri = Uri.FromFile(file);
            }

            var uri1 = FileProvider.GetUriForFile(MainActivity.MainActivityInstance.ApplicationContext, MainActivity.MainActivityInstance.ApplicationContext.PackageName + ".provider", f);
            var t = uri1.Authority;


            Intent intent = new Intent(Intent.ActionView);

            intent.SetDataAndType(uri1, GetMimeType(uri1, path));
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            try
            {
                Application.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException e)
            {
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (delete)
                {
                    Task.Delay(2000);
                    File.Delete(path);
                }
            }
        }

        protected virtual string GetMimeType(Uri uri, string path)
        {
            string application = MimeTypeMap.Singleton.GetMimeTypeFromExtension(
    MimeTypeMap.GetFileExtensionFromUrl(uri.Path));

            if (string.IsNullOrEmpty(application))
            {
                string type = Path.GetExtension(path);

                switch (type.ToLower())
                {
                    case ".doc":
                    case ".docx":
                        application = "application/msword";
                        break;
                    case ".pdf":
                        application = "application/pdf";
                        break;
                    case ".xls":
                    case ".xlsx":
                        application = "application/vnd.ms-excel";
                        break;
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                        application = "image/jpeg";
                        break;
                    default:
                        application = "*/*";
                        break;
                }
            }

            return application;
        }

        public void Open(string filePath)
        {
            ShowFileFromLocalPath(filePath);
        }

        public void Open(byte[] byteArray, string name)
        {
            string type = System.IO.Path.GetExtension(name);
            string _path = "";
            _path = Path.Combine(_newPath, name);

            File.WriteAllBytes(_path, byteArray);
            File.SetAttributes(_path, FileAttributes.ReadOnly);
            ShowFileFromLocalPath(_path, false);
        }

        public virtual string SaveFile(byte[] byteArray, string name)
        {
            string path = Path.Combine(directory, name);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, byteArray);

            return path;
        }

        public string RetrivePathForPDF(string name)
        {
            string _path = "";
            _path = Path.Combine(_newPath, name);
            Java.IO.File f = new Java.IO.File(_path);

            Uri uri;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                uri = Uri.Parse("content://" + _path);
            }
            else
            {
                Java.IO.File file = new Java.IO.File(_path);
                file.SetReadable(true);
                //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
                uri = Uri.FromFile(file);
            }

            var uri1 = FileProvider.GetUriForFile(MainActivity.MainActivityInstance.ApplicationContext, MainActivity.MainActivityInstance.ApplicationContext.PackageName + ".provider", f);
            return uri1.Path;
        }
    }
}