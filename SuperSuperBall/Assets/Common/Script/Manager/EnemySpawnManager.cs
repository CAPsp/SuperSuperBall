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
            public int _id     = 1;      // 生成される敵ID
            public float _sec  = 0f;     // 生成時間
            public float _x    = 0f;

            public SpawnData(int id, float x, float sec)
            {
                _id     = id;
                _x      = x;
                _sec    = sec;
            }
        }

        #endregion // クラス

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
            _GameTimeSec = 0f;

            // DEBUG
            _SpawnQueue.Enqueue(new SpawnData(1, 0f,    0f));
            _SpawnQueue.Enqueue(new SpawnData(1, 3f,    1f));
            _SpawnQueue.Enqueue(new SpawnData(1, -3f,   3f));
            _SpawnQueue.Enqueue(new SpawnData(1, 0f,    5f));
        }

        private void Update()
        {
            // スポーンする敵がもうないなら処理しない
            if(_SpawnQueue.Count <= 0)
            {
                return;
            }

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
                        new Vector3(data._x, CameraManager.Instance.TopRight.y - 2f, 0f),
                        Quaternion.identity,
                        _InstObj.transform
                    );    
            }
        }

        #endregion // 基本

    }
}
