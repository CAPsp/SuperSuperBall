using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{

    public class BGMManager : SingletonMonoBehaviour<BGMManager>
    {
        #region enum

        public enum BGMName
        {
            Main,
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

        // 引数に渡されたBGMを鳴らす
        public void playBGM(BGMName name)
        {
            _AudioSource.clip = testClip;
            _AudioSource.Play();
        }

        public void stopCurrentBGM()
        {
            _AudioSource.Stop();
        }

        #endregion // 公開メソッド

    }

}