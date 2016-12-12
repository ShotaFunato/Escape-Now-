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
    /// オブジェクト化するレイヤーデータテーブル
    /// </summary>
    static readonly bool[] MapLayerFlagTbl = new bool[(int)MapChipDef.MapLayerKind.LayerMax]
    {
        true,
        true,
        true,
        false
    };

    /// <summary>
    ///  マップチップのスケール
    /// </summary>
    [SerializeField]
    private float MapChipScale = 1.5f;

    /// <summary>
    /// ステージ全体のY軸オフセット値
    /// </summary>
    [SerializeField]
    private float mapOffsetY;

    /// <summary>
    /// 生成マップチップのオフセット値
    /// </summary>
    [SerializeField]
    private float objOffset;

    /// <summary>
    /// マップオブジェクトを設定する
    /// </summary>
    /// <param name="obj">マップオブジェクト</param>
    /// <param name="w">横軸座標添え字</param>
    /// <param name="h">縦軸座標添え字</param>
    /// <param name="maxW">横軸に並んでいるオブジェクト数</param>
    /// <param name="maxH">縦軸に並んでいるオブジェクト数</param>
    private void setMapObj(GameObject obj, int w, int h, int maxW, int maxH)
    {
        GameObject cameraObj = GameObject.Find("Main Camera");
        Camera camera = cameraObj.GetComponent<Camera>();
        float chipOfs = this.objOffset * this.MapChipScale;

        float posX = camera.pixelWidth / 2 - chipOfs * (maxW + ((maxW % 2 == 0) ? -0.5f : 0)) / 2 + chipOfs * w;
        float posY = camera.pixelHeight / 2 + chipOfs * maxH / 2 + this.mapOffsetY + chipOfs * -h;
        Vector3 pos = new Vector3(posX, posY, 0);
        obj.transform.localPosition = pos;
        obj.transform.localScale = new Vector3(this.MapChipScale, this.MapChipScale, 1);
    }

    /// <summary>
    /// マップ生成
    /// </summary>
    private void MapCreate()
    {
        MapChipController mapController = MapChipController.Instance;
        GameObject cameraObj = GameObject.Find("Main Camera");
        Camera camera = cameraObj.GetComponent<Camera>();

        mapController.MapCreate("stage1");
        int[] firstDatas = mapController.GetCsvFirstDatas();
        List<List<int>> mapLayerDatas = mapController.GetCsvDatas();
        int HNum = firstDatas[(int)MapChipController.CsvFirstLineData.HDataNum];
        int WNum = firstDatas[(int)MapChipController.CsvFirstLineData.WDataNum];
        float chipOfs = this.objOffset * this.MapChipScale;
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/mapchip");

        for (int layer = 0; layer < mapLayerDatas.Count; layer++)
        {
            if (!StageScene.MapLayerFlagTbl[layer]) continue;

            for (int h = 0; h < HNum; h++)
            {
                for (int w = 0; w < WNum; w++)
                {
                    MapChipDef.CsvCode value = (MapChipDef.CsvCode)mapLayerDatas[layer][h * WNum + w];
                    if ((value == MapChipDef.CsvCode.None) || (value == MapChipDef.CsvCode.AllWall))
                    {
                        continue;
                    }

                    GameObject prefab = null;
                    GameObject obj = null;
                    switch (value)
                    {
                        case MapChipDef.CsvCode.Start:
                            prefab = Resources.Load("Prefab/Player") as GameObject;
                            obj = Instantiate(prefab) as GameObject;
                            break;
                        case MapChipDef.CsvCode.EmCat:
                            prefab = Resources.Load("Prefab/EmCat") as GameObject;
                            obj = Instantiate(prefab) as GameObject;
                            break;
                        case MapChipDef.CsvCode.EmDog:
                            prefab = Resources.Load("Prefab/EmDog") as GameObject;
                            obj = Instantiate(prefab) as GameObject;
                            break;
                        case MapChipDef.CsvCode.ItmCheese:
                            prefab = Resources.Load("Prefab/ItmCheese") as GameObject;
                            obj = Instantiate(prefab) as GameObject;
                            break;
                        case MapChipDef.CsvCode.GimickWeb:
                            break;
                        default:
                            break;
                    };

                    if (obj != null)
                    {
                        this.setMapObj(obj, w, h, WNum, HNum);
                    }

                    if ((obj == null) || (value == MapChipDef.CsvCode.Start))
                    {
                        prefab = Resources.Load("Prefab/MapChipSprite") as GameObject;
                        obj = Instantiate(prefab) as GameObject;
                        this.setMapObj(obj, w, h, WNum, HNum);
                        MapChipSprite mapChipSc = obj.GetComponent<MapChipSprite>();
                        mapChipSc.SetData(sprites,value,layer);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    override protected void Start()
    {
        base.Start();

        // マップ生成
        this.MapCreate();
    }

    /// <summary>
    /// 更新
    /// </summary>
    override protected void Update()
    {
        base.Update();

        InputController inputController = InputController.Instance;

        if (inputController.GetMouseLeftClick())
        {
            SceneController.Instance.LoadScene(SceneController.SceneId.StageScene);
        }
    }
}
