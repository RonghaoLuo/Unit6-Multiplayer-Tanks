using TMPro;
using Unity.Netcode;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using System.Collections;
using Unity.VisualScripting;

public class UIMultiplayerMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chatMessage;
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private TMP_InputField nicknameInputField;

    private ChatManager chatManager;
    private ISession mySession;

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
        mySession?.LeaveAsync();
    }

    // Not used for Distributed Authority
    public void StartHostButton()
    {
        NetworkManager.Singleton.StartHost();
        TryLinkChatManager();

        messageInputField.interactable = true;
    }

    // Join Game Button
    public void StartClientButton()
    {
        Debug.Log("On Click Join Button");
        //NetworkManager.Singleton.StartClient();

        StartCoroutine(JoinMultiplayerGame());

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

    /// <summary>
    /// waits for the InitializeAsync to finish
    /// </summary>
    /// <returns></returns>
    public IEnumerator JoinMultiplayerGame()
    {

        Debug.Log("Begin coroutine");
        yield return UnityServices.InitializeAsync();
        Debug.Log("Unity Services Initialized");
        yield return JoinOrCreateGameSession();

        TryLinkChatManager();
    }

    /// <summary>
    /// Shortcut way to Join or Create a Session
    /// </summary>
    /// <returns></returns>
    public async Task JoinOrCreateGameSession()
    {
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("SignInAnonymously");
        SessionOptions sessionOptions = new SessionOptions()
        {
            Name = "TestID3",
            MaxPlayers = 5,
        }.WithDistributedAuthorityNetwork();

        mySession = await MultiplayerService.Instance.CreateOrJoinSessionAsync("TestID3", sessionOptions);
    }
}
