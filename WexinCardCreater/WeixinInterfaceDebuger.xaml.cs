﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Senparc.Weixin.MP.Containers;
using WexinCardCreater.Http;

namespace WexinCardCreater
{
    /// <summary>
    ///     WeixinInterfaceDebuger.xaml 的交互逻辑
    /// </summary>
    public partial class WeixinInterfaceDebuger : Window, INotifyPropertyChanged
    {
        private string _appId;
        private string _appSecret;
        private string _currentToken;
        private string _requestBody;
        private string _requestType;
        private string _requestUrl;
        private string _responseBody;

        public WeixinInterfaceDebuger()
        {
            InitializeComponent();
            DataContext = this;
        }

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
                OnPropertyChanged();
            }
        }


        public string RequestType
        {
            get { return _requestType; }
            set
            {
                _requestType = value;
                OnPropertyChanged();
            }
        }

        public string RequestUrl
        {
            get { return _requestUrl; }
            set
            {
                _requestUrl = value;
                OnPropertyChanged();
            }
        }

        public string RequestBody
        {
            get { return _requestBody; }
            set
            {
                _requestBody = value;
                OnPropertyChanged();
            }
        }

        public string ResponseBody
        {
            get { return _responseBody; }
            set
            {
                _responseBody = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     获取token按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TokenButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AppId.IsNotNullOrEmpty() && AppSecret.IsNotNullOrEmpty())
                CurrentToken = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);
            else
                MessageBox.Show(this, "请输入AppId和AppSecret！");
        }

        private void UrlButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentToken.IsNullOrEmpty())
            {
                MessageBox.Show(this, "请获取token后查询");
                return;
            }
            var realurl = string.Format(RequestUrl.Contains("?") ? @"{0}&access_token={1}" : @"{0}?access_token={1}", RequestUrl, CurrentToken);
            if (RequestType.ToUpper() == "POST")
            {
                var responsejson = HttpHelper.HttpRequestPost(realurl, RequestBody);
                ResponseBody = responsejson;
            }
            if (RequestType.ToUpper() != "GET") return;
            {
                var responsejson = HttpHelper.HttpRequestGet(realurl);
                ResponseBody = responsejson;
            }
        }
    }
}