using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class ShootGameObjectUpdateComponentDestroySystem : DestroySystem<ShootGameObjectUpdateComponent>
    {
        public override void Destroy(ShootGameObjectUpdateComponent self)
        {
            self.Clear();
        }
    }

    [ObjectSystem]
    public class ShootGameObjectUpdateComponentAwakeSystem : AwakeSystem<ShootGameObjectUpdateComponent>
    {
        public override void Awake(ShootGameObjectUpdateComponent self)
        {
            self.InitTimer();
        }
    }

    public static class ShootGameObjectUpdateComponentSystem
    {
        public static void InitTimer(this ShootGameObjectUpdateComponent self)
        {
            Debug.LogError("InitTimer");
            self.UpdateTimer = TimerComponent.Instance.NewFrameTimer(async () =>
            {
                try
                {
                    await ShootGameHelper.UpdateGameObject(self.DomainScene());

                }
                catch (Exception e)
                {
                    Log.Error($"init ShootGameObjectUpdate timer error\n{e}");
                }
            }
            );
        }

        public static void Clear(this ShootGameObjectUpdateComponent self)
        {
            TimerComponent.Instance.Remove(ref self.UpdateTimer);
        }
    }
}
