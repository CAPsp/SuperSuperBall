using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    // 敵の弾を管理する
    public class EnemyShotManager : SingletonMonoBehaviour<EnemyShotManager>
    {

        #region フィールド

        [SerializeField]
        public GameObject _enemyShotObj;

        [SerializeField]
        public GameObject _instantField;

        private List<GameObject> _ListShots = new List<GameObject>();

        #endregion

        #region 基本

        private void Update()
        {
            // 画面外に出た弾を消す
            for(int i = 0; i < _ListShots.Count; i++)
            {
                if (!(CameraManager.Instance.checkInsideScreen(_ListShots[i].transform.position)))
                {
                    Destroy(_ListShots[i]);
                    _ListShots.RemoveAt(i);
                    i--;
                }
            }
        }

        #endregion // 基本

        #region 公開メソッド

        public void shotGen(Vector2 pos, Vector2 angle, float speed)
        {
            GameObject shot = Instantiate(_enemyShotObj);
            shot.GetComponent<EnemyShot>().setEnemyShotParam(pos, angle, speed);
            shot.transform.parent = _instantField.transform;
            _ListShots.Add(shot);
        }

        public void shotDelete(GameObject shot)
        {
            if(_ListShots.Remove(shot))
            {
                Destroy(shot);
            }
        }

        #endregion // 公開メソッド
    }

}