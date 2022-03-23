using UnityEngine;

namespace ET
{
    public class PhysXPlayerComponentAwakeSystem : AwakeSystem<PhysXPlayerComponent, GameObject>
    {
        public override void Awake(PhysXPlayerComponent self, GameObject root)
        {
            ReferenceCollector rc = root.GetComponent<ReferenceCollector>();
            self.Camera = rc.Get<GameObject>("PlayerCamera").GetComponent<Camera>();
            self.PlayerArm = rc.Get<GameObject>("Arm");
            self.PlayerTransform = rc.Get<GameObject>("Player").GetComponent<Transform>();
        }
    }

    public class PhysXPlayerComponentDestorySystem : DestroySystem<PhysXPlayerComponent>
    {
        public override void Destroy(PhysXPlayerComponent self)
        {
        }
    }


    public static class PhysXPlayerComponentSystem
    {
        public static void Update(this PhysXPlayerComponent self, PhysXActor actor)
        {
            Transform playerTransform = self.PlayerTransform;
            playerTransform.position = actor.Pos;
            playerTransform.rotation = actor.Quat;
            float halfHeight = actor.ShapeParams[0];
            float radius = actor.ShapeParams[1];
            self.Camera.transform.localPosition = new Vector3(halfHeight - radius, 0, 0);
        }
    }
}

