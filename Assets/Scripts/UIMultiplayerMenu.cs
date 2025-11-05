using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIMultiplayerMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chatMessage;
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private TMP_InputField nicknameInputField;

    private ChatManager chatManager;

    private void Awake()
    {
        messageInputField.interactable = false;

        
    }

    private void TryLinkChatManager()
    {
        if (chatManager == null)
        {
            chatManager = FindAnyObjectByType<ChatManager>();
            chatManager.OnReceiveMessage += ReceiveMessageFromChat;
        }
    }

    private void OnDestroy()
    {
        chatManager.OnReceiveMessage -= ReceiveMessageFromChat;
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        TryLinkChatManager();

        messageInputField.interactable = true;
    }

    public void StartClient()
    {
        TryLinkChatManager();
        NetworkManager.Singleton.StartClient();

        messageInputField.interactable = true;
    }

    public void SendMessageToChat()
    {
        TryLinkChatManager();

        // send message from an input field to the chat after button click
        string messageToSend = messageInputField.text;
        chatManager.ReceiveChatMessageRpc(messageToSend);
        messageInputField.text = "";
    }

    public void ReceiveMessageFromChat(string msgParam)
    {
        // a new message was received
        // needs to be dispalyed on the chat

        chatMessage.text += msgParam + "\n";
    }

    public string GetNicknameInput()
    {
        return nicknameInputField.text;
    }
}
