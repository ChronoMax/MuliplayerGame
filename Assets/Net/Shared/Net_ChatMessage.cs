using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public class Net_ChatMessage : NetMessage
{
    //bit 0-8 OP

    public FixedString128Bytes chatMessage { set; get; }

    public Net_ChatMessage()
    {
        code = OpCode.CHAT_MESSAGE;
    }

    public Net_ChatMessage(DataStreamReader reader)
    {
        code = OpCode.CHAT_MESSAGE;
        Deserialize(reader);
    }
    public Net_ChatMessage(string msg)
    {
        code = OpCode.CHAT_MESSAGE;
        chatMessage = msg;
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code); //always on top
        writer.WriteFixedString128(chatMessage);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        chatMessage = reader.ReadFixedString128();
    }

    public override void RecievedOnServer(BaseServer server)
    {
        Debug.Log("[SERVER]: " + chatMessage);
    }

    public override void RecievedOnClient()
    {
        Debug.Log("[CLIENT]: " + chatMessage);
    }
}
