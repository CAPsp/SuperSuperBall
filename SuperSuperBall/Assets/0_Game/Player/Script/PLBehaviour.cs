using UnityEngine;
using UnityEngine.UI;

using ssb.param;
using ssb.state;

namespace ssb
{
    // PLの操作
    public class PLBehaviour : CharaBaseBehaviour
    {
        #region プロパティ

        public override ObjType _Type { protected set; get; } = ObjType.Player;

        // 無敵中の時間(setは無敵時間が切れたときのみ行える)
        public float _MutekiTimeSec { set; get; }

        // PL子要素に存在するPLアクション補佐するガイド矢印制御Behaviour
        public PLShotGuideArrowBehaviour _ShotGuideArrowBehaviour { private set; get; }

        #endregion // プロパティ

        #region メンバ変数

        private CircleCollider2D _CricleCollider;

        #endregion // メンバ変数

        #region 基本

        protected override void Start()
        {
            base.Start();

            // 矢印ガイド制御Behaviourの取得と設定
            _ShotGuideArrowBehaviour    = gameObject.transform.GetComponentInChildren<PLShotGuideArrowBehaviour>();
            if(_ShotGuideArrowBehaviour != null)
            {
                _ShotGuideArrowBehaviour.registerPLBehaviour(this);
            }
            _CricleCollider = GetComponent<CircleCollider2D>();

            // 初期化
            float randTheta = Random.value * Mathf.PI * 2f;
            _Speed          = new Vector3(Mathf.Sign(randTheta), Mathf.Cos(randTheta), 0f) * ParamManager.Instance.getParam<PLParam>()._IgnoreSpeedLimit;
            _StateMachine       = new StateMachine(new PLStateNormal(this), this);
            _MutekiTimeSec      = ParamManager.Instance.getParam<PLParam>()._MutekiTimeSec;
            _CollAttackPower    = 2;
        }

        // Update is called once per frame
        private void Update()
        {
            // ステート処理
            _StateMachine.update();

            // ----------------------------------------------------------
            // ステートに依存しない処理(常時必要な処理等)は以下に記述

            // 一定速度以上であれば速度の減算
            if(_Speed.sqrMagnitude > ParamManager.Instance.getParam<PLParam>()._IgnoreSpeedLimit)
            {
                _Speed -= new Vector3(
                    _Speed.x * Time.deltaTime * ParamManager.Instance.getParam<PLParam>()._SpeedResistance,
                    _Speed.y * Time.deltaTime * ParamManager.Instance.getParam<PLParam>()._SpeedResistance,
                    0f
                );

                if(_Speed.sqrMagnitude <= ParamManager.Instance.getParam<PLParam>()._IgnoreSpeedLimit)
                {
                    _Speed = _Speed.normalized * ParamManager.Instance.getParam<PLParam>()._IgnoreSpeedLimit;
                }
            }

            // 無敵時間処理
            if(_MutekiTimeSec > 0.0f)
            {
                _MutekiTimeSec -= Time.deltaTime;
                
                if(_MutekiTimeSec <= 0.0f)  // 無敵消滅
                {
                    gameObject.GetComponent<SpriteRenderer>().enabled   = true;
                }
                else    // 無敵中は体が明滅する
                {
                    bool isEnabled = ((int)(_MutekiTimeSec * 10f) % 2) == 0;
                    gameObject.GetComponent<SpriteRenderer>().enabled   = isEnabled;
                }
            }
        }

        // GUIとして描画する部分(Debug)
        //private void OnGUI()
        //{
        //    GUI.Label(new Rect(0, 0, 200, 50), _Speed.ToString(), GUI.skin.box);
        //    GUI.Label(new Rect(0, 50, 200, 50), _StateMachine._CurrentState.ToString(), GUI.skin.box);
        //}

        #endregion // 基本

        #region 非公開メソッド

        /// <summary>
        /// 移動処理
        /// </summary>
        public void move()
        {
            gameObject.transform.position += _Speed * Time.deltaTime;

            // 画面外に出た場合は跳ね返る
            Vector3 nextPos;
            Vector3 nextSpeed;
            if (!(StageManager.Instance.checkCharaInStageAndCalcReturnStage(this, out nextPos, out nextSpeed)))
            {
                gameObject.transform.position = nextPos;
                _Speed = nextSpeed;
            }
        }

        #endregion // 非公開メソッド

        #region 公開メソッド

        public void hit(bool isEnemy = false)
        {
            // 死亡時の処理
            if (isEnemy && _MutekiTimeSec <= 0.0f)
            {
                // ステート変更
                _StateMachine.changeState(new PLStateDeath(this));
            }
        }

        // スピードを加算する
        public void addSpeed(Vector3 speed)
        {
            _Speed += speed;
        }

        #endregion
    }

}