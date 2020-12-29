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

        // 残り残機数
        public int _CurrentLife { private set; get; } = 0;

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
            // 初手で残機を１減らしてＰＬを生成する
            _CurrentLife = ParamManager.Instance.getParam<PLParam>()._Life + 1;
        }

        private void LateUpdate()
        {
            // PL生存確認。死んでたら残機残っている場合に生成
            if(_CurrentPLObj == null && _CurrentLife > 0)
            {
                _CurrentLife--;

                // 残機が残っていたらＰＬ生成
                Vector3 spawnPos = Vector3.zero;
                //spawnPos.x = CameraManager.Instance.TopRight.x - (CameraManager.Instance._Width / 2f);
                //spawnPos.y = CameraManager.Instance.TopRight.y - (CameraManager.Instance._Height *  (3f / 4f));
                spawn(spawnPos);
            }
        }

        #endregion

        #region 非公開メソッド

        // PL生成
        private void spawn(Vector3 pos)
        {
             _CurrentPLObj = Instantiate(_PLPrefab, pos, Quaternion.identity, _PLInstObj.transform);
            CameraManager.Instance.registerTargetObj(_CurrentPLObj);
        }

        #endregion // 非公開メソッド
    }
}