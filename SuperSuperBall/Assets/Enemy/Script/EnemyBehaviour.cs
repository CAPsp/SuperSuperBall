using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    // 敵の操作
    public class EnemyBehaviour : CharaBaseBehaviour
    {

        #region 定数

        private static readonly float SHOT_GEN_INTERVAL_SEC = 0.5f;

        private static readonly float SPEED_RESISTANCE = 1.0f;

        private static readonly float NOT_COLLIDE_TIME_SEC = 0.1f;

        #endregion // 定数

        #region プロパティ

        public override ObjType _Type { protected set; get; } = ObjType.Enemy;

        #endregion // プロパティ

        #region フィールド

        private float _ShotGenTimerSec; // 弾生成に使用するタイマー

        private int _Hp;

        private Vector3 _Speed;

        private float _NotCollideTimeSec = 0.0f;

        #endregion // フィールド

        #region 基本

        private void Start()
        {
            // 初期化
            _ShotGenTimerSec = 0.0f;
            _Hp              = 4;
            _Speed           = Vector3.zero;
        }

        // Update is called once per frame
        private void Update()
        {
            if(_NotCollideTimeSec > 0f)
            {
                _NotCollideTimeSec -= Time.deltaTime;
                if(_NotCollideTimeSec <= 0f)
                {
                    GetComponent<BoxCollider2D>().enabled = true;
                }
            }

            Vector3 nextPos = gameObject.transform.position;
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
            gameObject.transform.position = nextPos;

            // 速度の減算
            _Speed.x -= (SPEED_RESISTANCE * Time.deltaTime * _Speed.x);
            _Speed.y -= (SPEED_RESISTANCE * Time.deltaTime * _Speed.y);

            // 弾を生成する
            _ShotGenTimerSec += Time.deltaTime;
            if (_ShotGenTimerSec >= SHOT_GEN_INTERVAL_SEC)
            {
                if (GameObject.FindGameObjectWithTag("Player") != null)
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

        }

        #endregion // 基本

        #region 衝突

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // PLに当たった場合
            if (collision.gameObject.GetComponent<PLBehaviour>() != null)
            {
                // PLから攻撃されたら吹っ飛ばされてHPが減る
               // if (collision.gameObject.GetComponent<PLBehaviour>()._State == PLBehaviour.PLState.Attack)
                {
                    SEManager.Instance.playSE(SEManager.SEName.Hit);
                    _Speed = collision.gameObject.GetComponent<PLBehaviour>().getSpeed();
                    damage(1);
                }

                collision.gameObject.GetComponent<PLBehaviour>().hit(true);
            }
            // 他の敵に当たった場合
            else if(collision.gameObject.GetComponent<EnemyBehaviour>() != null &&
                    (_Speed.sqrMagnitude > collision.gameObject.GetComponent<EnemyBehaviour>()._Speed.sqrMagnitude))
            {
                SEManager.Instance.playSE(SEManager.SEName.Hit);
                damage(1);

                Vector3 currentSpeed = _Speed;
                _Speed = collision.gameObject.GetComponent<EnemyBehaviour>()._Speed;
                collision.gameObject.GetComponent<EnemyBehaviour>()._Speed = currentSpeed;
            }

            _NotCollideTimeSec = NOT_COLLIDE_TIME_SEC;
            GetComponent<BoxCollider2D>().enabled = false;
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