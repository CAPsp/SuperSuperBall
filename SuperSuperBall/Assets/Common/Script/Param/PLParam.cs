using UnityEngine;

namespace ssb.param
{
    // PL関連のパラメータ管理
    [CreateAssetMenu(fileName = "PLParam", menuName = "Param/PLParam")]
    class PLParam : ScriptableObject
    {
        // 速度に対する抵抗力
        public float _SpeedResistance           = 0f;

        // 力をためるさいの抵抗力
        public float _ChargeResistance          = 0f;

        // 通常速度
        public float _MoveSpeedPerSec           = 0f;

        // 発射速度
        public float _ShotSpeedPerSec           = 0f;

        // 操作がきくようになる速度
        public float _BitControllSpeedLimit     = 0f;

        // 自由に操作できるようになる速度
        public float _FreeControllSpeedLimit    = 0f;

        // 無視する速度量
        public float _IgnoreSpeedLimit          = 0f;

        // 衝撃波のScale計算に考慮される除算値
        public float _ExplosionDiv              = 0f;

        // 無敵時間さん！？
        public float _MutekiTimeSec             = 0f;

        // 残機数
        public int _Life                        = 0;
    }
}
