using Unity.Networking.Transport;
using UnityEngine;

public class NetMessage
{
    public OpCode code { get; set; }

    public virtual void Serialize(ref DataStreamWriter writer)
    {

    }

    public virtual void Deserialize()
    {

    }
}
