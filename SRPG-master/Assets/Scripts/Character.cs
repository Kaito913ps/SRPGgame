using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
	// ���C���J����
	private Camera _mainCamera;

	// �L�����N�^�[�����ݒ�(�C���X�y�N�^�������)
	[SerializeField,Header("����X�ʒu(-4�`4)")]
	private int _initPos_X; 
	[SerializeField,Header("����Z�ʒu(-4�`4)")]
	private int _initPos_Z; 
	[Header("�G�t���O(ON�œG�L�����Ƃ��Ĉ���)")]
	public bool _isEnemy; 
	[Header("�L�����N�^�[��")]
	public string _charaName; 
	[Header("�ő�HP(����HP)")]
	public int _maxHP; 
	[Header("�U����")]
	public int _atk; 
	[Header("�h���")]
	public int _def; 
	[Header("����")]
	public Attribute _attribute; 
	[Header("�ړ����@")]
	public MoveType _moveType;
	[Header("���Z")]
	public SkillDefine.Skill _skill; 

	// �Q�[�����ɕω�����L�����N�^�[�f�[�^
	[HideInInspector]
	public int _xPos; 
	[HideInInspector]
	public int _zPos; 
	[HideInInspector]
	public int _nowHP; 
	// �e���Ԉُ�
	public bool _isSkillLock;

	public bool _isDefBreak; 

	// �L�����N�^�[������`(�񋓌^)
	public enum Attribute
	{
		Water, // ������
		Fire,  // �Α���
		Wind,  // ������
		Soil,  // �y����
	}

	// �L�����N�^�[�ړ����@��`(�񋓌^)
	public enum MoveType
	{
		Rook, // �c�E��
		Bishop, // �΂�
		Queen, // �c�E���E�΂�
	}

	void Start()
	{
		// ���C���J�����̎Q�Ƃ��擾
		_mainCamera = Camera.main;

		// �����ʒu�ɑΉ�������W�փI�u�W�F�N�g���ړ�������
		Vector3 pos = new Vector3();
        // x���W�F1�u���b�N�̃T�C�Y��1(1.0f)�Ȃ̂ł��̂܂ܑ��
        pos.x = _initPos_X;
        // y���W�i�Œ�j
        pos.y = 1.0f;
        // z���W
        pos.z = _initPos_Z;
        // �I�u�W�F�N�g�̍��W��ύX
        transform.position = pos; 

		// �I�u�W�F�N�g�����E���](�r���{�[�h�̏����ɂĈ�x���]���Ă��܂���)
		Vector2 scale = transform.localScale;
        // X�����̑傫���𐳕�����ւ���
        scale.x *= -1.0f;
		transform.localScale = scale;

		// ���̑��ϐ�������
		_xPos = _initPos_X;
		_zPos = _initPos_Z;
		_nowHP = _maxHP;
	}

	void Update()
	{
        // �r���{�[�h����(�X�v���C�g�I�u�W�F�N�g�����C���J�����̕����Ɍ�����)
        Vector3 cameraPos = _mainCamera.transform.position;
        // ���݂̃J�������W���擾
        cameraPos.y = transform.position.y;
        // �L�������n�ʂƐ����ɗ��悤�ɂ���
        transform.LookAt(cameraPos);
	}

	/// <summary>
	/// �Ώۂ̍��W�ւƃL�����N�^�[���ړ�������
	/// </summary>
	/// <param name="targetXPos">x���W</param>
	/// <param name="targetZPos">z���W</param>
	public void MovePosition(int targetXPos, int targetZPos)
	{
		// �I�u�W�F�N�g���ړ�������
		// �ړ�����W�ւ̑��΍��W���擾
		Vector3 movePos = Vector3.zero;
        // x�����̑��΋���
        movePos.x = targetXPos - _xPos;
        // z�����̑��΋���
        movePos.z = targetZPos - _zPos;

		// DoTween��Tween���g�p���ď��X�Ɉʒu���ω�����A�j���[�V�������s��
		transform.DOMove(movePos, // �w����W�܂ňړ�����
				0.5f) // �A�j���[�V��������(�b)
			.SetEase(Ease.Linear) // �C�[�W���O(�ω��̓x��)��ݒ�
			.SetRelative(); // �p�����[�^�𑊑Ύw��ɂ���

		// �L�����N�^�[�f�[�^�Ɉʒu��ۑ�
		_xPos = targetXPos;
		_zPos = targetZPos;
	}

	/// <summary>
	/// �L�����N�^�[�̋ߐڍU���A�j���[�V����
	/// </summary>
	/// <param name="targetChara">����L�����N�^�[</param>
	public void AttackAnimation(Character targetChara)
	{
		// �U���A�j���[�V����(DoTween)
		// ����L�����N�^�[�̈ʒu�փW�����v�ŋ߂Â��A���������Ō��̏ꏊ�ɖ߂�
		transform.DOJump(targetChara.transform.position, // �w����W�܂ŃW�����v���Ȃ���ړ�����
				1.0f, // �W�����v�̍���
				1, // �W�����v��
				0.5f) // �A�j���[�V��������(�b)
			.SetEase(Ease.Linear) // �C�[�W���O(�ω��̓x��)��ݒ�
			.SetLoops(2, LoopType.Yoyo); // ���[�v�񐔁E�������w��

		//effect
		//sound
	}
}