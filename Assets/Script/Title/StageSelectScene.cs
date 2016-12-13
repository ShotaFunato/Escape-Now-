/**
 * ステージ選択シーンクラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

using FunatoLib;

public class StageSelectScene : Work
{
    /// <summary>
    /// ステージ数
    /// </summary>
    static readonly int StageMaxNum = 6;

    /// <summary>
    /// ステージボタンを横軸に並べる個数
    /// </summary>
    static readonly int ButtonWNum = 2;

    /// <summary>
    /// ボタンのサイズ
    /// </summary>
    static readonly int ButtonSize = 256;

    /// <summary>
    /// ボタン間に空ける幅
    /// </summary>
    static readonly int ButtonSpace = 64;

    /// <summary>
    /// 初期化
    /// </summary>
    override protected void Start()
    {
        base.Start();

        CanvasRenderer renderer = this.GetComponent<CanvasRenderer>();
        GameObject prefab = null;
        GameObject obj = null;
        StageButton button = null;
        int wNum = StageSelectScene.ButtonWNum;
        int hNum = Mathf.CeilToInt(StageSelectScene.StageMaxNum / wNum);
        Vector2 startPos = new Vector2();
        startPos.x = -(((wNum - 1) * StageSelectScene.ButtonSize / 2) + ((wNum - 1) * StageSelectScene.ButtonSpace / 2));
        startPos.y = (((hNum - 1) * StageSelectScene.ButtonSize / 2) + ((hNum - 1) * StageSelectScene.ButtonSpace / 2));
        Vector2 pos = new Vector2();
        for (int i = 0; i < StageSelectScene.StageMaxNum; i++)
        {
            int w = i % wNum;
            int h = Mathf.FloorToInt(i / wNum);
            prefab = Resources.Load("Prefab/StageButton") as GameObject;
            obj = Instantiate(prefab) as GameObject;
            obj.transform.parent = this.transform;
            button = obj.GetComponent<StageButton>();
            pos = startPos;
            pos.x += w * StageSelectScene.ButtonSize + w * StageSelectScene.ButtonSpace;
            pos.y -= h * StageSelectScene.ButtonSize + h * StageSelectScene.ButtonSpace;
            button.SetButton(i, pos);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    override protected void Update()
    {
        base.Update();
    }
}
