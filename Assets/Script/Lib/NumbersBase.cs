/**
 * 数値群基底クラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

namespace FunatoLib
{
    public abstract class NumbersBase : MonoBehaviour
    {
        /// <summary>
        /// 表示サイズ
        /// </summary>
        [SerializeField]
        protected float fontSize = 0.1f;

        /// <summary>
        /// テクスチャデータ化している文字列
        /// </summary>
        protected string decodeStr;

        /// <summary>
        /// リソースへのパス
        /// </summary>
        protected string resPass;

        /// <summary>
        /// リソース名
        /// </summary>
        protected string resName;

        /// <summary>
        /// NumSpriteのプレハブへのパス
        /// </summary>
        protected string prefabPass;

        /// <summary>
        /// 指定数値でスプライトフォントを作成する
        /// </summary>
        public void SetNum(int num)
        {
            string tmpStr = num.ToString();
            int i = 0;

            foreach (char val in tmpStr)
            {
                GameObject obj = null;
                if (i < this.transform.childCount)
                {
                    // 作成済みであればそれを使う
                    obj = this.transform.GetChild(i).gameObject;
                }
                else
                {
                    // NumSpriteをプレハブから取得
                    GameObject prefab = Resources.Load(this.prefabPass) as GameObject;
                    obj = Instantiate(prefab) as GameObject;
                    // 子に設定する
                    obj.transform.parent = this.transform;
                    obj.transform.localScale = new Vector3(1, 1, 1);
                }

                // 文字を対応するスプライト番号に変換する
                int idx = this.decodeStr.IndexOf(val);

                // SpriteCharを取得してスプライトを変更する
                NumSprite sc = obj.GetComponent<NumSprite>();
                sc.SetNum(this.resPass, this.resName, idx);
                i++;
            }
            for (int id = i; id < this.transform.childCount; id++)
            {
                GameObject obj = this.transform.GetChild(id).gameObject;
                if (obj.activeInHierarchy)
                {
                    obj.SetActive(false);
                }
            }
            float tmpX = 0.0f;
            for (int id = this.transform.childCount - 1; id >= 0; id--)
            {
                GameObject obj = this.transform.GetChild(id).gameObject;
                if (obj.activeInHierarchy)
                {
                    Vector3 pos = new Vector3(tmpX, 0, 0);
                    obj.transform.localPosition = pos;
                    tmpX -= this.transform.localScale.x * this.fontSize;
                }
            }
        }
    }
}
