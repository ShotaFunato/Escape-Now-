/**
* 引き継ぎデータ登録関連定義ファイル
* @author Shota Funato
*/

namespace DataEntryDef
{
    /// <summary>
    /// 登録種別列挙。他のワークから引き出せるように用途によって種別を変更する。
    /// </summary>
    public enum NumbersKind
    {
        SelectStageId,
        TimeLimit,
        ItemGet
    };
}