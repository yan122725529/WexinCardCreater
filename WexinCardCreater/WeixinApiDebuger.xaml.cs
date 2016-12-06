using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Senparc.Weixin.MP.Containers;
using WexinCardCreater.Annotations;
using WexinCardCreater.Setting;
using WexinCardCreater.ThreadWapper;

namespace WexinCardCreater
{
    /// <summary>
    ///     WeixinApiDebuger.xaml 的交互逻辑
    /// </summary>
    public partial class WeixinApiDebuger : Window, INotifyPropertyChanged
    {
        private string _appId;
        private string _appSecret;
        private string _currentToken;

        public WeixinApiDebuger()
        {
            InitializeComponent();
            DataContext = this;
            Tabs = new ObservableCollection<TabViewModel>();
            var info = new TabViewModel();
            Tabs.Add(info);
            info.IsSelected = true;

        }

        public ObservableCollection<TabViewModel> Tabs { get; }

        //wxc62e07a280523aac
        public string AppId
        {
            get { return _appId; }
            set
            {
                _appId = value;
                OnPropertyChanged();
            }
        }


        //e783def39b437d06e891043f4ea830fa
        public string AppSecret
        {
            get { return _appSecret; }
            set
            {
                _appSecret = value;
                OnPropertyChanged();
            }
        }


        public string CurrentToken
        {
            get { return _currentToken.IsNullOrEmpty() ? "--" : _currentToken; }
            set
            {
                _currentToken = value;
                TokenCache.CurrentToken = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RefeshToken()
        {
            WpfTask.FactoryStartNew(() =>
            {
                while (true)
                {
                    if (AppId.IsNotNullOrEmpty() && AppSecret.IsNotNullOrEmpty())
                    {
                        var token = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);
                        UiThread.Invoke(() => { CurrentToken = token; });
                    }

                    Thread.Sleep(1000*60); //一分钟刷新一次
                }
            });
        }

        /// <summary>
        ///     获取token按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TokenButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AppId.IsNotNullOrEmpty() && AppSecret.IsNotNullOrEmpty())
            {
                CurrentToken = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);
                RefeshToken();
            }

            else
            {
                MessageBox.Show(this, "请输入AppId和AppSecret！");
            }
        }

        private void WeixinApiDebuger_OnLoaded(object sender, RoutedEventArgs e)
        {
            AppId = AppSetting.GetAppSetting().Appid;
            AppSecret = AppSetting.GetAppSetting().AppSecret;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Tabs.Count<2)return;
            var first = Tabs.FirstOrDefault(x => x.IsSelected);
            if (first != null)
                Tabs.Remove(first);
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tabViewModel = new TabViewModel();
            Tabs.Add(tabViewModel);
            tabViewModel.IsSelected = true;
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tmp= this.TabControl.SelectedContent;
        }
    }
}