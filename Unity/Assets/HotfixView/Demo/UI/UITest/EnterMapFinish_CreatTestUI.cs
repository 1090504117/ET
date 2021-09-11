

namespace ET
{
	public class EnterMapFinish_CreatTestUI : AEvent<EventType.EnterMapFinish>
	{
		protected override async ETTask Run(EventType.EnterMapFinish args)
		{
			await UIHelper.Create(args.ZoneScene, UIType.UITest);
		}
	}
}
