using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // パーティクル管理
    public class ParticleManager : SingletonMonoBehaviour<ParticleManager>
    {
        #region enum

        public enum ParticleName
        {
            Hit,
        }

        #endregion // enum

        #region フィールド

        [SerializeField]
        public GameObject _InstantField;

        [SerializeField]
        private GameObject _Hit = null;

        private List<GameObject> _InstantObjList = new List<GameObject>();

        #endregion // フィールド

        #region 基本

        private void Update()
        {
            for(int i = 0; i < _InstantObjList.Count; i++)
            {
                GameObject obj = _InstantObjList[i];
                if (obj.GetComponent<ParticleSystem>() != null)
                {
                    if(!(obj.GetComponent<ParticleSystem>().isPlaying))
                    {
                        _InstantObjList.Remove(obj);
                        Destroy(obj);
                    }
                }
            }
        }

        #endregion // 基本

        #region 公開メソッド

        // 引数で渡された座標にパーティクルを生成
        public void initiateParticle(ParticleName name, Vector3 pos)
        {
            GameObject obj = null;
            switch (name)
            {
                case ParticleName.Hit: obj = Instantiate(_Hit);  break;
            }

            if(obj != null)
            {
                obj.transform.parent    = _InstantField.transform;
                obj.transform.position  = pos;

                _InstantObjList.Add(obj);
            }
        }

        #endregion  // 公開メソッド

    }
}