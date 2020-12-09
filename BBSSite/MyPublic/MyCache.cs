using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace BBSSite.MyPublic
{    
    public class MyCache//自定义缓存类
    {
        public static MyCache Current = new MyCache();//定义静态实例对象
        Cache cache = null;//定义缓存类变量
        public MyCache()
        {
            cache = HttpRuntime.Cache;//获取当前应用程序运行时的Cache对象
        }
        public bool Contains(string Key)
        {
            return cache.Get(Key) != null;//判断缓存对象是否存在
        }
        public T Get<T>(string Key)//获取缓存
        {
            if (Contains(Key))//如果缓存存在
            {
                return (T)cache[Key];//返回缓存对象
            }
            return default(T);//否则返回默认值
        }
        public void Add<T>(T DataEntity, string Key)//添加缓存对象，设置有效时间为20分钟
        {
            //使用Key作为键将DataEntity对象添加到缓存中，过期参数为无绝对过期时间，并设置间隔20分钟无访问过期策略
            cache.Add(Key, DataEntity, null, Cache.NoAbsoluteExpiration, TimeSpan.Parse("00:20:00"), CacheItemPriority.Default, null);
        }
        public void AddNoExpiration<T>(T DataEntity, string Key)//添加缓存对象，设置无过期时间
        {
            //使用Key作为键将DataEntity对象添加到缓存中，并且设置无过期时间
            cache.Add(Key, DataEntity, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }
        public void UpdateNoExpiration<T>(T DataEntity, string Key)//更新缓存对象，设置无过期时间
        {
            //将Key键的对象更新为最新的DataEntity，并且设置无过期时间
            cache.Insert(Key, DataEntity, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }
        public void AddSingle<T>(T DataEntity, string Key)//向已添加到缓存中的集合(List)中追加一条新纪录
        {
            IList<T> List = Get<IList<T>>(Key);//获取缓存中List集合数据
            List.Add(DataEntity);              //将单条数据追加到集合中
            Update<IList<T>>(List, Key);       //重新更新该缓存对象
        }
        public void Update<T>(T DataEntity, string Key)//更新缓存对象，设置有效时间为20分钟
        {
            cache.Insert(Key, DataEntity, null, Cache.NoAbsoluteExpiration, TimeSpan.Parse("00:20:00"), CacheItemPriority.Default, null);
        }
        public void Remove(string Key)//移除缓存对象
        {
            if (Contains(Key))        //判断缓存项是否存在
            {
                cache.Remove(Key);    //移除缓存项
            }
        }
    }
}