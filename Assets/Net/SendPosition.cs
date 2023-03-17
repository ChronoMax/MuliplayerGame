using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPosition : MonoBehaviour
{
    private float lastSend;
    private BaseClient client;

    private void Start()
    {
        client = FindObjectOfType<BaseClient>();
    }

    private void FixedUpdate()
    {
        if (Time.time - lastSend > 1.0f)
        {
            Net_PlayerPos ps = new Net_PlayerPos(377, transform.position.x, transform.position.y, transform.position.z);
            client.SendToServer(ps);
            lastSend = Time.time;
        }
    }
}
