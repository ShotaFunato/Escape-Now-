/**
 * ステージボタンクラス
 * @author Shota Funato
 */

using FunatoLib;

public class StageButton : CanvasButton
{
    /// <summary>
    /// 選択された時に呼ばれる
    /// </summary>
    public override void OnClick(int num)
    {
        DataBankController.Instance.Entry((int)DataEntryDef.DataBankKind.SelectStageId, num);
        SceneController.Instance.LoadScene(SceneController.SceneId.StageScene);
    }
}
