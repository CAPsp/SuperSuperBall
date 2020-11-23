using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    public class EnemyBehaviour : MonoBehaviour
    {

        #region 定数

        private static readonly float SHOT_GEN_INTERVAL_SEC = 1.0f;

        #endregion

        #region フィールド

        private float _ShotGenTimerSec; // 弾生成に使用するタイマー

        #endregion

        #region 基本

        private void Start()
        {
            // 初期化
            _ShotGenTimerSec = 0.0f;
        }

        // Update is called once per frame
        private void Update()
        {
            // 弾を生成する
            _ShotGenTimerSec += Time.deltaTime;
            if (_ShotGenTimerSec >= SHOT_GEN_INTERVAL_SEC)
            {
                Vector3 plPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                Vector3 angle = (plPos - gameObject.transform.position).normalized;

                EnemyShotManager.Instance.shotGen
                    (
                        new Vector2(gameObject.transform.position.x, gameObject.transform.position.y),
                        new Vector2(angle.x, angle.y),
                        3.0f
                    );

                _ShotGenTimerSec = 0.0f;
            }

        }

        #endregion // 基本
    }

}