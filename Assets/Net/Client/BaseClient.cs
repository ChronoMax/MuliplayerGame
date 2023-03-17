using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public class BaseClient : MonoBehaviour
{
    public NetworkDriver driver; //the interface to speak anything network related.
    protected NetworkConnection connection;

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
        connection = default(NetworkConnection); //setting the connection to default.

        NetworkEndPoint endPoint = NetworkEndPoint.LoopbackIpv4; //connect to localhost
        //NetworkEndPoint endPoint = NetworkEndPoint.TryParse("192.168.178.122"); //connect to other ip
        endPoint.Port = 5522;

        connection = driver.Connect(endPoint); //connection to server
    }
    public virtual void Shutdown()
    {
        //Deletes the connection.
        driver.Dispose();
    }
    public virtual void UpdateServer()
    {
        driver.ScheduleUpdate().Complete();
        CheckAlive();
        UpdateMessagePump();
    }    
    private void CheckAlive()
    {
        if (!connection.IsCreated)
        {
            Debug.Log("Lost connection to server!");
        }
    }
    protected virtual void UpdateMessagePump() //Server pharse all message that clients send to server to apply to server.
    {
        DataStreamReader stream;

        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("You are connected to the server.");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                OnData(stream);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Disconnected from the server!");
            }
        }
    }
    public virtual void OnData(DataStreamReader stream)
    {
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();

        switch (opCode)
        {
            case OpCode.CHAT_MESSAGE:
                msg = new Net_ChatMessage(stream);
                break;
            case OpCode.PLAYER_POS:
                msg = new Net_PlayerPos(stream);
                break;
            default:
                Debug.Log("No OpCode recieved!");
                break;
        }

        msg.RecievedOnClient();
    }
    public virtual void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer); //write to server.
        msg.Serialize(ref writer); //fill the data.
        driver.EndSend(writer); //send to server.
    }
}
