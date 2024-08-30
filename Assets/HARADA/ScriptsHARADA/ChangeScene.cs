// ---------------------------------------------------------  
// ChangeScene.cs  
//   
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField, Header("シーン名")]
    private string _sceneName = default;
    #region メソッド  

    public void ChangeScnene()
    {
        SceneManager.LoadScene(_sceneName);
    }

    #endregion
}
