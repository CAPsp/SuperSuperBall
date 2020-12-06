using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // ゲーム全体管理
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {

        #region フィールド

        [SerializeField]
        private GameObject _PLPrefab;

        [SerializeField]
        private GameObject _PLInstObj;

        #endregion  // フィールド

        #region 公開メソッド

        void spawn()
        {

        }

        #endregion  // 公開メソッド
    }
}