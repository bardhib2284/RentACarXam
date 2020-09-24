using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using QuickLook;
using RentACar.Dependencies;
using RentACar.iOS.Dependencies;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileLauncher))]
namespace RentACar.iOS.Dependencies
{
    public class FileLauncher : IFileLauncher
    {
        private readonly string _documentsPath;

        public FileLauncher()
        {
            _documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        private void ShowPdfFromLocalPath(string path)
        {
            FileInfo fi = new FileInfo(path);
            QLPreviewController previewController = new QLPreviewController
            {
                DataSource = new ItemPreviewControllerDataSource(fi.FullName, fi.Name)
            };
            UINavigationController controller = FindNavigationController();
            controller?.PresentViewController(previewController, true, null);
        }

        public void Open(string path)
        {
            ShowPdfFromLocalPath(path);
        }

        public void Open(byte[] byteArray, string name)
        {
            //string type = System.IO.Path.GetExtension(name);
            string path = SaveFile(byteArray, Path.Combine(_documentsPath, name));

            ShowPdfFromLocalPath(path);
        }

        public virtual string SaveFile(byte[] byteArray, string name)
        {
            string path = Path.Combine(_documentsPath, name);
            File.WriteAllBytes(path, byteArray);
            return path;
        }

        private UINavigationController FindNavigationController()
        {
            foreach (var window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController.NavigationController != null)
                {
                    return window.RootViewController.NavigationController;
                }
                else
                {
                    UINavigationController val = CheckSubs
                        (window.RootViewController.ChildViewControllers);

                    if (val != null)
                    {
                        return val;
                    }
                }
            }

            return null;
        }

        private UINavigationController CheckSubs(UIViewController[] controllers)
        {
            foreach (var controller in controllers)
            {
                if (controller.NavigationController != null)
                {
                    return controller.NavigationController;
                }
                else
                {
                    UINavigationController val = CheckSubs
                        (controller.ChildViewControllers);

                    if (val != null)
                    {
                        return val;
                    }
                }
            }
            return null;
        }

        public class DocInteractionC : UIDocumentInteractionControllerDelegate
        {
            readonly UIViewController _viewController;

            public DocInteractionC(UIViewController controller)
            {
                _viewController = controller;
            }

            public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
            {

                return _viewController;

            }

            public override UIView ViewForPreview(UIDocumentInteractionController controller)
            {

                return _viewController.View;

            }

        }
        public string RetrivePathForPDF(string name)
        {
            return _documentsPath;
        }

    }
    public class PdfItem : QLPreviewItem
    {
        readonly string _uri;

        public PdfItem(string title, string uri)
        {
            ItemTitle = title;
            _uri = uri;
        }

        public override string ItemTitle { get; }

        public override NSUrl ItemUrl => NSUrl.FromFilename(_uri);
    }
    public class ItemPreviewControllerDataSource : QLPreviewControllerDataSource
    {
        readonly string _url;
        readonly string _filename;

        public ItemPreviewControllerDataSource(string url, string filename)
        {
            _url = url;
            _filename = filename;
        }

        public override IQLPreviewItem GetPreviewItem(QLPreviewController controller,
            nint index)
        {
            return new PdfItem(_filename, _url);
        }

        public override nint PreviewItemCount(QLPreviewController controller)
        {
            return 1;
        }
    }
}