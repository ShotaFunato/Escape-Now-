/**
 * 入力ラッパークラス
 * @author Shota Funato
 */

using UnityEngine;

namespace FunatoLib
{
    public class InputController : SingletonMonoBehaviour<InputController>
    {
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
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetMouseLeftClick()
        {
            return Input.GetMouseButton(0);
        }

        public bool GetMouseRightClick()
        {
            return Input.GetMouseButton(1);
        }

        public bool GetMouseWheelClick()
        {
            return Input.GetMouseButton(1);
        }

        public void GetMousePos(ref Vector3 screenPos)
        {
            screenPos = Input.mousePosition;
            screenPos = Camera.main.ScreenToWorldPoint(screenPos);
        }

        public bool GetMouseClickPos(ref Vector3 screenPos)
        {
            if (this.GetMouseLeftClick())
            {
                this.GetMousePos(ref screenPos);
                return true;
            }

            return false;
        }
    }
}