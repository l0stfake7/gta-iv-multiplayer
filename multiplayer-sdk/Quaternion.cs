namespace MIVSDK
{
    namespace Math
    {
        public class Quaternion : Vector4
        {
            public Quaternion(float X, float Y, float Z, float W)
                : base(X, Y, Z, W)
            {
            }

            public static Quaternion Zero
            {
                get { return new Quaternion(0, 0, 0, 0); }
                set { }
            }
        }
    }
}