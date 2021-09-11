using System;


namespace ET
{
	[ActorMessageHandler]
	public class C2M_TestSendInfoHandler : AMActorLocationRpcHandler<Unit, C2M_TestSendInfoRequest, M2C_TestSendInfoResponse>
	{
		protected async override ETTask Run(Unit unit, C2M_TestSendInfoRequest request, M2C_TestSendInfoResponse response, Action reply)
		{
			response.Message = "aaaaaaaaaaaaaaaaaaaaaa";
			reply();
			await ETTask.CompletedTask;
		}
	}
}