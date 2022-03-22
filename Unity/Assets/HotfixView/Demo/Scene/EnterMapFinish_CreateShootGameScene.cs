

namespace ET
{
	public class EnterMapFinish_CreateShootGameScene : AEvent<EventType.EnterMapFinish>
	{
		protected override async ETTask Run(EventType.EnterMapFinish args)
		{
			// 加载场景资源
			await ResourcesComponent.Instance.LoadBundleAsync("shootgame.unity3d");
			// 切换到map场景
			using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
			{
				await sceneChangeComponent.ChangeSceneAsync(UnitySceneType.ShootGame);
			}
			args.ZoneScene.AddComponent<ShootGameObjectUpdateComponent>();
			//args.ZoneScene.AddComponent<OperaComponent>();
		}
	}
}
