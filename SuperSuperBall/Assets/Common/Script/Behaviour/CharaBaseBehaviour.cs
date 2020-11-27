using UnityEngine;


namespace ssb
{

    public abstract class CharaBaseBehaviour : MonoBehaviour
    {

        #region enum

        // そのオブジェクトが何に属しているかを決める
        public enum ObjType
        {
            Player,
            Enemy,
            Shot,
            None,
        }

        #endregion // enum

        #region プロパティ

        // 継承先は必ずこれを定義する必要有
        public abstract ObjType _Type { protected set; get; }

        // 状態管理ステートマシン
        public StateMachine _StateMachine { protected set; get; }

        public Vector3 _Speed { protected set; get; }

        #endregion // プロパティ

    }

}