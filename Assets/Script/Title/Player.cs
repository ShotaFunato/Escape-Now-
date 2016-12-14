﻿/**
 * プレイヤークラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

using FunatoLib;

public class Player : Work
{
    /// <summary>
    /// 衝突した
    /// </summary>
    /// <param name="col">衝突情報</param>
    void OnTriggerEnter2D(Collider2D col)
    {
        // 当たっているものを探す
        EmCat cat = col.gameObject.GetComponent<EmCat>();
        EmDog dog = col.gameObject.GetComponent<EmDog>();
        ItmCheese cheese = col.gameObject.GetComponent<ItmCheese>();

        // 敵の猫、犬に当たった
        if ((cat) || (dog))
        {
            // 自分を削除する
            Destroy(this.gameObject);
        }
        // アイテムのチーズに当たった
        else if (cheese)
        {
            // チーズ取得数を増やす
            DataBankController dataBankController = DataBankController.Instance;
            int nowItemGetNum = 0;
            dataBankController.GetNumber(ref nowItemGetNum, (int)DataEntryDef.NumbersKind.ItemGet);
            nowItemGetNum++;
            dataBankController.Entry((int)DataEntryDef.NumbersKind.ItemGet, nowItemGetNum);
        }
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

        InputController inputController = InputController.Instance;

        // 一時変数に格納
        Vector3 pos = this.transform.position;
        // 値を変更
        pos.x += inputController.GetAxisHorizontal();
        pos.y += inputController.GetAxisVertical();
        // 代入する
        this.transform.position = pos;
    }
}
