using PhysX;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ET
{
	[ActorMessageHandler]
	public class C2M_PhysXWorldHandler : AMActorLocationRpcHandler<Unit, C2M_PhysXWorldRequest, M2C_PhysXWorldResponse>
	{
		protected async override ETTask Run(Unit unit, C2M_PhysXWorldRequest request, M2C_PhysXWorldResponse response, Action reply)
		{
			response.Message = null;
			PhysXComponent physXComponent = unit.DomainScene().GetComponent<PhysXComponent>();
			List<PhysX.Actor> actors = physXComponent.Actors;
			foreach (var actor in actors)
			{
				ActorExtraData actorExtraData = physXComponent.GetActorExtraDataByActor(actor);
				if (actorExtraData == null)
                {
					continue;
                }

				ActorType actorType = actor.Type;
				RigidActor rigidActor = null;
				if (actorType == ActorType.RigidDynamic || actorType == ActorType.RigidStatic)
				{
					rigidActor = actor as RigidActor;
				}
				else
                {
					continue;
				}

				Vector3 pos = rigidActor.GlobalPosePosition;
				Quaternion quat = rigidActor.GlobalPoseQuat;

				BodyType bodyType = actorExtraData.BodyType;
				var shapeParams = new List<float>();

				if (bodyType == BodyType.None)
				{

				}
				else if (bodyType == BodyType.Bump || bodyType == BodyType.Sphere)
				{
					Shape shape = rigidActor.GetShape(0);
					SphereGeometry sphere = shape.GetSphereGeometry();
					shapeParams.Add(sphere.Radius);
				}
				else if (bodyType == BodyType.Cube || bodyType == BodyType.Wall)
				{
					Shape shape = rigidActor.GetShape(0);
					BoxGeometry boxGeometry = shape.GetBoxGeometry();
					shapeParams.Add(boxGeometry.Size.X);
					shapeParams.Add(boxGeometry.Size.Y);
					shapeParams.Add(boxGeometry.Size.Z);
				}

				response.Actors.Add(new Actor()
				{
					BodyType = (int)bodyType,
					Pos = new ProtoVector3() { X = pos.X, Y = pos.Y, Z = pos.Z },
					Quat = new ProtoQuaternion() { X = quat.X, Y = quat.Y, Z = quat.Z, W = quat.W },
					ShapeParams = shapeParams,
					ActorId = actorExtraData.ActorId
				});
			}

			reply();
			await ETTask.CompletedTask;
		}
	}
}