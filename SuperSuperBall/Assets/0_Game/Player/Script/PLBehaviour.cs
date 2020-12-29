using UnityEngine;

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

        // PL子要素に存在するPL背面描画用GameObject
        public GameObject _BackGameObj { private set; get; }

        #endregion // プロパティ

        #region メンバ変数

        // PLの子要素に存在するPL体描画用GameObject
        private GameObject _BodyGameObj;

        // 状態に合わせて使い分けるコライダ
        private CircleCollider2D _CricleCollider;
        private EdgeCollider2D _EdgeCollider;
        
        #endregion // メンバ変数

        #region 基本

        private void Start()
        {
            // -------------------------------
            // 各種コンポーネント取得
            _BodyGameObj = gameObject.transform.Find("Body").gameObject;
            _BackGameObj = gameObject.transform.Find("Back").gameObject;

            _CricleCollider = GetComponent<CircleCollider2D>();
            _EdgeCollider   = GetComponent<EdgeCollider2D>();

            // 初期化
            _Speed         = Vector3.zero;
            _StateMachine  = new StateMachine(new PLStateNormal(this), this);
            _MutekiTimeSec = ParamManager.Instance.getParam<PLParam>()._MutekiTimeSec;
        }

        // Update is called once per frame
        private void Update()
        {
            // ステート処理
            _StateMachine.update();

            // ----------------------------------------------------------
            // ステートに依存しない処理(常時必要な処理等)は以下に記述

            // 体が伸びていたら。コライダを切り替えて体を描画する
            float distFaceToBody = Vector3.Distance(_BackGameObj.transform.position, gameObject.transform.position);
            if(distFaceToBody < _EdgeCollider.edgeRadius)
            {
                _CricleCollider.enabled = true;
                _EdgeCollider.enabled   = false;
                _BodyGameObj.SetActive(false);
                _BodyGameObj.transform.localPosition = Vector3.zero;
                _BodyGameObj.transform.localRotation = Quaternion.identity;
            }
            else
            {
                _CricleCollider.enabled = false;
                _EdgeCollider.enabled   = true;

                Vector2[] setPoints = new Vector2[]{
                    Vector2.zero,
                    new Vector2(_BackGameObj.transform.localPosition.x, _BackGameObj.transform.localPosition.y)
                };
                _EdgeCollider.points = setPoints;

                _BodyGameObj.SetActive(true);
                _BodyGameObj.transform.localPosition    = _BackGameObj.transform.localPosition / 2.0f;
                _BodyGameObj.transform.localRotation    = Quaternion.AngleAxis(Unity2DUtil.CalcAngle2D(Vector2.zero, _BackGameObj.transform.localPosition), Vector3.forward);
                _BodyGameObj.transform.localScale       = new Vector3(distFaceToBody * 2.0f, 1.0f, 1.0f);
            }

            // 速度の減算
            _Speed -= new Vector3(
                _Speed.x * Time.deltaTime * ParamManager.Instance.getParam<PLParam>()._SpeedResistance,
                _Speed.y * Time.deltaTime * ParamManager.Instance.getParam<PLParam>()._SpeedResistance,
                0f
            );
            if (_Speed.sqrMagnitude < ParamManager.Instance.getParam<PLParam>()._IgnoreSpeedLimit)
            {
                _Speed = Vector3.zero;
            }

            // 無敵時間処理
            if(_MutekiTimeSec > 0.0f)
            {
                _MutekiTimeSec -= Time.deltaTime;
                
                if(_MutekiTimeSec <= 0.0f)  // 無敵消滅
                {
                    foreach (var sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
                    {
                        sprite.enabled = true;
                    }
                }
                else    // 無敵中は体が明滅する
                {
                    bool isEnabled = ((int)(_MutekiTimeSec * 10f) % 2) == 0;
                    foreach (var sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
                    {
                        sprite.enabled = isEnabled;
                    }
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

        // 移動
        public void move(float x, float y)
        {
            gameObject.transform.position += new Vector3(x, y, 0f) * ParamManager.Instance.getParam<PLParam>()._MoveSpeedPerSec * Time.deltaTime;

            // 画面外に出ないよう調整
            Vector3 nextPos;
            Vector3 tmpSpeed;
            if (!(StageManager.Instance.checkCharaInStageAndCalcReturnStage(this, out nextPos, out tmpSpeed)))
            {
                gameObject.transform.position = nextPos;
            }
        }

        // ショット
        public void shot()
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

        // 死亡時の処理
        private void death()
        {
            _BackGameObj.transform.localPosition = Vector3.zero;
            _BodyGameObj.SetActive(false);
            _BackGameObj.SetActive(false);

            // ステート変更
            _StateMachine.changeState(new PLStateDeath(this));
        }

        #endregion // 非公開メソッド

        #region 公開メソッド

        public void hit(bool isEnemy = false)
        {
            // 敵にショットして直接あたった場合
            if(isEnemy && _StateMachine._CurrentState is PLStateShoot)
            {
                // 衝撃波生成
                GameObject explosion = ParticleManager.Instance.initiateParticle(ParticleManager.ParticleName.Explosion, gameObject.transform.position);
                float scale = _Speed.magnitude / ParamManager.Instance.getParam<PLParam>()._ExplosionDiv;
                explosion.transform.localScale = new Vector3(scale, scale, scale);

                // ヒットストップ
                RistrictManager.Instance.stop(0.002f * _Speed.magnitude);

                _Speed = Vector3.zero;
                _MutekiTimeSec = (_MutekiTimeSec <= 0f) ? ParamManager.Instance.getParam<PLParam>()._MutekiTimeSec : _MutekiTimeSec;
            }

            if (!(_StateMachine._CurrentState is PLStateShoot || _StateMachine._CurrentState is PLStateDeath) &&
                 _MutekiTimeSec <= 0.0f)
            {
                death();
            }
        }

        // 固定するローカル座標を設定
        public void setFixedLocalPos(Vector3 localPos)
        {
            _BackGameObj.transform.localPosition = localPos;
        }

        // スピードを加算する
        public void addSpeed(Vector3 speed)
        {
            _Speed += speed;
        }
        
        #endregion
    }

}