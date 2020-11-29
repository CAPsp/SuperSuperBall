using UnityEngine;

using ssb.param;

namespace ssb.state
{
    // 通常時
    public class PLStateNormal : BaseState
    {
        private PLBehaviour _Owner;

        public PLStateNormal(PLBehaviour owner)
        {
            _Owner = owner;
        }

        public override void enter()
        {

        }

        public override void update()
        {
            _Owner.move(InputManager.Instance.axisX, InputManager.Instance.axisY);

            if( InputManager.Instance.Hold == InputManager.KeyState.Down &&
                _Owner._MutekiTimeSec <= 0f)
            {
                _Owner._StateMachine.changeState(new PLStateCharge(_Owner));
            }
        }

        public override void exit()
        {

        }
    }

    // 力をためている　
    public class PLStateCharge : BaseState
    {
        private PLBehaviour _Owner;
        private Vector3 _FixedPos;

        public PLStateCharge(PLBehaviour owner)
        {
            _Owner = owner;
        }

        public override void enter()
        {
            _FixedPos = _Owner.transform.position;
        }

        public override void update()
        {
            float distToFixedPos = Vector3.Distance(_FixedPos, _Owner.transform.position);
            _Owner.move(
                InputManager.Instance.axisX / ((distToFixedPos + 1f) * ParamManager.Instance.getParam<PLParam>()._ChargeResistance),
                InputManager.Instance.axisY / ((distToFixedPos + 1f) * ParamManager.Instance.getParam<PLParam>()._ChargeResistance)
            );

            _Owner.setFixedLocalPos(_FixedPos - _Owner.transform.position);

            if (InputManager.Instance.Hold == InputManager.KeyState.Up) // ため解放
            {
                _Owner.addSpeed((_FixedPos - _Owner.transform.position) * ParamManager.Instance.getParam<PLParam>()._ShotSpeedPerSec);
                _Owner._StateMachine.changeState(new PLStateChargeRelease(_Owner));
            }
            else if (InputManager.Instance.isDecide)    // ため解除
            {
                _Owner.setFixedLocalPos(Vector3.zero);
                _Owner._StateMachine.changeState(new PLStateNormal(_Owner));
            }
        }

        public override void exit()
        {
        }
    }

    // 力解放直後　
    public class PLStateChargeRelease : BaseState
    {
        private PLBehaviour _Owner;
        private Vector3 _FixedPos;

        public PLStateChargeRelease(PLBehaviour owner)
        {
            _Owner = owner;
        }

        public override void enter()
        {
            _FixedPos = _Owner._BackGameObj.transform.position;
        }

        public override void update()
        {
            _Owner.shot();

            _Owner.setFixedLocalPos(_FixedPos - _Owner.transform.position);

            Vector3 diffToFixedPos = _FixedPos - _Owner.transform.position;

            // ＰＬが固定した場所から通り抜けたら次のステートへ
            bool isThroughFixedPos = Mathf.Sign(_Owner._Speed.x) != Mathf.Sign(diffToFixedPos.x) || Mathf.Sign(_Owner._Speed.y) != Mathf.Sign(diffToFixedPos.y);
            if (isThroughFixedPos)
            {
                _Owner._StateMachine.changeState(new PLStateShoot(_Owner));
            }

            // ＰＬ速度が自由に動ける速度になったら通常ステートへ戻る
            if (_Owner._Speed.magnitude < ParamManager.Instance.getParam<PLParam>()._FreeControllSpeedLimit)
            {
                _Owner._StateMachine.changeState(new PLStateNormal(_Owner));
            }
        }

        public override void exit()
        {
            _Owner.setFixedLocalPos(Vector3.zero);
        }
    }

    // 攻撃中（解放して吹っ飛んでいる）　
    public class PLStateShoot : BaseState
    {
        private PLBehaviour _Owner;

        public PLStateShoot(PLBehaviour owner)
        {
            _Owner = owner;
        }

        public override void enter()
        {

        }

        public override void update()
        {
            _Owner.shot();

            // 多少自由が利く速度になったらコントローラ入力を受け付ける
            if (_Owner._Speed.magnitude < ParamManager.Instance.getParam<PLParam>()._BitControllSpeedLimit)
            {
                _Owner.addSpeed(new Vector3(InputManager.Instance.axisX * Time.deltaTime, InputManager.Instance.axisY * Time.deltaTime, 0.0f));

                if (_Owner._Speed.magnitude < ParamManager.Instance.getParam<PLParam>()._FreeControllSpeedLimit)
                {
                    _Owner._MutekiTimeSec = (_Owner._MutekiTimeSec <= 0f) ? ParamManager.Instance.getParam<PLParam>()._MutekiTimeSec : _Owner._MutekiTimeSec;
                    _Owner._StateMachine.changeState(new PLStateNormal(_Owner));
                }
            }
        }

        public override void exit()
        {

        }
    }

    // 死亡時
    public class PLStateDeath : BaseState
    {
        private PLBehaviour _Owner;
        private Animator _Anim;

        public PLStateDeath(PLBehaviour owner)
        {
            _Owner = owner;
            _Anim = _Owner.GetComponent<Animator>();
        }

        public override void enter()
        {
            // SE発生とアニメーション開始
            SEManager.Instance.playSE(SEManager.SEName.PLDeath);
            _Anim.SetBool("isDeath", true);
        }

        public override void update()
        {
            // アニメーション処理が終わったらゲームオブジェクト破棄
            if (_Anim.GetCurrentAnimatorStateInfo(0).IsName("PL_death") &&
                _Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                UnityEngine.Object.Destroy(_Owner.gameObject);
            }
        }

        public override void exit()
        {
        }
    }
}