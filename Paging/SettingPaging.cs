using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Paging
{
    public interface ISettingPaging
    {
        /// <summary>
        /// 首尾页标签,长度为6,标签名字、首文本值、尾文本值、标签常规Calss属性,标签选中Class属性,标签自定义属性
        /// </summary>
        string[] FrstOrLast { get; }
        /// <summary>
        /// 前后页标签,长度为6,标签名字、前一页文本值、后一页文本值、标签常规Calss属性,标签选中Class属性,标签自定义属性
        /// </summary>
        string[] FrontOrAfter { get; }
        /// <summary>
        /// 页码标签,长度为4,标签名字、标签常规Calss属性,标签选中Class属性,标签自定义属性
        /// </summary>
        string[] PageNumber { get; }
        /// <summary>
        /// 输入页码标签,长度为3,标签名字、标签常规Calss属性,标签自定义属性
        /// </summary>
        string[] InputPageNumber { get; }
        /// <summary>
        /// 跳转标签,长度为4,标签名字、显示文本值、标签常规Calss属性,标签自定义属性
        /// </summary>
        string[] GotoPageIndex { get; }
        /// <summary>
        /// 显示文字标签,长度为3,标签名字、标签常规Calss属性,标签自定义属性
        /// </summary>
        string[] ShowChar { get; }
    }
    public sealed class DefaultPaging : ISettingPaging
    {
        public string[] FrstOrLast
        {
            get
            {
                return new string[6] { "a", "首页", "尾页", "PageButton PageFrstOrLast", "PageButtonSelected PageFrstOrLastSelected", "onclick=\"PagingClick({0})\"" };
            }
        }
        public string[] FrontOrAfter
        {
            get
            {
                return new string[6] { "a", "上一页", "下一页", "PageButton PageFrontOrAfter", "PageButtonSelected PageFrontOrAfterSelected", "onclick=\"PagingClick({0})\"" };
            }
        }
        public string[] PageNumber
        {
            get
            {
                return new string[4] { "a", "PageButton PagePageNumber", "PageButtonSelected PagePageNumberSelected", "onclick=\"PagingClick({0})\"" };
            }
        }
        public string[] InputPageNumber
        {
            get
            {
                return new string[3] { "input type=\"text\"", "PageButton PageInputPageNumber", "id=\"InputPageNumber\" onblur=\"PagingInputBlur(this)\" onclick=\"PagingInputClick(this)\"" };
            }
        }
        public string[] GotoPageIndex
        {
            get
            {
                return new string[4] { "input type=\"button\"", "跳转", "PageButton GotoPageIndex", "id=\"GotoPageIndex\" onclick=\"GotoPageIndexClick(this)\"" };
            }
        }
        public string[] ShowChar
        {
            get
            {
                return new string[3] { "lable", "PageButton PageShowChar", "" };
            }
        }
    }

}
