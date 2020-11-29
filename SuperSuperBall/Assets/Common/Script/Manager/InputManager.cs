﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssb
{
    // ゲーム全体の入力を管理する
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        #region enum

        public enum KeyState
        {
            None,
            Down,
            Hold,
            Up,
        }

        #endregion // enum


        #region 公開プロパティ

        // 決定
        public bool isDecide { get; private set; }

        // 移動値
        public float axisX { get; private set; }
        public float axisY { get; private set; }

        // PLの操作
        public KeyState Hold { get; private set; }

        #endregion

        #region 基本

        private void Update()
        {
            // 各入力の更新
            isDecide =
                Input.GetKeyDown(KeyCode.Space) ||
                Input.GetButtonDown("Submit");

            axisX = Input.GetAxis("Horizontal");
            axisY = Input.GetAxis("Vertical");

            Hold = convertButtonState("Hold");
        }

        #endregion // 基本

        #region 非公開メソッド

        private KeyState convertButtonState(string buttonName)
        {
            if (Input.GetButtonDown(buttonName))    { return KeyState.Down; }
            else if (Input.GetButtonUp(buttonName)) { return KeyState.Up; }
            else if (Input.GetButton(buttonName))   { return KeyState.Hold; }

            return KeyState.None;
        }

        #endregion
    }

}