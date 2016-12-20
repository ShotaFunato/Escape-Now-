/**
* 表示数値群クラス
* @author Shota Funato
*/

using UnityEngine;

public class PrintNumbers : Numbers
{
    /// <summary>
    /// 表示サイズ
    /// </summary>
    [SerializeField]
    private DataEntryDef.DataBankKind printKind;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        // 基底の方で処理する前に色々とゲームに合わせて設定しておく
        this.kind = this.printKind;

        base.Start();
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void Update()
    {
    }
}