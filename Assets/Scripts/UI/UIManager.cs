using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields
    [SerializeField]
    Button startServerButton;
    [SerializeField]
    Button startClientButton;
    [SerializeField]
    Button startHostButton;
    [SerializeField]
    Button physicsButton;
    [SerializeField]
    TextMeshProUGUI playersInGameText;

    bool isServerStarted;
    #endregion

    #region Unity methods
    void Awake()
    {
        Cursor.visible = true;    
    }

    void Start()
    {
        startClientButton.onClick.AddListener(() =>
        {
            if(NetworkManager.Singleton.StartClient())
            {
                Logger.Instance.LogInfo("Client started...");
            }
            else
            {
                Logger.Instance.LogInfo("Client can`t be started...");
            }
        });

        startServerButton.onClick.AddListener(() =>
        {
            if(NetworkManager.Singleton.StartServer())
            {
                Logger.Instance.LogInfo("Server started...");
            }
            else
            {
                Logger.Instance.LogInfo("Server can`t be started...");
            }
        });

        startHostButton.onClick.AddListener(() =>
        {
            if(NetworkManager.Singleton.StartHost())
            {
                Logger.Instance.LogInfo("Host started...");
            }
            else
            {
                Logger.Instance.LogInfo("Host can`t be started...");
            }
        });

        NetworkManager.Singleton.OnServerStarted += () =>
        {
            isServerStarted = true;
        };

        physicsButton.onClick.AddListener(() => 
        {
            if(!isServerStarted)
            {
                Logger.Instance.LogInfo("Server hasn`t started...");
            }
            SpawnManager.Instance.SpawnObjects();   
        });
    }

    void Update()
    {
        if(!isServerStarted)
        {
            physicsButton.gameObject.SetActive(false);
        }
        else
        {
            physicsButton.gameObject.SetActive(true);
        }

        playersInGameText.text = $"Players In Game:  {PlayerManager.Instance.PlayersInGame}";
    }
    #endregion
}
