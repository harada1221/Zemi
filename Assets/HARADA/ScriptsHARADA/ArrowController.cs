using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ArrowController : MonoBehaviour
{
    [SerializeField, Header("移動の速さ")]
    private float _speed = 3;
    [SerializeField, Header("rayの初期位置")]
    private Transform _rayTransform = default;
    [SerializeField, Header("ターゲットのレイヤー")]
    private LayerMask _targetLayer = default;
    [SerializeField, Header("選択時の丸")]
    private GameObject _selectCircle = default;

    private Vector2 _topLeft = default;
    private Vector2 _downRight = default;
    // 選択時の円保存
    private GameObject _circle = default;
    // 入力方向
    private Vector2 _inputMove = default;
    // プレイヤー番号
    private int _playerNum = default;
    // ポジション
    private Transform _myTransform = default;
    // 移動可能か
    private bool _isSelect = false;
    // キャラクターマネージャー
    private CharaSelectManeger _charaManeger = default;

    private const string PARENTNAME = "Canvas";

    public int PlayerNum
    {
        get { return _playerNum; }
        set { _playerNum = value; }
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag(PARENTNAME);
        _charaManeger = GameObject.FindGameObjectWithTag("CharaManeger").GetComponent<CharaSelectManeger>();
        _charaManeger.AllDecision = false;
        Vector3[] corners = new Vector3[4];
        canvas.GetComponent<RectTransform>().GetWorldCorners(corners);
        // 上限と下限の座標を取得
        _topLeft = corners[1];  // 左上
        _downRight = corners[3];  // 右下


        _myTransform = this.transform;
        // 丸を生成
        _circle = Instantiate(_selectCircle, _rayTransform);
        // プレイヤーの番号に合わせてカラーを決める
        switch (_playerNum)
        {
            case 0:
                this.GetComponent<Image>().color = Color.blue;
                _circle.GetComponent<Image>().color = Color.blue;
                break;
            case 1:
                this.GetComponent<Image>().color = Color.red;
                _circle.GetComponent<Image>().color = Color.red;
                break;
            case 2:
                this.GetComponent<Image>().color = Color.green;
                _circle.GetComponent<Image>().color = Color.green;
                break;
            case 3:
                this.GetComponent<Image>().color = Color.yellow;
                _circle.GetComponent<Image>().color = Color.yellow;
                break;
        }
    }

    /// <summary>
    /// 移動Action
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 決定ボタン
    /// </summary>
    public void OnSelectChara(InputAction.CallbackContext context)
    {
        // ボタンが押された瞬間に処理
        if (!context.performed)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(_rayTransform.position, Vector3.forward, out hit, Mathf.Infinity, _targetLayer))
        {
            ChangeScene changeScene = hit.transform.GetComponent<ChangeScene>();
            if (changeScene != null && changeScene.enabled == true)
            {
                changeScene.ChangeScnene();
            }
        }
        // ボタンを押したときにUIがあるか
        if (Physics.Raycast(_rayTransform.position, Vector3.forward, out hit, Mathf.Infinity, _targetLayer))
        {
            PlayerTypeSelect playerType = hit.transform.GetComponent<PlayerTypeSelect>();
            if (playerType == null || _isSelect == true)
            {
                return;
            }
            // キャラタイプ保存
            PlayerTypeSelect.PlayerType type = playerType.GetplayerType;
            Debug.Log(type);
            // 親オブジェクト変更
            _circle.transform.SetParent(playerType.transform);
            _isSelect = true;
            // プレイヤータイプ保存
            PlayerData.Instance.PlayerTypes[_playerNum] = type;
            // 決定人数増加
            _charaManeger.DecisionPlayer++;
            _charaManeger.ColorChangeActive();
        }
    }

    /// <summary>
    /// キャンセルボタン
    /// </summary>
    public void OnCancel(InputAction.CallbackContext context)
    {
        // ボタンが押された瞬間に処理
        if (!context.performed || _isSelect == false)
        {
            return;
        }
        // 親オブジェクト変更
        _circle.transform.SetParent(_rayTransform);
        _circle.transform.position = _rayTransform.position;
        // プレイヤータイプ削除
        PlayerData.Instance.PlayerTypes[_playerNum] = default;
        _isSelect = false;
        // 決定人数減少
        _charaManeger.DecisionPlayer--;
        _charaManeger.ColorChangeCancel();
    }

    private void Update()
    {
        // 操作入力と鉛直方向速度から、現在速度を計算
        Vector2 moveVelocity = new Vector2(
            _inputMove.x * _speed,
            _inputMove.y * _speed
        );
        // 現在フレームの移動量を移動速度から計算
        Vector3 moveDelta = moveVelocity * Time.deltaTime;

        // 移動制限
        if (_topLeft.x > _myTransform.position.x + moveDelta.x || _downRight.x < _myTransform.position.x + moveDelta.x)
        {
            moveDelta.x = 0f;
        }
        if (_topLeft.y < _myTransform.position.y + moveDelta.y || _downRight.y > _myTransform.position.y + moveDelta.y)
        {
            moveDelta.y = 0f;
        }
        _myTransform.position += moveDelta;

    }
}
