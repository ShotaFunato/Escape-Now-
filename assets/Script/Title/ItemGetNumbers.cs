/**
 * 取得アイテム数値群クラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;
using FunatoLib;

public class ItemGetNumbers : Numbers
{
    /// <summary>
    /// 初期値
    /// </summary>
    public static readonly int DefaultNum = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // データ管理に標準値を登録し、それを表示する
        this.dataBankKind = DataEntryDef.DataBankKind.ItemGet;
        DataBankController.Instance.Entry((int)this.dataBankKind, ItemGetNumbers.DefaultNum);
        this.SetDataBankNum();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        this.DataBankNumUpdate();
    }
}