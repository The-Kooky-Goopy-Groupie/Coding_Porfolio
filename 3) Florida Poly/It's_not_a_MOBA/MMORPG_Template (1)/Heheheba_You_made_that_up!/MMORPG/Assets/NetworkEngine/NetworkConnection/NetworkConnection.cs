using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NETWORK_ENGINE
{

    public class NetworkConnection 
    {
        public int PlayerId;
        public Socket TCPCon;
        public Socket UDPCon;
        public NetworkCore MyCore;

        public byte[] TCPbuffer = new byte[1024];
        public StringBuilder TCPsb = new StringBuilder();
        public bool TCPDidRecieve = false;
        public bool TCPIsSending = false;

        public byte[] UDPbuffer = new byte[1024];
        public StringBuilder UDPsb = new StringBuilder();
        public bool UDPdidRecieve = false;


        //Dealing with disconnect
        public bool IsDisconnecting = false;
        public bool DidDisconnect = false;

        





        /// <summary>
        /// SEND STUFF
        /// This will deal with sending information across the current 
        /// NET_Connection's socket.
        /// </summary>
        /// <param name="byteData"></param>
        public void Send(byte[] byteData)
        {

            TCPCon.BeginSend(byteData, 0, byteData.Length, 0,
                new System.AsyncCallback(this.SendCallback), TCPCon);
            TCPIsSending = true;
        }

        private void SendCallback(System.IAsyncResult ar)
        {
            try
            {
                TCPIsSending = false;
                // Retrieve the socket from the state object.  
                //Socket handler = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.  
                //int bytesSent = handler.EndSend(ar);      
                if (IsDisconnecting && MyCore.IsClient)
                {

                    DidDisconnect = true;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("Sending Failed: "+e.ToString());
            }
        }


        /// <summary>
        /// RECEIVE FUNCTION
        /// </summary>

        public IEnumerator TCPRecv()
        {
            while (true)
            {
                try
                {
                    TCPCon.BeginReceive(TCPbuffer, 0, 1024, 0, new System.AsyncCallback(TCPRecvCallback), this);
                }
                catch (System.Exception e)
                {
                    //If this goes south, disconnec this connection.
                    MyCore.Disconnect(PlayerId);
                    break;
                    //Debug.Log("Recv error: " + e.ToString());
                }
                //Wait to recv messages
                yield return new WaitUntil(() => TCPDidRecieve);
                TCPDidRecieve = false;
                string responce = TCPsb.ToString();
                if (responce.Trim(' ') == "")
                {
                    //We do NOT want any empty strings.  It will cause a problem.
                    //MyCore.Disconnect(PlayerId);
                    
                }
                //Parse string
                string[] commands = responce.Split('\n');
                for (int i = 0; i < commands.Length; i++)
                {
                    if (commands[i].Trim(' ') == "")
                    {
                        continue;
                    }
                    if (commands[i].Trim(' ') == "OK" && MyCore.IsClient)
                    {
                        Debug.Log("Client Recieved OK.");
                        //Do nothing, just a heartbeat
                    }
                    else if (commands[i].StartsWith("PlayerID"))
                    {
                        if (MyCore.IsClient)
                        {
                            try
                            {
                                //This is how the client get's their player id.
                                //All values will be seperated by a '#' mark.
                                //PLayerID#<NUM> will signify the player ID for this connection.
                                PlayerId = int.Parse(commands[i].Split('#')[1]);
                                MyCore.LocalPlayerId = PlayerId;
                            }
                            catch (System.FormatException)
                            {
                                Debug.Log("Got scrambled Message: " + commands[i]);
                            }
                        }
                        else
                        {//Should never happne
                        }
                    }
                    else if (commands[i].StartsWith("DISCON#"))
                    {
                        if (MyCore.IsServer)
                        {
                            try
                            {
                                int badCon = int.Parse(commands[i].Split('#')[1]);
                                MyCore.Disconnect(badCon);
                                Debug.Log("There are now only " + MyCore.Connections.Count + " players in the game.");
                            }
                            catch (System.FormatException)
                            {
                                Debug.Log("We received a scrambled message+ " + commands[i]);
                            }
                            catch (System.Exception e)
                            {
                                Debug.Log("Unkown exception: " + e.ToString());
                            }
                        }
                        else
                        {//If client
                            MyCore.LeaveGame();
                        }
                    }
                    else if (commands[i].StartsWith("CREATE"))
                    {
                        if (MyCore.IsClient)
                        {
                            string[] arg = commands[i].Split('#');
                            try
                            {
                                int o = int.Parse(arg[2]);
                                int n = int.Parse(arg[3]);
                                Vector3 pos = new Vector3(float.Parse(arg[4]), float.Parse(arg[5]), float.Parse(arg[6]));
                                int type = int.Parse(arg[1]);
                                GameObject Temp;
                                if (type != -1)
                                {
                                    Temp = GameObject.Instantiate(MyCore.SpawnPrefab[int.Parse(arg[1])], pos, Quaternion.identity);
                                }
                                else
                                {
                                    Temp = GameObject.Instantiate(MyCore.NetworkPlayerManager, pos, Quaternion.identity);
                                }
                                Temp.GetComponent<NetworkID>().Owner = o;
                                Temp.GetComponent<NetworkID>().NetId = n;
                                Temp.GetComponent<NetworkID>().Type = type;
                                MyCore.NetObjs[n] = Temp.GetComponent<NetworkID>();
                                lock(MyCore._masterMessage)
                                {   //Notify the server that we need to get update on this object.
                                    MyCore.MasterMessage += "DIRTY#" + n+"\n";
                                }
                            }
                            catch
                            {
                                //Malformed packet.
                            }
                        }
                    }
                    else if (commands[i].StartsWith("DELETE"))
                    {
                        if(MyCore.IsClient)
                        {
                            try
                            {
                                string[] args = commands[i].Split('#');
                                if (MyCore.NetObjs.ContainsKey(int.Parse(args[1])))
                                {
                                    GameObject.Destroy(MyCore.NetObjs[int.Parse(args[1])].gameObject);
                                    MyCore.NetObjs.Remove(int.Parse(args[1]));
                                }

                            }
                            catch (System.Exception e)
                            {
                                Debug.Log("ERROR OCCURED: " + e);
                            }
                        }
                    }
                    else if (commands[i].StartsWith("DIRTY"))
                    {
                        if(MyCore.IsServer)
                        {
                            int id = int.Parse(commands[i].Split('#')[1]);
                            if (MyCore.NetObjs.ContainsKey(id))
                            {
                                foreach (NetworkComponent n in MyCore.NetObjs[id].gameObject.GetComponents<NetworkComponent>())
                                {
                                    n.IsDirty = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        //We will assume it is Game Object specific message
                        //string msg = "COMMAND#" + myId.netId + "#" + var + "#" + value;
                        string[] args = commands[i].Split('#');
                        int n = int.Parse(args[1]);
                        if(MyCore.NetObjs.ContainsKey(n))
                        {
                            MyCore.NetObjs[n].Net_Update(args[0], args[2], args[3]);
                        }
                    }
                }

                TCPsb.Length = 0;
                TCPsb = new StringBuilder();
                TCPDidRecieve = false;
                yield return new WaitForSeconds(.05f);
            }
        }
        private void TCPRecvCallback(System.IAsyncResult ar)
        {
            try
            {
                NetworkConnection temp = (NetworkConnection)ar.AsyncState;
                int bytesRead = -1;
                bytesRead = TCPCon.EndReceive(ar);
                if (bytesRead > 0)
                {
                    temp.TCPsb.Append(Encoding.ASCII.GetString(temp.TCPbuffer, 0,
                        bytesRead));
                    string ts = temp.TCPsb.ToString();
                    if (ts[ts.Length - 1] != '\n')
                    {
                        TCPCon.BeginReceive(TCPbuffer, 0, 1024, 0, new System.AsyncCallback(TCPRecvCallback), this);
                    }
                    else
                    {
                        temp.TCPDidRecieve = true;
                    }
                }
            }
            catch (System.Exception e)
            {
                //We have to disconnect at the main thread not on the 
                //call back.
            }
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}