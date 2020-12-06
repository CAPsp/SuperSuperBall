using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // 画面遷移管理
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        #region enum

        public enum SceneName
        {
            Title = 0,
            Game,
            Result,

            Max,
        }

        #endregion // enum

        #region プロパティ

        public SceneName _CurrentScene { private set; get; } = SceneName.Title;

        #endregion // プロパティ

        #region フィールド

        [SerializeField]
        private List<GameObject> _SceneList = new List<GameObject>();

        #endregion // フィールド

        #region 基本

        private void Start()
        {
            changeScene(SceneName.Title);
        }

        private void Update()
        {
            // DEBUG: キャンセルボタンで次シーン遷移
            if(InputManager.Instance.isCancel)
            {
                int nextScene = (int)_CurrentScene + 1;
                if(nextScene >= (int)SceneName.Max)
                {
                    nextScene = 0;
                }

                changeScene((SceneName)nextScene);
            }
        }

        #endregion  // 基本

        #region 公開メソッド

        public void changeScene(SceneName nextScene)
        {
            // 既にシーン再生時は遷移処理を行わない
            if(nextScene == _CurrentScene && _SceneList[(int)_CurrentScene].activeSelf)
            {
                return;
            }
            _SceneList[(int)_CurrentScene].SetActive(false);

            switch (nextScene)
            {
                case SceneName.Title:  _SceneList[(int)nextScene].SetActive(true);  break;
                case SceneName.Game:   _SceneList[(int)nextScene].SetActive(true);  break;
                case SceneName.Result: _SceneList[(int)nextScene].SetActive(true);  break;
            }

            _CurrentScene = nextScene;
        }

        #endregion // 非公開メソッド

    }
}