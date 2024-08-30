using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの入退室の管理クラス（アウトゲーム）
/// </summary>
public class PlayerJoinManager : MonoBehaviour
{
    // プレイヤーがゲームにJoinするためのInputAction
    [SerializeField]
    private InputAction _inputAction = default;
    // カーソルの親オブジェクト
    [SerializeField]
    private RectTransform _root = default;
    [SerializeField]
    private GameObject _playerPrefab = default;

    // Join済みのデバイス情報
    private InputDevice[] _joinedDevices = default;
    // 現在のプレイヤー数
    private int _currentPlayerCount = 0;

    private void Start()
    {
        // 最大参加可能数で配列を初期化
        _joinedDevices = new InputDevice[PlayerData.Instance.MaxPlayer];

        // InputActionを有効化し、コールバックを設定
        _inputAction.Enable();
        _inputAction.performed += OnJoin;
    }

    private void OnDestroy()
    {
        _inputAction.Dispose();
    }

    /// <summary>
    /// デバイスによってJoin要求が発火したときに呼ばれる処理
    /// </summary>
    private void OnJoin(InputAction.CallbackContext context)
    {
        // プレイヤー数が最大数に達していたら、処理を終了
        if (_currentPlayerCount >= PlayerData.Instance.MaxPlayer)
        {
            return;
        }

        // Join要求元のデバイスが既に参加済みのとき、処理を終了
        foreach (var device in _joinedDevices)
        {
            if (context.control.device == device)
            {
                return;
            }
        }

        // プレイヤーをインスタンス化
        PlayerInput player = PlayerInput.Instantiate(
            prefab: _playerPrefab,
            playerIndex: _currentPlayerCount,
            pairWithDevice: context.control.device
        );

        // 親オブジェクトを設定
        player.transform.SetParent(_root, false);

        // Joinしたデバイス情報を保存
        _joinedDevices[_currentPlayerCount] = context.control.device;

        // プレイヤー番号保存
        ArrowController arrowController = player.GetComponent<ArrowController>();
        arrowController.PlayerNum = _currentPlayerCount;

        PlayerData.Instance.InputDevices[_currentPlayerCount] = _joinedDevices[_currentPlayerCount];

        _currentPlayerCount++;

        // シングルトンに現在のプレイヤー数を保存
        PlayerData.Instance.CurrentPlayerCount = _currentPlayerCount;

    }
}