﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ExclusiveDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
{
    private Dictionary<K, V> dictionary;
    public int Count
    {
        get
        {
            lock (DLock)
            {
                return dictionary.Count;
            }
        }
    }
    Object DLock;
    public bool ContainsKey(K key)
    {
        lock (DLock)
        {
            return dictionary.ContainsKey(key);
        }
    }
    public ExclusiveDictionary()
    {
        dictionary = new Dictionary<K, V>();
        DLock = new Object();
    }
    public V this[K key]
    {
        get
        {
            lock (DLock)
            {
                if (this.dictionary.ContainsKey(key))
                {
                    return this.dictionary[key];
                }
                return default(V);
            }
        }
        set
        {
            lock (DLock)
            {
                this.dictionary[key] = value;
            }
        }
    }
    public bool Remove(K key)
    {
        lock (DLock)
        {
            if (dictionary.ContainsKey(key))
            {

                dictionary.Remove(key);
                return true;
            }
            return false;
        }
    }
    public void Clear()
    {
        lock (DLock)
        {
            this.dictionary.Clear();
        }
    }
    public void Add(K key, V value)
    {
        lock (DLock)
        {
            dictionary.Add(key, value);
        }
    }
    public ExclusiveDictionary<K, V> Copy()
    {
        lock (DLock)
        {
            ExclusiveDictionary<K, V> temp = new ExclusiveDictionary<K, V>();
            foreach (KeyValuePair<K, V> x in dictionary)
            {
                temp[x.Key] = x.Value;
            }
            return temp;
        }
    }
    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
    {
        lock (DLock)
        {
            foreach (KeyValuePair<K, V> x in dictionary)
            {
                yield return x;
            }
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
public class ProducerConsumerQueue<T>
{
    List<T> data;
    object ListLock;
    public int Count
    {
        get
        {
            lock (ListLock)
            { return data.Count; }
        }
    }

    public void Append(T value)
    {
        lock (ListLock)
        {
            data.Add(value);
        }
    }

    public T Consume()
    {
        if (Count > 0)
        {
            lock (ListLock)
            {
                T temp = data[0];
                data.RemoveAt(0);
                return temp;
            }
        }
        return default(T);
    }

    public bool IsEmpty()
    {
        lock (ListLock)
        {
            return data.Count == 0;
        }
    }
    public T Peek()
    {
        lock (ListLock)
        {
            return data[data.Count - 1];
        }
    }
    public ProducerConsumerQueue()
    {
        data = new List<T>();
        ListLock = new object();
    }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>


public class ExclusiveString : IEnumerable<char>
{
    public bool IsDirty = false;
    private string data;
    private Object SLock;
    public char this[int k]
    {
        get { lock (SLock) { return data[k]; } }
    }
    public string Str
    {
        get { lock (SLock) { IsDirty = false; return data; } }

    }
    public int Length
    {
        get { lock (SLock) { return data.Length; } }
    }

    public string GetData()
    {
        lock (SLock)
        {
            return data.Clone().ToString();
        }
    }
    public void SetData(string s)
    {
        lock (SLock)
        {
            IsDirty = true;
            data = s;
        }
    }

    public ExclusiveString()
    {
        data = "";
        SLock = new Object();
    }
    public static ExclusiveString Parse(string s)
    {
        ExclusiveString temp = new ExclusiveString();
        temp.SetData(s);
        return temp;
    }

    public static ExclusiveString operator +(ExclusiveString a, ExclusiveString b)
    {
        ExclusiveString temp = new ExclusiveString();
        temp.SetData(a.GetData() + b.GetData());
        return temp;
    }
    public static ExclusiveString operator +(ExclusiveString a, string b)
    {
        ExclusiveString temp = new ExclusiveString();
        lock (a.SLock)
        {
            temp.SetData(a.GetData() + b);
        }
        return temp;
    }
    public override string ToString()
    {
        lock (SLock)
        {
            return data;
        }
    }
    public void Append(string s)
    {
        lock (SLock)
        {
            data += s;
        }
    }
    public string ReadAndClear()
    {
        string temp = "";
        lock (SLock)
        {
            temp = string.Copy(data);
            data = "";
            return temp;
        }
    }
    public void Trim()
    {
        lock (SLock)
        {
            data = data.Trim();
        }
    }
    public IEnumerator<char> GetEnumerator()
    {
        lock (SLock)
        {
            for (int i = 0; i < data.Length; i++)
            {
                yield return data[i];
            }
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


public class Connector2
{
    public System.DateTime StartCall;
    public Socket socket;
    public bool usingUDP;
    public int connectionID;
    public float latency;
    public bool acknowledged;
    public bool closing;
    public Thread thread;
    public ProducerConsumerQueue<string> TCPMessage;
    public object SocketLocker;
    public Coroutine MessageHandler;
    public Connector2(Socket s, int ID, bool useUdp = false)
    {
        socket = s;
        connectionID = ID;
        latency = 1;
        acknowledged = false;
        closing = false;
        TCPMessage = new ProducerConsumerQueue<string>();
        usingUDP = useUdp;
        SocketLocker = new object();
    }
}
public class ThreadNetworkSocket2
{
    
    public string IP;
    public int Port;
    public int packetSize = 1024;
    public ExclusiveDictionary<int, Connector2> Connections;
    public int ConCounter = 0;
    public ProducerConsumerQueue<int> NewCon;
    public  Thread ThreadListener;
    public bool IsConnected = false;
    public bool IsServer = false;
    public bool IsClient = false;
    public int AppNumber = 129829;
    public int ThreadTimer = 1000;
    public Socket Listener;
    public ThreadNetworkSocket2()
    {
        IP = "127.0.0.1";
        Port = 9000;
        NewCon = new ProducerConsumerQueue<int>();
        Connections = new ExclusiveDictionary<int, Connector2>();
}


    public bool Send(Connector2 s, string msg)
    {
        if (s.closing)
        {
            return false;
        }
        lock (s.SocketLocker)
        {
            try
            {
                List<string> msgs = new List<string>();
                if (msg.Length > packetSize)//Should stop overflow.
                {
                    int oldCursor = 0;
                    int cursor = packetSize;
                    while (cursor <= msg.Length - 1)
                    {
                        while (msg[cursor] != '\n')
                        {
                            cursor--;
                        }
                        msgs.Add(msg.Substring(oldCursor, cursor));
                        oldCursor = cursor + 1;
                        cursor += packetSize;
                        if (cursor > packetSize)
                        {
                            cursor = msg.Length - 1;
                        }
                    }
                }
                else
                {
                    msgs.Add(msg);
                }
                foreach (string mm in msgs)
                {
                    byte[] byteData = ASCIIEncoding.ASCII.GetBytes(mm+"\n");
                    int check = s.socket.Send(byteData);
                    if (check != byteData.Length)
                    {
                        throw new System.Exception("ERROR: Socket did not send as many bytes as it was supposed to!");
                    }
                }
                return true;
            }
            catch
            {
                s.closing = true;
                DisconnectConnection(s.connectionID);
                return false;
            }
        }
    }


    public void Receive(Connector2 R)
    {
        //This is A Thread
        try
        {
            while (IsConnected)
            {
                if (R.closing)
                {
                    break;
                }
                string tempMessage = "";
                byte[] TCPbuffer = new byte[packetSize];
                int byteRead = 0;
                byteRead += R.socket.Receive(TCPbuffer, packetSize, SocketFlags.None);
                //TCPMessage.Append(Encoding.ASCII.GetString(TCPbuffer));
                tempMessage += Encoding.ASCII.GetString(TCPbuffer);
               
                if(R.acknowledged)
                {
                    R.TCPMessage.Append(tempMessage);
                }
                string[] Commands = tempMessage.Split('\n');
                foreach(string C in Commands)
                {
                    if(C.StartsWith("ID#") && IsServer)
                    {
                        if(int.Parse(C.Split('#')[1]) == R.connectionID && int.Parse(C.Split('#')[2]) == AppNumber)
                        {
                            R.acknowledged = true;
                        }
                    }
                    if(C.StartsWith("ID") && IsClient)
                    {
                        try
                        {
                            R.connectionID = int.Parse(C.Split('#')[1]);
                        }
                        catch
                        { 
                            Debug.LogError("Could not read ID");
                            break;
                        }
                        Send(R, "ID#" + R.connectionID + "#" + AppNumber + "\n");
                        R.acknowledged = true;
                    }
                    if(C.StartsWith("DISCON\n") )
                    {
                        if (!R.closing)
                        {
                            R.closing = true;
                            Send(R, "DISCON\n" + R.connectionID + "\n");
                        }
                        else
                        {
                            DisconnectConnection(R.connectionID);
                        }
                    }
                    if (C.StartsWith("LAT#"))
                    {
                        R.latency = float.Parse(C.Split('#')[1].Split('\n')[0]);
                    }
                    if (C.StartsWith("OK"))
                    {
                        if (IsClient)
                        {
                            Send(R,"OK\n");
                        }
                        if (IsServer)
                        {
                            R.latency = (float)(System.DateTime.Now - R.StartCall).TotalSeconds;
                            Send(R,"LAT#" + R.latency.ToString("F2")+"\n");
                        }
                    }
                }
                Thread.Sleep(ThreadTimer);
            }
        }
        catch
        {

        }
    }



    public bool StartServer()
    {
        if(IsConnected)
        {
            return false;
        }
        try
        {
            ThreadListener = new Thread(this.ListenForConnections);
            ThreadListener.Start();
        }
        catch
        {
            Debug.LogError("Could not start Thread!");
            return false;
        }
        IsServer = true;
        IsClient = false;
        IsConnected = true;
        return true;
    }

    //Thread
    public void ListenForConnections()
    {
        
        try
        {
            Listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(new IPEndPoint(IPAddress.Any, Port));
            Listener.Listen(10);
            //Waiting on connections.
            while (IsConnected)
            {
                try
                {
                    Socket handler = Listener.Accept();
                    Debug.Log("Someone is trying to connect!");
                    if (!handler.Poll(500, SelectMode.SelectWrite))
                    {
                        //Connection failed to write
                        try
                        {
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }
                        catch
                        {
                            Debug.Log("Serverhandler could not write!");
                        }
                        continue;
                    }
                    Connector2 temp = new Connector2(handler, ConCounter);
                    ConCounter++;
                    try
                    {
                        temp.thread = new Thread(() => this.Receive(temp));
                        temp.thread.Start();
                    }
                    catch {
                        Debug.Log("Server Did not start thread");
                        continue; 
                    }
                    Debug.Log("Temp ID connection ID = " + temp.connectionID);
                    Send(temp, "ID#" + temp.connectionID + "\n");
                    for(int i =0; i < 100; i ++)
                    {
                        if(temp.acknowledged)
                        {
                            break;
                        }
                        Thread.Sleep(50);
                    }
                    if(temp.acknowledged)
                    {   
                        Connections.Add(temp.connectionID, temp);
                        NewCon.Append(temp.connectionID);

                        Debug.Log("Server Setup!");
                    }
                    else
                    {
                        //Failed to called
                        Debug.Log("Connection failed!");
                        try
                        {
                            temp.socket.Shutdown(SocketShutdown.Both);
                            temp.socket.Close();
                        }
                        catch { }
                    }
                }
                catch (System.Exception E)
                { Debug.Log("Internal Try threw! "+E.ToString()); }
            }
        }
        catch (System.Exception e)
        { Debug.Log("Master Try threw! : "+e.ToString()); }

    }

    //This is start client and connect
    //Expects IP and Port to be set.
    public bool Connect()
    {
        Connections = new ExclusiveDictionary<int, Connector2>();

        IPAddress ipAdd = (IPAddress.Parse(IP));
        IPEndPoint endP = new IPEndPoint(ipAdd, Port);
        Socket con = new Socket(SocketType.Stream, ProtocolType.Tcp);
        try
        {
            con.Connect(endP);
        }
        catch
        {
            //return false;
        }
        IsConnected = true;
        IsClient = true;
        while (con.Poll(10, SelectMode.SelectWrite) == false)
        {
            Thread.Sleep(10);
        }
        Connector2 temp = new Connector2(con, -1);
        try
        {
            temp.thread = new Thread(() => this.Receive(temp));
            temp.thread.Start();
        }
        catch
        {
            //return false;
        }
        Connections.Add(0, temp);
        //Need to work with connect Client!
        
        return true;
        
        //Will be start client as well as connect from Generic Core 1.
    }

    //This disconnects a specific connection 
    //Does not handle hand-shaking.
    public bool DisconnectConnection(int index)
    {
        if (Connections.ContainsKey(index))
        {
            Connector2 test = Connections[index];
            test.closing = true;
            if (!Connections[index].thread.Join(500))
            {
                Connections[index].thread.Abort();
            }
        }
        lock (Connections[index].SocketLocker)
        {
            try
            {
                Connections[index].socket.Shutdown(SocketShutdown.Both);
                Connections[index].socket.Close();
            }
            catch
            { return false;}
        }
        Connections.Remove(index);
        IsServer = false;
        IsClient = false;
        IsConnected = false;
        ConCounter = 0;
        return true;
    }
}

public class GenericNetworkCore : MonoBehaviour
{
    public static int MaxConsoleLogSize = 1024;
    public bool UsingUDP = false;
    protected ThreadNetworkSocket2 NetSystem = new ThreadNetworkSocket2();
    public bool IsServer 
        {
            get{return NetSystem.IsServer;}
            set{ NetSystem.IsServer = value; }
        }
    public bool IsClient
    {
        get { return NetSystem.IsClient; }
        set { NetSystem.IsClient = value; }
    }
    public bool IsConnected
    {
        get { return NetSystem.IsConnected; }
        set { NetSystem.IsConnected = value; }
    }
    public ExclusiveDictionary<int,Connector2> Connections
    {
        get { return NetSystem.Connections; }
        set { NetSystem.Connections = value;}
    }

    public string IP
    {
        get { return NetSystem.IP; }
        set { NetSystem.IP = value;}
    }    
    public int PortNumber
    {
        get { return NetSystem.Port; }
        set { NetSystem.Port = value;}
    }
    public static string SystemLog = "";
    public float MasterTimer = .05f;
    public int LocalConnectionID = -10;

    public IEnumerator ServerStart()
    {
        if (!IsConnected)
        {
            NetSystem.ThreadTimer = (int)(1000 * MasterTimer);
            NetSystem.StartServer();
            yield return new WaitUntil(() => IsConnected);
            LocalConnectionID = -1;
            StartCoroutine(SlowUpdate());
            StartCoroutine(OnServerStart());
            GenericNetworkCore.Logger("Server has started!");
        }
    }

    private void OnApplicationQuit()
    {
        try
        {
            NetSystem.ThreadListener.Abort();
        }
        catch
        { }
        foreach (KeyValuePair<int,Connector2> c in Connections)
        {
            try
            {
                c.Value.thread.Abort();
            }
            catch { }
        }
    }

    public IEnumerator ClientStart()
    {
        if (!IsConnected)
        {
            NetSystem.ThreadTimer = (int)(1000 * MasterTimer);
            bool NoContact = false;
            try
            {
                NetSystem.Connect();
            }
            catch
            {
                NoContact = true;
            }
            if (NoContact)
            {
                //If  you cannot connect as a client go back to the main menu scene.
                string message = "Could not Connect to Generic Server!";
                GenericNetworkCore.Logger(message);
                yield return new WaitForSeconds(10);
                SceneManager.LoadScene(0);
            }
            while (LocalConnectionID < 0)
            {
                yield return new WaitForSeconds(.2f);
                LocalConnectionID = NetSystem.Connections[0].connectionID;
            }
            StartCoroutine(SlowUpdate());
            Connections[0].MessageHandler = StartCoroutine(TCPHandleMessages(0));
            yield return new WaitUntil(() => IsClient);
            StartCoroutine(OnClientConnect(LocalConnectionID));
            GenericNetworkCore.Logger("Client connected to Generic Server!");
        }
    }

    private IEnumerator SlowUpdate()
    {

        int cycleCounter = 0;
        while (IsConnected)
        {

            if (IsServer)
            {
                foreach (KeyValuePair<int, Connector2> p in Connections.Copy())
                {
                    if (p.Value.closing)
                    {
                        GenericNetworkCore.Logger("Disconnecting: " + p.Key);
                        try
                        {
                            StartCoroutine(Disconnect(p.Key, true));
                        }
                        catch (System.Exception e)
                        {
                            Debug.Log("Recieved the following error when disconnecting server on " + name + ": " + e.ToString());
                        }
                    }
                }
                cycleCounter++;
                if (cycleCounter % 100 == 0)
                {
                    try
                    {
                        foreach (KeyValuePair<int, Connector2> p in Connections)
                        {
                            p.Value.StartCall = System.DateTime.Now;
                            NetSystem.Send(p.Value,"OK\n");
                        }
                    }
                    catch
                    {
                        //Dictionary corrupt due to a disconnect.
                        //Ignore and try again.
                    }
                }
                if (cycleCounter >= int.MaxValue)
                {
                    cycleCounter = 0;
                }
                //This is the only way to add new clients on ther server due to threads.
                try
                {
                    while (NetSystem.NewCon.Count > 0)
                    {
                        int newId = NetSystem.NewCon.Consume();
                        if (Connections.ContainsKey(newId))
                        {
                            Connections[newId].MessageHandler = StartCoroutine(TCPHandleMessages(newId));
                            StartCoroutine(OnClientConnect(newId));
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Logger("Recieved this exception while trying to start client's handle message. MSG=" + e.ToString());
                }
            }
            if (IsClient)
            {
                if (Connections[0] != null && Connections[0].closing)
                {
                    yield return StartCoroutine(Disconnect(LocalConnectionID));
                }
            }
            OnSlowUpdate();
            yield return new WaitForSeconds(MasterTimer);
        }
    }

    public bool Send(string msg, int connectionID, bool useTcp = true)
    {
        try
        {
            if(Connections.ContainsKey(connectionID))
            {
                if(!NetSystem.Send(Connections[connectionID], msg))
                {
                    try
                    {
                        StopCoroutine(Connections[connectionID].MessageHandler);
                    }
                    catch { }
                }
            }
        }
        catch 
        {
            return false;
        }
        return true;
    }


    public IEnumerator Disconnect(int id, bool sendMsg = false)
    {
        if (IsClient)
        {
            try
            {
                StopCoroutine(Connections[0].MessageHandler);
            }
            catch { }

            try
                {
                    Connections[0].closing = true;
                    NetSystem.Send(Connections[0],"DISCON\n");
                }
                catch 
                {
                    NetSystem.DisconnectConnection(0);
                }
            StartingDisconnect();
              
            if (Connections[0] == null)
            {
                yield break;
            }
            yield return StartCoroutine(OnClientDisconnect(id));
           // yield return StartCoroutine(SocketCloser(0));
            try
            {          
                Connections.Clear();
                LocalConnectionID = -10;
            }
            catch { }
            IsConnected = false;
            GenericNetworkCore.Logger("Disconnected from " + name + ":" + this.GetType().ToString());
            OnClientDisconnectCleanup(id);
        }
        if (IsServer)
        {
            if (Connections.ContainsKey(id))
            {
                Connections[id].closing = true;
                try
                {
                    NetSystem.Send(Connections[id], "DISCON\n");
                }
                catch
                {
                    NetSystem.DisconnectConnection(id);
                }
            }                 
            StartCoroutine(OnClientDisconnect(id));
            Connections.Remove(id);
            GenericNetworkCore.Logger("Client " + id + " disconnected from " + name + ":" + this.GetType().ToString());
            OnClientDisconnectCleanup(id);
        }
    }

    private IEnumerator TCPHandleMessages(int ConID)
    {
        while (IsConnected && Connections[ConID]!= null && !Connections[ConID].closing)
        {
            string msg = "";
            int timeOut = 10;
            while (IsConnected && Connections[ConID].TCPMessage.Count > 0 && !Connections[ConID].closing)
            {
                msg += Connections[ConID].TCPMessage.Consume();
                timeOut--;
                if (timeOut > 1)
                {
                    break;
                }
            }

            if (!IsConnected || Connections[ConID] == null)    //This should break...?
            {
                break;
            }

            string[] args = msg.Split('\n');
            foreach (string x in args)
            {
                if (x.Trim() != null)
                {
                    try
                    {
                        OnHandleMessages(x);
                    }
                    catch (System.Exception e)
                    {
                        GenericNetworkCore.Logger("TCP malformed a packet - or exception in handle message: " + e.ToString());
                    }
                }
            }
           yield return new WaitForSeconds(MasterTimer/2);//Wait for one frame - in case message floods.
         }
  
        
    }
    public IEnumerator DisconnectServer()
    {
        if (IsServer)
        {
            foreach (KeyValuePair<int, Connector2> c in Connections)
            {
                c.Value.closing = true;
                NetSystem.Send(c.Value, "DISCON\n");
            }
            yield return new WaitForSeconds(.5f);

            foreach (KeyValuePair<int, Connector2> c in Connections)
            {
                NetSystem.DisconnectConnection(c.Value.connectionID);
            }
            try
            {
                NetSystem.Listener.Close();
                NetSystem.Listener.Dispose();
            }
            catch { }
            NetSystem.ThreadListener.Abort();


            Connections.Clear();
            IsConnected = false;
            IsServer = false;
            IsClient = false;
            NetSystem.ConCounter = 0;
            OnServerDisconnectCleanup();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //------------------------------------------------------------Virtual Functions ------------------------------------

    /// <summary>
    /// Virtual functions OnClientConnect
    /// Sends in the ID of the new client
    /// Gives the programmer an option to 
    /// perform data.
    /// </summary>
    /// <param name="id"></param>
    ///<returns>IEnumerator in case you have to wait for something to initialize.</returns>
    public virtual IEnumerator OnClientConnect(int id)
    {
        yield return new WaitForSeconds(.1f);
    }

    /// <summary>
    /// Virtual function to read messages coming from the 
    /// Socket.
    /// Will be called after the GenericNetworkCore 
    /// Has read through them first.
    /// Will already be parsed to individual commands
    /// (Therefore, called multiple times)
    /// </summary>
    /// <param name="commands"></param>
    public virtual void OnHandleMessages(string commands)
    {
        Logger("Received a message: " + commands);
    }

    /// <summary>
    /// Allows the programmer to insert code on SlowUpdate
    /// NOTE!  You cannot while true inside this function!
    /// </summary>
    public virtual void OnSlowUpdate()
    {

    }

    /// <summary>
    /// Allows the programmer to insert code when the server is started.
    /// Sever values shoudl be initialized and set.
    /// </summary>
    /// <returns>IEnumerator in case you have to wait for something to initialize.</returns>
    public virtual IEnumerator OnServerStart()
    {
        yield return new WaitForSeconds(.1f);
    }
    /// <summary>
    /// Allows the programmer to insert code when a client disconnects.
    /// This will be called on both the client and the server.
    /// Note this will be called before disconnect happens.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns>IEnumerator in case you have to wait for something to initialize.</returns>
    public virtual IEnumerator OnClientDisconnect(int id)
    {
        yield return new WaitForSeconds(.1f);
    }

    /// <summary>
    /// Will be called when the server disconnects. 
    /// Allows the programmer to inject cleanup code.
    /// Note this will be called before disconnect happens.
    /// </summary>
    /// /// <returns>IEnumerator in case you have to wait for something to initialize.</returns>
    public virtual IEnumerator OnServerDisconnect()
    {
        yield return new WaitForSeconds(.1f);
    }

    /// <summary>
    /// This funcion is called after the disconnect has occured for a client.
    /// </summary>
    /// <param name="id">The id of the connection that was deleted.  Note the connection is already deleted.</param>
    public virtual void OnClientDisconnectCleanup(int id)
    {

    }

    /// <summary>
    /// This function is called after the server has disonnected and shut down.
    /// </summary>
    public virtual void OnServerDisconnectCleanup()
    {

    }

    /// <summary>
    /// This function will log all output.  
    /// All logs are added to a static public variable SystemLog that can be sent out to an output.
    /// </summary>
    /// <param name="msg">The messge to add to the log.</param>
    public static void Logger(string msg)
    {
        if (SystemLog.Length > GenericNetworkCore.MaxConsoleLogSize)
        {
            SystemLog = "";
        }
        Debug.Log(System.DateTime.Now.ToString() + ": " + msg);
        //SystemLog += System.DateTime.Now.ToString() + ": " + msg + "\n";
    }

    public virtual IEnumerator MenuManager()
    {
        yield break;
    }

    /// <summary>
    /// This function will be called when IsDisconnecting is set to true.
    /// There can be different elements that cause this, error, player quitting, etc.
    /// This is an oppurtunity for the programmmer to playce a UI or Panel screen to hide any messy
    /// visual artifacts of the disconnect.
    /// </summary>
    public virtual void StartingDisconnect()
    {

    }

    //-------------------------------------------------------------UI Call back functions ------------------------------------------------
    /// <summary>
    /// UI Call back functions
    /// </summary>
    public void UI_Quit()
    {
        try
        {
            if (IsConnected)
            {
                if (IsClient)
                {
                    StartCoroutine(Disconnect(Connections[0].connectionID, true));
                }
                if (IsServer)
                {
                    StartCoroutine(DisconnectServer());
                   
                }
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        catch (System.Exception e)
        { Debug.Log("Disconnect inside " + name + " had the following error: " + e.ToString()); }


    }
    /// <summary>
    /// UI callback to start the client.
    /// </summary>
    public void UI_StartClient()
    {
        StartCoroutine(ClientStart());
    }
    /// <summary>
    /// UI Callback to Start the server.
    /// </summary>
    public void UI_StartServer()
    {
        StartCoroutine(ServerStart());
    }
    /// <summary>
    /// UI call back-  Needs to be dynamic string from input field
    /// Will ensure it is a good IP address then set it.
    /// Otherwise, default is home IP address.
    /// </summary>
    /// <param name="ipAddr"></param>
    public void UI_SetIP(string ipAddr)
    {
        try
        {
            IPAddress.Parse(ipAddr);
            IP = ipAddr;
        }
        catch
        {
            IP = "127.0.0.1";
        }
    }
    /// UI callback - Needs to by dynamic string from an input field.
    /// Will ensure it is a good integer, then set the Port number.
    /// Otherwise, default is 9000!
    /// </summary>
    /// <param name="n"></param>
    public void UI_SetPort(string n)
    {
        try
        {
            PortNumber = int.Parse(n);
        }
        catch
        {
            PortNumber = 9000;
        }
    }

}
