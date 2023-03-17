using UnityEngine;
using Unity.Networking.Transport;

public class Net_ChatMessage : NetMessage
{
    //bit 0-8 OP

    public string chatMessage { set; get; }

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

    public override void Deserialize()
    {

    }
}
