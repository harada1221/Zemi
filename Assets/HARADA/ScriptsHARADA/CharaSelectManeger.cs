// ---------------------------------------------------------  
// CharaSelectManeger.cs  
//   
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharaSelectManeger : MonoBehaviour
{

    #region 変数  
    [SerializeField, Header("開始ボタン")]
    private Image _start = default;

    private ChangeScene _changeScene = default;

    public bool AllDecision { get; set; }

    public int DecisionPlayer { get; set; }
    #endregion

    #region プロパティ  

    #endregion

    #region メソッド  
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start()
    {
        _changeScene = _start.GetComponent<ChangeScene>();
        ColorChangeCancel();
    }

    public void ColorChangeActive()
    {
        if (PlayerData.Instance.CurrentPlayerCount == DecisionPlayer)
        {
            _changeScene.enabled = true;
            _start.color = new Color(255, 255, 255, 1f);
        }
    }
    public void ColorChangeCancel()
    {
        _changeScene.enabled = false;
        _start.color = new Color(255, 255, 255, 0.3f);
    }
    #endregion
}
