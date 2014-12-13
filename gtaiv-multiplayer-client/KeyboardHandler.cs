// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using GTA;
using MIVSDK;
using System.Collections.Generic;
using System.Linq;

namespace MIVClient
{
    public class KeyboardHandler
    {
        public int cursorpos = 0;
        public bool inKeyboardTypingMode;

        private Client client;
        private List<string> commandHistory;
        private int historyIndex = 0;
        private GTA.KeyboardLayoutUS keyboardUS;

        private int? lastKeyUp, lastKeyDown;

        public KeyboardHandler(Client client)
        {
            this.client = client;
            keyboardUS = new KeyboardLayoutUS();
            inKeyboardTypingMode = false;
            commandHistory = new List<string>();
            client.KeyDown += new GTA.KeyEventHandler(this.eventOnKeyDown);
            client.KeyUp += new GTA.KeyEventHandler(this.eventOnKeyUp);
        }

        private void eventOnKeyDown(object sender, GTA.KeyEventArgs e)
        {
            if (lastKeyDown != null && lastKeyDown == (int)e.Key) return;
            if (!inKeyboardTypingMode && e.Key == System.Windows.Forms.Keys.T)
            {
                inKeyboardTypingMode = true;
                cursorpos = 0;
                historyIndex = 0;
                client.getPlayer().CanControlCharacter = false;
            }
            else if (inKeyboardTypingMode)
            {
                if (e.Key == System.Windows.Forms.Keys.Enter)
                {
                    if (client.chatController.currentTypedText.Length > 0)
                    {
                        commandHistory.Add(client.chatController.currentTypedText);
                        if (commandHistory.Count > 100)
                        {
                            commandHistory = commandHistory.Skip(commandHistory.Count - 100).ToList();
                        }
                        var bpf = new BinaryPacketFormatter(Commands.Chat_sendMessage);
                        bpf.add(client.chatController.currentTypedText);
                        client.serverConnection.write(bpf.getBytes());
                    }
                    client.chatController.currentTypedText = "";
                    cursorpos = 0;
                    inKeyboardTypingMode = false;
                    client.getPlayer().CanControlCharacter = true;
                }
                else if (e.Key == System.Windows.Forms.Keys.Escape)
                {
                    client.chatController.currentTypedText = "";
                    cursorpos = 0;
                    inKeyboardTypingMode = false;
                }
                else if (e.Key == System.Windows.Forms.Keys.Left)
                {
                    cursorpos = cursorpos > 0 ? cursorpos - 1 : cursorpos;
                }
                else if (e.Key == System.Windows.Forms.Keys.Down)
                {
                    if (commandHistory.Count > 0)
                    {
                        if (historyIndex <= 1)
                        {
                            client.chatController.currentTypedText = commandHistory[commandHistory.Count - 1];
                            cursorpos = 0;
                        }
                        else
                        {
                            historyIndex--;
                            if (historyIndex < 0) historyIndex = 0;
                            client.chatController.currentTypedText = commandHistory[commandHistory.Count - historyIndex];
                            cursorpos = client.chatController.currentTypedText.Length;
                        }
                    }
                }
                else if (e.Key == System.Windows.Forms.Keys.Up)
                {
                    if (commandHistory.Count > 0)
                    {
                        historyIndex++;
                        if (historyIndex > commandHistory.Count) historyIndex = commandHistory.Count;
                        client.chatController.currentTypedText = commandHistory[commandHistory.Count - historyIndex];
                        cursorpos = client.chatController.currentTypedText.Length;
                    }
                }
                else if (e.Key == System.Windows.Forms.Keys.Right)
                {
                    cursorpos = cursorpos >= client.chatController.currentTypedText.Length ? client.chatController.currentTypedText.Length : cursorpos + 1;
                }
                else if (e.Key == System.Windows.Forms.Keys.Back)
                {
                    if (cursorpos > 0)
                    {
                        string leftcut = cursorpos > 0 ? client.chatController.currentTypedText.Substring(0, cursorpos - 1) : client.chatController.currentTypedText;
                        string rightcut = client.chatController.currentTypedText.Substring(cursorpos, client.chatController.currentTypedText.Length - cursorpos);
                        client.chatController.currentTypedText = leftcut + rightcut;
                        cursorpos = cursorpos > 0 ? cursorpos - 1 : cursorpos;
                    }
                }
                else
                {
                    string leftcut = client.chatController.currentTypedText.Substring(0, cursorpos);

                    string rightcut =
                        cursorpos >= client.chatController.currentTypedText.Length ?
                        "" :
                        client.chatController.currentTypedText.Substring(cursorpos, client.chatController.currentTypedText.Length - cursorpos);
                    string newstr = keyboardUS.ParseKey((int)e.Key, e.Shift, e.Control, e.Alt);
                    client.chatController.currentTypedText = leftcut + newstr + rightcut;
                    cursorpos += newstr.Length;
                }
                return;
            }

            if (e.Key == System.Windows.Forms.Keys.G)
            {
                Vehicle veh = World.GetClosestVehicle(client.getPlayerPed().Position, 20.0f);
                if (veh != null && veh.Exists())
                {
                    VehicleSeat seat = veh.GetFreePassengerSeat();
                    client.getPlayerPed().Task.EnterVehicle(veh, seat);
                }
            }

            if (client.currentState == ClientState.Connected)
            {
                var bpf = new BinaryPacketFormatter(Commands.Keys_down);
                bpf.add((int)e.Key);
                client.serverConnection.write(bpf.getBytes());
            }

            lastKeyDown = (int)e.Key;
            lastKeyUp = 0;
        }

        private void eventOnKeyUp(object sender, GTA.KeyEventArgs e)
        {
            if (lastKeyUp != null && lastKeyUp == (int)e.Key) return;
            if (client.currentState == ClientState.Connected)
            {
                var bpf = new BinaryPacketFormatter(Commands.Keys_up);
                bpf.add((int)e.Key);
                client.serverConnection.write(bpf.getBytes());
            }

            lastKeyUp = (int)e.Key;
            lastKeyDown = 0;
        }
    }
}