/**
* セーブクラス
* @author Shota Funato
*/
using UnityEngine;
using System.Collections.Generic;

namespace FunatoLib
{
    public static class Save
    {
        /// <summary>
        /// データを指定キーでセーブ登録
        /// </summary>
        /// <typeparam name="T">int,float,stringのどれか</typeparam>
        /// <param name="key">セーブするデータと紐づける識別キー</param>
        /// <param name="data">セーブしたいデータ</param>
        public static void DataEntry<T>(string key, T data)
        {
            object obj = (object)data;
            if (typeof(T) == typeof(int))
            {
                PlayerPrefs.SetInt(key, (int)obj);
            }
            if (typeof(T) == typeof(float))
            {
                PlayerPrefs.SetFloat(key, (float)obj);
            }
            if (typeof(T) == typeof(string))
            {
                PlayerPrefs.SetString(key, (string)obj);
            }
        }

        /// <summary>
        /// セーブデータから指定キーのデータを取得
        /// </summary>
        /// <typeparam name="T">int,float,stringのどれか</typeparam>
        /// <param name="key">取得したいセーブデータと紐づけられている識別キー</param>
        /// <param name="defData">取得したいデータがない場合、この値で登録されてから取得値として扱われる</param>
        /// <returns>登録データ</returns>
        public static T GetData<T>(string key, T defData)
        {
            object defObj = (object)defData;
            object obj = null;
            if (typeof(T) == typeof(int))
            {
                obj = PlayerPrefs.GetInt(key, (int)defObj);
            }
            if (typeof(T) == typeof(float))
            {
                obj = PlayerPrefs.GetFloat(key, (float)defObj);
            }
            if (typeof(T) == typeof(string))
            {
                obj = PlayerPrefs.GetString(key, (string)defObj);
            }

            return (T)obj;
        }

        /// <summary>
        /// 登録したデータをセーブする。この処理を使わなくてもアプリケーションが終了された際にセーブされるが、エラーの場合は正常にセーブされていない可能性がある
        /// </summary>
        public static void DataSave()
        {
            PlayerPrefs.Save();
        }
    }
}