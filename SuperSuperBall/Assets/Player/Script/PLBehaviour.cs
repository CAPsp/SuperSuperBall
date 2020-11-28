using UnityEngine;

using ssb.param;
using ssb.state;

namespace ssb
{

    // PLの操作
    public class PLBehaviour : CharaBaseBehaviour
    {
        /*
        #region 定数

        private static readonly float SPEED_RESISTANCE = 1.0f;

        private static readonly float MOVE_SPEED = 4.0f;

        // 若干操作がきくようになる速度
        private static readonly float BIT_FREE_SPEED_LIMIT = 50.0f;

        // 自由がきくようになる速度
        private static readonly float NOT_BIND_SPEED_LIMIT = 10.0f;

        private static readonly float MUTEKI_TIME_SEC = 2.0f;

        #endregion // 定数
        */

        #region プロパティ

        public override ObjType _Type { protected set; get; } = ObjType.Player;

        // 無敵中の時間(setは無敵時間が切れたときのみ行える)
        public float _MutekiTimeSec { set; get; }

        #endregion // プロパティ

        #region メンバ変数

        // PLの子要素に存在する描画用GameObject
        private GameObject _BodyGameObj;
        private GameObject _BackGameObj;

        private Animator _Animator;

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

            _Animator = gameObject.GetComponent<Animator>();

            _CricleCollider = GetComponent<CircleCollider2D>();
            _EdgeCollider   = GetComponent<EdgeCollider2D>();

            // 初期化
            _Speed         = Vector3.zero;
            _StateMachine  = new StateMachine(new PLStateNormal(this), this);
            _MutekiTimeSec = 0f;
        }

        // Update is called once per frame
        private void Update()
        {
            float x = InputManager.Instance.axisX;
            float y = InputManager.Instance.axisY;

            _StateMachine.update();

            float distFaceToBody = Vector3.Distance(_BackGameObj.transform.position, gameObject.transform.position);
            // 体が伸びていたら。コライダを切り替えて体を描画する
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
            if (_Speed.sqrMagnitude < 0.1f)
            {
                _Speed = Vector3.zero;
            }

            if(_MutekiTimeSec > 0.0f)
            {
                _MutekiTimeSec -= Time.deltaTime;
                
                if(_MutekiTimeSec <= 0.0f)  // 無敵中消滅
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

        private void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 100, 50), _Speed.ToString(), GUI.skin.box);
            GUI.Label(new Rect(0, 50, 100, 50), _StateMachine._CurrentState.ToString(), GUI.skin.box);
        }

        #endregion // 基本

        #region 非公開メソッド

        // 移動
        public void move(float x, float y)
        {
            Vector3 nextPos = gameObject.transform.position;
            nextPos += new Vector3(x, y, 0f) * 0.05f;


            // 画面外に出ないよう調整
            if (!(CameraManager.Instance.checkInsideScreen(nextPos)))
            {
                Vector3 topRight    = CameraManager.Instance.TopRight;
                Vector3 bottomLeft  = CameraManager.Instance.BottomLeft;

                if (nextPos.x < bottomLeft.x)
                {
                    nextPos.x = bottomLeft.x;
                }
                else if (topRight.x < nextPos.x)
                {
                    nextPos.x = topRight.x;
                }

                if (nextPos.y < bottomLeft.y)
                {
                    nextPos.y = bottomLeft.y;
                }
                else if (topRight.y < nextPos.y)
                {
                    nextPos.y = topRight.y;
                }
            }

            gameObject.transform.position = nextPos;
        }

        // ショット
        public void shot()
        {
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
              //      _Speed.x *= (-1.0f);
                }
                else if (topRight.x < nextPos.x)
                {
                    nextPos.x = topRight.x - (nextPos.x - topRight.x);
                //    _Speed.x *= (-1.0f);
                }

                if (nextPos.y < bottomLeft.y)
                {
                    nextPos.y = bottomLeft.y + (bottomLeft.y - nextPos.y);
                  //  _Speed.y *= (-1.0f);
                }
                else if (topRight.y < nextPos.y)
                {
                    nextPos.y = topRight.y - (nextPos.y - topRight.y);
                    //_Speed.y *= (-1.0f);
                }
            }

            gameObject.transform.position = nextPos;
        }

        // 死亡時の処理
        private void death()
        {
            SEManager.Instance.playSE(SEManager.SEName.PLDeath);
          //  _State = PLState.Death;
            _BackGameObj.transform.localPosition = Vector3.zero;
            _BodyGameObj.SetActive(false);
            _BackGameObj.SetActive(false);
            _Animator.SetBool("isDeath", true);
        }

        #endregion // 非公開メソッド

        #region 公開メソッド

        public void hit(bool isEnemy = false)
        {
            /*
            // 敵に直接あたった場合
            if(isEnemy && _State == PLState.Attack)
            {
                _Speed = Vector3.zero;
                _MutekiTimeSec = (_MutekiTimeSec <= 0f) ? MUTEKI_TIME_SEC : _MutekiTimeSec;
            }

            if ((_State != PLState.Attack && _State != PLState.Death) &&
                 _MutekiTimeSec <= 0.0f)
            {
                death();
            }
            */
        }

        // 固定するローカル座標を設定
        public void setFixedLocalPos(Vector3 localPos)
        {
            _BackGameObj.transform.localPosition = localPos;
        }

        // スピードを加算する
        public void addSpeed(Vector3 speed)
        {
            _Speed = speed;
        }
        
        #endregion
    }

}