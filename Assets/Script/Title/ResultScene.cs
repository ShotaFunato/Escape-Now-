/**
 * リザルトシーンクラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

using FunatoLib;

public class ResultScene : Work
{
    /// <summary>
    /// 初期化
    /// </summary>
    override protected void Start()
    {
        base.Start();

        // リザルトに遷移した時に、セーブデータとして保存する
        /*
        // ゴールまで到達してクリアしたなら
        if ()
        {
            PlayerPrefsでセーブデータに今回のスコア、ステージクリア済みを保存する
        }        
        */
    }

    /// <summary>
    /// 更新
    /// </summary>
    override protected void Update()
    {
        base.Update();

        InputController inputController = InputController.Instance;

        if (inputController.GetMouseLeftClick())
        {
            SceneController.Instance.LoadScene(SceneController.SceneId.StageScene);
        }
    }
}
