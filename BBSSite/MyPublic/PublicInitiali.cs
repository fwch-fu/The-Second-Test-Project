using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyPublic
{
    /// <summary>
    /// 公共初始化对象类
    /// </summary>
    public class PublicInitiali
    {
        /// <summary>
        /// 初始化资源访问对象类
        /// </summary>
        /// <param name="Url">Url帮助类</param>
        /// <returns>资源访问对象类</returns>
        public static IGettingUrl InitGettingUrl(UrlHelper Url)
        {
            return new GettingUrl(Url);
        }
        /// <summary>
        /// 通过标记的枚举表的值,返回已实现了IDataUnique接口的类
        /// </summary>
        /// <param name="edu">枚举的表</param>
        /// <returns>返回指定表的验证类</returns>
        public static ICheckUnique InitDataUnique(EnumDataUnique edu)
        {
            //定义ICheckUnique接口变量
            ICheckUnique iDataUnique = null;
            switch (edu)//判断枚举表
            {
                case EnumDataUnique.tb_UsersByCustomer://验证tb_UsersByCustomer表
                    iDataUnique = new CheckUniqueByUsersByCustomer();//初始化CheckUniqueByUsersByCustomer实现类
                    break;
                case EnumDataUnique.tb_UsersBySystem:  //验证tb_UsersBySystem表
                    iDataUnique = new CheckUniqueByUsersBySystem();//初始化CheckUniqueByUsersBySystem实现类
                    break;
                case EnumDataUnique.tb_UserByRole:     //验证tb_UserByRole表
                    iDataUnique = new CheckUniqueByUserByRole();//初始化CheckUniqueByUserByRole实现类
                    break;
                case EnumDataUnique.tb_ForumArea:      //验证tb_ForumArea表
                    iDataUnique = new CheckUniqueByForumArea();//初始化CheckUniqueByForumArea实现类
                    break;
                case EnumDataUnique.tb_ForumClassify:  //验证tb_ForumClassify表
                    iDataUnique = new CheckUniqueByForumClassify();//初始化CheckUniqueByForumClassify实现类
                    break;
            }
            return iDataUnique;
        }
    }
}
