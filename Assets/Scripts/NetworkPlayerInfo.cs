using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Collections;
using TMPro;

public class NetworkPlayerInfo : NetworkBehaviour
{
    public NetworkVariable<int> health = new(
        readPerm: NetworkVariableReadPermission.Everyone, 
        writePerm: NetworkVariableWritePermission.Server);

    public NetworkVariable<FixedString32Bytes> nickname = new(
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Owner);

    public int maxHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI nicknameText;
    [SerializeField] private NetworkPlayer networkPlayer;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsLocalPlayer && IsOwner)
        {
            nickname.Value = FindAnyObjectByType<UIMultiplayerMenu>().GetNicknameInput();
        }

        if (IsServer)
        {
            health.Value = maxHealth;
        }

        health.OnValueChanged += UpdateHealthDisplay;
        nickname.OnValueChanged += UpdateNickNameDisplay;

        UpdateHealthDisplay(0, health.Value);

        UpdateNickNameDisplay("", nickname.Value);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        health.OnValueChanged -= UpdateHealthDisplay;
        nickname.OnValueChanged -= UpdateNickNameDisplay;
    }

    private void UpdateNickNameDisplay(FixedString32Bytes oldValue, FixedString32Bytes newValue)
    {
        nicknameText.text = newValue.ConvertToString();
    }

    private void UpdateHealthDisplay(int oldValue, int newValue)
    {
        healthSlider.value = newValue;
    }

    /// <summary>
    /// Only executed on the server
    /// </summary>
    public void DecreaseHealth()
    {
        health.Value--;

        if (health.Value <= 0 )
        {
            networkPlayer.KillPlayer();
        }
    }
}
