using UnityEngine;

namespace ET
{
    class PhysXPlayerUpdateView : AEvent<EventType.PhysXPlayerUpdate>
    {
        protected override async ETTask Run(EventType.PhysXPlayerUpdate args)
        {
            PhysXActor actor = args.Actor;
            PhysXPlayerComponent playerComponent = actor.GetComponent<PhysXPlayerComponent>();
            playerComponent.Update(actor);

            await ETTask.CompletedTask;
        }
    }
}