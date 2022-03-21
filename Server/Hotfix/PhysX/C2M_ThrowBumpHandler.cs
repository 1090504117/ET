using System;


namespace ET
{
	[MessageHandler]
	public class C2M_ThrowBumpHandler : AMRpcHandler<C2M_ThrowBumpRequest, M2C_ThrowBumpResponse>
	{
		protected async override ETTask Run(Session session, C2M_ThrowBumpRequest request, M2C_ThrowBumpResponse response, Action reply)
		{
			response.Message = "aaaaaaaaaaaaaaaaaaaaaa";
			reply();
			await ETTask.CompletedTask;
		}
	}
}