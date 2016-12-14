/**
* 制限時間クラス
* @author Shota Funato
*/
using UnityEngine;
using FunatoLib;

public class TimeLimitNumbers : Numbers
{
    /// <summary>
    /// 制限時間
    /// </summary>
    private float timeLimit;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        // 基底の方で処理する前に色々とゲームに合わせて設定しておく

        this.timeLimit = this.defNumValue;

        base.Start();
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void Update()
    {
        // 基底の方で処理する前に計算する

        this.timeLimit -= Time.deltaTime;
        this.timeLimit = ((this.timeLimit < 0) ? 0 : this.timeLimit);

        DataBankController dataBankController = DataBankController.Instance;
        int tmpNum = (int)Mathf.Ceil(this.timeLimit);
        dataBankController.Entry(this.entrykind, tmpNum);

        base.Update();
    }
}