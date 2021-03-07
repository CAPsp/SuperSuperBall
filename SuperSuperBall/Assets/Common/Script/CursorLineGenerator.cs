//
// ゲーム中：画面上に描く線を生成する
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    public class CursorLineGenerator : MonoBehaviour
    {
        #region 定数

        // 線を書いているとき、座標を保存する更新頻度
        private static readonly float POS_UPDATE_INTERVAL_SEC = 0.05f;

        // 座標保存数
        private static readonly int SAVE_POS_NUM = 50;

        #endregion // 定数

        #region Property

        // 現在線を描いている状態かどうか
        public bool _IsDrawing { private set; get; }

        #endregion // Property

        #region Field

        // 描かれている線の始点
        private Vector3 _LineStartPoint;

        // 保存している座標
        private ssb.Collections.FixedQueue<Vector3> _SavePosFixedQueue;

        // タイマー
        private float _TimerSec;

        private LineRenderer _LineRenderer;

        #endregion // Field

        #region 基本
        // Start is called before the first frame update
        void Start()
        {
            // 初期化
            _IsDrawing          = false;
            _SavePosFixedQueue  = new ssb.Collections.FixedQueue<Vector3>(SAVE_POS_NUM);
            _TimerSec           = 0f;

            _LineRenderer   = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            // 線を描く前
            if(!_IsDrawing)
            {
                // クリックで線引き開始
                if(InputManager.Instance.mouseMainBtn == InputManager.KeyState.Down)
                {
                    _LineStartPoint = InputManager.Instance.mousePos;
                    _SavePosFixedQueue.Enqueue(_LineStartPoint);

                    _TimerSec   = 0f;
                    _IsDrawing  = true;
                }
            }
            else
            {
                // 座標の保存処理
                _TimerSec += Time.deltaTime;
                if(_TimerSec >= POS_UPDATE_INTERVAL_SEC)
                {
                    _SavePosFixedQueue.Enqueue(InputManager.Instance.mousePos);
                    _TimerSec = 0f;
                }

                // 線の描画
                _LineRenderer.positionCount = _SavePosFixedQueue.Count;
                _LineRenderer.SetPositions(_SavePosFixedQueue.ToArray());

                // 線引き終了
                if (InputManager.Instance.mouseMainBtn != InputManager.KeyState.Hold)
                {
                    // 保存した座標をクリア
                    _SavePosFixedQueue.Clear();

                    _IsDrawing = false;
                }
            }
        }
        #endregion // 基本

        #region 非公開メソッド



        #endregion // 非公開メソッド
    }

}