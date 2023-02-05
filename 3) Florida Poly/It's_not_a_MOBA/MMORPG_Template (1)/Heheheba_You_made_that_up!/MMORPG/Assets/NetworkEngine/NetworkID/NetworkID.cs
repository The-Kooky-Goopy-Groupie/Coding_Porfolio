using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NETWORK_ENGINE
{

    public class NetworkID : MonoBehaviour
    {
        public int Type;
        public int Owner = -10;
        public int NetId = -10;
        public bool IsInit;
        public bool IsLocalPlayer;
        public bool IsServer;
        public bool IsClient;
        public float UpdateFrequency = .1f;
        public NetworkCore MyCore;
        public string GameObjectMessages = "";
        public object _lock = new object();


        // Use this for initialization
        void Start()
        {
            MyCore = GameObject.FindObjectOfType<NetworkCore>();
            if(MyCore == null)
            {
                throw new System.Exception("There is no network core in the scene!");
            }
            IsServer = MyCore.IsServer;
            IsClient = MyCore.IsClient;
            StartCoroutine(SlowStart());
        }
        IEnumerator SlowStart()
        {
            if (!IsServer && !IsClient)
            {
                //This will ONLY be true if the object was in the scene before the connection
                yield return new WaitUntil(() => (MyCore.IsServer || MyCore.IsClient));
                IsClient = MyCore.IsClient;
                IsServer = MyCore.IsServer;
                yield return new WaitForSeconds(.1f);
            }
            if (IsClient)
            {
                //Then we know we need to destroy this object and wait for it to be re-created by the server
                if (NetId == -10)
                {
                    yield return new WaitForSeconds(.1f);
                    Debug.Log("We are destroying the non-server networked objects");
                    GameObject.Destroy(this.gameObject);
                }
            }
            if (IsServer && NetId == -10)
            {
                
                //We need to add ourselves to the networked object dictionary
                Type = -1;
                Debug.Log("We are adding the object to the netobjs dictionary.");
                for (int i = 0; i < MyCore.SpawnPrefab.Length; i++)
                {
                    if (MyCore.SpawnPrefab[i].gameObject.name == this.gameObject.name.Split('(')[0].Trim())
                    {
                        Type = i;
                        Debug.Log("Type is " + Type);
                        break;
                    }
                }
                if (Type == -1)
                {
                    Debug.LogError("Game object not found in prefab list! Game Object name - " + this.gameObject.name.Split('(')[0].Trim());
                    throw new System.Exception("FATAL - Game Object not found in prefab list!");
                }
                else
                {
                    lock (MyCore._objLock)
                    {
                        NetId = MyCore.ObjectCounter;
                        MyCore.ObjectCounter++;
                        Owner = -1;
                        MyCore.NetObjs.Add(NetId, this);
                    }
                }
            }

            yield return new WaitUntil(() => (Owner != -10 && NetId != -10));
            if(Owner == MyCore.LocalPlayerId)
            {
                IsLocalPlayer = true;
            }
            else
            {
                IsLocalPlayer = false;
            }
            IsInit = true;
        }
        public void AddMsg(string msg)
        {
            //Debug.Log("Message WAS: " + gameObjectMessages);
            //May need to put race condition blocks here.
            lock (_lock)
            {
                GameObjectMessages += (msg + "\n");
            }
            //Debug.Log("Message IS NOW: " + gameObjectMessages);
        }


        public void Net_Update(string type, string var, string value)
        {
            //Get components for network behaviours
            //Destroy self if owner connection is done.
            try
            {
                if (MyCore.IsServer && MyCore.Connections.ContainsKey(Owner) == false && Owner != -1)
                {
                    MyCore.NetDestroyObject(NetId);
                }
            }
            catch (System.NullReferenceException)
            {
                //Has not been initialized yet.  Ignore.
            }
            try
            {
                if (MyCore == null)
                {
                    MyCore = GameObject.FindObjectOfType<NetworkCore>();
                }
                if ((MyCore.IsServer && type == "COMMAND")
                    || (MyCore.IsClient && type == "UPDATE"))
                    {
                        NetworkComponent[] myNets = gameObject.GetComponents<NetworkComponent>();
                        for (int i = 0; i < myNets.Length; i++)
                        {
                            myNets[i].HandleMessage(var, value);
                        }
                    }
            }
            catch (System.Exception e)
            {
                Debug.Log("Caught Exception: " + e.ToString());
                //This can happen if myCore has not been set.  
                //I am not sure how this is possible, but it should be good for the next round.
            }
        }

        public void NotifyDirty()
        {
            this.AddMsg("DIRTY#" + NetId);
        }
    }
}