using BBSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.MyPublic
{
    /// <summary>
    /// 实现验证数据类,枚举表为tb_UsersByCustomer
    /// </summary>
    public class CheckUniqueByUsersByCustomer : ICheckUnique
    {
        public CheckUniqueByUsersByCustomer()
        { }
        public bool CheckUnique(string value, int MyType, int KeyID)
        {
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                bool IsExists = false;
                switch (MyType)
                {
                    case 1:
                        IsExists = db.tb_UsersByCustomer.Count(C => C.UserName == value) == 0;
                        break;
                    case 2:
                        IsExists = db.tb_UsersByCustomer.Count(C => C.Email == value) == 0;
                        break;
                }
                return IsExists;
            }
        }
    }
    /// <summary>
    /// 实现验证数据类,枚举表为tb_UsersBySystem
    /// </summary>
    public class CheckUniqueByUsersBySystem : ICheckUnique
    {
        public bool CheckUnique(string value, int MyType, int KeyID)
        {
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                bool IsExists = false;
                switch (MyType)
                {
                    case 1:
                        IsExists = db.tb_UsersBySystem.Count(C => C.UserName == value) == 0;
                        break;
                    case 2:
                        IsExists = db.tb_UsersBySystem.Count(C => C.Email == value) == 0;
                        break;
                }
                return IsExists;
            }
        }
    }

    /// <summary>
    /// 实现验证数据类,枚举表为tb_UserByRole
    /// </summary>
    public class CheckUniqueByUserByRole : ICheckUnique
    {        
        public bool CheckUnique(string value, int MyType, int KeyID)//实现ICheckUnique接口中的CheckUnique方法
        {
            using (DB_BBSEntities db = new DB_BBSEntities())//实例化操作数据库上下文类
            {
                bool IsExists = false;//定义数据验证是否成功变量
                switch (MyType)//判断要查询的字段，此处指查询RoleName角色名称
                {
                    case 1:
                        if (KeyID == 0)
                        {
                            //查询指定的角色名称是否存在于tb_UserByRole表中
                            IsExists = db.tb_UserByRole.Count(C => C.RoleName == value) == 0;
                        }
                        else
                        {
                            //查询指定的ID值和角色名称是否存在于tb_UserByRole表中
                            IsExists = db.tb_UserByRole.Count(C => C.RoleName == value && C.ID != KeyID) == 0;
                        }
                        break;
                }
                return IsExists;//返回验证结果
            }
        }
    }
    /// <summary>
    /// 实现验证数据类,枚举表为tb_UserByRole
    /// </summary>
    public class CheckUniqueByForumArea : ICheckUnique
    {
        public bool CheckUnique(string value, int MyType, int KeyID)
        {
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                bool IsExists = false;
                switch (MyType)
                {
                    case 1:
                        if (KeyID == 0)
                        {
                            IsExists = db.tb_ForumArea.Count(C => C.AreaName == value) == 0;
                        }
                        else
                        {
                            IsExists = db.tb_ForumArea.Count(C => C.AreaName == value && C.ID != KeyID) == 0;
                        }
                        break;
                }
                return IsExists;
            }
        }
    }
    public class CheckUniqueByForumClassify : ICheckUnique
    {
        public bool CheckUnique(string value, int MyType, int KeyID)
        {
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                bool IsExists = false;
                switch (MyType)
                {
                    case 1:
                        IsExists = db.tb_ForumClassify.Count(C => C.ClassifyName == value) == 0;
                        break;
                }
                return IsExists;
            }
        }
    }
}