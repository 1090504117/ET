using System;
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
		public static int ElapsedMillsecond = 16;

		public long TimeoutCheckTimer = 0;

		public List<int> SyncIdList = new List<int>();

		public static PhysXComponent Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
		}

		private Physics Physics;
		private PhysX.Scene Scene;
		private Dictionary<PhysX.Actor, ActorExtraData> ActorExtraDataDic = new Dictionary<PhysX.Actor, ActorExtraData>();
		private bool IsNeedNewWorld;
		private CapsuleController PlayerController;

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

			EventCallback callback = new EventCallback();
			Scene.SetSimulationEventCallback(callback);
		}

		private SceneDesc CreateSceneDesc(Foundation foundation)
		{

			var sceneDesc = new SceneDesc
			{
				Gravity = new Vector3(0, -PhysXUtil.G, 0),
				FilterShader = new FilterShader()
			};

			return sceneDesc;
		}

		public void ClearOldWorld()
        {
			List<PhysX.Actor> actorList = this.Actors;
			foreach(var actor in Actors)
            {
				PhysXUtil.InLateUpdateNeedRemoveActorSet.Add((RigidActor)actor);
			}

			IsNeedNewWorld = true;
		}

		private void CreateNewWorld()
		{
			foreach (PhysX.Plane wall in PhysXWorldConst.WallArray)
			{
				var material = Scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);
				var body = Scene.Physics.CreateRigidStatic();
				body.Name = "Wall";
				body.GlobalPosePosition = wall.Pos;
				body.GlobalPoseQuat = wall.Quat;
				var geom = new BoxGeometry(wall.HalfShap);
				RigidActorExt.CreateExclusiveShape(body, geom, material, null);
				Scene.AddActor(body);
				ActorExtraDataDic.Add(body, new ActorExtraData() { BodyType = BodyType.Wall, ActorId = PhysXUtil.GenActorId() });
			}

			foreach (Cube cube in PhysXWorldConst.CubeArray)
			{
				var material = Scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);
				var body = Scene.Physics.CreateRigidDynamic();
				body.Name = "Box";
				body.GlobalPosePosition = cube.Pos;
				body.GlobalPoseQuat = cube.Quat;
				body.Mass = cube.Mass;
				var geom = new BoxGeometry(cube.HalfShap);
				RigidActorExt.CreateExclusiveShape(body, geom, material, null);
				Scene.AddActor(body);
				ActorExtraDataDic.Add(body, new ActorExtraData() { BodyType = BodyType.Cube, ActorId = PhysXUtil.GenActorId() });
			}

			foreach (Sphere sphere in PhysXWorldConst.SphereArray)
			{
				var material = Scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);
				var body = Scene.Physics.CreateRigidDynamic();
				body.Name = "Sphere";
				body.GlobalPosePosition = sphere.Pos;
				body.GlobalPoseQuat = sphere.Quat;
				body.Mass = sphere.Mass;
				var geom = new SphereGeometry(sphere.Radius);
				RigidActorExt.CreateExclusiveShape(body, geom, material, null);
				Scene.AddActor(body);
				ActorExtraDataDic.Add(body, new ActorExtraData() { BodyType = BodyType.Sphere, ActorId = PhysXUtil.GenActorId() });
			}

			CreatePlayer();

			CheckAndConnectPvd();
		}

		private void CreatePlayer()
        {
			var material = Scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);
			Capsule capusule = PhysXWorldConst.Player;
			var desc = new CapsuleControllerDesc()
			{
				Height = capusule.Height,
				Radius = capusule.Redius,
				Material = material,
				UpDirection = capusule.UpDirection,
				//ReportCallback = new ControllerHitReport()
			};

			var controllerManager = Scene.CreateControllerManager();
			PlayerController = controllerManager.CreateController<CapsuleController>(desc);
			PlayerController.FootPosition = capusule.FootPos;
			RigidDynamic controllerActor = PlayerController.Actor;
			controllerActor.SetMassAndUpdateInertia(capusule.Mass);
			ActorExtraDataDic.Add(controllerActor, new ActorExtraData() { BodyType = BodyType.Player, ActorId = PhysXUtil.GenActorId() });
		}

		private void CheckAndConnectPvd()
        {
			if (Physics.Pvd.IsConnected(false))
			{
				this.Physics.Pvd.Disconnect();
			}
			this.Physics.Pvd.Connect("localhost");
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
				RemoveActorExtraData(rigidActor);

				Debug.WriteLine($"rigidActor?.Scene?.RemoveActor {rigidActor?.GetHashCode()}");
				rigidActor?.Scene?.RemoveActor(rigidActor);

				Debug.WriteLine($"rigidActor?.Dispose() {rigidActor?.GetHashCode()}");
				rigidActor?.Dispose();
			}

			PhysXUtil.InLateUpdateNeedRemoveActorSet.Clear();

			if (IsNeedNewWorld)
            {
				IsNeedNewWorld = false;
				CreateNewWorld();
			}
		}

		public List<PhysX.Actor> Actors
        {
			get
			{
				return (List<PhysX.Actor>)Scene.GetActors(ActorTypeFlag.RigidStatic|ActorTypeFlag.RigidDynamic);
			}
        }

		public ActorExtraData GetActorExtraData(PhysX.Actor actor)
		{
			if (actor == null) return null;
			ActorExtraDataDic.TryGetValue(actor, out ActorExtraData actorExtraData);
			return actorExtraData;
		}

		public ActorExtraData RemoveActorExtraData(PhysX.Actor actor)
		{
			if (actor == null) return null;
			ActorExtraDataDic.TryGetValue(actor, out ActorExtraData actorExtraData);
			return actorExtraData;
		}

		public void ThrowBomb(Vector3 direction)
        {
			Sphere bomb = PhysXWorldConst.Bomb;
			Vector3 pos = (PlayerController == null || PlayerController.Actor == null)? 
				Vector3.Zero : PlayerController.FootPosition + new Vector3(0, PlayerController.Height - PlayerController.Radius, 0) ;
			float radius = bomb.Radius;
			pos = pos + Vector3.Normalize(direction) * (radius + 0.5f);
			var material = Scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);
			var body = Scene.Physics.CreateRigidDynamic();
			body.GlobalPosePosition = pos;
			body.Name = "Bomb";
			body.Mass = bomb.Mass;
			var geom = new SphereGeometry(radius);
			RigidActorExt.CreateExclusiveShape(body, geom, material, null);
			Scene.AddActor(body);
			Vector3 normalizedDirection = Vector3.Normalize(direction);
			float force = 7 * PhysXUtil.G;
			body?.AddForceAtLocalPosition(normalizedDirection * force, Vector3.Zero, ForceMode.Impulse, true);

			ActorExtraDataDic.Add(body, new ActorExtraData() { BodyType = BodyType.Bomb, ActorId = PhysXUtil.GenActorId() });
		}

		private float _speed = 0.005f;

		public void MovePlayer(Vector3 direction)
		{
			if (PlayerController == null || PlayerController.Actor == null)
            {
				return;
            }
			PlayerController.Move(direction * _speed, new TimeSpan(0,0,0, ElapsedMillsecond));
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
