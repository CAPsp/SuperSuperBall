using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public PLStateCharge(PLBehaviour owner)
        {
            _Owner = owner;
        }

        public override void enter()
        {

        }

        public override void update()
        {
            _Owner.move(InputManager.Instance.axisX, InputManager.Instance.axisY);

            if (InputManager.Instance.Hold == InputManager.KeyState.Up)
            {
                _Owner._StateMachine.changeState(new PLStateChargeRelease(_Owner));
            }
            else if (InputManager.Instance.isDecide)
            {
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

        public PLStateChargeRelease(PLBehaviour owner)
        {
            _Owner = owner;
        }

        public override void enter()
        {

        }

        public override void update()
        {
            _Owner.shot();

            /*
            if (Mathf.Sign(_Speed.x) != Mathf.Sign(diff.x) || Mathf.Sign(_Speed.y) != Mathf.Sign(diff.y))
            {
                _Owner._StateMachine.changeState(new PLStateShoot(_Owner));
            }

            if (_Speed.magnitude < NOT_BIND_SPEED_LIMIT)
            {
                _Owner._StateMachine.changeState(new PLStateNormal(_Owner));
            }
            */
        }

        public override void exit()
        {

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

            /*
            if (_Speed.magnitude < BIT_FREE_SPEED_LIMIT)
            {
                _Speed += new Vector3(x, y, 0.0f) * 0.02f;
                if (_Speed.magnitude < NOT_BIND_SPEED_LIMIT)
                {
                    _Speed = Vector3.zero;
                    _MutekiTimeSec = (_MutekiTimeSec <= 0f) ? MUTEKI_TIME_SEC : _MutekiTimeSec;
                    _Owner._StateMachine.changeState(new PLStateNormal(_Owner));
                }
            }
            */
        }

        public override void exit()
        {

        }
    }

    // 死亡時
    public class PLStateDeath : BaseState
    {
        private PLBehaviour _Owner;

        public PLStateDeath(PLBehaviour owner)
        {
            _Owner = owner;
        }

        public override void enter()
        {

        }

        public override void update()
        {

        }

        public override void exit()
        {
        }
    }
}