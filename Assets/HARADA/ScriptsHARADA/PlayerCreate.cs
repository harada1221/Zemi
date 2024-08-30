using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerCreate : MonoBehaviour
{
    [SerializeField,Header("�v���C���[�l���\���e�L�X�g")]
    private Text playerCountText = default;
    [SerializeField,Header("�v���C���[�I�u�W�F�N�g")]
    private PlayerInput[] _playerObject = default;

    private void Start()
    {
        if (PlayerData.Instance == null)
        {
            return;
        }
        // �V���O���g�����猻�݂̃v���C���[�����擾���A�\��
        playerCountText.text = "Current Player Count: " + PlayerData.Instance.CurrentPlayerCount;
        // �v���C���[����
        for (int i = 0; i < PlayerData.Instance.InputDevices.Length; i++)
        {
            if (PlayerData.Instance.InputDevices[i] == null || PlayerData.Instance.PlayerTypes[i] == default)
            {
                continue;
            }
            GameObject character = default;
            switch (PlayerData.Instance.PlayerTypes[i])
            {
                case PlayerTypeSelect.PlayerType.Dog:
                    character = _playerObject[(int)PlayerTypeSelect.PlayerType.Dog].gameObject;
                    break;
                case PlayerTypeSelect.PlayerType.Bear:
                    character = _playerObject[(int)PlayerTypeSelect.PlayerType.Bear].gameObject;
                    break;
                case PlayerTypeSelect.PlayerType.Cat:
                    character = _playerObject[(int)PlayerTypeSelect.PlayerType.Cat].gameObject;
                    break;
                case PlayerTypeSelect.PlayerType.Dear:
                    character = _playerObject[(int)PlayerTypeSelect.PlayerType.Dear].gameObject;
                    break;
                case PlayerTypeSelect.PlayerType.Duck:
                    character = _playerObject[(int)PlayerTypeSelect.PlayerType.Duck].gameObject;
                    break;
                case PlayerTypeSelect.PlayerType.Fox:
                    character = _playerObject[(int)PlayerTypeSelect.PlayerType.Fox].gameObject;
                    break;
                case PlayerTypeSelect.PlayerType.Racoon:
                    character = _playerObject[(int)PlayerTypeSelect.PlayerType.Racoon].gameObject;
                    break;
                case PlayerTypeSelect.PlayerType.Shark:
                    character = _playerObject[(int)PlayerTypeSelect.PlayerType.Shark].gameObject;
                    break;
                case PlayerTypeSelect.PlayerType.Tiger:
                    character = _playerObject[(int)PlayerTypeSelect.PlayerType.Tiger].gameObject;
                    break;
            }
            PlayerInput.Instantiate(
            prefab: character,
            pairWithDevice: PlayerData.Instance.InputDevices[i]
            );
        }

    }
}