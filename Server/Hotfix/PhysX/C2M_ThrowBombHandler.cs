using System;
using Vector3 = System.Numerics.Vector3;

namespace ET
{
	[ActorMessageHandler]
	public class C2M_ThrowBombHandler : AMActorLocationRpcHandler<Unit, C2M_ThrowBombRequest, M2C_ThrowBombResponse>
	{
		protected async override ETTask Run(Unit unit, C2M_ThrowBombRequest request, M2C_ThrowBombResponse response, Action reply)
		{
			PhysXComponent physXComponent = unit.DomainScene().GetComponent<PhysXComponent>();
			ProtoVector3 direction = request.Direction;
			physXComponent.ThrowBomb(new Vector3(direction.X, direction.Y, direction.Z));
			reply();
			await ETTask.CompletedTask;
		}
	}
}