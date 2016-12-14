/**
 * タイトルシーンクラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

using FunatoLib;

public class TitleScene : Work
{
    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void Update()
    {
        base.Update();

        InputController inputController = InputController.Instance;

        if (inputController.GetMouseLeftClick())
        {
            SceneController.Instance.LoadScene(SceneController.SceneId.StageSelectScene);
        }
    }
}
