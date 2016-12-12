/**
* ゲーム管理クラス
* @author Shota Funato
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace FunatoLib
{
    public class SceneController : SceneControllerBase<SceneController>
    {
        public enum SceneId
        {
            TitleScene,
            StageSelectScene,
            StageScene,
            ResultScene,

            SceneMax
        };

        private SceneController()
        {
            // 生成時にゲームに存在するシーン名前を登録しておく。順序はIDと合わせる
            base.AddScene("TitleScene");
            base.AddScene("StageSelectScene");
            base.AddScene("StageScene");
            base.AddScene("ResultScene");
        }

        /// <summary>
        /// シーン遷移
        /// </summary>
        /// <param name="id">遷移したいシーン列挙番号</param>
        public void LoadScene(SceneController.SceneId id)
        {
            base.LoadScene((int)id);
        }
    }
}