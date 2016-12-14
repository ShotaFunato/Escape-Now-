/**
 * 敵「犬」クラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

using FunatoLib;

public class EmDog : Work
{
    /// <summary>
    /// 状態列挙
    /// </summary>
    enum StateRoutine1
    {
        StateDefault,
        StateBarK,

        StateMax
    };

    /// <summary>
    /// 状態
    /// </summary>
    EmDog.StateRoutine1 stateRoutine1;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        this.stateRoutine1 = StateRoutine1.StateDefault;
        Debug.Log(this.GetType().FullName + "   StateRoutine1:" + this.stateRoutine1);
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }
}
