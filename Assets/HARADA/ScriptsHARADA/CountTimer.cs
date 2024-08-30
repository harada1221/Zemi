// ---------------------------------------------------------  
// CountTimer.cs  
//   
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using System;
using UnityEngine;
using UnityEngine.UI;

public class CountTimer : MonoBehaviour
{

    #region 変数  
    [SerializeField, Header("分")]
    private int _min = 3;
    [SerializeField, Header("秒")]
    private int _second = default;
    [SerializeField, Header("タイム表示テキスト")]
    private Text _timerText = default;

    // 残り時間
    private float _remainingTime = default;

    #endregion

    #region プロパティ  

    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start()
    {
        _remainingTime = _min * 60 + _second;
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    private void Update()
    {
        _remainingTime -= Time.deltaTime;
        TimeSpan span = new TimeSpan(0, 0, (int)_remainingTime);
        _timerText.text = span.ToString(@"mm\:ss");
    }

    #endregion
}
