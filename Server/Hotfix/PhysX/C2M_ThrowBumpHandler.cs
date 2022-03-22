using System;
using Vector3 = System.Numerics.Vector3;

namespace ET
{
	[ActorMessageHandler]
	public class C2M_ThrowBumpHandler : AMActorLocationRpcHandler<Unit, C2M_ThrowBumpRequest, M2C_ThrowBumpResponse>
	{
		protected async override ETTask Run(Unit unit, C2M_ThrowBumpRequest request, M2C_ThrowBumpResponse response, Action reply)
		{
			PhysXComponent physXComponent = unit.DomainScene().GetComponent<PhysXComponent>();
			ProtoVector3 pos = request.Pos;
			ProtoVector3 direction = request.Direction;
			physXComponent.ThrowBump(new Vector3(pos.X, pos.Y, pos.Z) , new Vector3(direction.X, direction.Y, direction.Z));
			reply();
			await ETTask.CompletedTask;
		}
	}
}