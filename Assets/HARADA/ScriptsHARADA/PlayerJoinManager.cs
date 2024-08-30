using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�̓��ގ��̊Ǘ��N���X�i�A�E�g�Q�[���j
/// </summary>
public class PlayerJoinManager : MonoBehaviour
{
    // �v���C���[���Q�[����Join���邽�߂�InputAction
    [SerializeField]
    private InputAction _inputAction = default;
    // �J�[�\���̐e�I�u�W�F�N�g
    [SerializeField]
    private RectTransform _root = default;
    [SerializeField]
    private GameObject _playerPrefab = default;

    // Join�ς݂̃f�o�C�X���
    private InputDevice[] _joinedDevices = default;
    // ���݂̃v���C���[��
    private int _currentPlayerCount = 0;

    private void Start()
    {
        // �ő�Q���\���Ŕz���������
        _joinedDevices = new InputDevice[PlayerData.Instance.MaxPlayer];

        // InputAction��L�������A�R�[���o�b�N��ݒ�
        _inputAction.Enable();
        _inputAction.performed += OnJoin;
    }

    private void OnDestroy()
    {
        _inputAction.Dispose();
    }

    /// <summary>
    /// �f�o�C�X�ɂ����Join�v�������΂����Ƃ��ɌĂ΂�鏈��
    /// </summary>
    private void OnJoin(InputAction.CallbackContext context)
    {
        // �v���C���[�����ő吔�ɒB���Ă�����A�������I��
        if (_currentPlayerCount >= PlayerData.Instance.MaxPlayer)
        {
            return;
        }

        // Join�v�����̃f�o�C�X�����ɎQ���ς݂̂Ƃ��A�������I��
        foreach (var device in _joinedDevices)
        {
            if (context.control.device == device)
            {
                return;
            }
        }

        // �v���C���[���C���X�^���X��
        PlayerInput player = PlayerInput.Instantiate(
            prefab: _playerPrefab,
            playerIndex: _currentPlayerCount,
            pairWithDevice: context.control.device
        );

        // �e�I�u�W�F�N�g��ݒ�
        player.transform.SetParent(_root, false);

        // Join�����f�o�C�X����ۑ�
        _joinedDevices[_currentPlayerCount] = context.control.device;

        // �v���C���[�ԍ��ۑ�
        ArrowController arrowController = player.GetComponent<ArrowController>();
        arrowController.PlayerNum = _currentPlayerCount;

        PlayerData.Instance.InputDevices[_currentPlayerCount] = _joinedDevices[_currentPlayerCount];

        _currentPlayerCount++;

        // �V���O���g���Ɍ��݂̃v���C���[����ۑ�
        PlayerData.Instance.CurrentPlayerCount = _currentPlayerCount;

    }
}