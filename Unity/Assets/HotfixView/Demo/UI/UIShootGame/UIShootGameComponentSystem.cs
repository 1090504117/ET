using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIShootGameComponentAwakeSystem : AwakeSystem<UIShootGameComponent>
    {
        public override void Awake(UIShootGameComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			
            self.testBtn = rc.Get<GameObject>("TestBtn");
            self.testBtn.GetComponent<Button>().onClick.AddListener(self.Test);
            self.testText = rc.Get<GameObject>("TestText").GetComponent<Text>();
        }
    }
    
    public static class UIShootGameComponentSystem
    {
        public static void Test(this UIShootGameComponent self)
        {
            Debug.LogError("UIShootGameComponent.Test");
            TestHelper.Test(self.ZoneScene(), 1).Coroutine();
        }
    }
}
