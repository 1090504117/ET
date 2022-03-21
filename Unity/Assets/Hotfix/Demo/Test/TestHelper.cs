using System;


namespace ET
{
    public static class TestHelper
    {
        public static async ETTask Test(Scene zoneScene, int itemId)
        {
            /*
            C2M_TestSendInfoRequest msg = new C2M_TestSendInfoRequest() { ItemId = itemId};
            M2C_TestSendInfoResponse response =
                await zoneScene.Domain.GetComponent<SessionComponent>().Session.Call(msg) as M2C_TestSendInfoResponse;

            if (response != null && response.Error != ErrorCode.ERR_Success)
            {
                Log.Error(response.Message);
                return;
            }

            Log.Info(response.Message);
            */

            C2M_PhysXWorldRequest msg = new C2M_PhysXWorldRequest() { };
            M2C_PhysXWorldResponse response =
                await zoneScene.Domain.GetComponent<SessionComponent>().Session.Call(msg) as M2C_PhysXWorldResponse;
            if (response != null && response.Error != ErrorCode.ERR_Success)
            {
                Log.Error(response.Message);
                return;
            }

            Log.Info(response.Message);
        } 
    }
}