using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPublic
{
    /// <summary>
    /// 提供站点资源属性接口
    /// </summary>
    public interface IGettingUrl
    {
        /// <summary>
        /// Image目录地址
        /// </summary>
        string ContentImagesUrl { get; }
        /// <summary>
        /// SecondImage目录地址
        /// </summary>
        string ContentSecondImageUrl { get; }
        /// <summary>
        /// Bootstrap目录地址
        /// </summary>
        string ContentBootstrapUrl { get; }
        /// <summary>
        /// Css目录地址
        /// </summary>
        string ContentCssUrl { get; }
        /// <summary>
        /// 自定Javascript脚本文件目录地址
        /// </summary>
        string ContentJSUrl { get; }
        /// <summary>
        /// Uedit目录地址
        /// </summary>
        string ContentUedit { get; }
        /// <summary>
        /// Script目录地址
        /// </summary>
        string ScriptUrl { get; }  
    }
}
