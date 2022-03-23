using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using UnityEngine;

namespace ET
{
    [BsonIgnoreExtraElements]
    public sealed class PhysXActor : Entity
    {
		public BodyType BodyType;

		public Vector3 Pos;

		public Quaternion Quat;

		public List<float> ShapeParams = new List<float>();

		public long ActorId;
	}
}