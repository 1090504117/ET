using System;
using UnityEngine;


namespace ET
{
    public static class ShootGameHelper
    {
        private static void FillPhysXActor(PhysXActor physXActor, Actor actor)
        {
            physXActor.BodyType = (BodyType)actor.BodyType;
            physXActor.Pos = new Vector3(actor.Pos.X, actor.Pos.Y, actor.Pos.Z);
            physXActor.Quat = new Quaternion(actor.Quat.X, actor.Quat.Y, actor.Quat.Z, actor.Quat.W);
            physXActor.ShapeParams = actor.ShapeParams;
            physXActor.ActorId = actor.ActorId;
        }

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
                BodyType bodyType = (BodyType)actor.BodyType;
                bool isUpdate = false;
                if (bodyType == BodyType.Player)
                {
                    PhysXActor myPhysXActor = physXActorComponent.MyActor;
                    isUpdate = myPhysXActor != null;
                    if (!isUpdate)
                    {
                        myPhysXActor = new PhysXActor();
                    }
                    FillPhysXActor(myPhysXActor, actor);
                    physXActorComponent.MyActor = myPhysXActor;

                    if (isUpdate)
                    {
                        Game.EventSystem.Publish(new EventType.PhysXPlayerUpdate() { Actor = myPhysXActor }).Coroutine();
                    }
                    else
                    {
                        Game.EventSystem.Publish(new EventType.PhysXPlayerCreate() { Actor = myPhysXActor }).Coroutine();
                    }

                    continue;
                }

                long actorId = actor.ActorId;
                PhysXActor physXActor = null;
                isUpdate = physXActorComponent.ActorDic.TryGetValue(actorId, out physXActor);
                if (!isUpdate)
                {
                    physXActor = new PhysXActor();
                    physXActorComponent.ActorDic.Add(actor.ActorId, physXActor);
                }

                FillPhysXActor(physXActor, actor);

                if (isUpdate)
                {
                    Game.EventSystem.Publish(new EventType.PhysXActorUpdate() { Actor = physXActor }).Coroutine();
                }
                else
                {
                    Game.EventSystem.Publish(new EventType.PhysXActorCreate() { Actor = physXActor }).Coroutine();
                }
            }

            foreach (var keyValuePair in physXActorComponent.ActorDic)
            {
                bool isFind = false;
                long actorId = keyValuePair.Value.ActorId;
                foreach (var searchActor in response.Actors)
                {
                    if (actorId == searchActor.ActorId)
                    {
                        isFind = true;
                        break;
                    }
                }
                if (!isFind)
                {
                    physXActorComponent.ActorDic.Remove(actorId);
                    Game.EventSystem.Publish(new EventType.PhysXActorDelete() { Actor = keyValuePair.Value }).Coroutine();
                }
            }

            //Game.EventSystem.Publish(new EventType.UpdateObject() { ActorDic = physXActorComponent.ActorDic }).Coroutine();

            await ETTask.CompletedTask;
        }

        public static async ETTask ThrowBomb(Scene zoneScene)
        {
            C2M_ThrowBombRequest msg = new C2M_ThrowBombRequest() {Direction=new ProtoVector3() { X=1,Y=0,Z=0} };
            M2C_ThrowBombResponse response =
                await zoneScene.Domain.GetComponent<SessionComponent>().Session.Call(msg) as M2C_ThrowBombResponse;
            if (response != null && response.Error != ErrorCode.ERR_Success)
            {
                Log.Error(response.Message);
                return;
            }

            await ETTask.CompletedTask;
        }

        public static async ETTask MovePlayer(Scene zoneScene, Vector3 direction)
        {
            C2M_MovePlayerRequest msg = new C2M_MovePlayerRequest() { Direction = new ProtoVector3() { X = direction.x, Y = 0, Z = direction.y } };
            M2C_MovePlayerResponse response =
                await zoneScene.Domain.GetComponent<SessionComponent>().Session.Call(msg) as M2C_MovePlayerResponse;
            if (response != null && response.Error != ErrorCode.ERR_Success)
            {
                Log.Error(response.Message);
                return;
            }

            await ETTask.CompletedTask;
        }
    }
}