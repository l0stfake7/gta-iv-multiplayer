using SharpDX;
using System;

namespace MIVSDK
{
    public static class MathHelper
    {
        private static Vector3 RelativeBack { get { return new Vector3(0.0f, -1.0f, 0.0f); } }

        private static Vector3 RelativeBottom { get { return new Vector3(0.0f, 0.0f, -1.0f); } }

        private static Vector3 RelativeFront { get { return new Vector3(0.0f, 1.0f, 0.0f); } }

        private static Vector3 RelativeLeft { get { return new Vector3(-1.0f, 0.0f, 0.0f); } }

        private static Vector3 RelativeRight { get { return new Vector3(1.0f, 0.0f, 0.0f); } }

        private static Vector3 RelativeTop { get { return new Vector3(0.0f, 0.0f, 1.0f); } }

        private static Vector3 WorldDown { get { return new Vector3(0.0f, 0.0f, -1.0f); } }

        private static Vector3 WorldEast { get { return new Vector3(1.0f, 0.0f, 0.0f); } }

        private static Vector3 WorldNorth { get { return new Vector3(0.0f, 1.0f, 0.0f); } }

        private static Vector3 WorldSouth { get { return new Vector3(0.0f, -1.0f, 0.0f); } }

        /// <summary>
        /// Returns the world Up vector. (0,0,1)
        /// </summary>
        private static Vector3 WorldUp { get { return new Vector3(0.0f, 0.0f, 1.0f); } }

        private static Vector3 WorldWest { get { return new Vector3(-1.0f, 0.0f, 0.0f); } }

        static public Vector3 Around(Vector3 vect, float Distance)
        {
            return new Vector3(vect.X, vect.Y, vect.Z) + RandomXY() * Distance;
        }

        static public float DegreesToRadian(float deg)
        {
            return deg * (float)(Math.PI / 180.0f);
        }

        static public float DirectionToHeading(Vector3 dir)
        {
            dir.Z = 0.0f;
            dir.Normalize();
            return RadianToDegrees((float)-Math.Atan2(dir.X, dir.Y));
        }

        static public Vector3 DirectionToRotation(Vector3 dir, float roll = 0.0f)
        {
            dir = Vector3.Normalize(dir);

            Vector3 rotval;
            rotval.Z = -RadianToDegrees((float)Math.Atan2(dir.X, dir.Y));
            Vector3 rotpos = Vector3.Normalize(new Vector3(dir.Z, new Vector3(dir.X, dir.Y, 0.0f).Length(), 0.0f));

            rotval.X = RadianToDegrees((float)Math.Atan2(rotpos.X, rotpos.Y));

            rotval.Y = roll;
            return rotval;
        }

        static public float DistanceTo(Vector3 vect, Vector3 Position)
        {
            return (Position - new Vector3(vect.X, vect.Y, vect.Z)).Length();
        }

        static public float DistanceTo2D(Vector3 vect, Vector3 Position)
        {
            Position.Z = 0.0f;
            return (Position - new Vector3(vect.X, vect.Y, 0.0f)).Length();
        }

        static public Quaternion FromRotation(Vector3 Rotation)
        {
            float rotX = DegreesToRadian(Rotation.X);
            float rotY = DegreesToRadian(Rotation.Y);
            float rotZ = DegreesToRadian(Rotation.Z);

            Quaternion qyaw = Quaternion.RotationAxis(WorldUp, rotZ);
            qyaw.Normalize();
            Quaternion qpitch = Quaternion.RotationAxis(WorldEast, rotX);
            qpitch.Normalize();
            Quaternion qroll = Quaternion.RotationAxis(WorldNorth, rotY);
            qroll.Normalize();
            Quaternion yawpitch = qyaw * qpitch * qroll;
            yawpitch.Normalize();
            return yawpitch;
        }

        static public Vector3 HeadingToDirection(float Heading)
        {
            Heading = DegreesToRadian(Heading);
            Vector3 res = new Vector3();
            res.X = (float)-Math.Sin(Heading);
            res.Y = (float)Math.Cos(Heading);
            res.Z = 0.0f;
            return res;
        }

        static public Quaternion HeadingToQuaternion(float heading, float roll = 0.0f)
        {
            return MathHelper.FromRotation(HeadingToRotation(heading, roll));
        }

        static public Vector3 HeadingToRotation(float heading, float roll = 0.0f)
        {
            return MathHelper.DirectionToRotation(MathHelper.HeadingToDirection(heading), roll);
        }

        static public float RadianToDegrees(float rad)
        {
            return rad * (float)(180.0f / Math.PI);
        }

        static public Vector3 RandomXY()
        {
            Vector3 v;
            Random random = new Random();
            v.X = (float)(random.NextDouble() - 0.5);
            v.Y = (float)(random.NextDouble() - 0.5);
            v.Z = 0.0f;
            v.Normalize();
            return v;
        }

        static public Vector3 RandomXYZ()
        {
            Vector3 v;
            Random random = new Random();
            v.X = (float)(random.NextDouble() - 0.5);
            v.Y = (float)(random.NextDouble() - 0.5);
            v.Z = (float)(random.NextDouble() - 0.5);
            v.Normalize();
            return v;
        }

        static public Vector3 RotationToDirection(Vector3 Rotation)
        {
            float rotZ = DegreesToRadian(Rotation.Z);
            float rotX = DegreesToRadian(Rotation.X);
            float multXY = Math.Abs((float)Math.Cos(rotX));
            Vector3 res = new Vector3();
            res.X = (float)(-Math.Sin(rotZ)) * multXY;
            res.Y = (float)(Math.Cos(rotZ)) * multXY;
            res.Z = (float)(Math.Sin(rotX));
            return res;
        }

        static public float ToHeading(Vector3 vect)
        {
            return DirectionToHeading(new Vector3(vect.X, vect.Y, vect.Z));
        }

        static public Vector3 ToRotation(Quaternion quat)
        {
            float pitch = (float)Math.Atan2(2.0f * (quat.Y * quat.Z + quat.W * quat.X), quat.W * quat.W - quat.X * quat.X - quat.Y * quat.Y + quat.Z * quat.Z);
            float yaw = (float)Math.Atan2(2.0f * (quat.X * quat.Y + quat.W * quat.Z), quat.W * quat.W + quat.X * quat.X - quat.Y * quat.Y - quat.Z * quat.Z);
            float roll = (float)Math.Asin(-2.0f * (quat.X * quat.Z - quat.W * quat.Y));
            return new Vector3(RadianToDegrees(pitch), RadianToDegrees(roll), RadianToDegrees(yaw));
        }

        static public float Vector2Length(float X, float Y)
        {
            return (float)Math.Sqrt((X * X) + (Y * Y));
        }
    }
}