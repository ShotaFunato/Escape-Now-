/**
* シーンやオブジェクトをまたいで利用するデータ登録関連定義ファイル
* @author Shota Funato
*/

namespace DataEntryDef
{
    /// <summary>
    /// 登録種別列挙。他のワークから引き出せるように用途によって種別を変更する。
    /// </summary>
    public enum DataBankKind
    {
        SelectStageId,
        TimeLimit,
        ItemGet,
        StageClear,
    };

    /// <summary>
    /// セーブデータ保存時の登録キー
    /// </summary>
    public enum SaveKind
    {
        // Stage + ステージ数 + データ別キーで登録する時のStringにする
        Stage,

        ClearTime,
        ItemGet,
        IsClear,
    };
}