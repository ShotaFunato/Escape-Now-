/**
 * 敵クラス
 * @author Shota Funato
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FunatoLib;

public class Enemy : Work
{
    /// <summary>
    /// ルーチン１番号の状態列挙
    /// </summary>
    private enum StateRoutine1
    {
        StateDefault,
        StateMove,

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
    private Enemy.StateRoutine1 stateRoutine1;

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
    /// 現在のルート地点リスト添え字
    /// </summary>
    private int nowRouteNum;

    /// <summary>
    /// ルートを進行させる方向（true = +, false = -）
    /// </summary>
    private bool routeDir;

    /// <summary>
    /// 向いている方向
    /// </summary>
    private MapController.Dir dir;

    /// <summary>
    /// 時間
    /// </summary>
    private float time;

    /// <summary>
    /// ルート決定
    /// </summary>
    private void SetRoute()
    {
        // マップ構成データ取得
        MapController mapController = MapController.Instance;
        List<int> mapLayerData = mapController.GetMapDatas()[(int)MapController.MapLayerKind.Route];

        // ルート判定するための値を最初に略しておく
        int RouteFirst = (int)MapController.CsvCode.RouteU;
        int RouteLast = (int)MapController.CsvCode.RouteR;
        int RouteReturn = (int)MapController.CsvCode.RouteReturn;

        // 現在位置をマップチップの二次元配列上添え字に変換して、最初の地点として登録する
        Vector2 nowWH = mapController.CalcMapChipHW(this.transform.localPosition);
        routeWHList.Add(nowWH);

        // マップチップ上のデータを取得するために、レイヤーデータ配列の添え字に変換。現在位置からのルート情報を取得
        int nowNum = mapController.CalcListWHConvNum(nowWH);
        MapController.CsvCode routeData = (MapController.CsvCode)mapLayerData[nowNum];

        // 現在位置が折り返し地点なら、そこから繋がる情報を探す
        if ((int)routeData == RouteReturn)
        {
            // 現時点の通過可能情報を取得
            MapController.PassageRoute passageRoute = mapController.GetPassageRoute(nowWH);
            int dirNum = RouteLast - RouteFirst;
            for (int i = 0; i <= dirNum; i++)
            {
                if (!passageRoute.dir[i]) continue;
                Vector2 chkChipNum = nowWH + MapController.RouteChkTbl[i];
                if ((chkChipNum.x < 0) || (chkChipNum.y < 0)) continue;

                int chkRouteCode = RouteFirst + i;
                if (mapLayerData[mapController.CalcListWHConvNum(chkChipNum)] == chkRouteCode)
                {
                    // 繋がる情報を探せたら、そこに続くように現在の方向を設定する（そのまま入れれば大丈夫）
                    routeData = (MapController.CsvCode)chkRouteCode;
                    break;
                }
            }
        }

        // マップデータの進行方向を自分の進行方向に変換する
        this.dir = (MapController.Dir)(routeData - RouteFirst);

        // 進行方向にマップチップを、元の地点 or 折り返し地点に到達するまでチェックし続け、進行ルートとして保持する
        int tmpDir = (int)this.dir;
        while (true)
        {
            // 最後に保存した地点の添え字算出
            int lastIdx = this.routeWHList.Count - 1;
            // 最新の地点から現在の進行方向によって、次の地点を算出し、保持する
            Vector2 nextChipNum = this.routeWHList[lastIdx] + MapController.RouteChkTbl[tmpDir];
            // 進み先の地点のルート情報を取得し、次のチェックのために保持しておく
            tmpDir = mapLayerData[mapController.CalcListWHConvNum(nextChipNum)] - RouteFirst;

            // 進み先の地点がルート終了なのかチェック
            // 元の地点かチェック
            bool firstFlag = (this.routeWHList[0] == nextChipNum);
            // 折り返し地点かチェック
            bool returnFlag = (tmpDir == RouteReturn - RouteFirst);
            // どちらの地点でもなく通常地点 or 折り返し地点なら
            if ((!firstFlag && !returnFlag) || (returnFlag))
            {
                this.routeWHList.Add(nextChipNum);
            }
            if (firstFlag || returnFlag)
            {
                break;
            }
        }
    }

    /// <summary>
    /// 待機
    /// </summary>
    private void Stay()
    {
        this.time += Time.deltaTime;
        if (this.time >= this.moveWaitTime)
        {
            this.stateRoutine1 = StateRoutine1.StateMove;
            this.time = 0;
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        MapController mapController = MapController.Instance;
        List<int> mapLayerData = mapController.GetMapDatas()[(int)MapController.MapLayerKind.Route];
        int minNum = 0;
        int maxNum = this.routeWHList.Count - 1;
        this.nowRouteNum += ((this.routeDir) ? 1 : -1);
        if (this.nowRouteNum < minNum)
        {
            this.nowRouteNum = maxNum;
        }
        if (this.nowRouteNum > maxNum)
        {
            this.nowRouteNum = minNum;
        }
        Vector2 wh = this.routeWHList[this.nowRouteNum];
        int num = mapController.CalcListWHConvNum(wh);
        MapController.CsvCode routeData = (MapController.CsvCode)mapLayerData[num];
        if (routeData == MapController.CsvCode.RouteReturn)
        {
            this.routeDir = !this.routeDir;
            this.dir += ( ((this.dir == MapController.Dir.Up) || (this.dir == MapController.Dir.Left) ? 1 : -1));
        }
        else
        {
            this.dir = (MapController.Dir)(routeData - MapController.CsvCode.RouteU);
        }
        this.transform.localPosition = mapController.CalcMapChipPos(wh);
        this.stateRoutine1 = StateRoutine1.StateDefault;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // 関数テーブルに呼び出す関数を登録する
        this.routineFuncList.Add(this.Stay);
        this.routineFuncList.Add(this.Move);
        this.stateRoutine1 = StateRoutine1.StateDefault;

        // 経過時間初期化
        this.time = 0.0f;
        // 現在の位置は開始位置
        this.nowRouteNum = 0;
        // 進行方向を登録された順番で進むように設定
        this.routeDir = true;

        // ルート設定
        this.SetRoute();
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
