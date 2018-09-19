using FukaboriCore.Model;
using FukaboriCore.Service;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FukaboriWpf
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Register();
            base.OnStartup(e);

        }

        private void Register()
        {
            SimpleIoc.Default.Register<System.Net.Http.HttpClient>(() => {
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "fukabori-windows");
                return client;
            });
          //  SimpleIoc.Default.Register<IShowMessageService, ShowMessageService>();
            SimpleIoc.Default.Register<IFileService, FileService>();
            SimpleIoc.Default.Register<ISetClipBoardService, SetClipBoardService>();
//            SimpleIoc.Default.Register<IShowFolderService, ShowFolderService>();

        }
    }
}
