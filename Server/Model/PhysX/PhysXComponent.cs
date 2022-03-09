﻿using System.Collections.Generic;
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

            self.TimeoutCheckTimer = TimerComponent.Instance.NewRepeatedTimer(PhysXComponent.ElapsedMillsecond, self.Update);
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
		private PhysX.Scene PhysXScene;

		public void Init()
		{
			ErrorOutput errorOutput = new ErrorOutput();
			Foundation foundation = new Foundation(errorOutput);
			var pvd = new PhysX.VisualDebugger.Pvd(foundation);
			this.Physics = new Physics(foundation, true, pvd);
			this.PhysXScene = this.Physics.CreateScene(CreateSceneDesc(foundation));

			this.PhysXScene.SetVisualizationParameter(VisualizationParameter.Scale, 2.0f);
			this.PhysXScene.SetVisualizationParameter(VisualizationParameter.CollisionShapes, true);
			this.PhysXScene.SetVisualizationParameter(VisualizationParameter.JointLocalFrames, true);
			this.PhysXScene.SetVisualizationParameter(VisualizationParameter.JointLimits, true);
			this.PhysXScene.SetVisualizationParameter(VisualizationParameter.ActorAxes, true);

			// Connect to the PhysX Visual Debugger (if the PVD application is running)
			this.Physics.Pvd.Connect("localhost");

			CreateWorld();
		}

		private SceneDesc CreateSceneDesc(Foundation foundation)
		{
#if GPU
			var cudaContext = new CudaContextManager(foundation);
#endif

			var sceneDesc = new SceneDesc
			{
				Gravity = new Vector3(0, -9.81f, 0),
#if GPU
				GpuDispatcher = cudaContext.GpuDispatcher,
#endif
				FilterShader = new FilterShader()
			};

#if GPU
			sceneDesc.Flags |= SceneFlag.EnableGpuDynamics;
			sceneDesc.BroadPhaseType |= BroadPhaseType.Gpu;
#endif

			return sceneDesc;
		}

		private void CreateWorld()
		{
			var groundPlaneMaterial = this.PhysXScene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);

			var groundPlane = this.PhysXScene.Physics.CreateRigidStatic();
			groundPlane.Name = "Ground Plane";
			groundPlane.GlobalPose = Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(0, 0, 1), (float)System.Math.PI / 2);

			var planeGeom = new PlaneGeometry();

			RigidActorExt.CreateExclusiveShape(groundPlane, planeGeom, groundPlaneMaterial, null);

			this.PhysXScene.AddActor(groundPlane);
		}

		public void Update()
        {
			//必须模拟两帧才能被pvd所观察到
			this.PhysXScene.Simulate(0.0167f);
			this.PhysXScene.FetchResults(true);
		}
	}
}
