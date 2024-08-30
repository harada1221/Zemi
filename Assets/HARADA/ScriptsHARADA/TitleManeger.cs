// ---------------------------------------------------------  
// TitleManeger.cs  
//   
// 作成日7月25日:  
// 作成者　原田　智大:  
// ---------------------------------------------------------  
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TitleManeger : MonoBehaviour
{

    #region 変数  
    [SerializeField, Header("タイトルAnimator")]
    private Animator _animator = default;
    [SerializeField, Header("消えるテキスト")]
    private GameObject _deleteText = default;
    [SerializeField, Header("表示するテキスト")]
    private GameObject _createText = default;
    [SerializeField, Header("背景のポジション")]
    private Transform[] _position = default;
    [SerializeField, Header("背景イメージ")]
    private Image _backImage = default;
    private InputAction _pressAnyKeyAction =
                new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");

    // 選択中のメニュー
    private bool _isSelectMenu = false;
    // 選択可能か
    private bool _isSelect = false;
    // インプットの移動報告
    private Vector2 _inputMove = default;
    // タイトルの進捗状態
    private TitleState _titleState = TitleState.first;
    public enum TitleState
    {
        first,
        second
    }

    #endregion

    #region メソッド  
    private void OnEnable() => _pressAnyKeyAction.Enable();
    private void OnDisable() => _pressAnyKeyAction.Disable();

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    private void Update()
    {
        if (_pressAnyKeyAction.triggered && _titleState == TitleState.first)
        {
            _animator.SetTrigger("StratAnimeTrigger");
            _deleteText.SetActive(false);
            _createText.SetActive(true);
            _titleState = TitleState.second;
        }
        if (_titleState == TitleState.first)
        {
            return;
        }
        if ((_inputMove.y >= 0.8 || _inputMove.y <= -0.8) && _isSelect == false)
        {
            _isSelect = true;
            SelectGameState();
        }
        else if (_inputMove.y <= 0.8 && _inputMove.y >= -0.8)
        {
            _isSelect = false;
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
    /// 選択
    /// </summary>
    public void OnSelct(InputAction.CallbackContext context)
    {
        // ボタンが押された瞬間に処理
        if (!context.performed || _titleState == TitleState.first)
        {
            return;
        }

        if (_isSelectMenu == false)
        {
            _animator.SetTrigger("ChangeSceneAnimeTrigger");
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                    Application.Quit();//ゲームプレイ終了
#endif
        }
    }

    /// <summary>
    /// 選択している状態を変更
    /// </summary>
    private void SelectGameState()
    {
        _isSelectMenu = !_isSelectMenu;
        if (_isSelectMenu == false)
        {
            // 現在の位置を取得
            Vector3 currentPosition = _backImage.transform.position;
            // Y座標だけを変更
            _backImage.transform.position = new Vector3(currentPosition.x, _position[0].transform.position.y, currentPosition.z);
        }
        else
        {
            // 現在の位置を取得
            Vector3 currentPosition = _backImage.transform.position;
            // Y座標だけを変更
            _backImage.transform.position = new Vector3(currentPosition.x, _position[1].transform.position.y, currentPosition.z);
        }
    }
    #endregion
}
