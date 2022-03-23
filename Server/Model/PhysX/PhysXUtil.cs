using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysX
{
    public static class PhysXUtil
    {
        public static float G = 9.81f;

        public static Quaternion GetShortestRotation(Vector3 v0, Vector3 v1)
        {
            float d = Vector3.Dot(v0, v1);
            Vector3 cross = Vector3.Cross(v0, v1);
            Quaternion q = d > -1 ? new Quaternion(cross.X, cross.Y, cross.Z, 1 + d) : MathF.Abs(v0.X) < 1.0f ? new Quaternion(0f, v0.Z, -v0.Y, 0f) : new Quaternion(v0.Y, -v0.X, 0f, 0f);
            return Quaternion.Normalize(q);
        }

        private static Vector3 _normalizedXDirection = new Vector3(1, 0, 0);
        public static Quaternion GetQuatByDirection(Vector3 direction)
        {
            Vector3 d = Vector3.Normalize(direction);
            return GetShortestRotation(_normalizedXDirection, d);
        }

        private static int _innerActorId = 0;

        public static int GenActorId()
        {
            ++_innerActorId;
            return _innerActorId;
        }

        public static RigidActor ThrowBomb(Scene scene, Vector3 pos, Vector3 direction)
        {
            var material = scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);
            var body = scene.Physics.CreateRigidDynamic();
            body.GlobalPosePosition = pos;
            body.Name = "Sphere";
            body.UserData = (object)BodyType.Bomb;
            var geom = new SphereGeometry(1f);
            RigidActorExt.CreateExclusiveShape(body, geom, material, null);
            scene.AddActor(body);
            Vector3 normalizedDirection = Vector3.Normalize(direction);
            float force = 10 * G;
            body?.AddForceAtLocalPosition(normalizedDirection * force, Vector3.Zero, ForceMode.Impulse, true);
            return body;
        }

        public static bool IsBombByActor(Actor actor)
        {
            ActorExtraData extraData = ET.PhysXComponent.Instance.GetActorExtraData(actor);
            return extraData != null && extraData.BodyType == BodyType.Bomb;
        }

        private static void RealOnContact(ContactPairHeader pairHeader, ContactPair[] pairs)
        {
            foreach (var pair in pairs)
            {
                Actor? actor0 = pair.Shape0?.Actor;
                Actor? actor1 = pair.Shape1?.Actor;
                Actor? actor = null;

                bool isActor0Bomb = IsBombByActor(actor0);
                bool isActor1Bomb = IsBombByActor(actor1);

                if (isActor0Bomb || isActor1Bomb)
                {
                    if (!(isActor0Bomb && isActor1Bomb))
                    {
                        actor = isActor0Bomb ? actor0 : isActor1Bomb ? actor1 : null;
                    }

                    if (actor != null)
                    {
                        Debug.WriteLine($"actor = {actor}");
                        RigidActor? rigidActor = actor as RigidActor;
                        if (rigidActor != null)
                        {
                            InLateUpdateNeedRemoveActorSet.Add(rigidActor);
                            AddExplosionForce(rigidActor.Scene, rigidActor.GlobalPosePosition, 10, 1000, ForceMode.Impulse);
                        }

                    }
                }
            }
        }

        public static void OnContact(ContactPairHeader pairHeader, ContactPair[] pairs)
        {
            RealOnContact(pairHeader, pairs);
        }

        public static SceneStage SceneStage;

        public static void PrintStage()
        {
            Debug.WriteLine($"SceneStage = {SceneStage}");
        }

        public static HashSet<RigidActor> InLateUpdateNeedRemoveActorSet = new HashSet<RigidActor>();

        public static void AddExplosionForce(Scene scene, Vector3 pos, float distance, float power ,ForceMode forceMode)
        {
            SphereGeometry geom = new SphereGeometry(radius: distance);
            OverlapHit[]? hits = null;
            bool status = scene.Overlap
            (
                geometry: geom,
                pose: Matrix4x4.CreateTranslation(pos),
                maximumOverlaps: 100,
                hitCall: hit =>
                {
                    hits = hit;

                    return true;
                }
            );

            if (hits != null)
            {
                foreach (OverlapHit hit in hits)
                {
                    RigidActor? actor = hit.Actor;
                    if (actor != null && actor.Type == ActorType.RigidDynamic)
                    {
                        RigidDynamic? dynamicActor = actor as RigidDynamic;
                        if (dynamicActor != null)
                        {
                            Vector3 forceVector = actor.GlobalPosePosition - pos;
                            forceVector = Vector3.Normalize(forceVector) * power;
                            dynamicActor.AddForceAtLocalPosition(forceVector, Vector3.Zero, ForceMode.Impulse, true);
                        }
                    }

                }
            }
        }
    }
}
