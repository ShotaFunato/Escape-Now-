/**
 * ステージシーンクラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FunatoLib;

public class StageScene : Work
{
    /// <summary>
    /// マップオブジェクトを設定する
    /// </summary>
    /// <param name="obj">マップオブジェクト</param>
    /// <param name="w">横軸座標添え字</param>
    /// <param name="h">縦軸座標添え字</param>
    private void SetMapObj(GameObject obj, int w, int h)
    {
        MapController mapController = MapController.Instance;
        obj.transform.localPosition = mapController.CalcMapChipPos( w, h );
        obj.transform.localScale = new Vector3(MapController.MapChipScale, MapController.MapChipScale, 1);
    }

    /// <summary>
    /// マップ生成
    /// </summary>
    private void MapCreate()
    {
        DataBankController dataBankController = DataBankController.Instance;
        MapController mapController = MapController.Instance;

        int num = 0;
        dataBankController.GetNumber(ref num, (int)DataEntryDef.NumbersKind.SelectStageId);
        mapController.MapCreate("stage" + num);

        List<List<int>> mapLayerDatas = mapController.GetMapDatas();
        int HNum = mapController.GetMapHNum();
        int WNum = mapController.GetMapWNum();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/mapchip");

        for (int layer = 0; layer < mapLayerDatas.Count; layer++)
        {
            if (!MapController.MapLayerFlagTbl[layer]) continue;

            for (int h = 0; h < HNum; h++)
            {
                for (int w = 0; w < WNum; w++)
                {
                    MapController.CsvCode value = (MapController.CsvCode)mapLayerDatas[layer][h * WNum + w];
                    if ((value == MapController.CsvCode.None) || (value == MapController.CsvCode.AllWall))
                    {
                        continue;
                    }

                    GameObject prefab = null;
                    GameObject obj = null;
                    switch (value)
                    {
                        case MapController.CsvCode.Start:
                            prefab = Resources.Load("Prefab/Player") as GameObject;
                            obj = Instantiate(prefab) as GameObject;
                            break;
                        case MapController.CsvCode.EmCat:
                            prefab = Resources.Load("Prefab/EmCat") as GameObject;
                            obj = Instantiate(prefab) as GameObject;
                            break;
                        case MapController.CsvCode.EmDog:
                            prefab = Resources.Load("Prefab/EmDog") as GameObject;
                            obj = Instantiate(prefab) as GameObject;
                            break;
                        case MapController.CsvCode.ItmCheese:
                            prefab = Resources.Load("Prefab/ItmCheese") as GameObject;
                            obj = Instantiate(prefab) as GameObject;
                            break;
                        case MapController.CsvCode.GimickWeb:
                            break;
                        default:
                            break;
                    };

                    if (obj != null)
                    {
                        this.SetMapObj(obj, w, h);
                    }

                    if ((obj == null) || (value == MapController.CsvCode.Start))
                    {
                        prefab = Resources.Load("Prefab/MapChipSprite") as GameObject;
                        obj = Instantiate(prefab) as GameObject;
                        this.SetMapObj(obj, w, h);
                        MapChipSprite mapChipSc = obj.GetComponent<MapChipSprite>();
                        mapChipSc.SetData(sprites, value, layer);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // マップ生成
        this.MapCreate();
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void Update()
    {
        base.Update();

        InputController inputController = InputController.Instance;

        if (inputController.GetMouseLeftClick())
        {
            SceneController.Instance.LoadScene(SceneController.SceneId.StageScene);
        }
    }
}
