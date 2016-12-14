/**
 * リザルトボタンクラス
 * @author Shota Funato
 */

using UnityEngine;
using FunatoLib;

public class ResultButton : CanvasButton
{
    /// <summary>
    /// ボタン識別番号
    /// </summary>
    [SerializeField]
    private SceneController.SceneId nextSceneId;

    /// <summary>
    /// 選択された時に呼ばれる
    /// </summary>
    public override void OnClick(int num)
    {
        SceneController.Instance.LoadScene(num);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // エディタから設定された遷移先のシーン識別番号を選択した際に呼ばれるように設定する
        this.SetButton((int)this.nextSceneId);
    }
}
