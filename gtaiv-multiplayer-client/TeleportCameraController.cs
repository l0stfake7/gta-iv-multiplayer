using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GTA;

namespace MIVClient
{
    public class TeleportCameraController
    {
        Client client;
        Camera camera;
        public Vector3 currentPosition;
        public bool inCameraMode;
        bool cameraActivated;
        public bool zoomIn;
        public bool zoomOut;
        float zoom;
        public enum MoveDirection
        {
            None,Left,Right,Up,Down
        }

        public MoveDirection direction;

        public TeleportCameraController(Client client)
        {
            this.client = client;
            currentPosition = client.getPlayerPed().Position;
            inCameraMode = false;
            cameraActivated = false;
            zoomIn = false;
            zoomOut = false;
            zoom = 1000.0f;
            direction = MoveDirection.None;
        }

        private float getPercentageZoom()
        {
            return ((zoom - 50.0f) / 1950.0f) + 0.05f * 1.7f;
        }

        public void onUpdate()
        {
            if (inCameraMode)
            {
                if (!cameraActivated)
                {
                    camera = new Camera();
                    camera.Activate();
                    currentPosition = client.getPlayerPed().Position;
                    client.getPlayer().CanControlCharacter = false;
                    cameraActivated = true;
                }
                float z = World.GetGroundZ(currentPosition, GroundType.Highest);
                camera.Position = currentPosition + new Vector3(0, -300 * (getPercentageZoom() - 0.05f * 3.0f), zoom + z);
                camera.LookAt(new Vector3(currentPosition.X, currentPosition.Y, z));
            }
            else
            {
                if (cameraActivated)
                {
                    camera.Deactivate();
                    //Game.DefaultCamera.Activate();
                    client.getPlayer().CanControlCharacter = true;
                    cameraActivated = false;
                }
            }
        }

        public void drawCross(GTA.Graphics e)
        {
            if (inCameraMode)
            {
                if (zoomIn)
                {
                    zoom -= 10.0f;
                    if (zoom < 50.0f) zoom = 50.0f;
                }
                else if (zoomOut)
                {
                    zoom += 10.0f;
                    if (zoom > 2000.0f) zoom = 2000.0f;
                }
                float z = getPercentageZoom();
                if (direction == MoveDirection.Left)
                {
                    currentPosition -= new Vector3(30 * z, 0, 0);
                }
                if (direction == MoveDirection.Right)
                {
                    currentPosition += new Vector3(30 * z, 0, 0);
                }
                if (direction == MoveDirection.Up)
                {
                    currentPosition += new Vector3(0, 30 * z, 0);
                }
                if (direction == MoveDirection.Down)
                {
                    currentPosition -= new Vector3(0, 30 * z, 0);
                }
                var horizontal = new RectangleF(0, Game.Resolution.Height / 2 - 2, Game.Resolution.Width, 4);
                var vertical = new RectangleF(Game.Resolution.Width / 2 - 2, 0, 4, Game.Resolution.Height);
                var color = Color.FromArgb(70, 0, 0, 0);
                e.DrawRectangle(horizontal, color);
                e.DrawRectangle(vertical, color);
            }
        }
    }
}
