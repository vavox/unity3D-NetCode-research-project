using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    #region Fields
    [SerializeField]
    float walkSpeed = 3.5f;


    [SerializeField]
    Vector2 defaultPositionRange = new Vector2(-5, 5);

    [SerializeField]
    NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();
    NetworkVariable<Vector3> networkPositionRotation = new NetworkVariable<Vector3>();
    NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();
    

    // client position caching
    Vector3 oldPlayerPosition = Vector3.zero;
    Vector3 oldPlayerRotation = Vector3.zero;
    PlayerState oldPlayerState = PlayerState.Idle;

    Animator animator;

    CharacterController characterController;
    // float oldForwardBackPosition;
    // float oldLeftRightPosition;
    #endregion

    #region Unity methods
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(IsClient && IsOwner)
        {
            transform.position = new Vector3(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 1,
                                                Random.Range(defaultPositionRange.x, defaultPositionRange.y));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsClient && IsOwner)
        {
            ClientInput();
            // UpdateClient();
        }

        if(IsServer)
        {
            ClientMoveAndRotate();
            ClientVisuals();
        }
    }
    #endregion

    #region Private methods

    void ClientMoveAndRotate()
    {
        if(networkPositionDirection.Value != Vector3.zero)
        {
            characterController.SimpleMove(networkPositionDirection.Value);
        }

        if(networkPositionRotation.Value != Vector3.zero)
        {
            transform.Rotate(networkPositionRotation.Value);
        }
    }

    void ClientVisuals()
    {
        switch (networkPlayerState.Value)
        {
            case PlayerState.Walk:
                animator.SetFloat("Walk", 1);
                break;
            case PlayerState.ReverseWalk:
                animator.SetFloat("Walk", -1);
                break;
            default:
                animator.SetFloat("Walk", 0);
                break;
        }
        
    }

    void ClientInput()
    {
        // Position and Rotation change
        Vector3 inputRotation = new Vector3(0, Input.GetAxis("Horizontal"), 0);

        Vector3 direction = transform.TransformDirection(Vector3.forward);
        float forwardInput = Input.GetAxis("Vertical");
        Vector3 inputPosition = direction * forwardInput;

        if(oldPlayerPosition != inputPosition || oldPlayerRotation != inputRotation)
        {
            oldPlayerPosition = inputPosition;
            oldPlayerRotation = inputRotation;

            UpdateClientPositionAndRotationServerRpc(inputPosition * walkSpeed, inputRotation * walkSpeed);
        }

        // State update
        if(forwardInput > 0)
        {
            UpdateClientStateServerRpc(PlayerState.Walk);
        }
        else if(forwardInput < 0)
        {
            UpdateClientStateServerRpc(PlayerState.ReverseWalk);
        }
        else
        {
            UpdateClientStateServerRpc(PlayerState.Idle);
        }
    }

   
    [ServerRpc]
    void UpdateClientPositionAndRotationServerRpc(Vector3 inputPosition, Vector3 inputRotation)
    {
        networkPositionDirection.Value = inputPosition;
        networkPositionRotation.Value = inputRotation;
    }

    [ServerRpc]
    void UpdateClientStateServerRpc(PlayerState state)
    {
        networkPlayerState.Value = state;
    }
    #endregion

    #region Public methods
    #endregion
}
