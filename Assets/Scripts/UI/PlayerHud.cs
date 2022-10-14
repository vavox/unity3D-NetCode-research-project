using TMPro;
using Unity.Netcode;

public class PlayerHud : NetworkBehaviour
{
    #region Fields
    NetworkVariable<NetworkString> playerName = new NetworkVariable<NetworkString>();

    bool overlaySet = false;
    #endregion

    #region Unity methods
    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            playerName.Value = $"Player {OwnerClientId}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!overlaySet && !string.IsNullOrEmpty(playerName.Value))
        {
            SetOverlay();
            overlaySet = true;
        }
    }
    #endregion

    #region Private methods
    public void SetOverlay()
    {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = playerName.Value;
    }
    #endregion
}
