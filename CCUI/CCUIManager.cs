
using System.Collections.Generic;
using UnityEngine;
using CCU;

namespace CC.UI
{
    public class CCUIManager
    {
        private static CCUIManager _ins;

        public static CCUIManager Ins
        {
            get
            {
                if (_ins == null) _ins = new CCUIManager();
                return _ins;
            }
        }

        private readonly Dictionary<int, string> _uiUrl = new Dictionary<int, string>();
        private readonly Dictionary<int, GameObject> _uiParams = new Dictionary<int, GameObject>();
        private readonly Dictionary<int, CCUIBase> _oldUi = new Dictionary<int, CCUIBase>();

        public CCUIManager()
        {
            _uiUrl.Add((int) CCUiType.MainUi, "");
        }

        private GameObject UiParms(CCUiType ccuitype)
        {
            int type = (int) ccuitype;
            GameObject go = _uiParams.ContainsKey(type)? _uiParams[type]: Resources.Load<GameObject>(_uiUrl[type]);
            return go;
        }

        public void OpenUi(CCUiType ccuitype, bool coexist = false)
        {
            GameObject go = UiParms(ccuitype);
            if (go)
            {
                go.transform.SetAsLastSibling();
                CCUIBase ccuibase = go.GetComponent<CCUIBase>();
                ccuibase.UiAniIeStop();
                ccuibase.Init();
                go.SetActive(true);
                ccuibase.CCUiAni(true);
                if (!coexist)
                {
                    foreach (var old in _oldUi) CCuiBaseClose(old.Value);
                    _oldUi.Clear();
                }
            }
            _oldUi.Add((int) ccuitype, go.GetComponent<CCUIBase>());
        }

        public void CloseUi(CCUiType ccuitype)
        {
            int key = (int) ccuitype;
            if (_uiParams.ContainsKey(key))
            {
                CCuiBaseClose(_uiParams[key].GetComponent<CCUIBase>());
                if (_oldUi.ContainsKey(key)) _oldUi.Remove(key);
            }
            else
            {
                Debug.LogError("没找到UI" + ccuitype);
            }
        }

        private void CCuiBaseClose(CCUIBase ccuibase)
        {
            ccuibase.UiAniIeStop();
            ccuibase.Close();
            ccuibase.CCUiAni(false);
        }
    }
}
