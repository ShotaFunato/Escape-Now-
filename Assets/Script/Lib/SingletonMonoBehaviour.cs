/**
* シングルトン基底クラス
* @author Shota Funato
*/

using UnityEngine;
using System;

namespace FunatoLib
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// インスタンス。複製禁止しているのでここに保持される
        /// </summary>
        private static T instance = null;

        /// <summary>
        /// アプリが終了しているかどうか
        /// </summary>
        static bool applicationIsQuitting = false;

        /// <summary>
        /// インスタンス取得。まだ見つけてない場合は探す
        /// </summary>
        public static T Instance
        {
            get
            {
                // アプリ終了時に、再度インスタンスの呼び出しがある場合、オブジェクトを生成することを防ぐ
                if (applicationIsQuitting)
                {
                    return instance;
                }
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    // Findで見つからなかった場合、前もってセットしていないということなので、新しくオブジェクトを生成
                    if (instance == null)
                    {
                        // 動的にオブジェクトを生成して、シングルトンだと分かりやすいように名前を設定
                        GameObject singleton = new GameObject();
                        singleton.name = typeof(T).ToString() + " (singleton)";
                        instance = singleton.AddComponent<T>();
                        
                        // シーン変更時に破棄させない
                        DontDestroyOnLoad(singleton);
                    }
                }

                return instance;
            }
            // インスタンスをnull化するときに使うのでprivateに
            private set
            {
                instance = value;
            }
        }

        void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        void OnDestroy()
        {
            Instance = null;
        }

        // コンストラクタをprotectedにすることでインスタンスを生成出来なくする
        protected SingletonMonoBehaviour()
        {
        }
    }
}