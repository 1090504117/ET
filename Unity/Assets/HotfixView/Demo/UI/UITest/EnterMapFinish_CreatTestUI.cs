

namespace ET
{
	public class EnterMapFinish_CreatTestUI : AEvent<EventType.LoginFinish>
	{
		protected override async ETTask Run(EventType.LoginFinish args)
		{
			await UIHelper.Create(args.ZoneScene, UIType.UITest);
		}
	}
}
