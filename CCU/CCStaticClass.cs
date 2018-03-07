using System;

using UnityEngine;

namespace CCU
{
    /// <summary>
    /// 静态类
    /// </summary>
    public static class CCStaticClass
    {
        /// <summary>
        /// 动画状态机切动画特殊处理（为了短时间内切动画失败问题）
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="name"></param>
        /// <param name="transitionDuration"></param>
        /// <param name="layer"></param>
        /// <param name="normalizedTime"></param>
        public static void CCSetInteger(this Animator animator, string name, int transitionDuration, int layer = 0, float normalizedTime = float.NegativeInfinity)
        {
            animator.Update(0);
            if (animator.GetNextAnimatorStateInfo(layer).fullPathHash == 0)
            {
                animator.SetInteger(name, transitionDuration);
            }
            else
            {
                animator.Play(animator.GetNextAnimatorStateInfo(layer).fullPathHash, layer);
                animator.Update(0);
                animator.SetInteger(name, transitionDuration);
            }
        }
    }
}
