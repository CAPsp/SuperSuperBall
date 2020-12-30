using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // ゲーム中のステージ内管理を担う
    public class StageManager : SingletonMonoBehaviour<StageManager>
    {

        #region プロパティ

        public Vector3 _CenterPos { private set; get; } = Vector3.zero; // ステージ中心座標

        public float _Radius { private set; get; } = 25.0f;             // 円形ステージの半径

        #endregion // プロパティ

        #region 公開メソッド

        /// <summary>
        /// 引数渡されたキャラクタがステージ外に存在するかを計算
        /// ステージ外にいた場合はステージ内に戻った場合の座標と速度を引数で返す
        /// </summary>
        public bool checkCharaInStageAndCalcReturnStage(CharaBaseBehaviour chara, out Vector3 nextPos, out Vector3 nextSpeed)
        {
            // 初期化
            nextPos     = Vector3.zero;
            nextSpeed   = Vector3.zero;

            // 2次元上での計算であることに注意
            Vector3 charaPosFromCenter = chara.transform.position - _CenterPos;
            charaPosFromCenter.z = 0f;
            bool isInside = charaPosFromCenter.magnitude < _Radius;
            
            // ステージ外にいる場合、ステージ内に戻る場合の座標と速度を計算
            if(!(isInside))
            {
                // ステージ内に座標をそのまま押し戻す
                nextPos = charaPosFromCenter.normalized * _Radius;

                // 反射速度を計算
                Vector3 normalVec = (charaPosFromCenter * (-1f)).normalized;
                nextSpeed = chara._Speed + 2f * Vector3.Dot(chara._Speed * (-1f), normalVec) * normalVec;
            }

            return isInside;
        }

        #endregion // 公開メソッド

    }
}