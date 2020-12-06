using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ssb.param;

namespace ssb
{
    // 敵生成管理クラス
    public class EnemySpawnManager : SingletonMonoBehaviour<EnemySpawnManager>
    {

        #region クラス
        private class SpawnData
        {
            public int _id          = 1;            // 生成される敵ID
            public float _sec       = 0f;           // 生成時間
            public Vector2 _pos     = Vector2.zero; // 生成場所　

            public SpawnData(int id, Vector2 pos, float sec)
            {
                _id     = id;
                _pos    = pos;
                _sec    = sec;
            }
        }

        #endregion // クラス

        #region プロパティ

        public int _SpawnCnt { set; get; }

        #endregion // プロパティ

        #region フィールド

        // ゲーム開始からの時間を計測
        private float _GameTimeSec = 0f;

        private Queue<SpawnData> _SpawnQueue = new Queue<SpawnData>();

        [SerializeField]
        private GameObject _InstObj = null;

        [SerializeField]
        private GameObject _Em1Prefab = null;

        #endregion // フィールド

        #region 基本

        private void Start()
        {
            _GameTimeSec    = 0f;
            _SpawnCnt       = 0;

            // -----------------------------
            // DEBUG

            // Wave1
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(0f,  1f), 1f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(1f,  1f), 1f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(-1f, 1f), 1f));

            // Wave2
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(-1f, 1.5f), 4f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(-2f, 1.5f), 4f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(-3f, 1.5f), 4f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(-4f, 1.5f), 4f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(1f,  1.5f), 6f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(2f,  1.5f), 6f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(3f,  1.5f), 6f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(4f,  1.5f), 6f));

            // Wave3
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(0f, 1.5f), 12f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(1f, 1.5f), 12f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(2f, 1.5f), 12f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(-1f, 1.5f), 12f));
            _SpawnQueue.Enqueue(new SpawnData(1, new Vector2(-2f, 1.5f), 12f));
        }

        private void Update()
        {
            // スポーンする敵がもういない場合
            if(_SpawnQueue.Count <= 0)
            {
                // 敵が全滅している場合ゲームクリア
                if(_SpawnCnt <= 0)
                {
                    GameManager.Instance.gameEnd();
                }
                return;
            }

            // 時間経過で敵がスポーン
            _GameTimeSec += Time.deltaTime;
            if(_SpawnQueue.Peek()._sec <= _GameTimeSec)
            {
                SpawnData data = _SpawnQueue.Dequeue();

                GameObject genPrefab = null;
                switch(data._id)
                {
                    case 1: genPrefab = _Em1Prefab; break;

                    default: genPrefab = _Em1Prefab; break;
                }

                Instantiate
                    (
                        genPrefab,
                        new Vector3(data._pos.x, CameraManager.Instance.TopRight.y + data._pos.y, 0f),
                        Quaternion.identity,
                        _InstObj.transform
                    );

                _SpawnCnt++;
            }
        }

        #endregion // 基本
    }
}
