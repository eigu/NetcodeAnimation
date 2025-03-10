using StarterAssets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClientPlayerMove : NetworkBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
    [SerializeField] private ThirdPersonController _thirdPersonController;
    [SerializeField] private GameObject _camera;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            _playerInput.enabled = true;
            _starterAssetsInputs.enabled = true;
        }

        _thirdPersonController.enabled = true;
        _camera.SetActive(true);
    }

    [Rpc(SendTo.Server)]
    private void UpdateInputServerRpc(Vector2 move, Vector2 look, bool jump, bool sprint)
    {
        _starterAssetsInputs.MoveInput(move);
        _starterAssetsInputs.LookInput(look);
        _starterAssetsInputs.JumpInput(jump);
        _starterAssetsInputs.SprintInput(sprint);
    }
    
    private void LateUpdate()
    {
        if (!IsOwner)
            return;

        UpdateInputServerRpc(
            _starterAssetsInputs.move,
            _starterAssetsInputs.look,
            _starterAssetsInputs.jump,
            _starterAssetsInputs.sprint);
    }
}
