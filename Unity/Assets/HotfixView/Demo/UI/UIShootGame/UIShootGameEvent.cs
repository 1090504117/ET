using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UIShootGame)]
    public class UIShootGameEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            await ETTask.CompletedTask;
            ResourcesComponent.Instance.LoadBundle(UIType.UIShootGame.StringToAB());
            GameObject bundleGameObject = (GameObject) ResourcesComponent.Instance.GetAsset(UIType.UIShootGame.StringToAB(), UIType.UIShootGame);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.UIShootGame, gameObject);
            ui.AddComponent<UIShootGameComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            ResourcesComponent.Instance.UnloadBundle(UIType.UIShootGame.StringToAB());
        }
    }
}