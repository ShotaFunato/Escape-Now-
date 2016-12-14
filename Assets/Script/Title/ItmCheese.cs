/**
 * アイテム「チーズ」クラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

using FunatoLib;

public class ItmCheese : Work
{
    /// <summary>
    /// 衝突した
    /// </summary>
    /// <param name="col">衝突情報</param>
    void OnTriggerEnter2D(Collider2D col)
    {
        // 衝突相手はプレイヤーだけにしているので、自分を削除する
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }
}
