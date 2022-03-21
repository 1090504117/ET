

namespace ET
{
	public class EnterMapFinish_CreateShootGameUI : AEvent<EventType.EnterMapFinish>
	{
		protected override async ETTask Run(EventType.EnterMapFinish args)
		{
			await UIHelper.Create(args.ZoneScene, UIType.UIShootGame);
		}
	}
}
