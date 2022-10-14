using Unity.Netcode;
using Core.Singleton;

public class PlayerManager : NetworkSingleton<PlayerManager>
{
    #region Fields
    NetworkVariable<int> playersInGame = new NetworkVariable<int>();
    #endregion

    #region Properties
    public int PlayersInGame
    {
        get
        { return playersInGame.Value; }
    }
    #endregion

    #region Unity methods
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if(NetworkManager.Singleton.IsServer)
            {
                Logger.Instance.LogInfo($"Player[{id}] just connected...");
                playersInGame.Value++;
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if(NetworkManager.Singleton.IsServer)
            {
                Logger.Instance.LogInfo($"Player[{id}] just disconnected");
                playersInGame.Value--;
            }
        };        
    }
    #endregion
}
