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
            ShotDelete,
        }

        #endregion // enum

        #region フィールド

        [SerializeField]
        private AudioClip _Hit          = null;
        [SerializeField]
        private AudioClip _PLDeath      = null;
        [SerializeField]
        private AudioClip _ShotDelete   = null;

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
                case SEName.ShotDelete: _AudioSource.clip = _ShotDelete; break;
            }
            
            _AudioSource.Play();
        }

        #endregion // 公開メソッド

    }

}