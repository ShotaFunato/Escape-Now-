/**
* マップチップ管理クラス
* @author Shota Funato
*/
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using FunatoLib;

public class MapController : SingletonMonoBehaviour<MapController>
{
    /// <summary>
    /// マップレイヤーの種別
    /// </summary>
    public enum MapLayerKind
    {
        Map,
        Wall,
        Obj,
        Route,

        LayerMax
    };

    /// <summary>
    /// 構成データの識別番号
    /// </summary>
    private enum CsvFirstLineData
    {
        WDataNum,
        HDataNum,
        ChipSizeW,
        ChipSizeH,
        LayerNum,
        BitNum,

        DataMax
    };

    /// <summary>
    /// csv対応番号
    /// </summary>
    [Flags]
    public enum CsvCode
    {
        None = 0,
        AllWall,
        Land,
        Start,
        Goal,
        Warp1,
        Warp2,
        Warp3,
        EmCat,
        EmDog,

        ItmCheese = 16,
        GimickWeb,

        WallU = 32,
        WallR,
        WallL,
        WallD,
        WallLR,
        WallUD,
        WallUL,
        WallUR,
        WallDR,
        WallDL,
        WallULR,
        WallUDR,
        WallDLR,
        WallUDL,

        RouteU = 48,
        RouteD,
        RouteL,
        RouteR,
        RouteReturn,

        CodeEnd
    };

    /// <summary>
    /// 方向列挙
    /// </summary>
    public enum Dir
    {
        Up,
        Down,
        Left,
        Right,

        Max
    };

    /// <summary>
    /// 通過できる方向を定義する構造体
    /// </summary>
    public struct PassageRoute
    {
        public bool[] dir;

        public PassageRoute(bool u, bool d, bool l, bool r) : this()
        {
            dir = new bool[(int)MapController.Dir.Max];
            dir[(int)MapController.Dir.Up] = u;
            dir[(int)MapController.Dir.Down] = d;
            dir[(int)MapController.Dir.Left] = l;
            dir[(int)MapController.Dir.Right] = r;
        }
    };

    /// <summary>
    /// オブジェクト化するレイヤーデータテーブル
    /// </summary>
    public static readonly bool[] MapLayerFlagTbl = new bool[(int)MapController.MapLayerKind.LayerMax]
    {
        true,
        true,
        true,
        false
    };

    /// <summary>
    /// 壁があるマップから通過できる方向を定義したテーブル
    /// </summary>
    private static readonly PassageRoute[] PassageRouteTbl = new PassageRoute[(int)(MapController.CsvCode.WallUDL - MapController.CsvCode.WallU) + 2]
    {
        new PassageRoute( true, true, true, true ),
        new PassageRoute( false, true, true, true ),
        new PassageRoute( true, true, true, false ),
        new PassageRoute( true, true, false, true ),
        new PassageRoute( true, false, true, true ),
        new PassageRoute( true, true, false, false ),
        new PassageRoute( false, false, true, true ),
        new PassageRoute( false, true, false, true ),
        new PassageRoute( false, true, true, false ),
        new PassageRoute( true, false, true, false ),
        new PassageRoute( true, false, false, true ),
        new PassageRoute( false, true, false, false ),
        new PassageRoute( false, false, true, false ),
        new PassageRoute( true, false, false, false ),
        new PassageRoute( false, false, false, true ),
    };

    /// <summary>
    /// 移動方向によってチェックする二次元配列添え字テーブル
    /// </summary>
    public static readonly Vector2[] RouteChkTbl = new Vector2[(int)MapController.Dir.Max]
    {
        Vector2.up * -1,
        Vector2.down * -1,
        Vector2.left,
        Vector2.right,
    };

    /// <summary>
    /// マップチップの絵素材のサイズ
    /// </summary>
    public static readonly float MapChipPictSize = 64;

    /// <summary>
    ///  マップチップのスケール
    /// </summary>
    public static readonly float MapChipScale = 1.5f;

    /// <summary>
    /// マップチップの中心点割合
    /// </summary>
    public static readonly float MapChipPivot = 0.5f;

    /// <summary>
    /// マップチップのオブジェクトサイズ
    /// </summary>
    public static readonly float MapChipSize = MapController.MapChipPictSize * MapController.MapChipScale;

    /// <summary>
    /// CSVの最初の行に書いてあるデータをいれる。ステージの縦横幅、マップチップサイズ、レイヤー数がある
    /// </summary>
    private int[] csvFirstDatas = new int[(int)CsvFirstLineData.DataMax];

    /// <summary>
    /// CSVでレイヤー構造になっているステージ構成データをいれる
    /// </summary>
    private List<List<int>> mapDatas = new List<List<int>>();

    /// <summary>
    /// マップの左上座標と縦横の幅
    /// </summary>
    private Vector4 mapRange = new Vector4();

    /// <summary>
    /// ステージ構成データ取得
    /// </summary>
    /// <returns>CsvFirstLineDataを要素番号として取得する</returns>
    public int[] GetCsvFirstDatas()
    {
        return this.csvFirstDatas;
    }

    /// <summary>
    /// 読み込んだマップデータ取得
    /// </summary>
    /// <returns></returns>
    public List<List<int>> GetMapDatas()
    {
        return this.mapDatas;
    }

    /// <summary>
    /// マップチップの縦軸個数取得
    /// </summary>
    /// <returns></returns>
    public int GetMapHNum()
    {
        return this.csvFirstDatas[(int)MapController.CsvFirstLineData.HDataNum];
    }

    /// <summary>
    /// マップチップの横軸個数取得
    /// </summary>
    /// <returns></returns>
    public int GetMapWNum()
    {
        return this.csvFirstDatas[(int)MapController.CsvFirstLineData.WDataNum];
    }

    /// <summary>
    /// マップのレイヤー数取得
    /// </summary>
    /// <returns></returns>
    public int GetMapLayerNum()
    {
        return this.csvFirstDatas[(int)MapController.CsvFirstLineData.LayerNum];
    }

    /// <summary>
    /// 二次元配列添え字から一次元配列添え字に変更
    /// </summary>
    /// <param name="w">横軸添え字</param>
    /// <param name="h">縦軸添え字</param>
    /// <returns>変換した添え字</returns>
    public int CalcListWHConvNum(int w, int h)
    {
        return h * this.GetMapWNum() + w;
    }

    /// <summary>
    /// 二次元配列添え字から一次元配列添え字に変更
    /// </summary>
    /// <param name="wh">縦横軸添え字</param>
    /// <returns>変換した添え字</returns>
    public int CalcListWHConvNum(Vector2 wh)
    {
        return this.CalcListWHConvNum((int)wh.x, (int)wh.y);
    }

    /// <summary>
    /// 一次元配列添え字から二次元配列添え字に変更
    /// </summary>
    /// <param name="w">横軸添え字</param>
    /// <param name="h">縦軸添え字</param>
    /// <returns>変換した添え字</returns>
    public Vector2 CalcListNumConvWH(int num)
    {
        return new Vector2(Mathf.FloorToInt(num / this.GetMapWNum()), num % this.GetMapWNum());
    }

    /// <summary>
    /// 指定位置の通過可能方向取得
    /// </summary>
    /// <param name="wh"></param>
    /// <returns></returns>
    public PassageRoute GetPassageRoute(Vector2 wh)
    {
        int firstWallCode = (int)MapController.CsvCode.WallU;
        int num = this.CalcListWHConvNum(wh);
        num = this.mapDatas[(int)MapController.MapLayerKind.Wall][num];
        if (num < firstWallCode)
        {
            return MapController.PassageRouteTbl[0];
        }

        return MapController.PassageRouteTbl[num - firstWallCode + 1];
    }

    /// <summary>
    /// マップチップを並べて設置していく開始位置取得
    /// </summary>
    /// <returns>開始左上座標</returns>
    public Vector2 GetMapChipStartPos()
    {
        Vector2 pos = new Vector2(this.mapRange.x, this.mapRange.y);
        float chipPivotOfs = MapController.MapChipSize * MapController.MapChipPivot;
        pos.x += chipPivotOfs;
        pos.y -= chipPivotOfs;
        return pos;
    }
    
    /// <summary>
    /// 指定位置のマップチップ座標を算出して取得
    /// </summary>
    /// <param name="w">横軸番号</param>
    /// <param name="h">縦軸番号</param>
    /// <returns>マップチップ設置座標</returns>
    public Vector2 CalcMapChipPos(int w, int h)
    {
        Vector2 pos = this.GetMapChipStartPos();
        float chipOfs = MapController.MapChipSize;
        pos.x += chipOfs * w;
        pos.y -= chipOfs * h;

        return pos;
    }

    /// <summary>
    /// 指定位置のマップチップ座標を算出して取得
    /// </summary>
    /// <param name="w">縦横軸番号</param>
    /// <returns>マップチップ設置座標</returns>
    public Vector2 CalcMapChipPos(Vector2 wh)
    {
        Vector2 pos = this.GetMapChipStartPos();
        float chipOfs = MapController.MapChipSize;
        pos.x += chipOfs * wh.x;
        pos.y -= chipOfs * wh.y;

        return pos;
    }

    /// <summary>
    /// マップチップ座標からマップチップ縦横軸番号算出して取得
    /// </summary>
    /// <returns>マップチップの縦横軸番号</returns>
    public Vector2 CalcMapChipHW(Vector2 pos)
    {
        Vector2 mapChipHW = new Vector2();
        Vector2 start_pos = this.GetMapChipStartPos();
        float chipOfs = MapController.MapChipSize;
        pos -= start_pos;
        mapChipHW = pos / MapController.MapChipSize;
        mapChipHW.y = -mapChipHW.y;

        return mapChipHW;
    }

    /// <summary>
    /// 指定レイヤーに対して、指定した座標で選択したマップデータ取得
    /// </summary>
    /// <param name="wh">選択したマップチップ番号</param>
    /// <param name="chkPos">指定座標</param>
    /// <returns>指定座標でマップデータを選択できたらデータを返し、選択できなかった場合はNoneを返す</returns>
    public MapController.CsvCode ChangePosToChipWH(ref Vector2 wh, MapController.MapLayerKind layer, Vector2 chkPos)
    {
        int px = (int)chkPos.x;
        int py = (int)chkPos.y;
        int ax = (int)this.mapRange.x;
        int ay = (int)this.mapRange.y;
        int bx = ax + (int)this.mapRange.w;
        int by = ay + (int)this.mapRange.z;
        if ((px >= ax) && (px <= bx) && (py <= ay) && (py >= by))
        {
            wh.x = Mathf.FloorToInt(Math.Abs(px - ax) / MapController.MapChipSize );
            wh.y = Mathf.FloorToInt(Math.Abs(py - ay) / MapController.MapChipSize );
            int num = this.CalcListWHConvNum(wh);
            if (num >= this.mapDatas[(int)layer].Count)
            {
                return MapController.CsvCode.None;
            }
            return (MapController.CsvCode)this.mapDatas[(int)layer][num];
        }

        return MapController.CsvCode.None;
    }

    /// <summary>
    /// マップ生成
    /// </summary>
    /// <param name="fileName">読み込むcsvファイル名</param>
    public void MapCreate(string fileName)
    {
        // csvデータを変換したものをいれるもの
        TextAsset csvFile;
        string[] dataList;

        // データ削除
        this.mapDatas.Clear();

        // リソース内のCsvフォルダにある指定ファイルを読み込む
        csvFile = Resources.Load("Csv/" + fileName) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        // 一番最初の行にあるデータはステージ構成に必要なデータなので、最初に取り出す
        string firstLine = reader.ReadLine();
        dataList = firstLine.Split(',');
        for (int i = 0; i < this.csvFirstDatas.Length; i++)
        {
            int tmp = 0;
            int.TryParse(dataList[i], out tmp);
            this.csvFirstDatas[i] = tmp;
        }

        // csvで定義されたレイヤー分、Listを増やす
        for (int i = 0; i < this.csvFirstDatas[(int)CsvFirstLineData.LayerNum]; i++)
        {
            this.mapDatas.Add(new List<int>());
        }

        // csvのレイヤーごとのデータをList内にいれる
        int layerNum = 0;
        int lineNum = 0;
        while (reader.Peek() > -1)
        {
            // 行のデータを数値データに変換して保持する
            string line = reader.ReadLine();
            dataList = line.Split(',');
            bool convFlag = false;
            int tmp = 0;
            for (int i = 0; i < dataList.Length; i++)
            {
                if (dataList[i].Equals("")) continue;
                int.TryParse(dataList[i], out tmp);
                this.mapDatas[layerNum].Add(tmp);
                convFlag = true;
            }

            if (!convFlag) continue;

            // 変換した行数を数える
            lineNum++;

            // １レイヤー分の行数を超えたら、入れる先を変更する
            if (lineNum >= this.csvFirstDatas[(int)CsvFirstLineData.HDataNum])
            {
                layerNum++;
                lineNum = 0;
            }
        }

        GameObject cameraObj = GameObject.Find("Main Camera");
        Camera camera = cameraObj.GetComponent<Camera>();
        int maxH = this.GetMapHNum();
        int maxW = this.GetMapWNum();
        float chipSize = MapController.MapChipSize;
        float chipPivotOfs = chipSize * MapController.MapChipPivot;
        this.mapRange.x = camera.pixelWidth / 2 - chipSize * (maxW - 1) / 2 - chipPivotOfs;
        this.mapRange.y = camera.pixelHeight / 2 + chipSize * (maxH - 1) / 2 + chipPivotOfs;
        this.mapRange.w = MapController.MapChipSize * maxW;
        this.mapRange.z = MapController.MapChipSize * -maxH;
    }
}