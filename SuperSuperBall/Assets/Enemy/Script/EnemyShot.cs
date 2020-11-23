using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    // 敵の弾
    public class EnemyShot : MonoBehaviour
    {

        #region フィールド

        private Vector3 _Speed;

        #endregion

        #region 基本

        // Update is called once per frame
        private void Update()
        {
            gameObject.transform.position += _Speed * Time.deltaTime;
        }

        #endregion

        #region 衝突

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // PLに当たった場合
            if(collision.gameObject.GetComponent<PLBehaviour>() != null)
            {
                collision.gameObject.GetComponent<PLBehaviour>().hit();

                // 攻撃中のPLに当たったら消える
                if (collision.gameObject.GetComponent<PLBehaviour>()._State == PLBehaviour.PLState.Attack)
                {
                    SEManager.Instance.playSE(SEManager.SEName.ShotDelete);
                    EnemyShotManager.Instance.shotDelete(gameObject);
                }
            }
        }

        #endregion // 衝突

        #region 公開メソッド

        public void setEnemyShotParam(Vector2 pos, Vector2 angle, float speed)
        {
            gameObject.transform.position = new Vector3(pos.x, pos.y, 0.0f);

            angle.Normalize();
            _Speed = new Vector3(angle.x * speed, angle.y * speed, 0.0f);
        }

        #endregion // 公開メソッド
    }

}