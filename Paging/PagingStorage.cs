using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Paging
{
    public static class PagingStorage
    {
        public static bool IsNull = false;
        /// <summary>
        /// 数据总数
        /// </summary>
        public static int DataCount;
        /// <summary>
        /// 每页要显示的数据总数
        /// </summary>
        public static int ShowDataCount;
        /// <summary>
        /// 最多显示的页码数
        /// </summary>
        public static int ShowPageCount;
        /// <summary>
        /// 当前页
        /// </summary>
        public static int CurrentPageIndex;
        /// <summary>
        /// 页码起始值
        /// </summary>
        public static int StartIndex = 0;
        /// <summary>
        /// 页码结束值
        /// </summary>
        public static int EndIndex = 0;
        /// <summary>
        /// 已实现了接口ISettingPaging的实例对象
        /// </summary>
        public static ISettingPaging SettingPaging;
    }
}
