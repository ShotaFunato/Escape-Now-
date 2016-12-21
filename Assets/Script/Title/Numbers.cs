/**
 * 数値群クラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;
using FunatoLib;

public class Numbers : NumbersBase
{
    /// <summary>
    /// DataBankControllerに登録する種別
    /// </summary>
    protected DataEntryDef.DataBankKind dataBankKind;

    /// <summary>
    /// 1フレ前の数値
    /// </summary>
    protected int prevNumValue = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    protected virtual void Start()
    {
        // テクスチャ内に書いてある文字列設定
        this.decodeStr = "0123456789";

        // テクスチャがある場所設定
        this.resPass = "Sprite/";

        // テクスチャ名設定
        this.resName = "num";

        // 数字１つ１つを出すためのプレハブへのパス設定
        this.prefabPass = "Prefab/NumSprite";
    }

    /// <summary>
    /// DataBankControllerに登録されたもので値初期化
    /// </summary>
    protected virtual void SetDataBankNum()
    {
        // 数値スプライト設定
        int tmpInt = 0;
        DataBankController.Instance.GetNumber(ref tmpInt, (int)this.dataBankKind);
        this.SetNum(tmpInt);

        // 更新されたかを確認するため、値保持
        this.prevNumValue = tmpInt;
    }

    /// <summary>
    /// DataBankControllerに登録されたもので値更新
    /// </summary>
    protected virtual void DataBankNumUpdate()
    {
        DataBankController dataBankController = DataBankController.Instance;
        int tmpNum = this.prevNumValue;

        if (dataBankController.GetNumber(ref tmpNum, (int)this.dataBankKind))
        {
            if (this.prevNumValue != tmpNum)
            {
                this.SetNum(tmpNum);
                this.prevNumValue = tmpNum;
            }
        }
    }
}