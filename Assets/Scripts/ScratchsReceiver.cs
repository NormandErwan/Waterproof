using System;
using System.Collections.Generic;
using Augmenta.UnityOSC;
using UnityEngine;

namespace Waterproof
{
    public sealed class OscReceivers : MonoBehaviour
    {
        public event Augmenta.UnityOSC.OSCReceiver.MessageReceived MessageReceived;

        [SerializeField]
        private int port = 12000;

        [SerializeField]
        private List<string> destinations = new List<string>();

        private void Create()
        {
            Remove();

            foreach (var destination in destinations)
            {
                if (Augmenta.UnityOSC.OSCMaster.CreateReceiver(destination, port) != null)
                {
                    Augmenta.UnityOSC.OSCMaster.Receivers[destination].messageReceived += MessageReceived;
                }
                else
                {
                    Debug.LogError($"Could not create OSC receiver at port {port}");
                }
            }
        }

        private void Remove()
        {
            if (Augmenta.UnityOSC.OSCMaster.Instance == null)
            {
                return;
            }

            foreach (var destination in destinations)
            {
                if (Augmenta.UnityOSC.OSCMaster.Receivers.ContainsKey(destination))
                {
                    Augmenta.UnityOSC.OSCMaster.Receivers[destination].messageReceived -= MessageReceived;
                    Augmenta.UnityOSC.OSCMaster.RemoveReceiver(destination);
                }
            }
        }
    }
}