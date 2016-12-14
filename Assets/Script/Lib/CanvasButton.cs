/**
 * Canvasに登録するボタン基底クラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FunatoLib
{
    public abstract class CanvasButton : Work
    {
        /// <summary>
        /// 選択された時に呼ばれる
        /// </summary>
        /// <param name="num">識別番号により、どのボタンかを判別して処理を分岐させる</param>
        public abstract void OnClick(int num);

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
        public void SetButton(int num)
        {
            this.AddButtonEvent(num);
        }
    }
}
