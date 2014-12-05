using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Net.Sockets;

namespace MIVClient
{
    public partial class TeleportCameraScript : Script
    {
        Camera camera;
        public Vector3 currentPosition;
        public bool inCameraMode;
        bool cameraActivated;
        public bool zoomIn;
        public bool zoomOut;
        float zoom;
        public enum MoveDirection
        {
            None, Left, Right, Up, Down
        }

        public MoveDirection direction;

        public TeleportCameraScript()
        {
            currentPosition = Player.Character.Position;
            inCameraMode = false;
            cameraActivated = false;
            zoomIn = false;
            zoomOut = false;
            zoom = 1000.0f;
            direction = MoveDirection.None;
            GTA.Timer gfxupdate = new Timer(1);
            gfxupdate.Tick += (s, o) =>
            {
                onUpdate();
            };
            gfxupdate.Start();

            PerFrameDrawing += TeleportCameraScript_PerFrameDrawing;
            KeyDown += TeleportCameraScript_KeyDown;
            KeyUp += TeleportCameraScript_KeyUp;

        }

        void TeleportCameraScript_KeyUp(object sender, KeyEventArgs e)
        {

            if (inCameraMode)
            {
                if (e.Key == System.Windows.Forms.Keys.Add)
                {
                    zoomIn = false;
                }
                if (e.Key == System.Windows.Forms.Keys.Subtract)
                {
                    zoomOut = false;
                }
                if (e.Key == System.Windows.Forms.Keys.Left || e.Key == System.Windows.Forms.Keys.Right
                    || e.Key == System.Windows.Forms.Keys.Up || e.Key == System.Windows.Forms.Keys.Down)
                {
                    direction = MoveDirection.None;
                }
            }
        }

        void TeleportCameraScript_KeyDown(object sender, KeyEventArgs e)
        {

            if (inCameraMode)
            {

                if (e.Key == System.Windows.Forms.Keys.Add)
                {
                    zoomIn = true;
                }
                if (e.Key == System.Windows.Forms.Keys.Subtract)
                {
                    zoomOut = true;
                }
                if (e.Key == System.Windows.Forms.Keys.Left)
                {
                    direction = MoveDirection.Left;
                }
                if (e.Key == System.Windows.Forms.Keys.Right)
                {
                    direction = MoveDirection.Right;
                }
                if (e.Key == System.Windows.Forms.Keys.Up)
                {
                    direction = MoveDirection.Up;
                }
                if (e.Key == System.Windows.Forms.Keys.Down)
                {
                    direction = MoveDirection.Down;
                }
                if (e.Key == System.Windows.Forms.Keys.Enter)
                {
                    Player.TeleportTo(currentPosition.X, currentPosition.Y);
                    inCameraMode = false;
                }
                if (e.Key == System.Windows.Forms.Keys.Q)
                {
                    inCameraMode = false;
                }
            }
            else
            {
                if (e.Key == System.Windows.Forms.Keys.Multiply)
                {
                    inCameraMode = true;
                }
            }
        }

        void TeleportCameraScript_PerFrameDrawing(object sender, GraphicsEventArgs e)
        {
            drawCross(e.Graphics);
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
                    currentPosition = Player.Character.Position;
                    Player.CanControlCharacter = false;
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
                    Player.CanControlCharacter = true;
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
