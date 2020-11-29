using UnityEngine;

namespace ssb.param
{
    // PL関連のパラメータ管理
    [CreateAssetMenu(fileName = "Em1Param", menuName = "Param/Em1Param")]
    class Em1Param : ScriptableObject
    {
        // 速度に対する抵抗力
        public float _SpeedResistance       = 0f;

        // 弾生成インターバル
        public float _ShotGenIntervalSec    = 0f;

        // この速度量より上の場合は吹っ飛ばされている
        public float _NotControllSpeedLimit = 0f;

        // 無敵時間さん！？
        public float _MutekiTimeSec         = 0f;

        // HP
        public int _Hp                      = 0;
    }
}
