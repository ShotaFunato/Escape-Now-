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
    /// 初期化
    /// </summary>
    override protected void Start()
    {
        base.Start();

        CanvasRenderer renderer = this.GetComponent<CanvasRenderer>();
        GameObject prefab = null;
        GameObject obj = null;
        StageButton button = null;
        Vector2 pos = new Vector2();
        for ( int i = 0; i < 10; i++ )
        {
            prefab = Resources.Load("Prefab/StageButton") as GameObject;
            obj = Instantiate(prefab) as GameObject;
            obj.transform.parent = this.transform;
            button = obj.GetComponent<StageButton>();
            pos.x = 0 + i % 3 * 150;
            pos.y = 0 - Mathf.FloorToInt(i / 3) * 150;
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
