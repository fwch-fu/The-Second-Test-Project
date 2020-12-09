using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyPublic
{
    /// <summary>
    /// 获取站点资源属性
    /// </summary>
    internal class GettingUrl : IGettingUrl
    {
        UrlHelper url;
        /// <summary>
        /// 通过传入UrlHelper对象,构造站点资源类
        /// </summary>
        /// <param name="Url">Url帮助类</param>
        public GettingUrl(UrlHelper Url)
        {
            this.url = Url;
        }
        /// <summary>
        /// Image目录地址
        /// </summary>
        public string ContentImagesUrl { get { return url.Content("~/Content/image"); } }
        /// <summary>
        /// SecondImage目录地址
        /// </summary>
        public string ContentSecondImageUrl { get { return url.Content("~/Content/secondImage"); } }
        /// <summary>
        /// Bootstrap目录地址
        /// </summary>
        public string ContentBootstrapUrl { get { return url.Content("~/Content/bootstrap"); } }
        /// <summary>
        /// Css目录地址
        /// </summary>
        public string ContentCssUrl { get { return url.Content("~/Content/css"); } }
        /// <summary>
        /// 自定Javascript脚本文件目录地址
        /// </summary>
        public string ContentJSUrl { get { return url.Content("~/Content/js"); } }
        /// <summary>
        /// Uedit目录地址
        /// </summary>
        public string ContentUedit { get { return url.Content("~/Content/uedit"); } }
        /// <summary>
        /// Script目录地址
        /// </summary>
        public string ScriptUrl { get { return url.Content("~/Scripts"); } }
    }
}
