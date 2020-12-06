using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // ゲーム全体管理
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {

        #region 基本

        private void Start()
        {
            // BGM再生
            BGMManager.Instance.playBGM(BGMManager.BGMName.Main);
        }

        #endregion // 基本

    }
}