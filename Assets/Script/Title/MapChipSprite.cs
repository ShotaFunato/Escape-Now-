/**
* マップチップスプライトクラス
* @author Shota Funato
*/

using UnityEngine;
using System;

public class MapChipSprite : MonoBehaviour
{
    private static readonly string MapChipName = "mapchip_";

    // 指定のスプライト番号でスプライトを動的に変更する
    public void SetData(Sprite[] sprites, MapController.CsvCode code, int layer)
    {
        int spriteIdx = -1;
        for (int i = 0; i <= (int)code; i++)
        {
            if (Enum.IsDefined(typeof(MapController.CsvCode), (MapController.CsvCode)i))
            {
                spriteIdx++;
            }
        }

        if (spriteIdx == -1) return;

        string name = MapChipSprite.MapChipName + spriteIdx;
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        Sprite sp = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(name));
        sr.sprite = sp;
        sr.sortingOrder = layer;
    }
}