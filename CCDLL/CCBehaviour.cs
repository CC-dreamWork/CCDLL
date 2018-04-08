using UnityEngine;

namespace CC.UpdateManager
{
    /// <summary>
    /// 相当于monobehaviour
    /// </summary>
    public abstract class CCBehaviour : MonoBehaviour, IUpdate
    {
        /// <summary>
        /// Update
        /// </summary>
        protected virtual void COnUpdate(){ }

        /// <summary>
        /// FixedUpdate
        /// </summary>
        protected virtual void COnFixedUpdate(){}

        /// <summary>
        /// 外放的清除UpdateManager监控方法 
        /// </summary>
        protected virtual void COnDestroy()
        {
            COnManagerDe();
        }

        private void OnDestroy()
        {
            COnManagerDe();
        }

        /// <summary>
        /// 清除UpdateManager的监控
        /// </summary>
        private void COnManagerDe()
        {
            CCUpdateManager.Ins.RemoveFixedUpdate(this);
            CCUpdateManager.Ins.RemoveUpdate(this);
        }

        /// <summary>
        /// UndateManager增加Update监控
        /// </summary>
        protected void CcAddUpdate()
        {
            CCUpdateManager.Ins.AddUpdate(this);
        }

        /// <summary>
        /// UndateManager增加FixedUpdate监控
        /// </summary>
        protected void CcAddFixedUpdate()
        {
            CCUpdateManager.Ins.AddFixedUpdate(this);
        }

        /// <summary>
        /// FixedUpdate执行
        /// </summary>
        public void OnFixedUpdate()
        {
            COnFixedUpdate();
        }

        /// <summary>
        /// Update执行
        /// </summary>
        public void OnUpdate()
        {
            COnUpdate();
        }


    }
}

