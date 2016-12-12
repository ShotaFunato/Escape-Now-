/**
* 数値管理クラス
* @author Shota Funato
*/
using UnityEngine;
using System.Collections.Generic;

namespace FunatoLib
{
    public class NumbersController : SingletonMonoBehaviour<NumbersController>
    {
        /// <summary>
        /// 他のオブジェクトに数値を渡したり、シーンをまたいで引き継ぐための登録先
        /// </summary>
        private Dictionary<int, int> numbersDictionary = new Dictionary<int, int>();

        /// <summary>
        /// データ削除
        /// </summary>
        public void DataClear()
        {
            this.numbersDictionary.Clear();
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="id">数値に対応させる文字</param>
        /// <param name="num">数値</param>
        public void Entry(int id, int num)
        {
            this.numbersDictionary[id] = num;
        }

        /// <summary>
        /// 数値取得
        /// </summary>
        /// <param name="num">取得した数値をいれる</param>
        /// <param name="id">引き出したい数値に対応した文字</param>
        /// <returns>取得の成否</returns>
        public bool GetNumbers(ref int num, int id)
        {
            if (this.numbersDictionary.ContainsKey(id))
            {
                num = this.numbersDictionary[id];
                return true;
            }

            return false;
        }
    }
}