using System.Collections;
using System.Collections.Generic;


namespace ssb
{
    // 全ステートの基底クラス
    public abstract class BaseState
    {


        #region 公開メソッド

        // 開始メソッド
        public virtual void enter() { }

        // 更新時
        public abstract void update();

        // 終了メソッド
        public virtual void exit() { }

        #endregion

    }

}