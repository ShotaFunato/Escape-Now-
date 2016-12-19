/**
 * 入力ラッパークラス
 * @author Shota Funato
 */

using UnityEngine;

namespace FunatoLib
{
    public class InputController : SingletonMonoBehaviour<InputController>
    {
        public enum TouchInfo
        {
            None,
            Began,
            Now,
            End,
        };

        /// <summary>
        /// 左右入力取得
        /// </summary>
        /// <returns>-1.0 = 左、1.0 = 右</returns>
        public float GetAxisHorizontal()
        {
            return Input.GetAxis("Horizontal");
        }

        /// <summary>
        /// 上下入力取得
        /// </summary>
        /// <returns>-1.0 = 上、1.0 = 下</returns>
        public float GetAxisVertical()
        {
            return Input.GetAxis("Vertical");
        }

        /// <summary>
        /// タッチ入力プラットフォームかチェック
        /// </summary>
        /// <returns>タッチ入力プラットフォームならtrue</returns>
        public bool CheckTouchPlatform()
        {
            return ((Application.platform == RuntimePlatform.Android) || (Application.platform == RuntimePlatform.IPhonePlayer));
        }

        /// <summary>
        /// タッチ情報を取得(エディタと実機を考慮)
        /// </summary>
        /// <returns>タッチ情報。タッチされていない場合は TouchInfo.None</returns>
        public InputController.TouchInfo GetTouchInfo()
        {
            if (this.CheckTouchPlatform())
            {
                if (Input.touchCount > 0)
                {
                    return (InputController.TouchInfo)((int)Input.GetTouch(0).phase);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    return InputController.TouchInfo.Began;
                }
                if (Input.GetMouseButton(0))
                {
                    return InputController.TouchInfo.Now;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    return InputController.TouchInfo.End;
                }
            }
            return InputController.TouchInfo.None;
        }

        /// <summary>
        /// タッチポジションを取得(エディタと実機を考慮)
        /// </summary>
        /// <returns>タッチポジション。タッチされていない場合は (0, 0)</returns>
        public Vector2 GetTouchPosition()
        {
            if (this.CheckTouchPlatform())
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    return touch.position;
                }
            }
            else
            {
                InputController.TouchInfo touch = this.GetTouchInfo();
                if (touch != InputController.TouchInfo.None) { return Input.mousePosition; }
            }
            return Vector2.zero;
        }
    }
}