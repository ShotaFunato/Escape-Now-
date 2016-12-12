/**
 * 数字スプライトクラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

namespace FunatoLib
{
    public class NumSprite : MonoBehaviour
    {
        // 指定のスプライト番号でスプライトを動的に変更する
        public void SetNum(string resPath, string resName, int idx)
        {
            string name = resName + "_" + idx;
            SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
            Sprite[] sprites = Resources.LoadAll<Sprite>(resPath + resName);
            Sprite sp = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(name));
            sr.sprite = sp;

            // 非表示設定になっている場合があるので、表示扱いにする
            this.gameObject.SetActive(true);
        }
    }
}
