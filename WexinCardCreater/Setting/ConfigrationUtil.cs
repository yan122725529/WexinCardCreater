using System.Collections;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WexinCardCreater.Setting
{
    //<summary>     
    // 读写XML类   
    // 适合各种config 以及自定义配置   
    // </summary>
    public static class ConfigrationUtil
    {
        public static string ConfigPath { get; private set; }

        public static void SetZlWebConfigPath(string configPath)
        {
            ConfigPath = configPath;
        }

        /// <summary>
        ///     保存XML
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SaveConfig(string key, string value)
        {
            var doc = new XmlDocument();
            //获得配置文件的全路径            
            var strFileName = ConfigPath;
            //加载Xml            
            doc.Load(strFileName);
            //找出名称为“appSettings”的所有元素             
            var nodes = doc.GetElementsByTagName("appSettings");
            for (var i = 0; i < nodes[0].ChildNodes.Count; i++)
            {
                var node = nodes[0].ChildNodes[i];
                if (node.Attributes["key"].Value.Equals(key))
                    node.Attributes["value"].Value = value;
            } //保存上面的修改        
            doc.Save(strFileName);
            return true;
        }

        /// <summary>
        ///     读取XML
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadConfig(string key)
        {
            //加载Xml             
            var rootElement = XElement.Load(ConfigPath);
            //找出名称为“appSettings”的所有元素       
            var firstOrDefault = rootElement.Elements(key).FirstOrDefault();
            if (firstOrDefault != null)
                return firstOrDefault.Value;
            return string.Empty;
        }

        /// <summary>
        ///     读取XML
        /// </summary>
        /// <param name="configname"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadConfigs(string configname, string key)
        {
            var exceptionInfo = (IDictionary) ConfigurationManager.GetSection(configname);
            if (exceptionInfo[key] == null) return string.Empty;
            var pvalues = exceptionInfo[key].ToString();
            return pvalues;
        }
    }
}