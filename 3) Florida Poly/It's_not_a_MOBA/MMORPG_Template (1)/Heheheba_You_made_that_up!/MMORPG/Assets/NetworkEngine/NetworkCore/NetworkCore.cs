using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.WebSockets;
using System;

namespace NETWORK_ENGINE
{

    public class NetworkCore : MonoBehaviour
    {
        
        public string IpAddress;
        public int Port;
        public bool IsConnected = false;
        public bool IsServer = false;
        public bool IsClient = false;
        public bool CanJoin = true;
        public int LocalPlayerId = -1;
        public bool CurrentlyConnecting = false;

        public Dictionary<int, NetworkConnection> Connections;
        public Dictionary<int, NetworkID> NetObjs;

        public int ObjectCounter = 0;
        public int ConCounter = 0;
        public int MaxConnections = 100;
        public GameObject[] SpawnPrefab;

        public Socket TCP_Listener;
        public Socket UDP_Listener;
        public float MasterTimer = .05f;
        Coroutine ListeningThread;
        public string MasterMessage;

        public GameObject NetworkPlayerManager;//This will be the first thing that is spawned!

        //Locks
        public object _conLock = new object();
        public object _objLock = new object();
        public object _masterMessage = new object();

        public DateTime StartConnection;

        // Use this for initialization
        void Start()
        {

            IsServer = false;
            IsClient = false;
            IsConnected = false;
            CurrentlyConnecting = false;
            //ipAddress = "127.0.0.1";//Local host
            if (IpAddress == "")
            {
                IpAddress = "127.0.0.1";//Local host
            }
            if (Port == 0)
            {
                Port = 9001;
            }
            Connections = new Dictionary<int, NetworkConnection>();
            NetObjs = new Dictionary<int, NetworkID>();
        }


        /// <summary>
        /// Server Functions
        /// StartServer -> Initialize Listener and Slow Update
        ///     - WIll spawn the first prefab as a "NetworkPlayerManager"
        /// Listen -> Will bind to a port and allow clients to join.
        /// </summary>
        public void StartServer()
        {
            if (!IsConnected)
            {
                IsServer = true;
                IsClient = false;
                IsConnected = true;
                ListeningThread = StartCoroutine(Listen());
                StartCoroutine(SlowUpdate());
            }
        }

        public void StopListening()
        {
            if(IsServer && CanJoin)
            {
                
                CurrentlyConnecting = false;
                StopCoroutine(ListeningThread);
                TCP_Listener.Close();
            }
        }

        public IEnumerator Listen()
        {
            //If we are listening then we are the server.
            IsServer = true;
            IsConnected = true;
            IsClient = false;
            LocalPlayerId = -1; //For server the localplayer id will be -1.
                                //Initialize port to listen to

            
            IPAddress ip = (IPAddress.Any);
            IPEndPoint endP = new IPEndPoint(ip, Port);
            //We could do UDP in some cases but for now we will do TCP
            TCP_Listener = new Socket(ip.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            //Now I have a socket listener.
            TCP_Listener.Bind(endP);
            TCP_Listener.Listen(MaxConnections);
            Debug.Log("We are now listening");
            while(CanJoin)
            {
                CurrentlyConnecting = false;
                
                TCP_Listener.BeginAccept(new System.AsyncCallback(this.ListenCallBack), TCP_Listener);               
                yield return new WaitUntil(() => CurrentlyConnecting);
                DateTime time2 = DateTime.Now;
                TimeSpan timeS = time2 - StartConnection;

                //waiting here
                Debug.Log("Latency for connection setup is: " + timeS.TotalSeconds);
                
                CurrentlyConnecting = false;
                if (Connections.ContainsKey(ConCounter - 1) == false)
                {
                    //Connection was not fully established.
                    continue;
                }
                Connections[ConCounter - 1].Send(Encoding.ASCII.GetBytes("PlayerID#" + Connections[ConCounter - 1].PlayerId + "\n"));
                //Start Server side listening for client messages.
                StartCoroutine(Connections[ConCounter - 1].TCPRecv());
                yield return new WaitForSeconds(2*(float)timeS.TotalSeconds);
                //Udpate all current network objects
                foreach (KeyValuePair<int,NetworkID> entry in NetObjs)
                {//This will create a custom create string for each existing object in the game.
                    
                    string MSG = "CREATE#" + entry.Value.Type + "#" + entry.Value.Owner +
                   "#" + entry.Value.NetId + "#" + entry.Value.transform.position.x.ToString("n2") + 
                   "#" + entry.Value.transform.position.y.ToString("n2") + "#" 
                   + entry.Value.transform.position.z.ToString("n2") + "\n";
                    Connections[ConCounter - 1].Send(Encoding.ASCII.GetBytes(MSG));
                    yield return new WaitForSeconds(2 * (float)timeS.TotalSeconds);
                }
                //Create NetworkPlayerManager
                NetCreateObject(-1, ConCounter - 1, Vector3.zero);
                yield return new WaitForSeconds(.1f);
            }
        }
        public void ListenCallBack(System.IAsyncResult ar)
        {
            StartConnection = DateTime.Now;
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            NetworkConnection temp = new NetworkConnection();
            temp.TCPCon = handler;
            temp.PlayerId = ConCounter;
            ConCounter++;
            temp.MyCore = this;
            lock (_conLock)
            {
                Connections.Add(temp.PlayerId, temp);
                Debug.Log("There are now " + Connections.Count +
                    " player(s) connected.");
            }
            CurrentlyConnecting = true;
        }

        public void CloseGame()
        {
            if (IsServer && IsConnected && CanJoin)
            {
                CanJoin = false;
                StopCoroutine(ListeningThread);
            }
        }

        /// <summary>
        /// Client Functions 
        /// Start Client - Will join with a server specified
        /// at IpAddress and Port.
        /// </summary>

        public void StartClient()
        {
            if (!IsConnected)
            {
                IsServer = false;
                IsClient = false;
                IsConnected = false;
                CurrentlyConnecting = false;
                StartCoroutine(ConnectingClient());
            }
        }
        public IEnumerator ConnectingClient()
        {
            IsClient = false;
            IsServer = false;
            //Setup our socket
            IPAddress ip = (IPAddress.Parse(IpAddress));
            IPEndPoint endP = new IPEndPoint(ip, Port);
            Socket clientSocket = new Socket(ip.AddressFamily, SocketType.Stream,
                ProtocolType.Tcp);
            //Connect client
            clientSocket.BeginConnect(endP, ConnectingCallback, clientSocket);
            Debug.Log("Trying to wait for server...");
            //Wait for the client to connect
            yield return new WaitUntil(() => CurrentlyConnecting);
            StartCoroutine(Connections[0].TCPRecv());  //It is 0 on the client because we only have 1 socket.
            StartCoroutine(SlowUpdate());  //This will allow the client to send messages to the server.
        }
        public void ConnectingCallback(System.IAsyncResult ar)
        {
            //Client will use the con list (but only have one entry).
            NetworkConnection temp = new NetworkConnection();
            temp.TCPCon = (Socket)ar.AsyncState;
            temp.TCPCon.EndConnect(ar);//This finishes the TCP connection (DOES NOT DISCONNECT)
           
            IsConnected = true;
            IsClient = true;
            temp.MyCore = this;

            Connections.Add(0, temp);
            CurrentlyConnecting = true;
        }
        /// <summary>
        /// Disconnect functions
        /// Leave game 
        /// Disconnect
        /// OnClientDisconnect -> is virtual so you can override it
        /// </summary>
        public void Disconnect(int badConnection)
        {
            Debug.Log("Trying to disconnect: " + badConnection);
            
            if (IsClient)
            {
                if (Connections.ContainsKey(badConnection))
                {
                    NetworkConnection badCon = Connections[badConnection];
                    try
                    {
                        badCon.TCPCon.Shutdown(SocketShutdown.Both);
                    }
                    catch
                    { }
                    //but for now we will close it.
                    try
                    {
                        badCon.TCPCon.Close();
                    }
                    catch
                    {

                    }
                }
                this.IsClient = false;
                this.IsServer = false;
                this.IsConnected = false;
                this.LocalPlayerId = -10;
                foreach (KeyValuePair<int, NetworkID> obj in NetObjs)
                {
                    Destroy(obj.Value.gameObject);
                }
                NetObjs.Clear();
                Connections.Clear();              
            }
            if (IsServer)
            {
                try
                {
                    if (Connections.ContainsKey(badConnection))
                    {
                        NetworkConnection badCon = Connections[badConnection];
                        badCon.TCPCon.Shutdown(SocketShutdown.Both);
                        badCon.TCPCon.Close();
                    }
                }
                catch (System.Net.Sockets.SocketException)
                {
                    Debug.Log("Connection " + badConnection + " is already Closed!  Removing Objects.");
                }
                catch (System.ObjectDisposedException)
                {
                    Debug.Log("Socket already shutdown: ");
                }
                catch
                {
                    //In case anything else goes wrong.
                    Debug.Log("Warning - Error caught in the generic catch!");
                }
                //Delete All other players objects....
                OnClientDisc(badConnection);
                Connections.Remove(badConnection);
            }
        }
        public virtual void OnClientDisc(int badConnection)
        {
            if (IsServer)
            {
                Debug.Log("Here!");
                //Remove Connection from server
                List<int> badObjs = new List<int>();
                foreach (KeyValuePair<int, NetworkID> obj in NetObjs)
                {
                    if (obj.Value.Owner == badConnection)
                    {
                        badObjs.Add(obj.Key);
                        //I have to add the key to a temp list and delete
                        //it outside of this for loop
                    }
                }
                //Now I can remove the netObjs from the dictionary.
                for (int i = 0; i < badObjs.Count; i++)
                {
                    NetDestroyObject(badObjs[i]);
                }
            }
        }
        public void LeaveGame()
        {
            if (IsClient && IsConnected)
            {
                try
                {
                    lock (_conLock)
                    {
                        Debug.Log("Sending Disconnect!");
                        Connections[0].IsDisconnecting = true;

                        Connections[0].Send(Encoding.ASCII.
                                         GetBytes(
                                         "DISCON#" + Connections[0].PlayerId.ToString() + "\n")
                                         );
                        
                    }
                }
                catch (System.NullReferenceException)
                {
                    //Client double-tapped disconnect.
                    //Ignore.
                }
                StartCoroutine(WaitForDisc());
            }
            if (IsServer && IsConnected)
            {
                Debug.Log("A");
                
                foreach (KeyValuePair<int, NetworkConnection> obj in Connections)
                {
                    lock (_conLock)
                    {
                        Debug.Log("Sending Disconnect!");
                        Connections[obj.Key].Send(Encoding.ASCII.
                                         GetBytes(
                                         "DISCON#-1\n")
                                         );
                        Connections[obj.Key].IsDisconnecting = true;
                    }
                }
                Debug.Log("B");
                IsServer = false;
                try
                {
                    foreach (KeyValuePair<int, NetworkID> obj in NetObjs)
                    {
                        Destroy(obj.Value.gameObject);
                    }
                }
                catch (System.NullReferenceException)
                {
                    //Objects already destroyed.
                }
                try
                {
                    foreach (KeyValuePair<int, NetworkConnection> entry in Connections)
                    {
                        Disconnect(entry.Key);
                    }
                }
                catch (System.NullReferenceException)
                {
                    Debug.Log("Inside Disonnect error!");
                    //connections already destroyed.
                }
                Debug.Log("C");
                IsConnected = false;
                IsClient = false;
                CurrentlyConnecting = false;
                CanJoin = true;
                try
                {
                    NetObjs.Clear();
                    Connections.Clear();
                    StopCoroutine(ListeningThread);  
                    TCP_Listener.Close();
                    
                }
                catch (System.NullReferenceException)
                {
                    Debug.Log("Inside error.");
                    NetObjs = new Dictionary<int, NetworkID>();
                    Connections = new Dictionary<int, NetworkConnection>();
                }
                //StopAllCoroutines();
                Debug.Log("D");
            }
        }
        IEnumerator WaitForDisc()
        {
            if (IsClient)
             {
                yield return new WaitUntil(() => Connections[0].DidDisconnect);
                Disconnect(0);
            }
            yield return new WaitForSeconds(.1f);
        }
        public void OnApplicationQuit()
        {
            LeaveGame();
        }
        /// <summary>
        /// Object functions
        /// NetCreateObject -> creates an object across the network
        /// NetDestroyObject -> Destroys an object across the network
        /// </summary>
        public GameObject NetCreateObject(int type, int ownMe, Vector3 initPos)
        {
            if (IsServer)
            {
                GameObject temp;
                lock(_objLock)
                {
                    if (type != -1)
                    {
                        temp = GameObject.Instantiate(SpawnPrefab[type], initPos, Quaternion.identity);
                    }
                    else
                    {
                        temp = GameObject.Instantiate(NetworkPlayerManager, initPos, Quaternion.identity);
                    }
                    temp.GetComponent<NetworkID>().Owner = ownMe;
                    temp.GetComponent<NetworkID>().NetId = ObjectCounter;
                    temp.GetComponent<NetworkID>().Type = type;
                    NetObjs[ObjectCounter] = temp.GetComponent<NetworkID>();
                    ObjectCounter++;
                    string MSG = "CREATE#" + type + "#" + ownMe +
                    "#" + (ObjectCounter - 1) + "#" + initPos.x.ToString("n2") + "#" +
                    initPos.y.ToString("n2") + "#" + initPos.z.ToString("n2") + "\n";
                    lock(_masterMessage)
                    {
                        MasterMessage += MSG;
                    }
                    foreach(NetworkComponent n in temp.GetComponents<NetworkComponent>())
                    {
                        //Force update to all clients.
                        n.IsDirty = true;
                    }
                }
                return temp;
            }
            else
            {
                return null;
            }

        }
        public void NetDestroyObject(int netIDBad)
        {
            try
            {
                if (NetObjs.ContainsKey(netIDBad))
                {
                    Destroy(NetObjs[netIDBad].gameObject);
                    NetObjs.Remove(netIDBad);
                }
            }
            catch
            {
                //Already been destroyed.
            }
            string msg = "DELETE#" + netIDBad+"\n";
            lock(_masterMessage)
            {
                MasterMessage += msg;
            }
            
        }


        /// <summary>
        /// Support functions
        /// Slow Update()
        /// SetIP address
        /// SetPort
        /// </summary>

        public IEnumerator SlowUpdate()
        {
            while (true)
            {
                //Compose Master Message

                foreach(KeyValuePair<int, NetworkID> id in NetObjs)
                {
                    lock (_masterMessage)
                    {
                        //Add their message to the masterMessage (the one we send)
                        lock (id.Value._lock)
                        {
                            MasterMessage += id.Value.GameObjectMessages + "\n";
                            //Clear Game Objects messages.
                            id.Value.GameObjectMessages = "";
                        }

                    }

                }

                //Send Master Message
                List<int> bad = new List<int>();
                if(MasterMessage != "")
                {
                    foreach(KeyValuePair<int,NetworkConnection> item in Connections)
                    {
                        try
                        {
                            //This will send all of the information to the client (or to the server if on a client).
                            item.Value.Send(Encoding.ASCII.GetBytes(MasterMessage));
                        }
                        catch
                        {
                            bad.Add(item.Key);
                        }
                    }
                    lock(_masterMessage)
                    {
                        MasterMessage = "";//delete old values.
                    }
                    lock (_conLock)
                    {
                        foreach (int i in bad)
                        {
                            this.Disconnect(i);
                        }
                    }
                }
                yield return new WaitForSeconds(MasterTimer);
            }
        }

        public void SetIp(string ip)
        {
            IpAddress = ip;
        }
        public void SetPort(string p)
        {
            Port = int.Parse(p);
        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}
