using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ssb.param;

namespace ssb
{
    // プレイヤー管理
    public class PLManager : SingletonMonoBehaviour<PLManager>
    {

        #region プロパティ

        // 生存中のPLオブジェクト
        public GameObject _CurrentPLObj { private set; get; } = null;

        #endregion // プロパティ

        #region フィールド

        [SerializeField]
        private GameObject _PLPrefab = null;

        [SerializeField]
        private GameObject _PLInstObj = null;

        #endregion  // フィールド

        #region 基本

        private void Start()
        {
            // PL生成
            spawn(StageManager.Instance._CenterPos);
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            // DEBUG: PLが死んでいたらもう一度生成　
            if(_CurrentPLObj == null)
            {
                spawn(StageManager.Instance._CenterPos);
            }
#endif
        }

        #endregion

        #region 非公開メソッド

        // PL生成
        private void spawn(Vector3 pos)
        {
             _CurrentPLObj = Instantiate(_PLPrefab, pos, Quaternion.identity, _PLInstObj.transform);
        }

        #endregion // 非公開メソッド
    }
}