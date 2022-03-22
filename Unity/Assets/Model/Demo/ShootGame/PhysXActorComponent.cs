using System.Collections.Generic;
using System.Linq;

namespace ET
{

	public class PhysXActorComponent : Entity
	{
		public Dictionary<long, PhysXActor> ActorDic = new Dictionary<long, PhysXActor>();
		public PhysXActor MyActor;
	}
}