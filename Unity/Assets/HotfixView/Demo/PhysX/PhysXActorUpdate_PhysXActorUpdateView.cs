using UnityEngine;

namespace ET
{
    class PhysXActorUpdate_PhysXActorUpdateView : AEvent<EventType.PhysXActorUpdate>
    {
        protected override async ETTask Run(EventType.PhysXActorUpdate args)
        {
            PhysXActor actor = args.Actor;
            GameObjectComponent gameObjectComponent = actor.GetComponent<GameObjectComponent>();
            if (gameObjectComponent != null && gameObjectComponent.GameObject != null)
            {
                GameObject go = gameObjectComponent.GameObject;
                go.transform.position = actor.Pos;
                go.transform.rotation = actor.Quat;
            }
            
            await ETTask.CompletedTask;
        }
    }
}
