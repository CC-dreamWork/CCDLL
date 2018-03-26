
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.UpdateManager
{
    public class CCUpdateManager : MonoBehaviour
    {
        private static readonly List<IUpdate> UpdateList = new List<IUpdate>();
        private static readonly List<IUpdate> FixedupdateList = new List<IUpdate>();

        private static CCUpdateManager _ins;


        public static CCUpdateManager Ins
        {
            get
            {
                if (_ins) return _ins;
                GameObject ccupdate = new GameObject("CCUpdateManager");
                ((Object)ccupdate).hideFlags = HideFlags.HideInHierarchy;
                Object.DontDestroyOnLoad((Object)ccupdate);
                _ins = ccupdate.AddComponent<CCUpdateManager>();
                return _ins;
            }
        }

        /// <summary>
        /// 添加Update更新
        /// </summary>
        /// <param name="call"></param>
        public void AddUpdate(IUpdate call)
        {
            if (!UpdateList.Contains(call)) UpdateList.Add(call);
        }
        /// <summary>
        /// 移除Update更新
        /// </summary>
        /// <param name="call"></param>
        public void RemoveUpdate(IUpdate call)
        {
            if (UpdateList.Contains(call)) UpdateList[UpdateList.IndexOf(call)] = null;
        }

        /// <summary>
        /// 添加Update更新
        /// </summary>
        /// <param name="call"></param>
        public void AddFixedUpdate(IUpdate call)
        {
            if (!FixedupdateList.Contains(call)) FixedupdateList.Add(call);
        }
        /// <summary>
        /// 移除Update更新
        /// </summary>
        /// <param name="call"></param>
        public void RemoveFixedUpdate(IUpdate call)
        {
            if (FixedupdateList.Contains(call)) FixedupdateList[FixedupdateList.IndexOf(call)] = null;
        }

         // 之所以是从后向前遍历,是因为  从前向后遍历删除后索引值会不断变化
        void Update()
        {
            var num = UpdateList.Count;
            for (int i = num-1; i >=0; i--)
            {
                if (UpdateList[i] != null) UpdateList[i].OnUpdate();
                else UpdateList.RemoveAt(i);
            }

        }
        // 之所以是从后向前遍历,是因为  从前向后遍历删除后索引值会不断变化
        void FixedUpdate()
        {
            var num = FixedupdateList.Count;
            for (int i = num - 1; i >= 0; i--)
            {
                if (FixedupdateList[i] != null) FixedupdateList[i].OnFixedUpdate();
                else FixedupdateList.RemoveAt(i);
            }

        }
        /// <summary>
        /// 开启协程
        /// </summary>
        /// <param name="Ie"></param>
        /// <returns></returns>
        public Coroutine CCStartCoroutine(IEnumerator Ie)
        {
           return StartCoroutine(Ie);
        }
        /// <summary>
        /// 关闭协程
        /// </summary>
        /// <param name="Ie"></param>
        public void CCStopCoroutine(Coroutine Ie)
        {
             StopCoroutine(Ie);
        }


    }
}
