using System;
using Vector3 = System.Numerics.Vector3;

namespace ET
{
	[ActorMessageHandler]
	public class C2M_MovePlayerHandler : AMActorLocationRpcHandler<Unit, C2M_MovePlayerRequest, M2C_MovePlayerResponse>
	{
		protected async override ETTask Run(Unit unit, C2M_MovePlayerRequest request, M2C_MovePlayerResponse response, Action reply)
		{
			PhysXComponent physXComponent = unit.DomainScene().GetComponent<PhysXComponent>();
			ProtoVector3 direction = request.Direction;
			physXComponent.MovePlayer(new Vector3(direction.X, direction.Y, direction.Z));
			reply();
			await ETTask.CompletedTask;
		}
	}
}