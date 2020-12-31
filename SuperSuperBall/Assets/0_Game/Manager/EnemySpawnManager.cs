using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ssb.param;

namespace ssb
{
    // 敵生成管理クラス
    public class EnemySpawnManager : SingletonMonoBehaviour<EnemySpawnManager>
    {
        #region 定数

        // スポーン生成に必要とするマージン
        private static readonly float SPAWN_AREA_MARGIN = 2f;

        // スポーンに要するループ回数の上限
        private static readonly int UPPER_LIMIT_OF_SPAWN_LOOP_CNT = 10;

        // ゲーム中に存在できる敵数の上限
        private static readonly int UPPER_LIMIT_ENEMY_NUM = 600;

        #endregion

        #region フィールド

        // 敵生成先オブジェクト
        [SerializeField]
        private GameObject _InstObj = null;

        // 敵プレハブリスト
        [SerializeField]
        private List<GameObject>_EmPrefabList = null;

        private float _SpawnSecTimer = 0f;  // 敵スポーンタイマー
        private float _SpawnInterval = 3f;  // 敵スポーンまでの時間（だんだん短くなる）

        private int _SpawnCnt = 0;          // 生成された敵の数

        #endregion // フィールド

        #region 基本

        private void Start()
        {
            // 一時停止管理マネージャーに登録
            RistrictManager.Instance.registerBehaviour(this);

            // 初期化
            _SpawnCnt = 0;
        }

        private void Update()
        {
            // 時間経過で敵がスポーン
            _SpawnSecTimer -= Time.deltaTime;
            if(_SpawnSecTimer <= 0f)
            {
                // 敵スポーンはカメラ外かつステージ内のランダムな座標(極座標で計算)
                Vector3 spawnPos = Vector3.zero;
                float len, theta;
                int i;
                for (i = 0; i < UPPER_LIMIT_OF_SPAWN_LOOP_CNT; i++)
                {
                    len         = Random.Range(0f, StageManager.Instance._Radius - SPAWN_AREA_MARGIN);
                    theta       = Random.Range(0f, Mathf.PI * 2f);
                    spawnPos    = new Vector3(len * Mathf.Cos(theta), len * Mathf.Sin(theta), 0f);
                    if (!(CameraManager.Instance.checkInsideScreen(spawnPos)))
                    {
                        break;
                    }
                }

                if (i >= UPPER_LIMIT_OF_SPAWN_LOOP_CNT)
                {
#if UNITY_EDITOR
                    Debug.LogError("EnemySpawnManager::Update | 敵スポーンに要するループ回数の上限に達したので敵はスポーンしません");
#endif
                }
                else
                {
                    emSpawn(0, (_SpawnCnt / 20) + 1, spawnPos);
                }

                _SpawnSecTimer = _SpawnInterval;
                _SpawnInterval = Mathf.Clamp(3f - _SpawnCnt / 100f, 0.5f, 3f);
            }
        }

        #endregion // 基本

        #region 非公開メソッド

        // 敵スポーン処理メソッド
        private void emSpawn(int index, int num, Vector3 spawnCenterPos)
        {
            // 存在できる敵上限を超得ている場合はこれ以上スポーンさせない
            if (_InstObj.GetComponentsInChildren<Transform>().Length > UPPER_LIMIT_ENEMY_NUM)
            {
                return;
            }

            // 最大スポーン数は超えないように調整する
            num = (num > 8) ? 8 : num;

            Vector3 spawnPos;
            float theta = 0f;
            float len   = 1f;
            for(int i = 0; i < num; i++)
            {
                theta       = Mathf.PI / 4f * (float)i;
                spawnPos    = spawnCenterPos + new Vector3(len * Mathf.Cos(theta), len * Mathf.Sin(theta), 0f);
                Instantiate
                    (
                        _EmPrefabList[index],
                        spawnPos,
                        Quaternion.identity,
                        _InstObj.transform
                    );

            }

            _SpawnCnt += num;
        }

        #endregion // 非公開メソッド

    }
}
