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
        base.Start();

        // データ管理に登録されている値を表示する
        this.dataBankKind = this.printKind;
        this.SetDataBankNum();
    }
}