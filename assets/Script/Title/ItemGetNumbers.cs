/**
 * 取得アイテム数値群クラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

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
        this.defNumValue = ItemGetNumbers.DefaultNum;

        base.Start();
    }
}