using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    // �}�b�v�}�l�[�W��
    private MapManager _mapManager;
    // �S�L�����N�^�[�Ǘ��N���X
    private CharactersManager _charactersManager;
    // GUI�}�l�[�W��
    private GUIManager _guiManager;

    // �i�s�Ǘ��ϐ�
    // �I�𒆂̃L�����N�^�[(�N���I�����Ă��Ȃ��Ȃ�false)
    private Character _selectingChara;
    // �I�𒆂̓��Z(�ʏ�U������NONE�Œ�)
    private SkillDefine.Skill _selectingSkill;
    // �I�𒆂̃L�����N�^�[�̈ړ��\�u���b�N���X�g
    private List<MapBlock> _reachableBlocks;
    // �I�𒆂̃L�����N�^�[�̍U���\�u���b�N���X�g
    private List<MapBlock> _attackableBlocks;
    // �Q�[���I���t���O(������������Ȃ�true)
    private bool _isGameSet;

    // �s���L�����Z�������p�ϐ�
    // �I���L�����N�^�[�̈ړ��O�̈ʒu(X����)
    private int _charaStartPos_X;
    // �I���L�����N�^�[�̈ړ��O�̈ʒu(Z����)
    private int _charaStartPos_Z;

    // �I���L�����N�^�[�̍U����̃u���b�N
    private MapBlock _attackBlock; 

	// �^�[���i�s���[�h�ꗗ
	private enum Phase
	{
		MyTurn_Start,       // �����̃^�[���F�J�n��
		MyTurn_Moving,      // �����̃^�[���F�ړ���I��
		MyTurn_Command,     // �����̃^�[���F�ړ���̃R�}���h�I��
		MyTurn_Targeting,   // �����̃^�[���F�U���̑Ώۂ�I��
		MyTurn_Result,      // �����̃^�[���F�s�����ʕ\����
		EnemyTurn_Start,    // �G�̃^�[���F�J�n��
		EnemyTurn_Result    // �G�̃^�[���F�s�����ʕ\����
	}
    // ���݂̐i�s���[�h
    private Phase nowPhase;

	void Start()
	{
		// �Q�Ǝ擾
		_mapManager = GetComponent<MapManager>();
		_charactersManager = GetComponent<CharactersManager>();
		_guiManager = GetComponent<GUIManager>();

		// ���X�g��������
		_reachableBlocks = new List<MapBlock>();
		_attackableBlocks = new List<MapBlock>();

		nowPhase = Phase.MyTurn_Start; // �J�n���̐i�s���[�h
	}

	void Update()
	{
		// �Q�[���I����Ȃ珈�������I��
		if (_isGameSet)
			return;

		// �^�b�v���o����
		if (Input.GetMouseButtonDown(0) &&
			!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) 
		{
			GetMapBlockByTapPos();
		}
	}

	/// <summary>
	/// �^�b�v�����ꏊ�ɂ���I�u�W�F�N�g�������A�I�������Ȃǂ��J�n����
	/// </summary>
	private void GetMapBlockByTapPos()
	{
        // �^�b�v�Ώۂ̃I�u�W�F�N�g
        GameObject targetObject = null; 

		// �^�b�v���������ɃJ��������Ray���΂�
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			// Ray�ɓ�����ʒu�ɑ��݂���I�u�W�F�N�g���擾(�Ώۂ�Collider���t���Ă���K�v������)
			targetObject = hit.collider.gameObject;
		}

		// �ΏۃI�u�W�F�N�g(�}�b�v�u���b�N)�����݂���ꍇ�̏���
		if (targetObject != null)
		{
            // �u���b�N�I��������
            SelectBlock(targetObject.GetComponent<MapBlock>());
		}
	}

	/// <summary>
	/// �w�肵���u���b�N��I����Ԃɂ��鏈��
	/// </summary>
	/// <param name="targetMapBlock">�Ώۂ̃u���b�N�f�[�^</param>
	private void SelectBlock(MapBlock targetBlock)
	{
		// ���݂̐i�s���[�h���ƂɈقȂ鏈�����J�n����
		switch (nowPhase)
		{
			// �����̃^�[���F�J�n��
			case Phase.MyTurn_Start:
                // �S�u���b�N�̑I����Ԃ�����
                _mapManager.AllSelectionModeClear();
                // �u���b�N��I����Ԃ̕\���ɂ���
                targetBlock.SetSelectionMode(MapBlock.Highlight.Select);

				// �I�������ʒu�ɋ���L�����N�^�[�̃f�[�^���擾
				var charaData =	_charactersManager.GetCharacterDataByPos(targetBlock._xPos, targetBlock._zPos);
				if (charaData != null)
				{
                    // �L�����N�^�[�����݂���I�𒆂̃L�����N�^�[���ɋL��
                    _selectingChara = charaData;
					// �I���L�����N�^�[�̌��݈ʒu���L��
					_charaStartPos_X = _selectingChara._xPos;
					_charaStartPos_Z = _selectingChara._zPos;
                    // �L�����N�^�[�̃X�e�[�^�X��UI�ɕ\������
                    _guiManager.ShowStatusWindow(_selectingChara);

					// �ړ��\�ȏꏊ���X�g���擾����
					_reachableBlocks = _mapManager.SearchReachableBlocks(charaData._xPos, charaData._zPos);

					// �ړ��\�ȏꏊ���X�g��\������
					foreach (MapBlock mapBlock in _reachableBlocks)
						mapBlock.SetSelectionMode(MapBlock.Highlight.Reachable);

                    // �ړ��L�����Z���{�^���\��
                    _guiManager.ShowMoveCancelButton();
                    // �i�s���[�h��i�߂�F�ړ���I��
                    ChangePhase(Phase.MyTurn_Moving);
				}
				else
				{
                    // �L�����N�^�[�����݂��Ȃ��I�𒆂̃L�����N�^�[��������������
                    ClearSelectingChara();
				}
				break;

			// �����̃^�[���F�ړ���I��
			case Phase.MyTurn_Moving:
				// �G�L�����N�^�[��I�𒆂Ȃ�ړ����L�����Z�����ďI��
				if (_selectingChara._isEnemy)
				{
					CancelMoving();
					break;
				}

				// �I���u���b�N���ړ��\�ȏꏊ���X�g���ɂ���ꍇ�A�ړ��������J�n
				if (_reachableBlocks.Contains(targetBlock))
				{
                    // �I�𒆂̃L�����N�^�[���ړ�������
                    _selectingChara.MovePosition(targetBlock._xPos, targetBlock._zPos);

                    // �ړ��\�ȏꏊ���X�g������������
                    _reachableBlocks.Clear();

                    // �S�u���b�N�̑I����Ԃ�����
                    _mapManager.AllSelectionModeClear();

                    // �ړ��L�����Z���{�^����\��
                    _guiManager.HideMoveCancelButton();

                    // �w��b���o�ߌ�ɏ��������s����(DoTween)
                    DOVirtual.DelayedCall(
                        0.5f, // �x������(�b)
                        () =>
                        {
							// �x�����s������e
							// �R�}���h�{�^����\������
                            _guiManager.ShowCommandButtons(_selectingChara);
                            // �i�s���[�h��i�߂�F�ړ���̃R�}���h�I��
                            ChangePhase(Phase.MyTurn_Command);
                        }
                    );
                }
				break;

			// �����̃^�[���F�ړ���̃R�}���h�I��
			case Phase.MyTurn_Command:
				// �U���͈͂̃u���b�N��I���������A�s�����邩�̊m�F�{�^����\������
				if (_attackableBlocks.Contains(targetBlock))
				{
                    // �U����̃u���b�N�����L��
                    _attackBlock = targetBlock;
					// �s������E�L�����Z���{�^����\������
					_guiManager.ShowDecideButtons();

                    // �U���\�ȏꏊ���X�g������������
                    _attackableBlocks.Clear();
                    // �S�u���b�N�̑I����Ԃ�����
                    _mapManager.AllSelectionModeClear();

                    // �U����̃u���b�N�������\������
                    _attackBlock.SetSelectionMode(MapBlock.Highlight.Attackable);

                    // �i�s���[�h��i�߂�F�U���̑Ώۂ�I��
                    ChangePhase(Phase.MyTurn_Targeting);
				}
				break;
		}
	}

	/// <summary>
	/// �I�𒆂̃L�����N�^�[��������������
	/// </summary>
	private void ClearSelectingChara()
	{
        // �I�𒆂̃L�����N�^�[������������
        _selectingChara = null;
        // �U���͈͂��擾���ĕ\������
        _guiManager.HideStatusWindow();
	}

	/// <summary>
	/// �U���R�}���h�{�^������
	/// </summary>
	public void AttackCommand()
	{
        // ���Z�̑I�����I�t�ɂ���
        _selectingSkill = SkillDefine.Skill._None;
        // �U���͈͂��擾���ĕ\������
        GetAttackableBlocks();
	}
	/// <summary>
	/// ���Z�R�}���h�{�^������
	/// </summary>
	public void SkillCommand()
	{
        // ���Z�̑I�����I�t�ɂ���
        _selectingSkill = _selectingChara._skill;
        // �U���͈͂��擾���ĕ\������
        GetAttackableBlocks();
	}
	/// <summary>
	/// �U���E���Z�R�}���h�I����ɑΏۃu���b�N��\�����鏈��
	/// </summary>
	private void GetAttackableBlocks()
	{
		_guiManager.HideCommandButtons();

		// �U���\�ȏꏊ���X�g���擾����
		// �i���Z�F�t�@�C�A�{�[���̏ꍇ�̓}�b�v�S��ɑΉ��j
		if (_selectingSkill == SkillDefine.Skill.FireBall)
			_attackableBlocks = _mapManager.MapBlocksToList();
		else
			_attackableBlocks = _mapManager.SearchAttackableBlocks(_selectingChara._xPos, _selectingChara._zPos);

		// �U���\�ȏꏊ���X�g��\������
		foreach (MapBlock mapBlock in _attackableBlocks)
			mapBlock.SetSelectionMode(MapBlock.Highlight.Attackable);
	}

	/// <summary>
	/// �ҋ@�R�}���h�{�^������
	/// </summary>
	public void StandbyCommand()
	{
        // �R�}���h�{�^�����\���ɂ���
        _guiManager.HideCommandButtons();
        // �i�s���[�h��i�߂�(�G�̃^�[����)
        ChangePhase(Phase.EnemyTurn_Start);
	}

	/// <summary>
	/// �s�����e����{�^������
	/// </summary>
	public void ActionDecideButton()
	{
        // �s������E�L�����Z���{�^�����\���ɂ���
        _guiManager.HideDecideButtons();
        // �U����̃u���b�N�̋����\������������
        _attackBlock.SetSelectionMode(MapBlock.Highlight.Off);

        // �U���Ώۂ̈ʒu�ɋ���L�����N�^�[�̃f�[�^���擾
        var targetChara = _charactersManager.GetCharacterDataByPos(_attackBlock._xPos, _attackBlock._zPos);
		if (targetChara != null)
		{
            // �U���Ώۂ̃L�����N�^�[�����݂���
            // �L�����N�^�[�U������
            CharaAttack(_selectingChara, targetChara);

            // �i�s���[�h��i�߂�(�s�����ʕ\����)
            ChangePhase(Phase.MyTurn_Result);
			return;
		}
		else
		{
            // �U���Ώۂ����݂��Ȃ�
            // �i�s���[�h��i�߂�(�G�̃^�[����)
            ChangePhase(Phase.EnemyTurn_Start);
		}
	}
	/// <summary>
	/// �s�����e���Z�b�g�{�^������
	/// </summary>
	public void ActionCancelButton()
	{
        // �s������E�L�����Z���{�^�����\���ɂ���
        _guiManager.HideDecideButtons();
        // �U����̃u���b�N�̋����\������������
        _attackBlock.SetSelectionMode(MapBlock.Highlight.Off);

        // �L�����N�^�[���ړ��O�̈ʒu�ɖ߂�
        _selectingChara.MovePosition(_charaStartPos_X, _charaStartPos_Z);

        // �L�����N�^�[�̑I������������
        ClearSelectingChara();

        // �i�s���[�h��߂�(�^�[���̍ŏ���)
        ChangePhase(Phase.MyTurn_Start, true);
	}

	/// <summary>
	/// �L�����N�^�[�����̃L�����N�^�[�ɍU�����鏈��
	/// </summary>
	/// <param name="attackChara">�U�����L�����f�[�^</param>
	/// <param name="defenseChara">�h�䑤�L�����f�[�^</param>
	private void CharaAttack(Character attackChara, Character defenseChara)
	{
		// �_���[�W�v�Z����
		int damageValue;
		int attackPoint = attackChara._atk; 
		int defencePoint = defenseChara._def; 
		// �h���0���f�o�t���������Ă������̏���
		if (defenseChara._isDefBreak)
			defencePoint = 0;

		// �_���[�W���U���́|�h��͂Ōv�Z
		damageValue = attackPoint - defencePoint;
		// �����ɂ��_���[�W�{�����v�Z
		float ratio = GetDamageRatioByAttribute(attackChara, defenseChara); 
		damageValue = (int)(damageValue * ratio);
		
	
		if (damageValue < 0)
			damageValue = 0;

		// �I���������Z�ɂ��_���[�W�l�␳����ь��ʏ���
		switch (_selectingSkill)
		{
            // ��S�̈ꌂ
            case SkillDefine.Skill.Critical:
                // �_���[�W�Q�{
                damageValue *= 2;
                // ���Z�g�p�s��Ԃɂ���
                attackChara._isSkillLock = true;
				break;

            // �V�[���h�j��
            case SkillDefine.Skill.DefBreak:
                // �_���[�W�O�Œ�
                damageValue = 0;
                // �h���0���f�o�t���Z�b�g
                defenseChara._isDefBreak = true;
				break;

            // �q�[��
            case SkillDefine.Skill.Heal:
                // ��
                // (�񕜗ʂ͍U���͂̔����B�����ɂ��鎖�Ń_���[�W�v�Z���ɉ񕜂���)
                damageValue = (int)(attackPoint * -0.5f);
				break;

            // �t�@�C�A�{�[��
            case SkillDefine.Skill.FireBall:
                // �_���[�W����
                damageValue /= 2;
				break;

            // ���Z����or�ʏ�U����
            default:
				break;
		}

		// �L�����N�^�[�U���A�j���[�V����
		// (�q�[���E�t�@�C�A�{�[���̓A�j���Ȃ�)
		if (_selectingSkill != SkillDefine.Skill.Heal &&
			_selectingSkill != SkillDefine.Skill.FireBall)
			attackChara.AttackAnimation(defenseChara);
        // �A�j���[�V�������ōU���������������炢�̃^�C�~���O��SE���Đ�
        DOVirtual.DelayedCall(
            0.45f, // �x������(�b)
            () =>
            {// �x�����s������e
             // AudioSource���Đ�
                GetComponent<AudioSource>().Play();
            }
        );

        // �o�g�����ʕ\���E�B���h�E�̕\���ݒ�
        _guiManager._battleWindowUI.ShowWindow(defenseChara, damageValue);

        // �_���[�W�ʕ��h�䑤��HP������
        defenseChara._nowHP -= damageValue;
        // HP��0�`�ő�l�͈̔͂Ɏ��܂�悤�␳
        defenseChara._nowHP = Mathf.Clamp(defenseChara._nowHP, 0, defenseChara._maxHP);

		// HP0�ɂȂ����L�����N�^�[���폜����
		if (defenseChara._nowHP == 0)
			_charactersManager.DeleteCharaData(defenseChara);

        // ���Z�̑I����Ԃ���������
        _selectingSkill = SkillDefine.Skill._None;

        // �^�[���؂�ւ�����(�x�����s)
        DOVirtual.DelayedCall(
            2.0f, // �x������(�b)
            () =>
            {// �x�����s������e
             // �E�B���h�E���\����
                _guiManager._battleWindowUI.HideWindow();
                // �^�[����؂�ւ���
                // �G�̃^�[����
                if (nowPhase == Phase.MyTurn_Result) 
                    ChangePhase(Phase.EnemyTurn_Start);
                // �����̃^�[����
                else if (nowPhase == Phase.EnemyTurn_Result) 
                    ChangePhase(Phase.MyTurn_Start);
            }
        );
    }


	/// <summary>
	/// �^�[���i�s���[�h��ύX����
	/// </summary>
	/// <param name="newPhase">�ύX�惂�[�h</param>
	/// <param name="noLogos">���S��\���t���O(�ȗ��\�E�ȗ������false)</param>
	private void ChangePhase(Phase newPhase, bool noLogos = false)
	{
		// �Q�[���I����Ȃ珈�������I��
		if (_isGameSet)
			return;

        // ���[�h�ύX��ۑ�
        nowPhase = newPhase;

		// ����̃��[�h�ɐ؂�ւ�����^�C�~���O�ōs������
		switch (nowPhase)
		{

			// �����̃^�[���F�J�n��
			case Phase.MyTurn_Start:

                // �����̃^�[���J�n���̃��S��\��
                if (!noLogos)
					_guiManager.ShowLogo_PlayerTurn();
				break;

			// �G�̃^�[���F�J�n��
			case Phase.EnemyTurn_Start:
				// �G�̃^�[���J�n���̃��S��\��
				if (!noLogos)
					_guiManager.ShowLogo_EnemyTurn();

                // �G�̍s�����J�n���鏈��
                // (���S�\����ɊJ�n�������̂Œx�������ɂ���)
                DOVirtual.DelayedCall(
                    1.0f, // �x������(�b)
                    () =>
                    {// �x�����s������e
                        EnemyCommand();
                    }
                );
                break;
        }
	}

	/// <summary>
	/// (�G�̃^�[���J�n���Ɍďo)
	/// �G�L�����N�^�[�̂��������ꂩ��̂��s�������ă^�[�����I������
	/// </summary>
	private void EnemyCommand()
	{
        // ���Z�̑I�����I�t�ɂ���
        _selectingSkill = SkillDefine.Skill._None;

		// �������̓G�L�����N�^�[�̃��X�g���쐬����
		var enemyCharas = new List<Character>(); 
		foreach (Character charaData in _charactersManager._characters)
		{
			// �S�����L�����N�^�[����G�t���O�̗����Ă���L�����N�^�[�����X�g�ɒǉ�
			if (charaData._isEnemy)
				enemyCharas.Add(charaData);
		}

        // �U���\�ȃL�����N�^�[�E�ʒu�̑g�ݍ��킹�̓��P�������_���Ɏ擾
        var actionPlan = TargetFinder.GetRandomActionPlan(_mapManager, _charactersManager, enemyCharas);
		// �g�ݍ��킹�̃f�[�^�����݂���΍U���J�n
		if (actionPlan != null)
		{
            // �G�L�����N�^�[�ړ�����
            actionPlan._charaData.MovePosition(actionPlan._toMoveBlock._xPos, actionPlan._toMoveBlock._zPos);

            // �G�L�����N�^�[�U������
            // (�ړ���̃^�C�~���O�ōU���J�n����悤�x�����s)
            DOVirtual.DelayedCall(
                1.0f, // �x������(�b)
                () =>
                {// �x�����s������e
                    CharaAttack(actionPlan._charaData, actionPlan._toAttackChara);
                }
            );

            // �i�s���[�h��i�߂�(�s�����ʕ\����)
            ChangePhase(Phase.EnemyTurn_Result);
			return;
		}

        // �U���\�ȑ��肪������Ȃ������ꍇ�ړ�������P�̂������_���ɑI��
        int randId = Random.Range(0, enemyCharas.Count);
        // �s���Ώۂ̓G�f�[�^
        Character targetEnemy = enemyCharas[randId];

		_reachableBlocks = _mapManager.SearchReachableBlocks(targetEnemy._xPos, targetEnemy._zPos);
		randId = Random.Range(0, _reachableBlocks.Count);
		MapBlock targetBlock = _reachableBlocks[randId]; 
		targetEnemy.MovePosition(targetBlock._xPos, targetBlock._zPos);

        // �ړ��ꏊ�E�U���ꏊ���X�g���N���A����
        _reachableBlocks.Clear();
		_attackableBlocks.Clear();

        // �i�s���[�h��i�߂�(�����̃^�[����)
        ChangePhase(Phase.MyTurn_Start);
	}

	/// <summary>
	/// �U�����E�h�䑤�̑����̑����ɂ��_���[�W�{����Ԃ�
	/// </summary>
	/// <returns>�_���[�W�{��</returns>
	private float GetDamageRatioByAttribute(Character attackChara, Character defenseChara)
	{
        // �e�_���[�W�{�����`
        // �ʏ�
        const float RATIO_NORMAL = 1.0f;
        // �������ǂ�(�U�������L��)
        const float RATIO_GOOD = 1.2f;
        // ����������(�U�������s��)
        const float RATIO_BAD = 0.8f;

        // �U�����̑���
        Character.Attribute atkAttr = attackChara._attribute;
        // �h�䑤�̑���
        Character.Attribute defAttr = defenseChara._attribute; 

		// �������菈��
		// �������ƂɗǑ������������̏��Ń`�F�b�N���A�ǂ���ɂ����Ă͂܂�Ȃ��Ȃ�ʏ�{����Ԃ�
		switch (atkAttr)
		{
			case Character.Attribute.Water: // �U�����F������
				if (defAttr == Character.Attribute.Fire)
					return RATIO_GOOD;
				else if (defAttr == Character.Attribute.Soil)
					return RATIO_BAD;
				else
					return RATIO_NORMAL;

			case Character.Attribute.Fire: // �U�����F�Α���
				if (defAttr == Character.Attribute.Wind)
					return RATIO_GOOD;
				else if (defAttr == Character.Attribute.Water)
					return RATIO_BAD;
				else
					return RATIO_NORMAL;

			case Character.Attribute.Wind: // �U�����F������
				if (defAttr == Character.Attribute.Soil)
					return RATIO_GOOD;
				else if (defAttr == Character.Attribute.Fire)
					return RATIO_BAD;
				else
					return RATIO_NORMAL;

			case Character.Attribute.Soil: // �U�����F�y����
				if (defAttr == Character.Attribute.Water)
					return RATIO_GOOD;
				else if (defAttr == Character.Attribute.Wind)
					return RATIO_BAD;
				else
					return RATIO_NORMAL;

			default:
				return RATIO_NORMAL;
		}
	}

	/// <summary>
	/// �I�𒆂̃L�����N�^�[�̈ړ����͑҂���Ԃ���������
	/// </summary>
	public void CancelMoving()
	{
        // �S�u���b�N�̑I����Ԃ�����
        _mapManager.AllSelectionModeClear();
        // �ړ��\�ȏꏊ���X�g������������
        _reachableBlocks.Clear();
        // �I�𒆂̃L�����N�^�[��������������
        ClearSelectingChara();
        // �ړ���߂�{�^����\��
        _guiManager.HideMoveCancelButton();
        // �t�F�[�Y�����ɖ߂�(���S��\�����Ȃ��ݒ�)
        ChangePhase(Phase.MyTurn_Start, true);
	}

	/// <summary>
	/// �Q�[���̏I�������𖞂������m�F���A�������Ȃ�Q�[�����I������
	/// </summary>
	public void CheckGameSet()
	{
        // �v���C���[�����t���O(�����Ă���G������Ȃ�Off�ɂȂ�)
        bool isWin = true;
        // �v���C���[�s�k�t���O(�����Ă��閡��������Ȃ�Off�ɂȂ�)
        bool isLose = true;

		// ���ꂼ�ꐶ���Ă���G�E���������݂��邩���`�F�b�N
		foreach (var charaData in _charactersManager._characters)
		{
            // �G������̂ŏ����t���OOff
            if (charaData._isEnemy)
				isWin = false;
            // ����������̂Ŕs�k�t���OOff
            else
                isLose = false;
		}

		// �����܂��͔s�k�̃t���O���������܂܂Ȃ�Q�[�����I������
		// (�ǂ���̃t���O�������Ă��Ȃ��Ȃ牽�������^�[�����i�s����)
		if (isWin || isLose)
		{
			// �Q�[���I���t���O�𗧂Ă�
			_isGameSet = true;

			// ���SUI�ƃt�F�[�h�C����\������(�x�����s)
			DOVirtual.DelayedCall(1.5f, () =>
				{
                    // �Q�[���N���A���o
                    if (isWin) 
						_guiManager.ShowLogo_GameClear();
                    // �Q�[���I�[�o�[���o
                    else
                        _guiManager.ShowLogo_GameOver();

                    // �ړ��\�ȏꏊ���X�g������������
                    _reachableBlocks.Clear();
                    // �S�u���b�N�̑I����Ԃ�����
                    _mapManager.AllSelectionModeClear();
                    // �t�F�[�h�C���J�n
                    _guiManager.StartFadeIn();
				}
			);

            // Game�V�[���̍ēǂݍ���(�x�����s)
            DOVirtual.DelayedCall(7.0f, () =>
				{
					SceneManager.LoadScene("Enhance");
				}
			);
		}
	}

}