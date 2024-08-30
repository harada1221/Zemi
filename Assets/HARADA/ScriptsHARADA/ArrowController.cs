using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ArrowController : MonoBehaviour
{
    [SerializeField, Header("�ړ��̑���")]
    private float _speed = 3;
    [SerializeField, Header("ray�̏����ʒu")]
    private Transform _rayTransform = default;
    [SerializeField, Header("�^�[�Q�b�g�̃��C���[")]
    private LayerMask _targetLayer = default;
    [SerializeField, Header("�I�����̊�")]
    private GameObject _selectCircle = default;

    private Vector2 _topLeft = default;
    private Vector2 _downRight = default;
    // �I�����̉~�ۑ�
    private GameObject _circle = default;
    // ���͕���
    private Vector2 _inputMove = default;
    // �v���C���[�ԍ�
    private int _playerNum = default;
    // �|�W�V����
    private Transform _myTransform = default;
    // �ړ��\��
    private bool _isSelect = false;
    // �L�����N�^�[�}�l�[�W���[
    private CharaSelectManeger _charaManeger = default;

    private const string PARENTNAME = "Canvas";

    public int PlayerNum
    {
        get { return _playerNum; }
        set { _playerNum = value; }
    }

    /// <summary>
    /// ����������
    /// </summary>
    private void Start()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag(PARENTNAME);
        _charaManeger = GameObject.FindGameObjectWithTag("CharaManeger").GetComponent<CharaSelectManeger>();
        _charaManeger.AllDecision = false;
        Vector3[] corners = new Vector3[4];
        canvas.GetComponent<RectTransform>().GetWorldCorners(corners);
        // ����Ɖ����̍��W���擾
        _topLeft = corners[1];  // ����
        _downRight = corners[3];  // �E��


        _myTransform = this.transform;
        // �ۂ𐶐�
        _circle = Instantiate(_selectCircle, _rayTransform);
        // �v���C���[�̔ԍ��ɍ��킹�ăJ���[�����߂�
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
    /// �ړ�Action
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // ���͒l��ێ����Ă���
        _inputMove = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ����{�^��
    /// </summary>
    public void OnSelectChara(InputAction.CallbackContext context)
    {
        // �{�^���������ꂽ�u�Ԃɏ���
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
        // �{�^�����������Ƃ���UI�����邩
        if (Physics.Raycast(_rayTransform.position, Vector3.forward, out hit, Mathf.Infinity, _targetLayer))
        {
            PlayerTypeSelect playerType = hit.transform.GetComponent<PlayerTypeSelect>();
            if (playerType == null || _isSelect == true)
            {
                return;
            }
            // �L�����^�C�v�ۑ�
            PlayerTypeSelect.PlayerType type = playerType.GetplayerType;
            Debug.Log(type);
            // �e�I�u�W�F�N�g�ύX
            _circle.transform.SetParent(playerType.transform);
            _isSelect = true;
            // �v���C���[�^�C�v�ۑ�
            PlayerData.Instance.PlayerTypes[_playerNum] = type;
            // ����l������
            _charaManeger.DecisionPlayer++;
            _charaManeger.ColorChangeActive();
        }
    }

    /// <summary>
    /// �L�����Z���{�^��
    /// </summary>
    public void OnCancel(InputAction.CallbackContext context)
    {
        // �{�^���������ꂽ�u�Ԃɏ���
        if (!context.performed || _isSelect == false)
        {
            return;
        }
        // �e�I�u�W�F�N�g�ύX
        _circle.transform.SetParent(_rayTransform);
        _circle.transform.position = _rayTransform.position;
        // �v���C���[�^�C�v�폜
        PlayerData.Instance.PlayerTypes[_playerNum] = default;
        _isSelect = false;
        // ����l������
        _charaManeger.DecisionPlayer--;
        _charaManeger.ColorChangeCancel();
    }

    private void Update()
    {
        // ������͂Ɖ����������x����A���ݑ��x���v�Z
        Vector2 moveVelocity = new Vector2(
            _inputMove.x * _speed,
            _inputMove.y * _speed
        );
        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        Vector3 moveDelta = moveVelocity * Time.deltaTime;

        // �ړ�����
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
