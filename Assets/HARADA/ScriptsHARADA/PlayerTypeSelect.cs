using UnityEngine;

public class PlayerTypeSelect : MonoBehaviour
{
    [SerializeField, Header("�L�����N�^�[�^�C�v")]
    private PlayerType _playerType = default;
    public enum PlayerType
    {
        Dog,
        Bear,
        Cat,
        Dear,
        Duck,
        Fox,
        Racoon,
        Shark,
        Tiger
    }

    public PlayerType GetplayerType { get => _playerType; }
}
