using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIVSDK
{
    namespace Math
    {
        public class Vector4
        {
            public float X, Y, Z, W;
            public Vector4()
            {
                this.X = 0;
                this.Y = 0;
                this.Z = 0;
                this.W = 0;
            }

            public static Vector4 Zero
            {
                get { return new Vector4(); }
                set { }
            }

            public Vector4(float X, float Y, float Z, float W)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
                this.W = W;
            }

            public Vector4(Vector4 vec)
            {
                this.X = vec.X;
                this.Y = vec.Y;
                this.Z = vec.Z;
                this.W = vec.W;
            }

            public static bool operator ==(Vector4 v1, Vector4 v2)
            {
                return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z && v1.W == v2.W;
            }
            public static bool operator !=(Vector4 v1, Vector4 v2)
            {
                return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z || v1.W != v2.W;
            }
            public static Vector4 operator +(Vector4 v1, Vector4 v2)
            {
                Vector4 ret = new Vector4(v1);

                ret.X += v2.X;
                ret.Y += v2.Y;
                ret.Z += v2.Z;
                ret.W += v2.W;

                return ret;
            }
        }
    }
}