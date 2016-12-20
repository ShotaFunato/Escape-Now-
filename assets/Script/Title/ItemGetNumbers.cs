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
        // 基底の方で処理する前に色々とゲームに合わせて設定しておく
        this.kind = DataEntryDef.DataBankKind.ItemGet;

        // 標準値で登録
        DataBankController.Instance.Entry((int)this.kind, ItemGetNumbers.DefaultNum);

        base.Start();
    }
}