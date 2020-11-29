using UnityEngine;

using ssb.state;
using ssb.param;

namespace ssb
{
    // 敵１
    public class Em1Behaviour : EnemyBehaviour
    {
        #region プロパティ

        public override ObjType _Type { protected set; get; } = ObjType.Enemy;

        #endregion // プロパティ

        #region 基本

        private void Start()
        {
            _Hp             = ParamManager.Instance.getParam<Em1Param>()._Hp;
            _Speed          = Vector3.zero;
            _StateMachine   = new StateMachine(new Em1StateNormal(this), this);
        }

        // Update is called once per frame
        private void Update()
        {
            _StateMachine.update();

            moveUpdate();
        }

        #endregion // 基本

        #region 衝突

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // PLに当たった場合
            if (collision.gameObject.GetComponent<PLBehaviour>() != null)
            {
                // PLから攻撃されたら吹っ飛ばされてHPが減る
                if (collision.gameObject.GetComponent<PLBehaviour>()._StateMachine._CurrentState is PLStateShoot)
                {
                    SEManager.Instance.playSE(SEManager.SEName.Hit);
                    _Speed = collision.gameObject.GetComponent<PLBehaviour>()._Speed;
                    damage(1);
                }

                collision.gameObject.GetComponent<PLBehaviour>().hit(true);
            }
            // 他の敵に当たった場合
            else if (collision.gameObject.GetComponent<EnemyBehaviour>() != null &&
                    (_Speed.sqrMagnitude > collision.gameObject.GetComponent<EnemyBehaviour>()._Speed.sqrMagnitude))
            {
                SEManager.Instance.playSE(SEManager.SEName.Hit);
                damage(1);

                Vector3 currentSpeed = _Speed;
                _Speed = collision.gameObject.GetComponent<EnemyBehaviour>()._Speed;
                collision.gameObject.GetComponent<Em1Behaviour>()._Speed = currentSpeed;
            }
        }

        #endregion // 衝突

        #region 非公開メソッド

        // 移動関係の更新
        private void moveUpdate()
        {
            Vector3 nextPos = gameObject.transform.position;
            Vector3 nextSpeed = _Speed;

            nextPos += _Speed * Time.deltaTime;

            // 画面外に出た場合は跳ね返る
            if (!(CameraManager.Instance.checkInsideScreen(nextPos)))
            {
                Vector3 topRight = CameraManager.Instance.TopRight;
                Vector3 bottomLeft = CameraManager.Instance.BottomLeft;

                if (nextPos.x < bottomLeft.x)
                {
                    nextPos.x = bottomLeft.x + (bottomLeft.x - nextPos.x);
                    nextSpeed.x *= (-1.0f);
                }
                else if (topRight.x < nextPos.x)
                {
                    nextPos.x = topRight.x - (nextPos.x - topRight.x);
                    nextSpeed.x *= (-1.0f);
                }

                if (nextPos.y < bottomLeft.y)
                {
                    nextPos.y = bottomLeft.y + (bottomLeft.y - nextPos.y);
                    nextSpeed.y *= (-1.0f);
                }
                else if (topRight.y < nextPos.y)
                {
                    nextPos.y = topRight.y - (nextPos.y - topRight.y);
                    nextSpeed.y *= (-1.0f);
                }
            }

            // 速度の減算
            nextSpeed.x -= (ParamManager.Instance.getParam<Em1Param>()._SpeedResistance * Time.deltaTime * _Speed.x);
            nextSpeed.y -= (ParamManager.Instance.getParam<Em1Param>()._SpeedResistance * Time.deltaTime * _Speed.y);

            gameObject.transform.position = nextPos;
            _Speed = nextSpeed;
        }

        // ダメージを受けた際の処理
        private void damage(int damage)
        {
            _Hp -= damage;
            if (_Hp <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _StateMachine.changeState(new Em1StateDamage(this));
            }
        }

        #endregion

    }

}