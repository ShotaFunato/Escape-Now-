/**
 * ステージボタンクラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FunatoLib;

public class StageButton : Work
{
    /// <summary>
    /// 選択された時に呼ばれる
    /// </summary>
    public void OnClick(int num)
    {
        DataBankController.Instance.Entry((int)DataEntryDef.NumbersKind.SelectStageId, num + 1);
        SceneController.Instance.LoadScene(SceneController.SceneId.StageScene);
    }

    /// <summary>
    /// ボタンに選択時機能を付与する
    /// </summary>
    /// <param name="num">ボタンの識別番号</param>
    private void AddButtonEvent(int num)
    {
        Button button = this.GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            this.OnClick(num);
        });
    }

    /// <summary>
    /// ボタン設定
    /// </summary>
    /// <param name="num">ボタンの識別番号</param>
    /// <param name="pos">表示座標</param>
    public void SetButton(int num, Vector2 pos)
    {
        this.transform.localPosition = pos;
        this.AddButtonEvent(num);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    override protected void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 更新
    /// </summary>
    override protected void Update()
    {
        base.Update();
    }
}
