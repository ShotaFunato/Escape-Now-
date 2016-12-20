/**
 * ステージボタンクラス
 * @author Shota Funato
 */

using UnityEngine;
using FunatoLib;
using UnityEngine.UI;

public class StageButton : CanvasButton
{
    /// <summary>
    /// 対応のステージ番号
    /// </summary>
    [SerializeField]
    private int stageId;

    /// <summary>
    /// 選択された時に呼ばれる
    /// </summary>
    public override void OnClick(int num)
    {
        DataBankController.Instance.Entry((int)DataEntryDef.DataBankKind.SelectStageId, num);
        SceneController.Instance.LoadScene(SceneController.SceneId.StageScene);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        string key = "";
        bool clearFlag = false;

        this.stageId += 1;

        key = DataEntryDef.SaveKind.Stage.ToString() + this.stageId;
        clearFlag = Save.GetData(key + DataEntryDef.SaveKind.IsClear.ToString(), false);

        // クリア済みなら
        if (clearFlag)
        {
            // クリア済み表示の星を出す
        }
        // まだクリアしていなくて、１つ前にステージがあるものなら
        else if (this.stageId > 1)
        {
            key = DataEntryDef.SaveKind.Stage.ToString() + ( this.stageId - 1);
            clearFlag = Save.GetData(key + DataEntryDef.SaveKind.IsClear.ToString(), false);
            // １つ前のステージも未クリアなら選択不可にする
            if (!clearFlag)
            {
                Button button = this.GetComponent<Button>();
                button.interactable = false;
            }
        }

        // ボタン設定
        this.SetButton(this.stageId);
    }
}
