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

        public override void update()
        {
            _Owner.move();
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
    }
}