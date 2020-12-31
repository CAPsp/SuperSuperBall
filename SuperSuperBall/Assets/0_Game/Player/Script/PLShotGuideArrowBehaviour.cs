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
            Hide,       // 非表示
            Shot,       // ＰＬショット時
            Reflect,    // ＰＬ反射時
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

                case ArrowState.Shot:
                    Vector3 baseLocalPos = _plBehaviour._BackGameObj.transform.localPosition;
                    if(baseLocalPos.magnitude < Mathf.Epsilon)
                    {
                        _Sprite.enabled = false;
                    }
                    else
                    {
                        gameObject.transform.localPosition = baseLocalPos.normalized * DIST_FROM_PL;
                        gameObject.transform.localRotation = Quaternion.FromToRotation(Vector3.up, baseLocalPos.normalized);
                        _Sprite.enabled = true;
                    }
                    break;

                case ArrowState.Reflect:
                    Vector3 inputVec = new Vector3(InputManager.Instance.axisX, InputManager.Instance.axisY, 0f);
                    if(inputVec.magnitude > Mathf.Epsilon && Vector3.Dot(inputVec, _plBehaviour._Speed.normalized) <= 0)
                    {
                        _Sprite.enabled = true;
                        gameObject.transform.localPosition = inputVec.normalized * DIST_FROM_PL;
                        gameObject.transform.localRotation = Quaternion.FromToRotation(Vector3.up, inputVec.normalized);
                    }
                    else
                    {
                        _Sprite.enabled = false;

                    }

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