
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CCU
{
    public class LoadManager
    {
        private static LoadManager _ins;

        public static LoadManager Ins
        {
            get
            {
                if(_ins==null) _ins=new LoadManager();
                return _ins;
            }
        }

        public T Load<T>(string url) where T : Object
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("LoadManage.LoadRes load failed , url = " + url);
                return null;
            }
            T obj = Resources.Load<T>(url);
            if (obj == null)
            {
                Debug.LogError("LoadManage.LoadRes load failed , url = " + url);
            }
            return obj;
        }
    }

}
