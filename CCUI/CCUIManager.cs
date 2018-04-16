
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

        private readonly Dictionary<int, string> _uiUrl=new Dictionary<int, string>();
        private readonly Dictionary<int, GameObject> _uiParams=new Dictionary<int, GameObject>();
        public CCUIManager() {
            _uiUrl.Add((int)CCUiType.MainUi,"");
        }

        private void AddUi()
        {

        }

        private GameObject UiParms(CCUiType ccuitype)
        {
            int type = (int)ccuitype;
            GameObject go;
            if (_uiParams.ContainsKey(type)) go = _uiParams[type];
            else  go=LoadManager.Ins.Load<GameObject>(_uiUrl[type]);
            return go;
        }

        public void OpenUi(CCUiType ccuitype,bool coexist=false)
        {
            _uiParams[(int)ccuitype].transform.SetAsLastSibling();
        }
    }
}
