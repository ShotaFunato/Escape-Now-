/**
* 制限時間クラス
* @author Shota Funato
*/

using UnityEngine;
using FunatoLib;

public class TimeLimitNumbers : Numbers
{
    /// <summary>
    /// 初期値
    /// </summary>
    public static readonly int DefaultTime = 60;

    /// <summary>
    /// 制限時間
    /// </summary>
    private float timeLimit;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // データ管理に標準値を登録し、それを表示する
        this.dataBankKind = DataEntryDef.DataBankKind.TimeLimit;
        DataBankController.Instance.Entry((int)this.dataBankKind, TimeLimitNumbers.DefaultTime);
        this.SetDataBankNum();

        this.timeLimit = TimeLimitNumbers.DefaultTime;
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 基底の方で処理する前に計算する
        this.timeLimit -= Time.deltaTime;
        this.timeLimit = ((this.timeLimit < 0) ? 0 : this.timeLimit);

        int tmpNum = (int)Mathf.Ceil(this.timeLimit);
        DataBankController.Instance.Entry((int)this.dataBankKind, tmpNum);

        this.DataBankNumUpdate();
    }
}