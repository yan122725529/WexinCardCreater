using System.Windows;
using System.Windows.Controls;

namespace WexinCardCreater
{
    /// <summary>
    ///     WeixinRequestCreater.xaml 的交互逻辑
    /// </summary>
    public partial class WeixinRequestCreater : UserControl
    {
        public WeixinRequestCreater()
        {
            InitializeComponent();
        }


        private void UrlButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dataContext = DataContext as TabViewModel;

            dataContext?.UrlButton_OnClick(sender, e);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dataContext = DataContext as TabViewModel;
            dataContext?.ButtonBase_OnClick(sender, e);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataContext = DataContext as TabViewModel;
            dataContext?.Selector_OnSelectionChanged(sender, e);
        }
    }
}