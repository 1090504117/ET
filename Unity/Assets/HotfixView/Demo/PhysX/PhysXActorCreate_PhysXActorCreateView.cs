using UnityEngine;

namespace ET
{
    class PhysXActorCreate_PhysXActorCreateView : AEvent<EventType.PhysXActorCreate>
    {
        protected override async ETTask Run(EventType.PhysXActorCreate args)
        {
            GameObject go = null;
            GameObject prefab = null;
            PhysXActor actor = args.Actor;
            BodyType bodyType = (BodyType)actor.BodyType;
            switch (bodyType)
            {
                case BodyType.Wall:
                    ResourcesComponent.Instance.LoadBundle("PhysXActorWall.unity3d");
                    prefab = (GameObject)ResourcesComponent.Instance.GetAsset("PhysXActorWall.unity3d", "PhysXActorWall");
                    go = UnityEngine.Object.Instantiate(prefab);
                    go.transform.localScale = new Vector3(actor.ShapeParams[0], actor.ShapeParams[1], actor.ShapeParams[2]);
                    break;
                case BodyType.Cube:
                    ResourcesComponent.Instance.LoadBundle("PhysXActorCube.unity3d");
                    prefab = (GameObject)ResourcesComponent.Instance.GetAsset("PhysXActorCube.unity3d", "PhysXActorCube");
                    go = UnityEngine.Object.Instantiate(prefab);
                    go.transform.localScale = new Vector3(actor.ShapeParams[0], actor.ShapeParams[1], actor.ShapeParams[2]);
                    break;
                case BodyType.Sphere:
                    ResourcesComponent.Instance.LoadBundle("PhysXActorSphere.unity3d");
                    prefab = (GameObject)ResourcesComponent.Instance.GetAsset("PhysXActorSphere.unity3d", "PhysXActorSphere");
                    go = UnityEngine.Object.Instantiate(prefab);
                    float scale = 2 * actor.ShapeParams[0];
                    go.transform.localScale = new Vector3(scale, scale, scale);
                    break;
                case BodyType.Bomb:
                    ResourcesComponent.Instance.LoadBundle("PhysXActorBomb.unity3d");
                    prefab = (GameObject)ResourcesComponent.Instance.GetAsset("PhysXActorBomb.unity3d", "PhysXActorBomb");
                    go = UnityEngine.Object.Instantiate(prefab);
                    scale = 2 * actor.ShapeParams[0];
                    go.transform.localScale = new Vector3(scale, scale, scale);
                    break;
                default:
                    break;
            }
            if (go != null)
            {
                go.transform.position = actor.Pos;
                go.transform.rotation = actor.Quat;
                args.Actor.AddComponent<GameObjectComponent>().GameObject = go;
            }

            await ETTask.CompletedTask;
        }
    }
}
