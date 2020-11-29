using UnityEngine;

using ssb.state;
using ssb.param;

namespace ssb
{
    // 敵基底クラス
    public abstract class EnemyBehaviour : CharaBaseBehaviour
    {
        #region プロパティ

        public override ObjType _Type { protected set; get; } = ObjType.Enemy;

        public int _Hp { protected set; get; }

        #endregion // プロパティ

    }

}