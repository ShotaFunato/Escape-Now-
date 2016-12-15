/**
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
        Enemy enemy = col.gameObject.GetComponent<Enemy>();
        ItmCheese cheese = col.gameObject.GetComponent<ItmCheese>();

        // 敵に当たった
        if (enemy)
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

        MapController mapController = MapController.Instance;
        InputController inputController = InputController.Instance;

        // 現在位置をマップチップの二次元配列上添え字に変換して、最初の地点として登録する
        Vector2 nowWH = mapController.CalcMapChipHW(this.transform.localPosition);
        // 現時点の通過可能情報を取得
        MapController.PassageRoute passageRoute = mapController.GetPassageRoute(nowWH);
        // 値を変更
        Vector2 move = new Vector2(inputController.GetAxisHorizontal(), inputController.GetAxisVertical());
        if (((move.x < 0) && (!passageRoute.dir[(int)MapController.Dir.Left])) ||
            ((move.x > 0) && (!passageRoute.dir[(int)MapController.Dir.Right])))
        {
            move.x = 0;
        }
        if (((move.y > 0) && (!passageRoute.dir[(int)MapController.Dir.Up])) ||
            ((move.y < 0) && (!passageRoute.dir[(int)MapController.Dir.Down])))
        {
            move.y = 0;
        }
        if ((move.x != 0) && (move.y != 0))
        {
            move.y = 0;
        }
        move.y = -move.y;
        nowWH += move;
        this.transform.position = mapController.CalcMapChipPos(nowWH);
    }
}
