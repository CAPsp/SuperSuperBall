﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    // PLの操作
    public class PLBehaviour : MonoBehaviour
    {
        #region 定数

        private static readonly float SPEED_RESISTANCE = 1.0f;

        private static readonly float MOVE_SPEED = 4.0f;

        // 若干操作がきくようになる速度
        private static readonly float BIT_FREE_SPEED_LIMIT = 50.0f;

        // 自由がきくようになる速度
        private static readonly float NOT_BIND_SPEED_LIMIT = 10.0f;

        #endregion // 定数

        #region enum

        public enum PLState
        {
            Normal,
            Hold,
            Shoot,
            Attack,
        }

        #endregion  // enum

        #region プロパティ

        public PLState _State { private set; get; }

        #endregion

        #region メンバ変数

        // PLに紐づく描画用GameObject
        private GameObject _FaceGameObj;
        private GameObject _BodyGameObj;
        private GameObject _BackGameObj;

        private GameObject _CurrentMoveObj;

        private Vector3 _Speed;

        private CircleCollider2D _CricleCollider;
        private EdgeCollider2D _EdgeCollider;

        #endregion // メンバ変数

        #region 基本

        private void Start()
        {
            // -------------------------------
            // 各種コンポーネント取得
            _FaceGameObj = gameObject.transform.Find("Face").gameObject;
            _BodyGameObj = gameObject.transform.Find("Body").gameObject;
            _BackGameObj = gameObject.transform.Find("Back").gameObject;

            _CricleCollider = GetComponent<CircleCollider2D>();
            _EdgeCollider   = GetComponent<EdgeCollider2D>();

            // 初期化
            _CurrentMoveObj = gameObject;
            _Speed = Vector3.zero;
            _State = PLState.Normal;
        }

        // Update is called once per frame
        private void Update()
        {
            float x = InputManager.Instance.axisX;
            float y = InputManager.Instance.axisY;

            switch (_State)
            {
                case PLState.Normal:

                    PLMove(x, y);

                    if (InputManager.Instance.Hold == InputManager.KeyState.Down)
                    {
                        _CurrentMoveObj = _FaceGameObj;
                        _State = PLState.Hold;
                    }
                    break;

                case PLState.Hold:

                    float dist = Vector3.Distance(gameObject.transform.position, _FaceGameObj.transform.position);
                    dist += 1.0f;
                    dist *= dist;

                    PLMove(x / dist, y / dist);

                    if (InputManager.Instance.Hold == InputManager.KeyState.Up)
                    {
                        _Speed = (gameObject.transform.position - _FaceGameObj.transform.position) * MOVE_SPEED * MOVE_SPEED;
                        _State = PLState.Shoot;
                    }
                    else if(InputManager.Instance.isDecide)
                    {
                        _FaceGameObj.transform.position = gameObject.transform.position;
                        _CurrentMoveObj = gameObject;
                        _State = PLState.Normal;
                    }
                    break;

                case PLState.Shoot:

                    PLShot();

                    // 基点を追い越したら基点がくっついてくるようになる
                    Vector3 diff = gameObject.transform.position - _FaceGameObj.transform.position;
                    if (Mathf.Sign(_Speed.x) != Mathf.Sign(diff.x) || Mathf.Sign(_Speed.y) != Mathf.Sign(diff.y))
                    {
                        _FaceGameObj.transform.position = gameObject.transform.position;
                        _CurrentMoveObj = gameObject;
                        _State = PLState.Attack;
                    }

                    if(_Speed.magnitude < NOT_BIND_SPEED_LIMIT)
                    {
                        _FaceGameObj.transform.position = gameObject.transform.position;
                        _CurrentMoveObj = gameObject;
                        _State = PLState.Normal;
                    }
                    break;

                case PLState.Attack:

                    PLShot();

                    if (_Speed.magnitude < BIT_FREE_SPEED_LIMIT)
                    {
                        _Speed += new Vector3(x, y, 0.0f) * 0.02f;
                        if (_Speed.magnitude < NOT_BIND_SPEED_LIMIT)
                        {
                            _Speed = Vector3.zero;
                            _State = PLState.Normal;
                        }
                    }
                    break;
            }

            float distFaceToBody = Vector3.Distance(gameObject.transform.position, _FaceGameObj.transform.position);

            // 体が伸びていたら。コライダを切り替えて体を描画する
            if(distFaceToBody < _EdgeCollider.edgeRadius)
            {
                _CricleCollider.enabled = true;
                _EdgeCollider.enabled   = false;
                _BodyGameObj.SetActive(false);
                _BodyGameObj.transform.localPosition = Vector3.zero;
                _BodyGameObj.transform.localRotation = Quaternion.identity;
            }
            else
            {
                _CricleCollider.enabled = false;
                _EdgeCollider.enabled   = true;

                Vector2[] setPoints = new Vector2[]{
                    Vector2.zero,
                    new Vector2(_FaceGameObj.transform.localPosition.x, _FaceGameObj.transform.localPosition.y)
                };
                _EdgeCollider.points = setPoints;

                _BodyGameObj.SetActive(true);
                _BodyGameObj.transform.localPosition    = _FaceGameObj.transform.localPosition / 2.0f;
                _BodyGameObj.transform.localRotation    = Quaternion.AngleAxis(Unity2DUtil.CalcAngle2D(Vector2.zero, _FaceGameObj.transform.localPosition), Vector3.forward);
                _BodyGameObj.transform.localScale       = new Vector3(distFaceToBody * 2.0f, 1.0f, 1.0f);
            }

            // 速度の減算
            _Speed.x -= (SPEED_RESISTANCE * Time.deltaTime * _Speed.x);
            _Speed.y -= (SPEED_RESISTANCE * Time.deltaTime * _Speed.y);
            if (_Speed.sqrMagnitude < 0.1f)
            {
                _Speed = Vector3.zero;
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 100, 50), _Speed.ToString());
            GUI.Label(new Rect(0, 50, 100, 50), _State.ToString());
        }

        #endregion // 基本

        #region 衝突

        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool isDeath = false;

            // 敵に当たる
            if(collision.gameObject.GetComponent<EnemyBehaviour>() != null)
            {
                if (_State == PLState.Attack)
                {
                    _Speed = Vector3.zero;
                }
                else
                {
                    isDeath = true;
                }
            }
            // 敵の弾に当たる
            else if (collision.gameObject.GetComponent<EnemyShot>() != null)
            {
                if (_State != PLState.Attack)
                {
                    isDeath = true;
                }
            }

            // 死亡w
            if(isDeath)
            {
                SEManager.Instance.playSE(SEManager.SEName.Hit);
                Debug.Log("Death");
            }
        }

        #endregion // 衝突

        #region 非公開メソッド

        // 移動
        private void PLMove(float x, float y)
        {
            Vector3 nextPos = _CurrentMoveObj.transform.position;
            nextPos += new Vector3(x, y, 0f) * 0.05f;


            // 画面外に出ないよう調整
            if (!(CameraManager.Instance.checkInsideScreen(nextPos)))
            {
                Vector3 topRight    = CameraManager.Instance.TopRight;
                Vector3 bottomLeft  = CameraManager.Instance.BottomLeft;

                if (nextPos.x < bottomLeft.x)
                {
                    nextPos.x = bottomLeft.x;
                }
                else if (topRight.x < nextPos.x)
                {
                    nextPos.x = topRight.x;
                }

                if (nextPos.y < bottomLeft.y)
                {
                    nextPos.y = bottomLeft.y;
                }
                else if (topRight.y < nextPos.y)
                {
                    nextPos.y = topRight.y;
                }
            }

            _CurrentMoveObj.transform.position = nextPos;
        }

        // ショット
        private void PLShot()
        {
            Vector3 nextPos = _CurrentMoveObj.transform.position;
            nextPos += _Speed * Time.deltaTime;

            // 画面外に出た場合は跳ね返る
            if (!(CameraManager.Instance.checkInsideScreen(nextPos)))
            {
                Vector3 topRight = CameraManager.Instance.TopRight;
                Vector3 bottomLeft = CameraManager.Instance.BottomLeft;

                if (nextPos.x < bottomLeft.x)
                {
                    nextPos.x = bottomLeft.x + (bottomLeft.x - nextPos.x);
                    _Speed.x *= (-1.0f);
                }
                else if (topRight.x < nextPos.x)
                {
                    nextPos.x = topRight.x - (nextPos.x - topRight.x);
                    _Speed.x *= (-1.0f);
                }

                if (nextPos.y < bottomLeft.y)
                {
                    nextPos.y = bottomLeft.y + (bottomLeft.y - nextPos.y);
                    _Speed.y *= (-1.0f);
                }
                else if (topRight.y < nextPos.y)
                {
                    nextPos.y = topRight.y - (nextPos.y - topRight.y);
                    _Speed.y *= (-1.0f);
                }
            }

            _CurrentMoveObj.transform.position = nextPos;
        }

        #endregion // 非公開メソッド
    }

}