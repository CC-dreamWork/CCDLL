
using UnityEngine;

namespace CCU
{
    public static class CCObjectPool
    {
        //private static CCObjectPool _ins;

        //public static CCObjectPool Ins
        //{
        //    get
        //    {
        //        if (_ins == null) _ins = new CCObjectPool();
        //        return _ins;
        //    }
        //}

        /// <summary>
        /// 取出
        /// </summary>
        public static T TakeOut<T>(string url) where T:Object
        {
            T obj = CCLoadManager.Ins.Load<T>(url);
            return obj;
        }
        /// <summary>
        /// 存储
        /// </summary>
        public static bool Storage<T>(string name)
        {
            return false;
        }


    }

}
