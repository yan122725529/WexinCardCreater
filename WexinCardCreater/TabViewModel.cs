using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using WexinCardCreater.Annotations;
using WexinCardCreater.Http;

namespace WexinCardCreater
{
    public class TabViewModel: INotifyPropertyChanged
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value;
                OnPropertyChanged();
            }
        }

       

        private FileInfo _curSelectdFile;
        private ObservableCollection<FileInfo> _fileHis = new ObservableCollection<FileInfo>();

        private string _requestBody;
        private string _requestType;
        private string _requestUrl;
        private string _responseBody;
        private ObservableCollection<string> _urlList = new ObservableCollection<string>();

        public string PageGuid { get; set; }

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

        public ObservableCollection<string> UrlList
        {
            get { return _urlList; }
            set
            {
                _urlList = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<FileInfo> FileHis
        {
            get { return _fileHis; }
            set
            {
                _fileHis = value;
                OnPropertyChanged();
            }
        }

        public FileInfo CurSelectdFile
        {
            get { return _curSelectdFile; }
            set
            {
                _curSelectdFile = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void UrlButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TokenCache.CurrentToken.IsNullOrEmpty())
            {
                MessageBox.Show("请获取token后查询");
                return;
            }
            var realurl = string.Format(RequestUrl.Contains("?") ? @"{0}&access_token={1}" : @"{0}?access_token={1}",
                RequestUrl, TokenCache.CurrentToken);
            if (RequestType.ToUpper() == "POST")
            {
                var responsejson = HttpHelper.HttpRequestPost(realurl, RequestBody);
                ResponseBody = responsejson;
            }
            if (RequestType.ToUpper() == "GET")
            {
                var responsejson = HttpHelper.HttpRequestGet(realurl);
                ResponseBody = responsejson;
            }

            UrlList.Add(RequestUrl);
        }


        public void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "请选择文件",
                Filter = "所有文件(*.*)|*.*"
            };

            if (fileDialog.ShowDialog() ?? false)
                try
                {
                    using (var sr = new StreamReader(fileDialog.FileName, Encoding.UTF8))
                    {
                        var build = new StringBuilder();
                        string line;
                        while ((line = sr.ReadLine()) != null)
                            build.AppendLine(line);
                        RequestBody = build.ToString();
                        sr.Close();
                    }

                    var info = new FileInfo(fileDialog.FileName);
                    var first = FileHis.FirstOrDefault(x => x.FullName == fileDialog.FileName);

                    if (first == null)
                    {
                        FileHis.Add(info);
                        CurSelectdFile = info;
                    }
                    else
                    {
                        CurSelectdFile = first;
                    }
                }
                catch (IOException ex)
                {
                }
        }

        public void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          if (CurSelectdFile==null) return;

            using (var sr = new StreamReader(CurSelectdFile.FullName, Encoding.UTF8))
            {
                var build = new StringBuilder();
                string line;
                while ((line = sr.ReadLine()) != null)
                    build.AppendLine(line);
                RequestBody = build.ToString();
                sr.Close();
            }
        }
    }
}