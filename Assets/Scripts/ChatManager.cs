using System;
using Unity.Netcode;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    public Action<string> OnReceiveMessage;

    [Rpc(SendTo.Everyone)]
    public void ReceiveChatMessageRpc(string chatMessage, RpcParams rpcParams = default)
    {
        Debug.Log("Received message... Displaying in the chat");
        chatMessage = rpcParams.Receive.SenderClientId.ToString() + ": " + chatMessage;

        OnReceiveMessage?.Invoke(chatMessage);
        //FindAnyObjectByType<UIMultiplayerMenu>().ReceiveMessageFromChat(chatMessage);
        
    }
}
