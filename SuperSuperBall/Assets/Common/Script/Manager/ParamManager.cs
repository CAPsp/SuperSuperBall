using UnityEngine;

using System.Collections.Generic;

namespace ssb
{
    // パラメータ管理
    public class ParamManager : SingletonMonoBehaviour<ParamManager>
    {
        #region 定数

        // パラメータを管理しているフォルダ
        private static readonly string PARAM_FOLDER = "/Param/";

        #endregion

        #region フィールド

        // 読み込んだパラメータ群
        [SerializeField]
        private List<ScriptableObject> _ParamList = new List<ScriptableObject>();

        #endregion

        #region 公開メソッド

        public T getParam<T>() where T : ScriptableObject
        {
            foreach(ScriptableObject param in _ParamList)
            {
                if(param is T)
                {
                    return (T)param;
                }
            }

            // 見つからなかった場合はnullを返す
            return null;
        }

        #endregion

    }

}