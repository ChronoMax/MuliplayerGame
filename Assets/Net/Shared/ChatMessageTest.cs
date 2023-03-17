using TMPro;
using UnityEngine;

public class ChatMessageTest : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;

    public void OnButtonClick()
    {
        Net_ChatMessage msg = new Net_ChatMessage(input.text);
        FindAnyObjectByType<BaseClient>().SendToServer(msg);
    }
}
