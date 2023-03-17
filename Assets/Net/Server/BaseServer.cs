using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public class BaseServer : MonoBehaviour
{
    public NetworkDriver driver; //the interface to speak anything network related.
    protected NativeList<NetworkConnection> connections; //required to have a list for every connection to the server.

#if UNITY_EDITOR //Only works in the unity editor! (This is to make the application run outside the editor.
    private void Start() { Init(); }
    private void Update() { UpdateServer(); }
    private void OnDestroy() { Shutdown(); }
#endif

    /*Virtual is used because this is a 'base' class.
     * If you want to override something you can do that in a child class.
     */
    public virtual void Init()
    {
        //Init the driver.
        driver = NetworkDriver.Create(); //Creating the driver
        NetworkEndPoint endPoint = NetworkEndPoint.AnyIpv4; //who can connect to server.
        endPoint.Port = 5522;

        if (driver.Bind(endPoint) != 0) //Connection error (port based).
        {
            Debug.Log("An error occurd! Cannot bind to port: " + endPoint.Port);
        } else
        {
            driver.Listen(); //server listening to other clients.
        }

        //init the connection list.
        connections = new NativeList<NetworkConnection>(4, Allocator.Persistent); //Creating list (max players)
    }
    public virtual void Shutdown()
    {
        //Deletes the driver and connection.
        driver.Dispose();
        connections.Dispose();
    }
    public virtual void UpdateServer()
    {
        driver.ScheduleUpdate().Complete();
        CleanupConn(); 
        AcceptNewConn();
        UpdateMessagePump(); 
    }
    private void CleanupConn() //When client disconnects abrupt.
    {
        for (int i = 0; i < connections.Length; i++) //Go trough all connections.
        {
            if (!connections[i].IsCreated) //If connection is not created.
            {
                connections.RemoveAtSwapBack(i); //remove connection.
                --i;
            }
        }
    }
    private void AcceptNewConn() //See if new clients try to connect to server, if yes add to connection list.
    {
        NetworkConnection conn;
        while ((conn = driver.Accept()) != default(NetworkConnection)) //If there is a new connection to be made and it it not the default one.
        {
            connections.Add(conn);
            Debug.Log("Accepted a new Connection!");
        }
    }
    protected virtual void UpdateMessagePump() //Server pharse all message that clients send to server to apply to server.
    {
        DataStreamReader stream; //
        for (int i = 0; i < connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    byte OpCode = stream.ReadByte();
                    FixedString128Bytes chatmessage = stream.ReadFixedString128();
                    Debug.Log("Got " + OpCode + "from the client.");
                    Debug.Log("Got " + chatmessage + "from the client.");
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client has been disconnected from the server");
                    connections[i] = default(NetworkConnection);
                }
            }
        }
    }
}
