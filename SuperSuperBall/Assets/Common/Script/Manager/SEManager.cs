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
            AudioClip tmpClip = null;
            switch(name)
            {
                case SEName.Hit:        tmpClip = _Hit;        break;
                case SEName.PLDeath:    tmpClip = _PLDeath;    break;
                case SEName.ShotDelete: tmpClip = _ShotDelete; break;
            }

            // SE重複再生、nullの阻止
            if (tmpClip == null ||
                (_AudioSource.clip == tmpClip && _AudioSource.isPlaying))
            {
                return;
            }

            _AudioSource.clip = tmpClip;
            _AudioSource.Play();
        }

        #endregion // 公開メソッド

    }

}