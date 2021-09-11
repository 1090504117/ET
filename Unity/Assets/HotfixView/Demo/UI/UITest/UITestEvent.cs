using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UITest)]
    public class UITestEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            await ETTask.CompletedTask;
            ResourcesComponent.Instance.LoadBundle(UIType.UITest.StringToAB());
            GameObject bundleGameObject = (GameObject) ResourcesComponent.Instance.GetAsset(UIType.UITest.StringToAB(), UIType.UITest);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.UITest, gameObject);

            ui.AddComponent<UITestComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            ResourcesComponent.Instance.UnloadBundle(UIType.UITest.StringToAB());
        }
    }
}