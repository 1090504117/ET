using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysX
{
    public enum BodyType
    {
        None,
        Bomb,
        Player,
        Sphere,
        Cube,
        Wall
    }

    public enum SceneStage
    {
        PreUpdate,
        Update,
        LateUpdate,
    }

    public class UserData
    {
        public BodyType BodyType;
        public int Id;
    }

    public class Cube
    {
        public Vector3 Pos;
        public Quaternion Quat;
        public Vector3 HalfShap;
        public float Mass;
    }

    public class Sphere
    {
        public Vector3 Pos;
        public Quaternion Quat;
        public float Radius;
        public float Mass;
    }

    public class Plane
    {
        public Vector3 Pos;
        public Quaternion Quat;
        public Vector3 HalfShap;
    }

    public class Capsule
    {
        public Vector3 FootPos;
        public Quaternion Quat;
        public float Height;
        public float Redius;
        public Vector3 UpDirection;
        public float Mass;
    }

    public class ActorExtraData
    {
        public BodyType BodyType;
        public int ActorId;
    }


    static class PhysXWorldConst
    {
        public static Cube[] CubeArray = new Cube[]
        {
            new Cube(){ Pos=new Vector3(1,1,0), Quat=Quaternion.Identity, HalfShap=new Vector3(1,1,1), Mass=10},
            new Cube(){ Pos=new Vector3(-13,1,0), Quat=Quaternion.Identity, HalfShap=new Vector3(2,1,1), Mass=20},
            new Cube(){ Pos=new Vector3(5,1,0), Quat=Quaternion.Identity, HalfShap=new Vector3(4,1,1), Mass=100},
            new Cube(){ Pos=new Vector3(27,1,0), Quat=Quaternion.Identity, HalfShap=new Vector3(1,1,1), Mass=50},
        };

        public static Sphere[] SphereArray = new Sphere[]
        {
            new Sphere(){ Pos=new Vector3(1,10,0), Quat=Quaternion.Identity, Radius=1, Mass=10},
            new Sphere(){ Pos=new Vector3(3,10,0), Quat=Quaternion.Identity, Radius=3, Mass=20},
            new Sphere(){ Pos=new Vector3(5,10,0), Quat=Quaternion.Identity, Radius=2, Mass=100},
            new Sphere(){ Pos=new Vector3(7,10,0), Quat=Quaternion.Identity, Radius=4, Mass=50},
        };

        private static float WallPlaneHalfLength = 60;
        private static float WallPlaneHalfHeight = 1;

        public static Plane[] WallArray = new Plane[]
        {
            new Plane(){ Pos=new Vector3(0,-WallPlaneHalfHeight,0), Quat=Quaternion.Identity, HalfShap=new Vector3(WallPlaneHalfLength,WallPlaneHalfHeight,WallPlaneHalfLength)},
            new Plane(){ Pos=new Vector3(0,2*WallPlaneHalfLength+WallPlaneHalfHeight,0), Quat=Quaternion.Identity, HalfShap=new Vector3(WallPlaneHalfLength,WallPlaneHalfHeight,WallPlaneHalfLength)},
            new Plane(){ Pos=new Vector3(-(WallPlaneHalfLength+WallPlaneHalfHeight),WallPlaneHalfLength,0), Quat=Quaternion.Identity, HalfShap=new Vector3(WallPlaneHalfHeight,WallPlaneHalfLength,WallPlaneHalfLength)},
            new Plane(){ Pos=new Vector3((WallPlaneHalfLength+WallPlaneHalfHeight),WallPlaneHalfLength,0), Quat=Quaternion.Identity, HalfShap=new Vector3(WallPlaneHalfHeight,WallPlaneHalfLength,WallPlaneHalfLength)},
            new Plane(){ Pos=new Vector3(0,WallPlaneHalfLength,-(WallPlaneHalfLength+WallPlaneHalfHeight)), Quat=Quaternion.Identity, HalfShap=new Vector3(WallPlaneHalfLength,WallPlaneHalfLength,WallPlaneHalfHeight)},
            new Plane(){ Pos=new Vector3(0,WallPlaneHalfLength,(WallPlaneHalfLength+WallPlaneHalfHeight)), Quat=Quaternion.Identity, HalfShap=new Vector3(WallPlaneHalfLength,WallPlaneHalfLength,WallPlaneHalfHeight)},
        };

        public static Capsule Player = new Capsule()
        {
            Height = 2,
            Redius = 0.5f,
            FootPos = new Vector3(0, 0, -40),
            UpDirection = new Vector3(0, 1, 0),
            Mass = 50,
        };

        public static Sphere Bomb = new Sphere()
        {
            Pos = Vector3.Zero,
            Quat = Quaternion.Identity,
            Radius = 1,
            Mass = 0.5f,
        };
    }
}
