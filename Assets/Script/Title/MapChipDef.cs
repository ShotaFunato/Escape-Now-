/**
* マップチップ関連定義ファイル
* @author Shota Funato
*/

using System;

namespace MapChipDef
{
    /// <summary>
    /// マップレイヤーの種別
    /// </summary>
    public enum MapLayerKind
    {
        Route,
        Obj,
        Wall,
        Map,

        LayerMax
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
}