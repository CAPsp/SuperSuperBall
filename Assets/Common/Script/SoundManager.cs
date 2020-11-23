using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        #region enum

        public enum SEName
        {
            Hit,
        }

        #endregion // enum

        #region フィールド

        [SerializeField]
        public AudioClip testClip;

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
            _AudioSource.clip = testClip;
            _AudioSource.Play();
        }

        #endregion // 公開メソッド

    }

}