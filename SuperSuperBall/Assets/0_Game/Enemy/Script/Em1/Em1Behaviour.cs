using System.Collections.Generic;
using System;
using UnityEngine;

using ssb.state;
using ssb.param;

namespace ssb
{
    // 敵１
    public class Em1Behaviour : EnemyBehaviour
    {
        #region 内部クラス

        private class NotReHit
        {
            public int _Hash;
            public float _RemainTime;
            public NotReHit(int hash, float time) { _Hash = hash; _RemainTime = time; }
        }

        #endregion

        #region フィールド

        // ここに格納された同じオブジェクトはValue値が0になるまで衝突しない
        private List<NotReHit> _NotReHitObjList = new List<NotReHit>();

        #endregion

        #region 基本

        private void Start()
        {
            // 初期化
            _Hp                 = ParamManager.Instance.getParam<Em1Param>()._Hp;
            _Speed              = Vector3.zero;
            _StateMachine       = new StateMachine(new Em1StateNormal(this), this);
            _CollAttackPower    = 1;
        }

        // Update is called once per frame
        private void Update()
        {
            _StateMachine.update();

            moveUpdate();
            
            // 短時間衝突回避リストからの除外処理
            for(int i = _NotReHitObjList.Count - 1; 0 <= i; i--)
            {
                _NotReHitObjList[i]._RemainTime -= Time.deltaTime;
                if(_NotReHitObjList[i]._RemainTime <= 0f)
                {
                    _NotReHitObjList.RemoveAt(i);
                }
            }
        }

        // GUIとして描画する部分(Debug)
        //private void OnGUI()
        //{
        //    Vector2 pos = new Vector2(
        //        transform.position.x - CameraManager.Instance.BottomLeft.x,
        //        (-1) * transform.position.y - CameraManager.Instance.BottomLeft.y
        //    );

        //    GUI.Label(new Rect(
        //        pos.x * 40,
        //        pos.y * 40,
        //        30,
        //        30), _Hp.ToString(), GUI.skin.box);

        //    GUI.Label(new Rect(
        //        pos.x * 40,
        //        pos.y * 40 + 30,
        //        150,
        //        30), _StateMachine._CurrentState.ToString(), GUI.skin.box);
        //}

        #endregion // 基本

        #region 衝突

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // PLに当たった場合
            if (collision.gameObject.GetComponent<PLBehaviour>() != null)
            {
                PLBehaviour pl = collision.gameObject.GetComponent<PLBehaviour>();

                // PLから攻撃されたら吹っ飛ばされてHPが減る
                if (pl._StateMachine._CurrentState is PLStateShoot)
                {
                    SEManager.Instance.playSE(SEManager.SEName.Hit);
                    _Speed = collision.gameObject.GetComponent<PLBehaviour>()._Speed;
                    damage(pl._CollAttackPower);

                    // パーティクル
                    ParticleManager.Instance.initiateParticle(ParticleManager.ParticleName.Hit, collision.contacts[0].point);
                }

                collision.gameObject.GetComponent<PLBehaviour>().hit(true);
            }
            // 他の敵に当たった場合
            else if (   _StateMachine._CurrentState is Em1StateBlowOut &&
                        collision.gameObject.GetComponent<EnemyBehaviour>() != null)
            {
                Em1Behaviour otherEm = collision.gameObject.GetComponent<Em1Behaviour>();

                SEManager.Instance.playSE(SEManager.SEName.Hit);
                damage(otherEm._CollAttackPower);
                ParticleManager.Instance.initiateParticle(ParticleManager.ParticleName.Hit, collision.contacts[0].point);

                Vector3 currentSpeed = _Speed;
                _Speed = otherEm._Speed;

                // 当たった敵にも影響を与える
                otherEm._Speed = currentSpeed;
                otherEm.damage(_CollAttackPower);
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 短時間で複数回同じオブジェクトに当たらないように調整
            foreach(var obj in _NotReHitObjList)
            {
                if(obj._Hash == collision.gameObject.GetHashCode())
                {
                    return;
                }
            }

            if(collision.gameObject.GetComponent<CharaBaseBehaviour>() == null)
            {
                // ＰＬから生じる爆発に当たったとする
                if(_StateMachine._CurrentState is Em1StateNormal || _StateMachine._CurrentState is Em1StateBlowOut)
                {
                    // 爆発に応じた速度加算
                    Vector3 angle = (gameObject.transform.position - collision.gameObject.transform.position).normalized;
                    angle.z = 0f;
                    _Speed += angle * ParamManager.Instance.getParam<Em1Param>()._SpeedByExplosion;

                    // 再度爆発に当たらないように辞書へ追加
                    _NotReHitObjList.Add(new NotReHit(collision.gameObject.GetHashCode(), ParamManager.Instance.getParam<Em1Param>()._TimeSecOfReHitSameObj));

                    // ダメージ処理
                    damage(1);
                }
            }
        }

        #endregion // 衝突

        #region 非公開メソッド

        // 移動関係の更新
        private void moveUpdate()
        {
            // 速度の減算
            Vector3 nextSpeed = _Speed;
            nextSpeed.x -= (ParamManager.Instance.getParam<Em1Param>()._SpeedResistance * Time.deltaTime * _Speed.x);
            nextSpeed.y -= (ParamManager.Instance.getParam<Em1Param>()._SpeedResistance * Time.deltaTime * _Speed.y);
            _Speed = nextSpeed;

            gameObject.transform.position += _Speed * Time.deltaTime;

            // 画面外に出た場合は跳ね返る
            Vector3 nextPos;
            if (!(StageManager.Instance.checkCharaInStageAndCalcReturnStage(this, out nextPos, out nextSpeed)))
            {
                gameObject.transform.position = nextPos;
                _Speed = nextSpeed;
            }

        }

        // ダメージを受けた際の処理
        public void damage(int damage)
        {
            _Hp -= damage;
            _StateMachine.changeState(new Em1StateDamage(this));
        }
        #endregion

    }

}