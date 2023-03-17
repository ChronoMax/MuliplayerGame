using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public class Net_PlayerPos : NetMessage
{
    //bit 0-8 OP

    public int PlayerID { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }

    public Net_PlayerPos()
    {
        code = OpCode.PLAYER_POS;
    }

    public Net_PlayerPos(DataStreamReader reader)
    {
        code = OpCode.PLAYER_POS;
        Deserialize(reader);
    }
    public Net_PlayerPos(int playerId, float x, float y, float z)
    {
        code = OpCode.PLAYER_POS;
        playerId = playerId;
        PosX = x;
        PosY = y;
        PosZ = z;
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code); //always on top
        writer.WriteInt(PlayerID);
        writer.WriteFloat(PosX);
        writer.WriteFloat(PosY);
        writer.WriteFloat(PosZ);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        PlayerID = reader.ReadInt();
        PosX = reader.ReadFloat();
        PosY = reader.ReadFloat();
        PosZ = reader.ReadFloat();
    }

    public override void RecievedOnServer()
    {
        Debug.Log("[SERVER]: " + PlayerID + " :: " + PosX + " :: " + PosY + ":: " + PosZ);
    }

    public override void RecievedOnClient()
    {
        Debug.Log("[CLIENT]: " + PlayerID + " :: " + PosX + " :: " + PosY + ":: " + PosZ);
    }
}
