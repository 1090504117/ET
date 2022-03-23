using UnityEngine;

namespace ET
{
    class PhysXActorDelete_PhysXActorDeleteView : AEvent<EventType.PhysXActorDelete>
    {
        protected override async ETTask Run(EventType.PhysXActorDelete args)
        {
            PhysXActor actor = args.Actor;
            GameObjectComponent gameObjectComponent = actor.GetComponent<GameObjectComponent>();
            if (gameObjectComponent != null && gameObjectComponent.GameObject != null)
            {
                GameObject go = gameObjectComponent.GameObject;
                GameObject.Destroy(go);
                actor.RemoveComponent<GameObjectComponent>();
            }

            await ETTask.CompletedTask;
        }
    }
}