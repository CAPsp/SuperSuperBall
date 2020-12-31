using UnityEngine;
using UnityEngine.UI;

namespace ssb
{
    public class ResultScoreUI : MonoBehaviour
    {
        #region 基本

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Text>().text = $"SCORE: {DataManager.Instance._GameScore}";
        }

        #endregion // 基本
    }

}