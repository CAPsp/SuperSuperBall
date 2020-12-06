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
        private List<GameObject> _ScenePrefabList = new List<GameObject>();

        private GameObject _GenGamePrefab = null;

        #endregion // フィールド

        #region 基本

        private void Start()
        {
            changeScene(SceneName.Title);
        }

        #endregion  // 基本

        #region 公開メソッド

        public void changeScene(SceneName nextScene)
        {
            // 既にシーン再生時は遷移処理を行わない
            if (nextScene == _CurrentScene && _GenGamePrefab != null)
            {
                return;
            }

            Destroy(_GenGamePrefab);

            _GenGamePrefab = Instantiate(_ScenePrefabList[(int)nextScene]);

            _CurrentScene = nextScene;
        }

        #endregion // 非公開メソッド

    }
}