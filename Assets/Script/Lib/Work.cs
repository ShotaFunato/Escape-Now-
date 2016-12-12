/**
 * ワーククラス
 * @author Shota Funato
 */

using UnityEngine;
using System.Collections;

namespace FunatoLib
{
    public abstract class Work : MonoBehaviour
    {
        /// <summary>
        /// 状態列挙
        /// </summary>
        public enum StateRoutine0
        {
            StateMain,
            StateDie,

            StateMax
        };

        /// <summary>
        /// 状態
        /// </summary>
        protected Work.StateRoutine0 stateRoutine0;

        /// <summary>
        /// 初期化
        /// </summary>
	    protected virtual void Start()
        {
            this.stateRoutine0 = StateRoutine0.StateMain;
            Debug.Log(this.GetType().FullName + "   StateRoutine0:" + this.stateRoutine0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        protected virtual void Update()
        {
        }
    }
}
