/**
* マップチップ管理クラス
* @author Shota Funato
*/
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace FunatoLib
{
    public class MapChipController : SingletonMonoBehaviour<MapChipController>
    {
        public enum CsvFirstLineData
        {
            WDataNum,
            HDataNum,
            ChipSizeW,
            ChipSizeH,
            LayerNum,
            BitNum,

            DataMax
        };

        /// <summary>
        /// CSVの最初の行に書いてあるデータをいれる。ステージの縦横幅、マップチップサイズ、レイヤー数がある
        /// </summary>
        private int[] csvFirstDatas = new int[(int)CsvFirstLineData.DataMax];

        /// <summary>
        /// CSVでレイヤー構造になっているステージ構成データをいれる
        /// </summary>
        private List<List<int>> csvDatas = new List<List<int>>();

        /// <summary>
        /// ステージ構成データ取得
        /// </summary>
        /// <returns>CsvFirstLineDataを要素番号として取得する</returns>
        public int[] GetCsvFirstDatas()
        {
            return this.csvFirstDatas;
        }

        /// <summary>
        /// 読み込んだCsvデータ取得
        /// </summary>
        /// <returns></returns>
        public List<List<int>> GetCsvDatas()
        {
            return this.csvDatas;
        }

        /// <summary>
        /// マップ生成
        /// </summary>
        /// <param name="fileName">読み込むcsvファイル名</param>
        public void MapCreate(string fileName)
        {
            // csvデータを変換したものをいれるもの
            TextAsset csvFile;
            string[] dataList;

            // リソース内のCsvフォルダにある指定ファイルを読み込む
            csvFile = Resources.Load("Csv/" + fileName) as TextAsset;
            StringReader reader = new StringReader(csvFile.text);

            // 一番最初の行にあるデータはステージ構成に必要なデータなので、最初に取り出す
            string firstLine = reader.ReadLine();
            dataList = firstLine.Split(',');
            for (int i = 0; i < this.csvFirstDatas.Length; i++)
            {
                int tmp = 0;
                int.TryParse(dataList[i], out tmp);
                this.csvFirstDatas[i] = tmp;
            }

            // csvで定義されたレイヤー分、Listを増やす
            for (int i = 0; i < this.csvFirstDatas[(int)CsvFirstLineData.LayerNum]; i++)
            {
                this.csvDatas.Add(new List<int>());
            }

            // csvのレイヤーごとのデータをList内にいれる
            int layerNum = 0;
            int lineNum = 0;
            while (reader.Peek() > -1)
            {
                // 行のデータを数値データに変換して保持する
                string line = reader.ReadLine();
                dataList = line.Split(',');
                bool convFlag = false;
                int tmp = 0;
                for (int i = 0; i < dataList.Length; i++)
                {
                    if (dataList[i].Equals("")) continue;
                    int.TryParse(dataList[i], out tmp);
                    this.csvDatas[layerNum].Add(tmp);
                    convFlag = true;
                }

                if (!convFlag) continue;

                // 変換した行数を数える
                lineNum++;

                // １レイヤー分の行数を超えたら、入れる先を変更する
                if (lineNum >= this.csvFirstDatas[(int)CsvFirstLineData.HDataNum])
                {
                    layerNum++;
                    lineNum = 0;
                }
            }
        }
    }
}