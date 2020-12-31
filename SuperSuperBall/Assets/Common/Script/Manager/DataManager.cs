using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // ゲーム全体で使用するデータを管理するマネージャ
    public class DataManager : SingletonMonoBehaviour<DataManager>
    {
        #region プロパティ

        // ゲームスコア
        public int _GameScore { set; get; } = 0;

        #endregion
    }
}