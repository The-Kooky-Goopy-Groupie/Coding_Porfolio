﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NETWORK_ENGINE
{
    [RequireComponent(typeof(NetworkID))]
    public abstract class NetworkComponent : MonoBehaviour
    {
        public bool IsDirty = false;
        public bool IsClient;
        public bool IsServer;
        public bool IsLocalPlayer;
        public int Owner;
        public int Type;
        public int NetId;
        public NetworkCore MyCore;
        public NetworkID MyId;
        // Start is called before the first frame update
        public void Awake()
        {
            MyId = gameObject.GetComponent<NetworkID>();
            MyCore = GameObject.FindObjectOfType<NetworkCore>();
            if(MyCore == null)
            {
                throw new System.Exception("ERROR: There is no network core on the scene.");
            }
            if(MyId == null)
            {
                throw new System.Exception("ERROR: There is no network ID on this object");
            }
            StartCoroutine(SlowStart());
        }
        void Start()
        {
         
        }

        IEnumerator SlowStart()
        {
            yield return new WaitUntil(() => MyId.IsInit);
            IsClient = MyId.IsClient;
            IsServer = MyId.IsServer;
            IsLocalPlayer = MyId.IsLocalPlayer;
            Owner = MyId.Owner;
            Type = MyId.Type;
            NetId = MyId.NetId;
            StartCoroutine(SlowUpdate());
        }

        public abstract IEnumerator SlowUpdate();
        public abstract void HandleMessage(string flag, string value);

        public void SendCommand(string var, string value)
        {
            var = var.Replace('#', ' ');
            value = value.Replace('#', ' ');
            if (MyCore != null && MyCore.IsClient)
            {
                string msg = "COMMAND#" + MyId.NetId + "#" + var + "#" + value;
                MyId.AddMsg(msg);
            }
        }
        public void SendUpdate(string var, string value)
        {
            var = var.Replace('#', ' ');
            value = value.Replace('#', ' ');
            if (MyCore != null && MyCore.IsServer)
            {
                string msg = "UPDATE#" + MyId.NetId + "#" + var + "#" + value;
                MyId.AddMsg(msg);
            }
        }
    }
}
