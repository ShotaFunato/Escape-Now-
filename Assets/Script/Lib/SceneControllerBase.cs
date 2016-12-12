/**
* ゲーム管理基底クラス
* @author Shota Funato
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace FunatoLib
{
    public class SceneControllerBase<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
    {
        /// <summary>
        /// シーン名リスト
        /// </summary>
        private List<string> sceneNameList = new List<string>();

        /// <summary>
        /// シーン追加。登録した順番が指定番号になる
        /// </summary>
        /// <param name="sceneName">登録したいシーン名</param>
        protected void AddScene(string sceneName)
        {
            this.sceneNameList.Add(sceneName);
        }

        /// <summary>
        /// シーン遷移
        /// </summary>
        /// <param name="id">遷移したいシーン番号</param>
        public virtual void LoadScene(int id)
        {
            if (id >= this.sceneNameList.Count)
            {
                Debug.Log("GameController LoadScene EntryOverId : " + id);
                return;
            }
            SceneManager.LoadScene(this.sceneNameList[id]);
        }
    }
}