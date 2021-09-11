using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UITestComponentAwakeSystem : AwakeSystem<UITestComponent>
    {
        public override void Awake(UITestComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			
            self.testBtn = rc.Get<GameObject>("TestBtn");
            self.testBtn.GetComponent<Button>().onClick.AddListener(self.Test);
            self.testText = rc.Get<GameObject>("TestText").GetComponent<Text>();
        }
    }
    
    public static class UITestComponentSystem
    {
        public static void Test(this UITestComponent self)
        {
            Debug.LogError("UITestComponent.Test");
            TestHelper.Test(self.ZoneScene(), 1).Coroutine();
        }
    }
}
