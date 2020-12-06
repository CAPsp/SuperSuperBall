using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // 一時停止等の制御に使用
    public class RistrictManager : SingletonMonoBehaviour<RistrictManager>
    {
        #region フィールド

        private float _StopTimerSec = 0f;

        private List<MonoBehaviour> _BehaviourList = new List<MonoBehaviour>();

        #endregion // フィールド

        #region 基本

        private void Update()
        {
            // 停止タイマーが作動している場合、タイマー切れで停止を解除する
            if(_StopTimerSec >= 0f)
            {
                _StopTimerSec -= Time.unscaledDeltaTime;
                if(_StopTimerSec <= 0f)
                {
                    unStop();
                }
            }
            
        }

        #endregion  // 基本

        #region 公開メソッド

        public void stop()
        {
            for(int i = _BehaviourList.Count - 1; i >= 0; i--)
            {
                if(_BehaviourList[i] == null)
                {
                    _BehaviourList.RemoveAt(i);
                }
                else
                {
                    _BehaviourList[i].enabled = false;
                }
            }

            Time.timeScale = 0f;
        }

        // 引数に渡してある秒数分、管理下にあるBehaviourを停止させる
        public void stop(float stopSec)
        {
            stop();
            _StopTimerSec = stopSec;
        }

        // 停止解除
        public void unStop()
        {
            for (int i = _BehaviourList.Count - 1; i >= 0; i--)
            {
                if (_BehaviourList[i] == null)
                {
                    _BehaviourList.RemoveAt(i);
                }
                else
                {
                    _BehaviourList[i].enabled = true;
                }
            }
            Time.timeScale = 1f;
        }

        // 引数に渡したBehaviourを管理下に入れる
        public void registerBehaviour(MonoBehaviour behaviour)
        {
            _BehaviourList.Add(behaviour);
        }

        #endregion // 公開メソッド
    }
}