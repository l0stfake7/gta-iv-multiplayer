using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace MIVClient
{
    public class CameraController
    {
        Client client;
        Camera camera;

        public CameraController(Client client)
        {
            this.client = client;
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
    }
}
