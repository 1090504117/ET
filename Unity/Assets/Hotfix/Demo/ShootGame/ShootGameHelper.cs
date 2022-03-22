using System;
using UnityEngine;


namespace ET
{
    public static class ShootGameHelper
    {
        public static async ETTask UpdateGameObject(Scene zoneScene)
        {
            C2M_PhysXWorldRequest msg = new C2M_PhysXWorldRequest() { };
            M2C_PhysXWorldResponse response =
                await zoneScene.Domain.GetComponent<SessionComponent>().Session.Call(msg) as M2C_PhysXWorldResponse;
            if (response != null && response.Error != ErrorCode.ERR_Success)
            {
                Log.Error(response.Message);
                return;
            }

            PhysXActorComponent physXActorComponent = zoneScene.GetComponent<PhysXActorComponent>();
            foreach(Actor actor in response.Actors)
            {

                long actorId = actor.ActorId;
                PhysXActor physXActor = null;
                bool isUpdate = physXActorComponent.ActorDic.TryGetValue(actorId, out physXActor);
                if (!isUpdate)
                {
                    physXActor = new PhysXActor();
                    physXActorComponent.ActorDic.Add(actor.ActorId, physXActor);
                }
                physXActor.BodyType = actor.BodyType;
                physXActor.Pos = new Vector3(actor.Pos.X, actor.Pos.Y, actor.Pos.Z);
                physXActor.Quat = new Quaternion(actor.Quat.X, actor.Quat.Y, actor.Quat.Z, actor.Quat.W);
                physXActor.ShapeParams = actor.ShapeParams;
                physXActor.ActorId = actor.ActorId;

                if (isUpdate)
                {
                    Game.EventSystem.Publish(new EventType.PhysXActorUpdate() { Actor = physXActor }).Coroutine();
                }
                else
                {
                    Game.EventSystem.Publish(new EventType.PhysXActorCreate() { Actor = physXActor }).Coroutine();
                }
            }

            //Game.EventSystem.Publish(new EventType.UpdateObject() { ActorDic = physXActorComponent.ActorDic }).Coroutine();

            await ETTask.CompletedTask;
        }

        public static async ETTask ThrowBump(Scene zoneScene)
        {
            C2M_ThrowBumpRequest msg = new C2M_ThrowBumpRequest() {Pos=new ProtoVector3() {X=0,Y=0,Z=0}, Direction=new ProtoVector3() { X=1,Y=0,Z=0} };
            M2C_ThrowBumpResponse response =
                await zoneScene.Domain.GetComponent<SessionComponent>().Session.Call(msg) as M2C_ThrowBumpResponse;
            if (response != null && response.Error != ErrorCode.ERR_Success)
            {
                Log.Error(response.Message);
                return;
            }

            await ETTask.CompletedTask;
        }
    }
}