using UnityEngine;

using ssb.param;

namespace ssb.state
{
    // 通常時
    public class Em1StateNormal : BaseState
    {
        private EnemyBehaviour _Owner;

        private float _ShotGenTimerSec; // 弾生成に使用するタイマー

        public Em1StateNormal(EnemyBehaviour owner)
        {
            _Owner = owner;
        }

        public override void enter()
        {
            // 初期化
            _ShotGenTimerSec = 0.0f;
        }

        public override void update()
        {
            // 弾を生成する
            _ShotGenTimerSec += Time.deltaTime;
            if (_ShotGenTimerSec >= ParamManager.Instance.getParam<Em1Param>()._ShotGenIntervalSec)
            {
                if (GameObject.FindGameObjectWithTag("Player") != null)
                {
                    Vector3 plPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                    Vector3 angle = ((plPos + Unity2DUtil.genRandomVector2(3.0f)) - _Owner.transform.position).normalized;

                    EnemyShotManager.Instance.shotGen
                        (
                            new Vector2(_Owner.transform.position.x, _Owner.transform.position.y),
                            new Vector2(angle.x, angle.y),
                            3.0f
                        );

                    _ShotGenTimerSec = 0.0f;
                }
            }
        }

        public override void exit()
        {

        }
    }

    // ダメージを受けたとき
    public class Em1StateDamage : BaseState
    {
        private EnemyBehaviour _Owner;
        private float _TimerSec;
        private Collider2D _Collider2D;

        private Color _SpriteDefaultColor;
        private SpriteRenderer _SpriteRenderer;

        public Em1StateDamage(EnemyBehaviour owner)
        {
            _Owner = owner;

            _Collider2D     = _Owner.GetComponent<Collider2D>();
            _SpriteRenderer = _Owner.GetComponent<SpriteRenderer>();
        }

        public override void enter()
        {
            _TimerSec               = ParamManager.Instance.getParam<Em1Param>()._MutekiTimeSec;
            _Collider2D.enabled     = false;

            // ダメージ受けている最中は色が変化
            _SpriteDefaultColor     = _SpriteRenderer.color;
            _SpriteRenderer.color   = Color.blue;
        }

        public override void update()
        {
            // 無敵時間終了で吹っ飛びステートへ
            _TimerSec -= Time.deltaTime;
            if(_TimerSec <= 0f)
            {
                _Owner._StateMachine.changeState(new Em1StateBlowOut(_Owner));
            }
        }

        public override void exit()
        {
            _Collider2D.enabled     = true;
            _SpriteRenderer.color   = _SpriteDefaultColor;
        }
    }

    // ダメージを受けて飛ばされているとき
    public class Em1StateBlowOut : BaseState
    {

        private EnemyBehaviour _Owner;
        private float _TimerSec;

        public Em1StateBlowOut(EnemyBehaviour owner)
        {
            _Owner = owner;
        }

        public override void enter()
        {
        }

        public override void update()
        {
            // 速度が一定未満になった場合通常ステートに戻る
            if (_Owner._Speed.magnitude < ParamManager.Instance.getParam<Em1Param>()._NotControllSpeedLimit)
            {
                _Owner.resetSpeed();
                _Owner._StateMachine.changeState(new Em1StateNormal(_Owner));
            }

        }

        public override void exit()
        {
        }
    }

}