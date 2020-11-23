using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    public class EnemyBehaviour : MonoBehaviour
    {

        #region 定数

        private static readonly float SHOT_GEN_INTERVAL_SEC = 0.5f;

        #endregion

        #region フィールド

        private float _ShotGenTimerSec; // 弾生成に使用するタイマー

        private int _Hp;

        #endregion

        #region 基本

        private void Start()
        {
            // 初期化
            _ShotGenTimerSec = 0.0f;
            _Hp = 2;
        }

        // Update is called once per frame
        private void Update()
        {
            // 弾を生成する
            _ShotGenTimerSec += Time.deltaTime;
            if (_ShotGenTimerSec >= SHOT_GEN_INTERVAL_SEC)
            {
                Vector3 plPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                Vector3 angle = ((plPos + Unity2DUtil.genRandomVector2(3.0f)) - gameObject.transform.position).normalized;

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

        #region 衝突

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // PLに当たった場合
            if(collision.gameObject.GetComponent<PLBehaviour>() != null)
            {
                // PLから攻撃されたらHPが減る
                if (collision.gameObject.GetComponent<PLBehaviour>()._State == PLBehaviour.PLState.Attack)
                {
                    SEManager.Instance.playSE(SEManager.SEName.Hit);
                    damage(1);
                }
            }
        }

        #endregion // 衝突

        #region 非公開メソッド

        private void damage(int damage)
        {
            _Hp -= damage;
            if(_Hp <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        #endregion
    }

}