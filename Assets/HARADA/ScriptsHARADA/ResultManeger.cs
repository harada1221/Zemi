// ---------------------------------------------------------  
// ResultManeger.cs  
//   
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ResultManeger : MonoBehaviour
{

    #region 変数  
    [SerializeField, Header("勝利数テキスト")]
    private Text[] _wincount = default;
    [SerializeField, Header("テキストのポジション")]
    private Transform[] _position = default;
    [SerializeField, Header("セレクトイメージ")]
    private Image _backImage = default;

    private Vector2 _inputMove = default;
    private int _selectNum = default;
    private bool _isMove = false;

    #endregion

    #region プロパティ  

    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start()
    {
        WinCountDisplay();
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    private void Update()
    {
        if (_inputMove.y >= 0.6f && _isMove == false)
        {
            SelectState(true);
        }
        else if (_inputMove.y <= -0.6f && _isMove == false)
        {
            SelectState(false);
        }
        if (_inputMove.y <= 0.6f && _inputMove.y >= -0.6f)
        {
            _isMove = false;
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
        if (!context.performed)
        {
            return;
        }
        _position[_selectNum].GetComponent<ChangeScene>().ChangeScnene();
    }

    /// <summary>
    /// 勝利数表示
    /// </summary>
    public void WinCountDisplay()
    {
        for (int i = 0; i < PlayerData.Instance.MaxPlayer; i++)
        {
            _wincount[i].text = PlayerData.Instance.PlayerWins[i].ToString() + ("勝");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void SelectState(bool Direction)
    {
        _isMove = true;
        // 選択中の番号を更新
        if (Direction == true)
        {
            _selectNum--;
        }
        else
        {
            _selectNum++;
        }
        // 超えたら上限値OR下限値にする
        if (_selectNum < 0)
        {
            _selectNum = _position.Length - 1;
        }
        else if (_selectNum >= _position.Length)
        {
            _selectNum = 0;
        }

        // 現在の位置を取得
        Vector3 currentPosition = _backImage.transform.position;
        // Y座標だけを変更
        _backImage.transform.position = new Vector3(currentPosition.x, _position[_selectNum].transform.position.y, currentPosition.z);
    }

    #endregion
}
