using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイ人数
/// </summary>
public sealed class PlayerData : MonoBehaviour
{

    [SerializeField, Header("最大人数")]
    private int _maxPlayerCount = 4;
    // シングルトンインスタンス
    public static PlayerData Instance { get; private set; }

    // 現在のプレイヤー数
    public int CurrentPlayerCount { get; set; }
    // デバイス情報配列
    public InputDevice[] InputDevices{ get; set; }
    // プレイヤー配列
    public PlayerTypeSelect.PlayerType[] PlayerTypes { get; set; }
    // プレイヤー勝利数
    public int[] PlayerWins { get; set; }
    // 最大プレイヤー人数
    public int MaxPlayer { get=>_maxPlayerCount;}

    private void Awake()
    {
        // インスタンスを設定
        if (Instance == null)
        {
            Instance = this;
            // オブジェクトを保持
            DontDestroyOnLoad(gameObject);
            InputDevices = new InputDevice[_maxPlayerCount];
            PlayerTypes = new PlayerTypeSelect.PlayerType[_maxPlayerCount];
            PlayerWins = new int[_maxPlayerCount];
        }
        else
        {
            Destroy(gameObject);
        }
    }
}