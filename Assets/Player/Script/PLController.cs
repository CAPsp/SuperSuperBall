using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    // PLの操作
    public class PLController : MonoBehaviour
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
            Free,
        }

        #endregion  // enum

        #region メンバ変数

        private GameObject _FaceGameObj;
        private GameObject _BackGameObj;

        private GameObject _CurrentMoveObj;

        private LineRenderer _LineRenderer;

        private Vector3 _Speed;

        private PLState _State;

        private CircleCollider2D _CricleCollider;
        private EdgeCollider2D _EdgeCollider;

        #endregion // メンバ変数

        #region 基本

        private void Start()
        {
            _LineRenderer = gameObject.GetComponent<LineRenderer>();

            _FaceGameObj = gameObject.transform.Find("Face").gameObject;
            _BackGameObj = gameObject.transform.Find("Back").gameObject;

            _CurrentMoveObj = gameObject;

            _Speed = Vector3.zero;

            _State = PLState.Normal;

            _CricleCollider = GetComponent<CircleCollider2D>();
            _EdgeCollider   = GetComponent<EdgeCollider2D>();
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
                        setState(PLState.Hold);
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
                        setState(PLState.Shoot);
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
                        setState(PLState.Free);
                    }

                    if(_Speed.magnitude < NOT_BIND_SPEED_LIMIT)
                    {
                        _CurrentMoveObj = gameObject;
                         setState(PLState.Normal);
                    }
                    break;

                case PLState.Free:

                    PLShot();

                    if (_Speed.magnitude < BIT_FREE_SPEED_LIMIT)
                    {
                        _Speed += new Vector3(x, y, 0.0f) * 0.02f;
                        if (_Speed.magnitude < NOT_BIND_SPEED_LIMIT)
                        {
                            _Speed = Vector3.zero;
                            setState(PLState.Normal);
                        }
                    }
                    break;
            }

            // 伸びてる体の表現
            _LineRenderer.SetPosition(0, _FaceGameObj.transform.position);
            _LineRenderer.SetPosition(1, gameObject.transform.position);

            // コライダの切り替え
            if(Vector3.Distance(_FaceGameObj.transform.position, gameObject.transform.position) < _EdgeCollider.edgeRadius)
            {
                _CricleCollider.enabled = true;
                _EdgeCollider.enabled   = false;
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
            // 敵の弾に当たる
            if(_State == PLState.Normal || _State == PLState.Hold)
            {
                SoundManager.Instance.playSE(SoundManager.SEName.Hit);
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

        private void setState(PLState state)
        {
            _State = state;
        }

        #endregion // 非公開メソッド
    }

}