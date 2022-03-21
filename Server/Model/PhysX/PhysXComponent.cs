using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using PhysX;

namespace ET
{
    public class PhysXComponentSystem : AwakeSystem<PhysXComponent>
	{
		public override void Awake(PhysXComponent self)
		{
			self.Awake();
			self.Init();

            self.TimeoutCheckTimer = TimerComponent.Instance.NewRepeatedTimer(PhysXComponent.ElapsedMillsecond, self.TimerCallback);
        }
    }

	public class PhysXComponent : Entity
	{
		public static long ElapsedMillsecond = 16;

		public long TimeoutCheckTimer = 0;

		public List<int> SyncIdList = new List<int>();

		public static PhysXComponent Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
		}

		private Physics Physics;
		private PhysX.Scene Scene;

		public void Init()
		{
			ErrorOutput errorOutput = new ErrorOutput();
			Foundation foundation = new Foundation(errorOutput);
			var pvd = new PhysX.VisualDebugger.Pvd(foundation);
			this.Physics = new Physics(foundation, true, pvd);
			Scene = this.Physics.CreateScene(CreateSceneDesc(foundation));

			Scene.SetVisualizationParameter(VisualizationParameter.Scale, 2.0f);
			Scene.SetVisualizationParameter(VisualizationParameter.CollisionShapes, true);
			Scene.SetVisualizationParameter(VisualizationParameter.JointLocalFrames, true);
			Scene.SetVisualizationParameter(VisualizationParameter.JointLimits, true);
			Scene.SetVisualizationParameter(VisualizationParameter.ActorAxes, true);

			// Connect to the PhysX Visual Debugger (if the PVD application is running)
			this.Physics.Pvd.Connect("localhost");

			EventCallback callback = new EventCallback();
			Scene.SetSimulationEventCallback(callback);

			CreateWorld();
		}

		private SceneDesc CreateSceneDesc(Foundation foundation)
		{

			var sceneDesc = new SceneDesc
			{
				Gravity = new Vector3(0, -9.81f, 0),
				FilterShader = new FilterShader()
			};

			return sceneDesc;
		}

		private void CreateWorld()
		{
			foreach (PhysX.Plane wall in PhysXWorldConst.WallArray)
			{
				var material = Scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);
				var body = Scene.Physics.CreateRigidStatic();
				body.Name = "Wall";
				body.GlobalPosePosition = wall.Pos;
				body.GlobalPoseQuat = wall.Quat;
				body.UserData = BodyType.Wall;
				var geom = new BoxGeometry(wall.HalfShap);
				RigidActorExt.CreateExclusiveShape(body, geom, material, null);
				Scene.AddActor(body);
			}

			foreach (Cube cube in PhysXWorldConst.CubeArray)
			{
				var material = Scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);
				var body = Scene.Physics.CreateRigidDynamic();
				body.Name = "Box";
				body.GlobalPosePosition = cube.Pos;
				body.GlobalPoseQuat = cube.Quat;
				body.UserData = BodyType.Cube;
				var geom = new BoxGeometry(cube.HalfShap);
				RigidActorExt.CreateExclusiveShape(body, geom, material, null);
				Scene.AddActor(body);
			}

			foreach (Sphere sphere in PhysXWorldConst.SphereArray)
			{
				var material = Scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);
				var body = Scene.Physics.CreateRigidDynamic();
				body.Name = "Sphere";
				body.GlobalPosePosition = sphere.Pos;
				body.GlobalPoseQuat = sphere.Quat;
				body.UserData = BodyType.Sphere;
				var geom = new SphereGeometry(sphere.Radius);
				RigidActorExt.CreateExclusiveShape(body, geom, material, null);
				Scene.AddActor(body);
			}
		}

		public void TimerCallback()
        {
			PhysXUtil.SceneStage = SceneStage.PreUpdate;
			PreUpdate();

			PhysXUtil.SceneStage = SceneStage.Update;
			Scene.Simulate(0.0167f);
			Scene.FetchResults(true);

			PhysXUtil.SceneStage = SceneStage.LateUpdate;
			LateUpdate();
		}

		private void PreUpdate()
        {

        }

		private void LateUpdate()
        {
			foreach (var rigidActor in PhysXUtil.InLateUpdateNeedRemoveActorSet)
			{
				Debug.WriteLine($"rigidActor?.Scene?.RemoveActor {rigidActor?.GetHashCode()}");
				rigidActor?.Scene?.RemoveActor(rigidActor);

				Debug.WriteLine($"rigidActor?.Dispose() {rigidActor?.GetHashCode()}");
				rigidActor?.Dispose();
			}

			PhysXUtil.InLateUpdateNeedRemoveActorSet.Clear();
		}

		public List<PhysX.Actor> Actors
        {
			get
			{
				return (List<PhysX.Actor>)Scene.GetActors(ActorTypeFlag.RigidStatic|ActorTypeFlag.RigidDynamic);
			}
        }
	}

	public class EventCallback : SimulationEventCallback
	{
		public override void OnContact(ContactPairHeader pairHeader, ContactPair[] pairs)
		{
			base.OnContact(pairHeader, pairs);

			PhysXUtil.OnContact(pairHeader, pairs);
		}
	}
}
