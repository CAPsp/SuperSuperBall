using UnityEngine;

namespace ssb.param
{
    // PL関連のパラメータ管理
    [CreateAssetMenu(fileName = "PLParam", menuName = "Param/PLParam")]
    class PLParam : ScriptableObject
    {
        // 速度に対する抵抗力
        public float _SpeedResistance;

        // 通常速度
        public float _MoveSpeedPerSec;

        // 発射速度
        public float _ShotSpeedPerSec;

        // 操作がきくようになる速度
        public float _BitControllSpeedLimit;

        // 自由に操作できるようになる速度
        public float _FreeControllSpeedLimit;

        // 無敵時間さん！？
        public float _MutekiTimeSec;
    }
}
