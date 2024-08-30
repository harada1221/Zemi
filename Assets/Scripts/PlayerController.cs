using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    #region 変数
    [Header("移動の速さ"), SerializeField]
    private float _speed = 3;

    private Transform _transform = default;
    private CharacterController _characterController = default;
    private PlayerInput _playerInput = default;

    //private Vector2 _inputMove = default;
    private float _verticalVelocity = default;
    private float _turnVelocity = default;

    private Vector2ReactiveProperty _inputMove = new Vector2ReactiveProperty(default);
    #endregion

    #region プロパティ
    public Vector2ReactiveProperty InputMove { get => _inputMove; private set => _inputMove = value; }
    #endregion

    #region メソッド
    /// <summary>
    /// 移動Action
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        InputMove.Value = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 爆弾設置
    /// </summary>
    public void OnSetBom(InputAction.CallbackContext context)
    {
        // ボタンが押された瞬間に処理
        if (!context.performed)
        {
            return;
        }

        // 爆弾設置
        Debug.Log("爆弾設置");
    }

    /// <summary>
    /// 旗設置
    /// </summary>
    public void OnSetFlag(InputAction.CallbackContext context)
    {
        // ボタンが押された瞬間に処理
        if (!context.performed)
        {
            return;
        }

        // 旗設置
        Debug.Log("旗設置OR旗回収");

    }

    /// <summary>
    /// 初期処理
    /// </summary>
    private void Awake()
    {
        _transform = this.transform;
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        //操作キャラ移動用オペレーター
        this.UpdateAsObservable()
            .Where(_ => _playerInput.actions["Move"].IsPressed())
            .Subscribe(_ => Debug.Log("MpveMpveMove"));

        //爆弾設置用オペレーター
        this.UpdateAsObservable()
            .Where(_ => _playerInput.actions["PutBomb"].IsPressed())
            .Subscribe(_ => Debug.Log("PutBomb"));

        //旗設置・回収用オペレーター
        this.UpdateAsObservable()
            .Where(_ => _playerInput.actions["Flag"].IsPressed())
            .Subscribe(_ => Debug.Log("Flag"));

        //爆弾軌道変更用オペレーター
        this.UpdateAsObservable()
            .Where(_ => _playerInput.actions["ChangeBomb"].IsPressed())
            .Subscribe(_ => Debug.Log("PutBomb"));
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {

        // 操作入力と鉛直方向速度から、現在速度を計算
        Vector3 moveVelocity = new Vector3(
            InputMove.Value.x * _speed,
            _verticalVelocity,
            InputMove.Value.y * _speed
        );
        // 現在フレームの移動量を移動速度から計算
        Vector3 moveDelta = moveVelocity * Time.deltaTime;

        // CharacterControllerに移動量を指定し、オブジェクトを動かす
        _characterController.Move(moveDelta);

        if (InputMove.Value != Vector2.zero)
        {
            // 移動入力がある場合は、振り向き動作も行う

            // 操作入力からy軸周りの目標角度[deg]を計算
            float targetAngleY = -Mathf.Atan2(InputMove.Value.y, InputMove.Value.x)
                * Mathf.Rad2Deg + 90;

            // イージングしながら次の回転角度[deg]を計算
            float angleY = Mathf.SmoothDampAngle(
                _transform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );

            // オブジェクトの回転を更新
            _transform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }
    #endregion
}
