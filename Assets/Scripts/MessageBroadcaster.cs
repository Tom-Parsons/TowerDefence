using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBroadcaster : MonoBehaviour
{
    public static MessageBroadcaster instance;
    [SerializeField] private Text messageText;

    private void Start()
    {
        instance = this;
        messageText.enabled = false;
    }

    public void Broadcast(string message, float displayTime)
    {
        messageText.enabled = true;
        messageText.text = message;
        StartCoroutine(RemoveMessage(displayTime));
    }

    IEnumerator RemoveMessage(float displayTime)
    {
        yield return new WaitForSeconds(displayTime);

        messageText.enabled = false;
    }

}
