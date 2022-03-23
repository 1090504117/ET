using UnityEngine;

namespace ET
{
    class PhysXPlayerCreateView : AEvent<EventType.PhysXPlayerCreate>
    {
        protected override async ETTask Run(EventType.PhysXPlayerCreate args)
        {
            PhysXActor actor = args.Actor;

            ResourcesComponent.Instance.LoadBundle("Player.unity3d");
            GameObject prefab = (GameObject)ResourcesComponent.Instance.GetAsset("Player.unity3d", "Player");
            GameObject playerRoot = UnityEngine.Object.Instantiate(prefab);

            PhysXPlayerComponent playerComponent = actor.AddComponent<PhysXPlayerComponent, GameObject>(playerRoot);
            playerComponent.Update(actor);

            await ETTask.CompletedTask;
        }
    }
}