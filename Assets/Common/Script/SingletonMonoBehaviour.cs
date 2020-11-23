using System;
using UnityEngine;


namespace ssb
{

    // シングルトン基底クラス
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour
        where T : MonoBehaviour
    {

        private static T instance = null;
        public static T Instance
        {
            get 
            {
                if(instance == null)
                {
                    Type t = typeof(T);

                    instance = (T)FindObjectOfType(t);
                    if(instance == null)
                    {
                        Debug.LogError(t + "をアタッチしているGameObjectがシーンに存在しません");
                    }
                }

                return instance;
            }
        }

        virtual protected void Awake()
        {
            // 他のゲームオブジェクトにアタッチされているか調べる
            // アタッチされている場合は破棄する
            if(instance != null && instance != this)
            {
                Debug.LogWarning(typeof(T) + "がアタッチされているGameObjectが他に存在するため、" + this.name + "は削除されます");
                Destroy(this);
            }
        }
    }

}