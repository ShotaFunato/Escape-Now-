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
    protected override void Start()
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
        DataBankController dataBankController = DataBankController.Instance;
        int stageId = 0;
        int stageClear = 0;
        int timeLimit = 0;
        int itemGetNum = 0;
        
        dataBankController.GetNumber(ref stageId, (int)DataEntryDef.DataBankKind.SelectStageId);
        dataBankController.GetNumber(ref stageClear, (int)DataEntryDef.DataBankKind.StageClear);
        dataBankController.GetNumber(ref timeLimit, (int)DataEntryDef.DataBankKind.TimeLimit);
        dataBankController.GetNumber(ref itemGetNum, (int)DataEntryDef.DataBankKind.ItemGet);

        if (stageClear != 0)
        {
            string key = DataEntryDef.SaveKind.Stage.ToString() + stageId;
            Save.DataEntry(key + DataEntryDef.SaveKind.IsClear.ToString(), true);

            int saveClearTime = Save.GetData(key + DataEntryDef.SaveKind.ClearTime.ToString(), TimeLimitNumbers.DefaultTime);
            int saveItemGetNum = Save.GetData(key + DataEntryDef.SaveKind.ItemGet.ToString(), ItemGetNumbers.DefaultNum);

            int clearTime = TimeLimitNumbers.DefaultTime - timeLimit;
            if (clearTime < saveClearTime)
            {
                Save.DataEntry(key + DataEntryDef.SaveKind.ClearTime.ToString(), clearTime);
            }
            if (itemGetNum > saveItemGetNum)
            {
                Save.DataEntry(key + DataEntryDef.SaveKind.ItemGet.ToString(), itemGetNum);
            }

            // 登録したデータをセーブする
            Save.DataSave();
        }
    }
}
