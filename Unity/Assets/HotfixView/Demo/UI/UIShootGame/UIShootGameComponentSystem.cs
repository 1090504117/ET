using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIShootGameComponentAwakeSystem : AwakeSystem<UIShootGameComponent>
    {
        public override void Awake(UIShootGameComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.MoveJointedArm = rc.Get<GameObject>("MoveJointedArm").GetComponent<JointedArm>();
            self.RotateJointedArm = rc.Get<GameObject>("RotateJointedArm").GetComponent<JointedArm>();
            self.FireBtn = rc.Get<GameObject>("FireButton");
            self.BombBtn = rc.Get<GameObject>("BombButton");
            self.JumpBtn = rc.Get<GameObject>("JumpButton");
            self.BulletBtn = rc.Get<GameObject>("BulletButton");

            self.MoveJointedArm.onDragCb = (direction) =>
            {
                self.MoveDragCallback(direction);
            };

            self.MoveJointedArm.onStopCb = () =>
            {
                self.StopMoveCallback();
            };


            self.RotateJointedArm.onDragCb = (direction) =>
            {
                self.RotateDragCallback(direction);
            };

            self.RotateJointedArm.onStopCb = () =>
            {
                self.StopRotateCallback();
            };


            // 开炮
            EventTriggerListener.Get(self.FireBtn).onDown += (btn) =>
            {
                Debug.LogError("(self.FireBtn).onDown");

                //EventDispatcher.instance.DispatchEvent(EventNameDef.FIRE, true);
            };

            EventTriggerListener.Get(self.FireBtn).onUp += (btn) =>
            {
                Debug.LogError("(self.FireBtn).onUp");

                //EventDispatcher.instance.DispatchEvent(EventNameDef.FIRE, false);
            };
            // 丢手雷
            EventTriggerListener.Get(self.BombBtn).onClick += async (btn) =>
            {
                Debug.LogError("(self.BombBtn).onClick");
                await self.ThrowBomb();
            };
            // 跳跃
            EventTriggerListener.Get(self.JumpBtn).onClick += (btn) =>
            {
                Debug.LogError("(self.JumpBtn).onClick");

                //EventDispatcher.instance.DispatchEvent(EventNameDef.JUMP);
            };
            // 装子弹
            EventTriggerListener.Get(self.BulletBtn).onClick += (btn) =>
            {
                Debug.LogError("(self.BulletBtn).onClick");

                //EventDispatcher.instance.DispatchEvent(EventNameDef.BULLET);
            };
        }
    }
    
    public static class UIShootGameComponentSystem
    {
        public static void MoveDragCallback(this UIShootGameComponent self, Vector3 direction)
        {
            ShootGameHelper.MovePlayer(self.ZoneScene(),direction).Coroutine();
        }

        public static void StopMoveCallback(this UIShootGameComponent self)
        {

        }

        public static void RotateDragCallback(this UIShootGameComponent self, Vector3 direction)
        {
            Debug.LogError("RotateDragCallback");

        }

        public static void StopRotateCallback(this UIShootGameComponent self)
        {
            Debug.LogError("StopRotateCallback");

        }

        public static async ETTask ThrowBomb(this UIShootGameComponent self)
        {
            PhysXActorComponent physXActorComponent = self.ZoneScene().GetComponent<PhysXActorComponent>();
            PhysXActor actor = physXActorComponent.MyActor;
            if (actor == null)
            {
                return;
            }
            Camera camera = actor.GetComponent<PhysXPlayerComponent>().Camera;
            if (camera == null)
            {
                return;
            }
            Vector3 direction = camera.transform.forward;
            C2M_ThrowBombRequest msg = new C2M_ThrowBombRequest() { Direction = new ProtoVector3() { X = direction.x, Y = direction.y, Z = direction.z } };
            M2C_ThrowBombResponse response =
                await self.ZoneScene().Domain.GetComponent<SessionComponent>().Session.Call(msg) as M2C_ThrowBombResponse;
            if (response != null && response.Error != ErrorCode.ERR_Success)
            {
                Log.Error(response.Message);
                return;
            }
            await ETTask.CompletedTask;
        }
    }
}
