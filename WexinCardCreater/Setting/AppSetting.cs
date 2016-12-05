using System;

namespace WexinCardCreater.Setting
{
    [Serializable]
    public class AppSetting
    {
        private static AppSetting _appSetting;
        //public ZlWebApiAppSetting();
        private AppSetting()
        {
        }

        public static AppSetting GetAppSetting()
        {
            if (_appSetting == null)
            {
                var configpath = AppDomain.CurrentDomain.BaseDirectory;
#if DEBUG
                if (configpath.LastIndexOf(@"bin", StringComparison.Ordinal) == -1)
                    configpath = configpath + @"bin";
#endif
                _appSetting = new AppSetting();
                _appSetting.LoadData(configpath + "\\AppSetting.xml");
            }

            return _appSetting ?? (_appSetting = new AppSetting()
                   );
        }

        public void LoadData(string file)
        {
            ConfigrationUtil.SetZlWebConfigPath(file);

            foreach (var info in typeof(AppSetting).GetProperties())
                try
                {
                    var configvalue = ConfigrationUtil.ReadConfig(info.Name);
                    _appSetting.GetType().GetProperty(info.Name).SetValue(_appSetting, configvalue, null);
                }
                catch (Exception)
                {
                    throw;
                }
        }

        #region appinfo

        //e783def39b437d06e891043f4ea830fa
        public string AppSecret { get; set; }
        //wxc62e07a280523aac
        public string Appid { get; set; }
        #endregion
    }
}