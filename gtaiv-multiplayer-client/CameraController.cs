using GTA;

namespace MIVClient
{
    public class CameraController
    {
        private Camera camera;
        private Client client;

        public CameraController(Client client)
        {
            this.client = client;
        }

        public Vector3 Direction
        {
            get
            {
                return camera.Direction;
            }
            set
            {
                if (camera == null) createCamera();
                camera.Direction = value;
            }
        }

        public float FOV
        {
            get
            {
                return camera.FOV;
            }
            set
            {
                if (camera == null) createCamera();
                camera.FOV = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return camera.Position;
            }
            set
            {
                if (camera == null) createCamera();
                camera.Position = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return camera.Rotation;
            }
            set
            {
                if (camera == null) createCamera();
                camera.Rotation = value;
            }
        }

        public void LookAt(Vector3 position)
        {
            if (camera == null) createCamera();
            camera.LookAt(position);
        }

        public void Reset()
        {
            if (camera == null) createCamera();
            camera.Deactivate();
            camera = null;
        }

        private void createCamera()
        {
            if (camera != null && camera.Exists())
            {
                camera.Deactivate();
            }
            camera = new Camera();
            camera.Position = Game.CurrentCamera.Position;
            camera.Direction = Game.CurrentCamera.Direction;
            camera.Rotation = Game.CurrentCamera.Rotation;
            camera.Activate();
        }
    }
}