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
        /// 表示サイズ
        /// </summary>
        public float fontSize = 0.1f;

        /// <summary>
        /// 外部設定用
        /// </summary>
        [SerializeField]
        protected int defNumValue = 0;

        /// <summary>
        /// 1フレ前の数値
        /// </summary>
        protected int prevNumValue = 0;

        /// <summary>
        /// 管理に登録する際の種別
        /// </summary>
        protected int entrykind;

        /// <summary>
        /// 指定の文字でスプライトフォントを作成する
        /// </summary>
        protected void SetNum()
        {
            NumbersController numController = NumbersController.Instance;
            int tmpInt = 0;
            numController.GetNumbers(ref tmpInt, this.entrykind);
            string tmpStr = tmpInt.ToString();
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

        /// <summary>
        /// 初期化
        /// </summary>
        protected virtual void Start()
        {
            // 起動時に既に登録されているかチェック。されていないなら新規登録
            NumbersController numController = NumbersController.Instance;
            int tmpInt = 0;
            if (!numController.GetNumbers(ref tmpInt, this.entrykind))
            {
                numController.Entry(this.entrykind, this.defNumValue);
            }

            this.SetNum();

            this.prevNumValue = this.defNumValue;
        }

        /// <summary>
        /// 更新
        /// </summary>
        protected virtual void Update()
        {
            NumbersController numController = NumbersController.Instance;
            int tmpNum = this.prevNumValue;

            if (numController.GetNumbers(ref tmpNum, this.entrykind))
            {
                if (this.prevNumValue != tmpNum)
                {
                    numController.Entry(this.entrykind, tmpNum);
                    this.SetNum();
                }
            }
            this.prevNumValue = tmpNum;
        }
    }
}
