// ---------------------------------------------------------  
// MapLoader.cs  
//
// ステージ生成処理 
//
// 作成日: 2025/6/21
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;
using System.IO;

public class MapLoader : MonoBehaviour
{
    #region 変数  

    #region const定数

    // 加工しないままだと配列外になるため調整する
    private const int MINUS_ONE = 1;

    // 壁の高さ
    private const float HEIGHT = 0.75f;

    // 
    

    #endregion

    [SerializeField, Header("テキストファイル")]
    private TextAsset _textAsset = default;

    [SerializeField, Header("ブロックオブジェクト")]
    private GameObject[] _blockObjs = default;

    [SerializeField, Header("壁オブジェクト")]
    private GameObject[] _wallObjs = default;

    [SerializeField,Header("8*8マスに何個の地雷を置くか")]
    private int _mine = 4;

    //[SerializeField,Header("地雷のランダム設置")]
    //private bool flag = true;

    // テキストファイルの中身を格納
    private string[] _stageDetaText;

    // テキストファイルを解析した結果を格納
    private string[,] _stageDeta;

    // テキストの行数
    private int _rowLength;

    // テキストの列数
    private int _columnLength;

    // 8*8の縦マスの境界を格納
    int[] _warpIndex = default;

    // 8*8の横マスの境界を格納 
    int[] _sideIndex = default;

    // 壁の設置フラグ。trueなら置く、falseなら置かない
    private bool[,] _isFloor = default;

    #endregion

    #region プロパティ  

    /// <summary>
    /// テキストの行数
    /// </summary>
    public int RowLength
    {
        get => _rowLength;
        set => _rowLength = value;
    }

    /// <summary>
    /// テキストの列数
    /// </summary>
    public int ColumnLength
    {
        get => _columnLength;
        set => _columnLength = value;
    }

    public string[,] StageDeta
    {
        get => _stageDeta;
        set => _stageDeta = value;
    }


    #endregion

    #region メソッド  

    private void Start()
    {
        Generate();
    }

    /// <summary>
    /// ステージ生成処理
    /// </summary>
    public void Generate()
    {
        // テキストファイルを読み込めるようにする
        string textName = _textAsset.name;
        _textAsset = new TextAsset();
        _textAsset = Resources.Load(textName, typeof(TextAsset)) as TextAsset;

        // テキストファイルの中身取得
        string TextLines = _textAsset.text; 

        // ],があるごとに区切って配列に代入
        _stageDetaText = TextLines.Split("],");

        // いらない文字を削除
        for(int i = 0; i< _stageDetaText.Length; i++)
        {
            string s = _stageDetaText[i].Replace(",","");
            s = s.Replace("[", "");
            s = s.Replace("]", "");
            _stageDetaText[i] = s;
        }

        // 配列定義
        _stageDeta = new string[_stageDetaText.Length, _stageDetaText[0].Length];
        _isFloor = new bool[_stageDeta.GetLength(0), _stageDeta.GetLength(1)];

        // 配列の初期化
        for (int i = 0; i < _isFloor.GetLength(0); i++)
        {
            for (int j = 0; j < _isFloor.GetLength(1); j++)
            {
                _isFloor[i, j] = true;
            }
        }

        // ステージ情報取得
        for (int i = 0; i < _stageDeta.GetLength(0); i++)
        {
            string s = _stageDetaText[i];
            for(int j = 0; j < s.Length; j++)
            {
                // 1文字だけ取得
                _stageDeta[i, j] = s[j].ToString();
                switch (_stageDeta[i, j])
                {
                    // なにもないとき
                    case "0":
                        _stageDeta[i, j] = "N";
                        break;

                    // 地雷のとき
                    case "1":
                        _stageDeta[i, j] = "B";
                        break;

                    // 壁のとき
                    case "2":
                        _stageDeta[i, j] = "W";
                        break;

                    // 床のとき
                    case "3":
                        // 壁が置けないようにする
                        _isFloor[i, j] = false;
                        _stageDeta[i, j] = "C";
                        break;
                }
            }
        }

        // 外側2マスを除いたマス数を8マスごとに区切る
        int side = (_stageDeta.GetLength(1) - 4) / 8;
        int warp = (_stageDeta.GetLength(0) - 4) / 8;

        // 8で割り切れないとき余ったマスが4マス以上なら8で区切ったことにする
        if ((_stageDeta.GetLength(1) - 4) % 8 >= 4)
        {
            side++;
        }

        if ((_stageDeta.GetLength(0) - 4) % 8 >= 4)
        {
            warp++;
        }

        // 配列定義
        _sideIndex = new int[side + 1];
        _warpIndex = new int[warp + 1];

        // 外2マス飛ばしたので最初のマスを2固定
        _sideIndex[0] = 2;
        _warpIndex[0] = 2;

        // 境界値を計算して格納
        for (int i = 1; i < _sideIndex.Length; i++)
        {
            // もう割り切れないとき
            if (_sideIndex[i - 1] + 8 > _stageDeta.GetLength(1) - 4)
            {
                // Randomに使用するため全体のマス数 - 1
                _sideIndex[i] = _stageDeta.GetLength(1) - 1;
            }
            else
            {
                _sideIndex[i] = _sideIndex[i - 1] + 8;
            }
        }

        for (int i = 1; i < _warpIndex.Length; i++)
        {
            // もう割り切れないとき
            if (warp - (_warpIndex[i - 1] + 8) < 8)
            {
                // Randomに使用するため全体のマス数 - 1
                _warpIndex[i] = _stageDeta.GetLength(0) - 1;
            }
            else
            {
                _warpIndex[i] = _warpIndex[i - 1] + 8;
            }
        }

        // 配列外にならないようにする
        if(side <= 1)
        {
            for (int i = 0; i < _sideIndex.Length - 1; i++)
            {
                for (int j = 0; j < _warpIndex.Length - 1; j++)
                {
                    RandomMine(i, j);
                }
            }
        }
        else if(warp <= 1)
        {
            for (int i = 0; i < _warpIndex.Length - 1; i++)
            {
                for (int j = 0; j < _sideIndex.Length - 1; j++)
                {
                    RandomMine(i, j);
                }
            }
        }
        else
        {
            for (int i = 0; i < _sideIndex.Length - 1; i++)
            {
                for (int j = 0; j < _warpIndex.Length - 1; j++)
                {
                    RandomMine(i, j);
                }
            }
        }

        // 地雷の数を数える
        for (int i = 0; i < _stageDeta.GetLength(0); i++)
        {
            for (int j = 0; j < _stageDeta.GetLength(1); j++)
            {
                Seach(i, j);
            }
        }

        // 地面の設置
        for (int i = 0; i < _stageDeta.GetLength(0); i++)
        {
            for (int j = 0; j < _stageDeta.GetLength(1); j++)
            {
                switch (_stageDeta[i, j])
                {
                    // 地雷
                    case "B":
                        Instantiate(_blockObjs[0], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y, this.gameObject.transform.position.z - i), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f)), this.gameObject.transform);
                        break;

                    // 床
                    case "C":
                        Instantiate(_blockObjs[1], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y + 0.5f, this.gameObject.transform.position.z - i), Quaternion.identity, this.gameObject.transform);
                        break;

                    // 周辺の地雷数 = 0
                    case "0":
                        Instantiate(_blockObjs[2], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y, this.gameObject.transform.position.z - i), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f)), this.gameObject.transform);
                        break;

                    // 周辺の地雷数 = 1    
                    case "1":
                        Instantiate(_blockObjs[3], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y, this.gameObject.transform.position.z - i), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f)), this.gameObject.transform);
                        break;

                    // 周辺の地雷数 = 2
                    case "2":
                        Instantiate(_blockObjs[4], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y, this.gameObject.transform.position.z - i), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f)), this.gameObject.transform);
                        break;

                    // 周辺の地雷数 = 3
                    case "3":
                        Instantiate(_blockObjs[5], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y, this.gameObject.transform.position.z - i), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f)), this.gameObject.transform);
                        break;

                    // 周辺の地雷数 = 4
                    case "4":
                        Instantiate(_blockObjs[6], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y, this.gameObject.transform.position.z - i), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f)), this.gameObject.transform);
                        break;

                    // 周辺の地雷数 = 5
                    case "5":
                        Instantiate(_blockObjs[7], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y, this.gameObject.transform.position.z - i), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f)), this.gameObject.transform);
                        break;

                    // 周辺の地雷数 = 6
                    case "6":
                        Instantiate(_blockObjs[8], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y, this.gameObject.transform.position.z - i), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f)), this.gameObject.transform);
                        break;

                    // 周辺の地雷数 = 7
                    case "7":
                        Instantiate(_blockObjs[9], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y, this.gameObject.transform.position.z - i), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f)), this.gameObject.transform);
                        break;

                    // 周辺の地雷数 = 8
                    case "8":
                        Instantiate(_blockObjs[10], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y, this.gameObject.transform.position.z - i), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f)), this.gameObject.transform);
                        break;

                }
            }
        }

        // 壁の設置
        for (int i = 0; i < _stageDeta.GetLength(0); i++)
        {
            for (int j = 0; j < _stageDeta.GetLength(1); j++)
            {
                // 壁が置けるとき
                if (_isFloor[i, j])
                {
                    WallGenerater(i, j);
                }
            }
        }
    }

    /// <summary>
    /// 地雷のランダム設置処理
    /// </summary>
    /// <param name="i">縦のマス目</param>
    /// <param name="j">横のマス目</param>
    private void RandomMine(int i ,int j)
    {
        // 置く地雷の個数分回す
        for (int k = 0; k < _mine; k++)
        {
            // 座標の決定
            int x = Random.Range(_warpIndex[i], _warpIndex[i + 1]);
            int y = Random.Range(_sideIndex[j], _sideIndex[j + 1]);

            // なにもないところなら地雷生成
            if (_stageDeta[x, y] == "N")
            {
                _stageDeta[x, y] = "B";
            }
            // 壊せない壁か地雷以外なら地雷を埋める
            else if(_stageDeta[x, y] == "W" || _stageDeta[x, y] == "B")
            {
                x += Random.Range(-1, 2);
                y += Random.Range(-1, 2);
                if(_stageDeta[x,y] == "N")
                {
                    _stageDeta[x, y] = "B";
                }
            }
        }
    }


    /// <summary>
    /// 壁生成
    /// </summary>
    /// <param name="i">縦のマス目</param>
    /// <param name="j">横のマス目</param>
    private void WallGenerater(int i, int j)
    {
        // 壁を生成
        switch (_stageDeta[i, j])
        {
            // 壊せない壁の設置
            case "W":
                Instantiate(_wallObjs[1], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y + HEIGHT, this.gameObject.transform.position.z - i), Quaternion.identity, this.gameObject.transform);
                break;

            // 壊せる壁の設置
            default:
                Instantiate(_wallObjs[0], new Vector3(this.gameObject.transform.position.x + j, this.gameObject.transform.position.y + HEIGHT, this.gameObject.transform.position.z - i - 0.5f), Quaternion.identity, this.gameObject.transform);
                break;

        }
    }

    /// <summary>
    /// 周りの地雷の数を数える処理
    /// </summary>
    /// <param name="i">縦のマス目</param>
    /// <param name="j">横のマス目</param>
    private void Seach(int i, int j)
    {
        // 周辺の地雷の数をカウント
        int conut = 0;

        // 外側がないとき、現在のマスが地雷や壊せない壁じゃないとき
        if (i != 0 && i != _stageDeta.GetLength(0) - 1 && j != 0 && j != _stageDeta.GetLength(1) - 1 && _stageDeta[i, j] != "B" && _stageDeta[i, j] != "W")
        {

            // 
            if (_stageDeta[i + 1, j] == "B")
            {
                conut++;
            }

            if (_stageDeta[i + 1, j + 1] == "B")
            {
                conut++;
            }

            if (_stageDeta[i + 1, j - 1] == "B")
            {
                conut++;
            }

            if (_stageDeta[i - 1, j] == "B")
            {
                conut++;
            }

            if (_stageDeta[i - 1, j + 1] == "B")
            {
                conut++;
            }

            if (_stageDeta[i - 1, j - 1] == "B")
            {
                conut++;
            }

            if (_stageDeta[i, j + 1] == "B")
            {
                conut++;
            }

            if (_stageDeta[i, j - 1] == "B")
            {
                conut++;
            }
            _stageDeta[i, j] = conut.ToString();
        }
    }
    #endregion

}

