using Unity.Networking.Transport;
using UnityEngine;

public class NetMessage
{
    public OpCode code { get; set; }

    public virtual void Serialize(ref DataStreamWriter writer)
    {

    }

    public virtual void Deserialize(DataStreamReader reader)
    {

    }

    public virtual void RecievedOnClient()
    {

    }
    public virtual void RecievedOnServer(BaseServer server)
    {

    }
}
