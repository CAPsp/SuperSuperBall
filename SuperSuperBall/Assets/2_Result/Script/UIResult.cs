using UnityEngine;

using UnityEngine.UI;

namespace ssb
{
    public class UIResult : MonoBehaviour
    {

        #region 基本

        // Update is called once per frame
        void Update()
        {
            // 次シーンへ遷移
            if (InputManager.Instance.isDecide || InputManager.Instance.isCancel)
            {
                SceneManager.Instance.changeScene(SceneManager.SceneName.Title);
            }
        }

        #endregion // 基本
    }
}