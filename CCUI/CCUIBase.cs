using UnityEngine;
using CC.UpdateManager;
using System;
using System.Collections;

namespace CC.UI
{
   public abstract class CCUIBase:CCBehaviour
   {
       private Coroutine _anicor;
       public abstract void Init();

       public abstract void Close();

       public abstract void MessageChange();

       public void UiAniIeStop()
       {
           if (_anicor != null) StopCoroutine(_anicor);
       }

       public void CCUiAni(bool open)
        {
            float leng = 0f;
            Animation ani = gameObject.GetComponent<Animation>();
            if (!ani) return;
            foreach (AnimationState state in ani)
            {
                if (open)
                {
                    if (!Aniname(state.clip.name, "OpenAni")) continue;
                    ani.Play(state.clip.name);
                    break;
                }
                if (!Aniname(state.clip.name, "CloseAni")) continue;
                ani.Play(state.clip.name);
                leng = state.clip.length;
                break;
            }
            if (!open) _anicor =StartCoroutine(AniCloseF(leng));
        }
       private IEnumerator AniCloseF(float time)
       {
           yield return new WaitForSeconds(time);
           gameObject.SetActive(false);
       }

       private bool Aniname(string name, string name1)
        {
            var naarr = name.Split('_');
            return (naarr.Length > 1 && naarr[1] == name1);
        }
    }
}
