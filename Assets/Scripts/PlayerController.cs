using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    #region �ϐ�
    [Header("�ړ��̑���"), SerializeField]
    private float _speed = 3;

    private Transform _transform = default;
    private CharacterController _characterController = default;
    private PlayerInput _playerInput = default;

    //private Vector2 _inputMove = default;
    private float _verticalVelocity = default;
    private float _turnVelocity = default;

    private Vector2ReactiveProperty _inputMove = new Vector2ReactiveProperty(default);
    #endregion

    #region �v���p�e�B
    public Vector2ReactiveProperty InputMove { get => _inputMove; private set => _inputMove = value; }
    #endregion

    #region ���\�b�h
    /// <summary>
    /// �ړ�Action
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // ���͒l��ێ����Ă���
        InputMove.Value = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ���e�ݒu
    /// </summary>
    public void OnSetBom(InputAction.CallbackContext context)
    {
        // �{�^���������ꂽ�u�Ԃɏ���
        if (!context.performed)
        {
            return;
        }

        // ���e�ݒu
        Debug.Log("���e�ݒu");
    }

    /// <summary>
    /// ���ݒu
    /// </summary>
    public void OnSetFlag(InputAction.CallbackContext context)
    {
        // �{�^���������ꂽ�u�Ԃɏ���
        if (!context.performed)
        {
            return;
        }

        // ���ݒu
        Debug.Log("���ݒuOR�����");

    }

    /// <summary>
    /// ��������
    /// </summary>
    private void Awake()
    {
        _transform = this.transform;
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        //����L�����ړ��p�I�y���[�^�[
        this.UpdateAsObservable()
            .Where(_ => _playerInput.actions["Move"].IsPressed())
            .Subscribe(_ => Debug.Log("MpveMpveMove"));

        //���e�ݒu�p�I�y���[�^�[
        this.UpdateAsObservable()
            .Where(_ => _playerInput.actions["PutBomb"].IsPressed())
            .Subscribe(_ => Debug.Log("PutBomb"));

        //���ݒu�E����p�I�y���[�^�[
        this.UpdateAsObservable()
            .Where(_ => _playerInput.actions["Flag"].IsPressed())
            .Subscribe(_ => Debug.Log("Flag"));

        //���e�O���ύX�p�I�y���[�^�[
        this.UpdateAsObservable()
            .Where(_ => _playerInput.actions["ChangeBomb"].IsPressed())
            .Subscribe(_ => Debug.Log("PutBomb"));
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update()
    {

        // ������͂Ɖ����������x����A���ݑ��x���v�Z
        Vector3 moveVelocity = new Vector3(
            InputMove.Value.x * _speed,
            _verticalVelocity,
            InputMove.Value.y * _speed
        );
        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        Vector3 moveDelta = moveVelocity * Time.deltaTime;

        // CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        _characterController.Move(moveDelta);

        if (InputMove.Value != Vector2.zero)
        {
            // �ړ����͂�����ꍇ�́A�U�����������s��

            // ������͂���y������̖ڕW�p�x[deg]���v�Z
            float targetAngleY = -Mathf.Atan2(InputMove.Value.y, InputMove.Value.x)
                * Mathf.Rad2Deg + 90;

            // �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
            float angleY = Mathf.SmoothDampAngle(
                _transform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );

            // �I�u�W�F�N�g�̉�]���X�V
            _transform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }
    #endregion
}
