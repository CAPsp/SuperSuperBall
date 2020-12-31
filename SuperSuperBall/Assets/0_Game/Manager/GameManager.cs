using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ssb
{
    // ゲーム全体管理
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {

        #region SerializeField

        [SerializeField]
        Text _ScoreText = null;

        #endregion // SerializeField

        #region Property

        public int _Score { private set; get; } = 0;

        #endregion // Property

        #region 基本

        private void Start()
        {
            // スコア初期化
            _Score = 0;
            rewriteScoreText();

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

        #region 公開メソッド

        // スコア加算メソッド
        public void addScore(int addScore)
        {
            _Score += addScore;
            rewriteScoreText();
        }

        // ゲーム終了時の処理
        public void gameEnd()
        {
            // BGM停止
            BGMManager.Instance.stopCurrentBGM();

            // シーン遷移
            SceneManager.Instance.changeScene(SceneManager.SceneName.Result);
        }

        #endregion // 公開メソッド

        #region 非公開メソッド

        // スコアテキストの書き換え
        private void rewriteScoreText()
        {
            _ScoreText.text = $"SCORE: {_Score}";

        }

        #endregion // 非公開メソッド

    }
}