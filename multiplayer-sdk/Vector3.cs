using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIVSDK
{
    namespace Math
    {
        public class Vector3
        {
            public float X, Y, Z;
            public Vector3()
            {
                this.X = 0;
                this.Y = 0;
                this.Z = 0;
            }

            public static Vector3 Zero
            {
                get { return new Vector3(); }
                set { }
            }

            public Vector3(float X, float Y, float Z)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
            }

            public Vector3(Vector3 vec)
            {
                this.X = vec.X;
                this.Y = vec.Y;
                this.Z = vec.Z;
            }

            public static bool operator ==(Vector3 v1, Vector3 v2)
            {
                if ((object)v1 == null && (object)v2 == null) return true;
                if ((object)v1 == null && (object)v2 != null) return false;
                if ((object)v1 != null && (object)v2 == null) return false;
                return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
            }
            public static bool operator !=(Vector3 v1, Vector3 v2)
            {
                if ((object)v1 == null && (object)v2 == null) return false;
                return (object)v1 == null || (object)v2 == null || v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
            }
            public static Vector3 operator +(Vector3 v1, Vector3 v2)
            {
                Vector3 ret = new Vector3(v1);

                ret.X += v2.X;
                ret.Y += v2.Y;
                ret.Z += v2.Z;

                return ret;
            }
        }
    }

}