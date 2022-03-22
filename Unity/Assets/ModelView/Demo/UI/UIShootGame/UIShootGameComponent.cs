
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public class UIShootGameComponent : Entity
	{
        /// <summary>
        /// 移动摇杆
        /// </summary>
        public JointedArm MoveJointedArm;
        /// <summary>
        /// 旋转摇杆
        /// </summary>
        public JointedArm RotateJointedArm;
        /// <summary>
        /// 开枪按钮
        /// </summary>
        public GameObject FireBtn;
        /// <summary>
        /// 丢手雷按钮
        /// </summary>
        public GameObject BombBtn;
        /// <summary>
        /// 跳跃按钮
        /// </summary>
        public GameObject JumpBtn;
        /// <summary>
        /// 装子弹按钮
        /// </summary>
        public GameObject BulletBtn;
    }
}
