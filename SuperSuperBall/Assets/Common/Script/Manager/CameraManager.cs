using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // カメラ管理
    public class CameraManager : SingletonMonoBehaviour<CameraManager>
    {
        #region SerializeField

        // 基本となるゲームカメラ
        [SerializeField]
        private Camera _MainCamera = null;

        #endregion

        #region Field

        private Vector3 _BasePos = Vector3.zero;    // 初期座標

        private GameObject _TargetObj = null;       // 画面に捉え続けるオブジェクト

        private float _CurrentCameraDist = 0f;      // カメラの距離

        #endregion

        #region 基本

        private void Start()
        {
            UnityEngine.Assertions.Assert.IsTrue(_MainCamera != null, "CameraManagerに基本となるカメラが設定されていません");

            _BasePos            = _MainCamera.transform.position;
            _CurrentCameraDist  = _BasePos.z;
        }

        private void LateUpdate()
        {
            // 追従するゲームオブジェクトが存在する場合は常にそのオブジェクトを中心に捉え続ける
            if(_TargetObj != null)
            {
                _MainCamera.transform.position = _TargetObj.transform.position + new Vector3(0f, 0f, _CurrentCameraDist);
            }
        }

        #endregion // 基本

        #region 公開メソッド

        // 追従するオブジェクトの登録
        public void registerTargetObj(GameObject obj)
        {
            _TargetObj = obj;
        }
        // 登録解除
        public void unregisterTargetObj()
        {
            _TargetObj = null;
        }

        // 初期ポジションに座標をリセットする
        public void resetPotToInit()
        {
            transform.position = _BasePos;
        }

        // 渡された座標が画面内に存在するかを確認する
        public bool checkInsideScreen(Vector3 pos)
        {
            // 現在の画面上における左下と右上座標をとる
            Vector3 bottomLeft = _MainCamera.ScreenToWorldPoint(Vector3.zero);
            Vector3 topRight = _MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));

            return (bottomLeft.x < pos.x) && (pos.x < topRight.x) && (bottomLeft.y < pos.y) && (pos.y < topRight.y);
        }

        #endregion // 公開メソッド
    }
}