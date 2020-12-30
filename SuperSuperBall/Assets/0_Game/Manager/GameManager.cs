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

        private void Update()
        {
            // PL残機0処理
            if(PLManager.Instance._CurrentPLObj == null)
            {
                gameEnd();
            }
        }

        #endregion // 基本

        #region 非公開メソッド

        // ゲーム終了時の処理
        public void gameEnd()
        {
            // BGM停止
            BGMManager.Instance.stopCurrentBGM();
            
            // シーン遷移
            SceneManager.Instance.changeScene(SceneManager.SceneName.Result);
        }

        #endregion // 非公開メソッド

    }
}