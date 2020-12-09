using MyPublic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BBSSite.MyPublic
{
    public class PublicFunctions
    {
        /// <summary>
        /// 将指定的实体序列化为Json字符串
        /// </summary>
        /// <typeparam name="T">泛型实体类型</typeparam>
        /// <param name="Entity">实体数据</param>
        /// <returns>已序列化的实体Json字符串</returns>
        public static string ToJson<T>(T Entity)
        {
            DataContractJsonSerializer JsonSerializer = new DataContractJsonSerializer(Entity.GetType());
            using (MemoryStream MS = new MemoryStream())
            {
                JsonSerializer.WriteObject(MS, Entity);
                return Encoding.UTF8.GetString(MS.ToArray());
            }
        }
        /// <summary>
        /// 将指定的字符串反序列化为实体对象
        /// </summary>
        /// <typeparam name="T">要序列化的泛型实体类型</typeparam>
        /// <param name="JsonString">Json字符串</param>
        /// <returns>已序列化的实体对象</returns>
        public static T ParseJson<T>(string JsonString)
        {
            using (MemoryStream MS = new MemoryStream(Encoding.UTF8.GetBytes(JsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(MS);
            }
        }
        /// <summary>
        /// 返回原页面(如果存在)或首页并支持发送消息方法,可通过委托自定义返回的页面
        /// </summary>
        /// <param name="C">当前请求的控制器对象</param>
        /// <param name="Key">发送消息的Key标识</param>
        /// <param name="Message">发送消息的值</param>
        /// <param name="oldUrl">如果原地址对象存在则调用的委托对象,该调用传入原地址</param>
        /// <param name="defaultUrl">如果原地址对象不存在则调用的委托对象,该调用传入以"~/"代表为首页的地址</param>
        /// <returns></returns>
        public static ActionResult ToRedirect(Controller C, string Key, string Message, Func<string, RedirectResult> oldUrl, Func<string, RedirectToRouteResult> defaultUrl)
        {
            if (Key != null && Message != null)
            {
                C.TempData[Key] = Message;
            }
            string AbsolutePath = "";
            if (C.Request != null && C.Request.UrlReferrer != null && (AbsolutePath = C.Request.UrlReferrer.AbsolutePath) != "")
            {
                return oldUrl(AbsolutePath);
            }
            else
            {
                return defaultUrl("~/");
            }
        }

        /// <summary>
        /// 通过调用构造资源路径类向视图动态数据类型Urls加载资源实体数据
        /// </summary>
        public static void SetUrls(dynamic ViewBag, UrlHelper Url)
        {
            ViewBag.Urls = PublicInitiali.InitGettingUrl(Url);
        }

        /// <summary>
        /// MD5加密,通常为将密码数据进行加密
        /// </summary>
        /// <param name="Key">传入要加密的字符串</param>
        /// <returns>返回已经过加密的字符串</returns>
        public static string MD5(string Key)
        {
            MD5CryptoServiceProvider MD5Hasher = new MD5CryptoServiceProvider();
            byte[] data = MD5Hasher.ComputeHash(Encoding.Unicode.GetBytes(Key));
            StringBuilder ber = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                ber.Append(data[i].ToString("x2"));
            }
            return ber.ToString();
        }

        public static string CalcColumnCode(int ColumnGrade, string CurrentCode, string ChooseColumnGrade)
        {
            if (ColumnGrade == 1)
            {
                if (CurrentCode == null)
                {
                    return "A";
                }
                byte[] arr = new byte[1];
                arr = Encoding.ASCII.GetBytes(CurrentCode);
                int ASCIICode = (short)arr[0];
                ASCIICode++;
                if (ASCIICode > 90)
                {
                    return null;
                }
                arr[0] = (byte)Convert.ToInt32(ASCIICode);
                return Encoding.ASCII.GetString(arr);
            }
            else
            {
                if (CurrentCode == null)
                {
                    return ChooseColumnGrade + "-1";
                }
                string[] CodeSp = CurrentCode.Split('-');
                string CodeA = CodeSp[0];
                int Code = Convert.ToInt32(CodeSp[1]);
                return CodeA + "-" + (++Code);
            }
        }
        public static string GetRawUrl(HttpRequestBase Request)
        {
            string RawUrl = "";
            string[] Segments = Request.Url.Segments;
            for (int si = 0; si < 4 && si < Segments.Length; si++)
            {
                RawUrl = RawUrl + Segments[si];
            }
            if (RawUrl.IndexOf("/", RawUrl.Length - 1) > -1)
            {
                RawUrl = RawUrl.Substring(0, RawUrl.Length - 1);
            }
            return RawUrl;
        }
    }
}