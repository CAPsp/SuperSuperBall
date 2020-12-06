using UnityEngine;

using UnityEngine.UI;

namespace ssb
{

    public class UITitle : MonoBehaviour
    {
        #region 定数

        public float FLASH_INTERVAL_SEC = 2f;

        #endregion

        #region フィールド

        private Image _TitleImg = null;

        private Color _ImgColor = new Color(0f, 0f, 0f, 0f);

        private float _AlphaSigTheta = 0f;

        #endregion // フィールド

        #region 基本

        private void Awake()
        {
            _TitleImg = gameObject.GetComponent<Image>();
        }

        private void Start()
        {
            _ImgColor = _TitleImg.color;
        }

        // Update is called once per frame
        void Update()
        {
            // 「PRESS ANY BUTTON」の点滅処理
            _ImgColor.a = Mathf.Sin(_AlphaSigTheta);
            _TitleImg.color = _ImgColor;
            _AlphaSigTheta += Time.deltaTime * Mathf.PI / FLASH_INTERVAL_SEC;
            if (_AlphaSigTheta >= Mathf.PI)
            {
                _AlphaSigTheta = 0;
            }

            // 次シーンへ遷移
            if(InputManager.Instance.isDecide || InputManager.Instance.isCancel)
            {
                SceneManager.Instance.changeScene(SceneManager.SceneName.Game);
            }
        }

        #endregion // 基本
    }
}