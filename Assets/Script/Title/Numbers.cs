/**
 * 数値群クラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

namespace FunatoLib
{
    public class Numbers : NumbersBase
    {
        /// <summary>
        /// 登録種別列挙。他のワークから引き出せるように用途によって種別を変更する。
        /// </summary>
        public enum NumbersKind
        {
            TimeLimit,
            ItemGet
        };

        /// <summary>
        /// 種別
        /// </summary>
        [SerializeField]
        private Numbers.NumbersKind kind;

        /// <summary>
        /// 初期化
        /// </summary>
        override protected void Start()
        {
            // 基底の方で処理する前に色々とゲームに合わせて設定しておく

            // エディタ側で設定された登録種別を設定しておく
            this.entrykind = (int)this.kind;

            // テクスチャ内に書いてある文字列設定
            this.decodeStr = "0123456789";

            // テクスチャがある場所設定
            this.resPass = "Sprite/";

            // テクスチャ名設定
            this.resName = "num";

            // 数字１つ１つを出すためのプレハブへのパス設定
            this.prefabPass = "Prefab/NumSprite";

            base.Start();
        }
    }
}
