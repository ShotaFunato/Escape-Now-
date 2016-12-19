/**
 * プレイヤークラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FunatoLib;

public class Player : Work
{
    /// <summary>
    /// ルーチン１番号の状態列挙
    /// </summary>
    private enum StateRoutine1
    {
        StateStay,
        StateRouteSelect,
        StateMove,
        StateMoveWait,

        StateMax
    };

    /// <summary>
    /// 移動待機時間
    /// </summary>
    [SerializeField]
    private float moveWaitTime = 1.0f;

    /// <summary>
    /// 状態
    /// </summary>
    private Player.StateRoutine1 stateRoutine1;

    /// <summary>
    /// ルーチン１番の関数テーブル
    /// </summary>
    private delegate void Routine1Func();
    List<Routine1Func> routineFuncList = new List<Routine1Func>();

    /// <summary>
    /// ルートを構成する地点リスト
    /// </summary>
    List<Vector2> routeWHList = new List<Vector2>();

    /// <summary>
    /// 時間
    /// </summary>
    private float time;

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

            // 死亡したのでリザルトへ
            SceneController.Instance.LoadScene(SceneController.SceneId.ResultScene);
        }
        // アイテムのチーズに当たった
        else if (cheese)
        {
            // チーズ取得数を増やす
            DataBankController dataBankController = DataBankController.Instance;
            int nowItemGetNum = 0;
            dataBankController.GetNumber(ref nowItemGetNum, (int)DataEntryDef.DataBankKind.ItemGet);
            nowItemGetNum++;
            dataBankController.Entry((int)DataEntryDef.DataBankKind.ItemGet, nowItemGetNum);
        }
    }

    /// <summary>
    /// 待機
    /// </summary>
    private void Stay()
    {
        MapController mapController = MapController.Instance;
        InputController inputController = InputController.Instance;

        // タッチしているか
        InputController.TouchInfo info = inputController.GetTouchInfo();
        if (info != InputController.TouchInfo.None)
        {
            // 現在位置をタッチしたか判定
            Vector2 nowWH = mapController.CalcMapChipHW(this.transform.localPosition);
            Vector2 selectWH = new Vector2();
            MapController.CsvCode code = mapController.ChangePosToChipWH(ref selectWH, MapController.MapLayerKind.Map, inputController.GetTouchPosition());
            if ((code != MapController.CsvCode.None) && (nowWH.x == selectWH.x) && (nowWH.y == selectWH.y))
            {
                // 現在位置を最初の地点として登録
                this.routeWHList.Add(nowWH);

                // ここからルートを繋げる状態に遷移
                this.stateRoutine1 = StateRoutine1.StateRouteSelect;
            }
        }
    }

    /// <summary>
    /// ルート選択
    /// </summary>
    private void RouteSelect()
    {
        MapController mapController = MapController.Instance;
        InputController inputController = InputController.Instance;

        InputController.TouchInfo info = inputController.GetTouchInfo();
        // タッチが離れた or タッチしていない
        if ((info == InputController.TouchInfo.None) || (info == InputController.TouchInfo.End))
        {
            if (this.routeWHList.Count <= 1)
            {
                this.routeWHList.Clear();
                this.stateRoutine1 = StateRoutine1.StateStay;
            }
            else
            {
                this.stateRoutine1 = StateRoutine1.StateMove;
            }
        }
        // タッチしている
        else
        {
            int prevId = ((this.routeWHList.Count <= 2) ? 0 : this.routeWHList.Count - 2);
            Vector2 prevWH = this.routeWHList[prevId];
            Vector2 selectWH = new Vector2();
            MapController.CsvCode code = mapController.ChangePosToChipWH(ref selectWH, MapController.MapLayerKind.Map, inputController.GetTouchPosition());
            if ((code != MapController.CsvCode.None) && (prevWH != selectWH))
            {
                // 現在位置で通過できるルートを取得
                int nowId = ((this.routeWHList.Count <= 1) ? 0 : this.routeWHList.Count - 1);
                Vector2 nowWH = this.routeWHList[nowId];
                MapController.PassageRoute passageRoute = mapController.GetPassageRoute(nowWH);
                // 通過できるルートのいずれかを選択していたら登録する
                for (int i = 0; i < (int)MapController.Dir.Max; i++)
                {
                    Vector2 moveWH = nowWH + MapController.RouteChkTbl[i];
                    if ((passageRoute.dir[i]) && (selectWH == moveWH))
                    {
                        this.routeWHList.Add(moveWH);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        MapController mapController = MapController.Instance;

        Vector2 tmp = new Vector2();
        MapController.CsvCode code = mapController.ChangePosToChipWH(ref tmp, MapController.MapLayerKind.Map, this.transform.localPosition);
        if (code == MapController.CsvCode.Goal)
        {
            DataBankController.Instance.Entry((int)DataEntryDef.DataBankKind.StageClear, 1);
            SceneController.Instance.LoadScene(SceneController.SceneId.ResultScene);
            return;
        }

        if (this.routeWHList.Count == 0)
        {
            this.stateRoutine1 = StateRoutine1.StateStay;
        }
        else
        {
            this.transform.localPosition = mapController.CalcMapChipPos(this.routeWHList[0]);
            this.routeWHList.Remove(this.routeWHList[0]);
            this.stateRoutine1 = StateRoutine1.StateMoveWait;
        }
    }

    /// <summary>
    /// 移動後待機時間
    /// </summary>
    private void MoveWait()
    {
        this.time += Time.deltaTime;
        if (this.time >= this.moveWaitTime)
        {
            this.stateRoutine1 = StateRoutine1.StateMove;
            this.time = 0;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // 関数テーブルに呼び出す関数を登録する
        this.routineFuncList.Add(this.Stay);
        this.routineFuncList.Add(this.RouteSelect);
        this.routineFuncList.Add(this.Move);
        this.routineFuncList.Add(this.MoveWait);
        this.stateRoutine1 = StateRoutine1.StateStay;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void Update()
    {
        base.Update();

        this.routineFuncList[(int)this.stateRoutine1]();
    }
}
