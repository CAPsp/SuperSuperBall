using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // プレイヤー管理
    public class PLManager : SingletonMonoBehaviour<PLManager>
    {

        #region プロパティ

        // 生存中のPLオブジェクト
        public GameObject _CurrentPLObj { private set; get; } = null;

        // 残り残機数
        public int _CurrentLife { private set; get; } = 3;

        #endregion // プロパティ

        #region フィールド

        [SerializeField]
        private GameObject _PLPrefab = null;

        [SerializeField]
        private GameObject _PLInstObj = null;

        #endregion  // フィールド

        #region 基本

        private void LateUpdate()
        {
            // PL生存確認
            if(_CurrentPLObj == null)
            {
                _CurrentLife--;

                // 残機0でGameOverへ遷移
                if (_CurrentLife <= 0)
                {
                    // ゲームオーバー処理
                }
                else
                {
                    Vector3 spawnPos = Vector3.zero;
                    spawnPos.x = CameraManager.Instance.TopRight.x - (CameraManager.Instance._Width / 2f);
                    spawnPos.y = CameraManager.Instance.TopRight.y - (CameraManager.Instance._Height *  (3f / 4f));
                    spawn(spawnPos);
                }
            }
        }

        #endregion

        #region 公開メソッド

        // PL生成
        void spawn(Vector3 pos)
        {
             _CurrentPLObj = Instantiate(_PLPrefab, pos, Quaternion.identity, _PLInstObj.transform);
        }

        #endregion  // 公開メソッド
    }
}