using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // カメラ管理
    public class CameraManager : SingletonMonoBehaviour<CameraManager>
    {
        #region 公開プロパティ

        // スクリーン座標
        public Vector3 TopRight     { private set; get; }
        public Vector3 BottomLeft   { private set; get; }

        #endregion

        #region フィールド

        private Camera _MainCam;


        #endregion

        #region 基本

        private void Start()
        {
            _MainCam = GameObject.Find("Main Camera").GetComponent<Camera>();

            BottomLeft = _MainCam.ScreenToWorldPoint(Vector3.zero);
            BottomLeft.Scale(new Vector3(1f, -1f, 1f));

            TopRight = _MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
            TopRight.Scale(new Vector3(1f, -1f, 1f));
        }

        #endregion // 基本

        #region 公開メソッド

        // 渡された座標が画面内に存在するかを確認する
        public bool checkInsideScreen(Vector3 pos)
        {
            return (BottomLeft.x < pos.x) && (pos.x < TopRight.x) && (BottomLeft.y < pos.y) && (pos.y < TopRight.y);
        }

        #endregion // 公開メソッド
    }
}