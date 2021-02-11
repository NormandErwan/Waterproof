using System;
using System.Collections.Generic;
using Augmenta.UnityOSC;
using UnityEngine;
using UnityEngine.Events;

namespace Waterproof
{
    public sealed class OscReceivers : MonoBehaviour
    {
        private const string ReceiverId = "Waterproof-Receivers";

        public event Augmenta.UnityOSC.OSCReceiver.MessageReceived MessageReceived;

        [SerializeField]
        private int port = 12000;

        [SerializeField]
        private List<Destination> destinations = new List<Destination>();


        private void Start()
        {
            Create();
        }

        private void Create()
        {
            Remove();

            if (Augmenta.UnityOSC.OSCMaster.CreateReceiver(ReceiverId, port) != null)
            {
                Augmenta.UnityOSC.OSCMaster.Receivers[ReceiverId].messageReceived += message =>
                {
                    //print(message.Address);
                    foreach (var destination in destinations)
                    {
                        if (message.Address == destination.Address)
                        {
                            float value = (float)message.Data[0] / destination.MaxValue;
                            destination.Event.Invoke(value);
                        }
                    }
                };
            }
            else
            {
                Debug.LogError($"Could not create OSC receiver at port {port}");
            }
        }

        private void Remove()
        {
            if (Augmenta.UnityOSC.OSCMaster.Instance == null)
            {
                return;
            }

            if (Augmenta.UnityOSC.OSCMaster.Receivers.ContainsKey(ReceiverId))
            {
                //Augmenta.UnityOSC.OSCMaster.Receivers[ReceiverId].messageReceived -= MessageReceived;
                Augmenta.UnityOSC.OSCMaster.RemoveReceiver(ReceiverId);
            }
        }

        [Serializable]
        public struct Destination
        {
            public string Address;
            public float MaxValue;
            public UnityEvent<float> Event;
        }
    }
}