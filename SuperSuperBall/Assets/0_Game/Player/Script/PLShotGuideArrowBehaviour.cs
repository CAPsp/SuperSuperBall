using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // PLアクション補佐ガイド矢印を制御する
    public class PLShotGuideArrowBehaviour : MonoBehaviour
    {
        #region 定数

        // ガイドのＰＬからの距離
        private static readonly float DIST_FROM_PL = 0.8f;

        #endregion

        #region enum

        // 矢印の状態
        public enum ArrowState
        {
            Hide,           // 非表示
            Visible,        // 表示
        }

        #endregion // enum

        #region メンバ変数

        private ArrowState _State = ArrowState.Hide;

        private SpriteRenderer _Sprite = null;

        private PLBehaviour _plBehaviour = null;

        #endregion // メンバ変数

        #region 基本

        private void Awake()
        {
            _Sprite = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            switch(_State)
            {
                case ArrowState.Hide:
                    _Sprite.enabled = false;
                    break;

                case ArrowState.Visible:
                    gameObject.transform.localPosition = _plBehaviour._Speed.normalized * DIST_FROM_PL;
                    gameObject.transform.localRotation = Quaternion.FromToRotation(Vector3.up, _plBehaviour._Speed.normalized);
                    _Sprite.enabled = true;
                    break;
            }

        }
        #endregion // 基本

        #region 公開メソッド

        public void setState(ArrowState state)
        {
            _State = state;
        }

        public void registerPLBehaviour(PLBehaviour pLBehaviour)
        {
            _plBehaviour = pLBehaviour;
        }

        #endregion // 公開メソッド
    }
}