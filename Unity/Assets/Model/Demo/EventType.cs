using System.Collections.Generic;

namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
        }

        public struct ChangePosition
        {
            public Unit Unit;
        }

        public struct ChangeRotation
        {
            public Unit Unit;
        }

        public struct PingChange
        {
            public Scene ZoneScene;
            public long Ping;
        }
        
        public struct AfterCreateZoneScene
        {
            public Scene ZoneScene;
        }
        
        public struct AfterCreateLoginScene
        {
            public Scene LoginScene;
        }

        public struct AppStartInitFinish
        {
            public Scene ZoneScene;
        }

        public struct LoginFinish
        {
            public Scene ZoneScene;
        }

        public struct LoadingBegin
        {
            public Scene Scene;
        }

        public struct LoadingFinish
        {
            public Scene Scene;
        }

        public struct EnterMapFinish
        {
            public Scene ZoneScene;
        }

        public struct AfterUnitCreate
        {
            public Unit Unit;
        }
        
        public struct MoveStart
        {
            public Unit Unit;
        }

        public struct MoveStop
        {
            public Unit Unit;
        }

        public struct UpdateObject
        {
            public Dictionary<long, PhysXActor> ActorDic;
        }

        public struct PhysXActorCreate
        {
            public PhysXActor Actor;
        }

        public struct PhysXActorDelete
        {
            public Dictionary<long, PhysXActor> ActorDic;
            public PhysXActor Actor;
        }

        public struct PhysXActorUpdate
        {
            public PhysXActor Actor;
        }
    }
}