using Cinemachine;
using StarterAssets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClientHandler : NetworkBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
    [SerializeField] private AudioListener _audioListener;
    [SerializeField] private ThirdPersonController _thirdPersonController;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsClient && IsOwner)
        {
            _playerInput.enabled = true;
            _starterAssetsInputs.enabled = true;
            _audioListener.enabled = true;
            _cinemachineVirtualCamera.Priority = 1;
        }

        _thirdPersonController.enabled = true;
    }

    private void LateUpdate()
    {
        if (IsOwner)
        {
            UpdateInputServerRpc(
                _starterAssetsInputs.move,
                _starterAssetsInputs.look,
                _starterAssetsInputs.jump,
                _starterAssetsInputs.sprint);
        }
    }

    [ServerRpc]
    private void UpdateInputServerRpc(Vector2 move, Vector2 look, bool jump, bool sprint)
    {
        _starterAssetsInputs.MoveInput(move);
        _starterAssetsInputs.LookInput(look);
        _starterAssetsInputs.JumpInput(jump);
        _starterAssetsInputs.SprintInput(sprint);
    }
}
