using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    public class SEManager : SingletonMonoBehaviour<SEManager>
    {
        #region enum

        public enum SEName
        {
            Hit,
            PLDeath,
        }

        #endregion // enum

        #region フィールド

        [SerializeField]
        public AudioClip _Hit;
        public AudioClip _PLDeath;

        private AudioSource _AudioSource;

        #endregion // フィールド

        #region 基本

        private void Start()
        {
            _AudioSource = GetComponent<AudioSource>();
        }

        #endregion

        #region 公開メソッド

        // 引数に渡されたＳＥを鳴らす
        public void playSE(SEName name)
        {
            switch(name)
            {
                case SEName.Hit:        _AudioSource.clip = _Hit;       break;
                case SEName.PLDeath:    _AudioSource.clip = _PLDeath;   break;
            }
            
            _AudioSource.Play();
        }

        #endregion // 公開メソッド

    }

}