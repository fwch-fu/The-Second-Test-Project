using BBSSite.Models;
using MyPublic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BBSSite.MyPublic
{
    /// <summary>
    /// 自定义验证类特性，AttributeUsage表示DataUnique特性类的用法，ValidationAttribute类表示所有验证属性的基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DataUnique : ValidationAttribute
    {        
        public EnumDataUnique edu { get; set; }//定义全局枚举，表示验证的表
        public int MyType { get; set; }//定义实现同一表中不同字段的验证
        public string Key { get; set; }//定义验证的数据需要指定其他条件的字段名称
        public DataUnique() { }//构造方法
        //实现抽象类ValidationAttribute中IsValid方法，该方法用于验证属性值是否有效，Value为验证的值，validationContext为要验证的类
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)//判断如果value值不为空
            {
                int KeyID = 0;//定义其他条件查询的条件值变量
                if (Key != null && Key != "")//如果不为空，则表示需要制定其他条件查询，Key为属性的名称，例如“ID”
                {
                    //通过反射获取出条件属性的值
                    KeyID = Convert.ToInt32(validationContext.ObjectInstance.GetType().GetProperty(Key).GetValue(validationContext.ObjectInstance));
                }
                //通过传入表枚举值调用简单工厂模式类的InitDataUnique方法，并返回已实现了ICheckUnique接口的各实现类
                ICheckUnique iDataUnique = PublicInitiali.InitDataUnique(edu);
                //调用实现类中CheckUnique方法，参数分别为要验证的属性值、验证类型和其他条件值。方法实现了对数据库表的数据校验工作
                if (iDataUnique.CheckUnique((string)value, MyType, KeyID))//如果验证成功，则返回true
                {   
                    return ValidationResult.Success;//返回验证成功
                }
            }            
            return new ValidationResult(null);//返回验证失败，value值为null
        }
    }
    /// <summary>
    /// 数据表的枚举
    /// </summary>
    public enum EnumDataUnique
    {
        tb_ForumArea = 1,
        tb_ForumClassify,
        tb_ForumInfoStatus,
        tb_ForumMain,
        tb_ForumSecond,
        tb_UserByPermission,
        tb_UserByRole,
        tb_ZY_Sex,
        tb_UsersByCustomer,
        tb_UsersBySystem,
    }
}