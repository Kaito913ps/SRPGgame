using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening;

public class GUIManager : MonoBehaviour
{
    // �X�e�[�^�X�E�B���h�EUI
    // �X�e�[�^�X�E�B���h�E�I�u�W�F�N�g
    public GameObject _statusWindow;
    // ���OText
    public Text _nameText;
    // �����A�C�R��Image
    public Image _attributeIcon;
    // HP�Q�[�WImage
    public Image _hpGageImage;
    // HPText
    public Text _hpText;
    // �U����Text
    public Text _atkText;
    // �h���Text
    public Text _defText;

    // �����A�C�R���摜
    // �������A�C�R���摜
    public Sprite attr_Water;
    // �Α����A�C�R���摜
    public Sprite attr_Fire;
    // �������A�C�R���摜
    public Sprite attr_Wind;
    // �y�����A�C�R���摜
    public Sprite attr_Soil;

    // �L�����N�^�[�̃R�}���h�{�^��
    // �S�R�}���h�{�^���̐e�I�u�W�F�N�g
    public GameObject _commandButtons;
    // ���Z�R�}���h��Button
    public Button _skillCommandButton;
    // �I���L�����N�^�[�̓��Z�̐���Text
    public Text _skillText; 


	// �o�g�����ʕ\��UI�����N���X
	public BattleWindowUI _battleWindowUI;

    // �e�탍�S�摜
    // �v���C���[�^�[���J�n���摜
    public Image _playerTurnImage;
    // �G�^�[���J�n���摜
    public Image _enemyTurnImage;
    // �Q�[���N���A�摜
    public Image _gameClearImage;
    // �Q�[���I�[�o�[�摜
    public Image _gameOverImage; 

	// �t�F�[�h�C���p�摜
	public Image _fadeImage;

	// �ړ��L�����Z���{�^��UI
	public GameObject _moveCancelButton;

	// �s������E�L�����Z���{�^��UI
	public GameObject _decideButtons;

	void Start()
	{
        // UI������
        // �X�e�[�^�X�E�B���h�E���B��
        HideStatusWindow();
        // �R�}���h�{�^�����B��
        HideCommandButtons();
        // �ړ��L�����Z���{�^�����B��
        HideMoveCancelButton();
        // �s������E�L�����Z���{�^�����B��
        HideDecideButtons();
	}

	/// <summary>
	/// �X�e�[�^�X�E�B���h�E��\������
	/// </summary>
	/// <param name="charaData">�\���L�����N�^�[�f�[�^</param>
	public void ShowStatusWindow(Character charaData)
	{
        // �I�u�W�F�N�g�A�N�e�B�u��
        _statusWindow.SetActive(true);

        // ���OText�\��
        _nameText.text = charaData._charaName;

		// ����Image�\��
		switch (charaData._attribute)
		{
			case Character.Attribute.Water:
				_attributeIcon.sprite = attr_Water;
				break;
			case Character.Attribute.Fire:
				_attributeIcon.sprite = attr_Fire;
				break;
			case Character.Attribute.Wind:
				_attributeIcon.sprite = attr_Wind;
				break;
			case Character.Attribute.Soil:
				_attributeIcon.sprite = attr_Soil;
				break;
		}

		// HP�Q�[�W�\��
		// �ő�l�ɑ΂��錻��HP�̊������Q�[�WImage��fillAmount�ɃZ�b�g����
		float ratio = (float)charaData._nowHP / charaData._maxHP;
		_hpGageImage.fillAmount = ratio;

        // HPText�\��(���ݒl�ƍő�l������\��)
        _hpText.text = charaData._nowHP + "/" + charaData._maxHP;
        // �U����Text�\��(int����string�ɕϊ�)
        _atkText.text = charaData._atk.ToString();
		// �h���Text�\��(int����string�ɕϊ�)
		if (!charaData._isDefBreak)
			_defText.text = charaData._def.ToString();
        // (�h���0�����Ă���ꍇ)
        else
            _defText.text = "<color=red>0</color>";
	}
	/// <summary>
	/// �X�e�[�^�X�E�B���h�E���B��
	/// </summary>
	public void HideStatusWindow()
	{
		// �I�u�W�F�N�g��A�N�e�B�u��
		_statusWindow.SetActive(false);
	}

	/// <summary>
	/// �R�}���h�{�^����\������
	/// </summary>
	/// <param name="selectChara">�s�����̃L�����N�^�[�f�[�^</param>
	public void ShowCommandButtons(Character selectChara)
	{
		_commandButtons.SetActive(true);

        // �I���L�����N�^�[�̓��Z��Text�ɕ\������
        // �I���L�����N�^�[�̓��Z
        SkillDefine.Skill skill = selectChara._skill;
        // ���Z�̖��O
        string skillName = SkillDefine.dic_SkillName[skill];
        // ���Z�̐�����
        string skillInfo = SkillDefine.dic_SkillInfo[skill]; 
		// ���b�`�e�L�X�g�ŃT�C�Y��ύX���Ȃ��當����\��
		_skillText.text = "<size=40>" + skillName + "</size>\n" + skillInfo;

		// ���Z�g�p�s��ԂȂ���Z�{�^���������Ȃ�����
		if (selectChara._isSkillLock)
			_skillCommandButton.interactable = false;
		else
			_skillCommandButton.interactable = true;
	}

	/// <summary>
	/// �R�}���h�{�^�����B��
	/// </summary>
	public void HideCommandButtons()
	{
		_commandButtons.SetActive(false);
	}

	/// <summary>
	/// �v���C���[�̃^�[���ɐ؂�ւ�������̃��S�摜��\������
	/// </summary>
	public void ShowLogo_PlayerTurn()
	{
		// ���X�ɕ\������\�����s���A�j���[�V����(Tween)
		_playerTurnImage
            .DOFade(1.0f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
                1.0f) // �A�j���[�V��������(�b)
            .SetEase(Ease.OutCubic) // �C�[�W���O(�ω��̓x��)��ݒ�
            .SetLoops(2, LoopType.Yoyo); // ���[�v�񐔁E�������w��
    }
	/// <summary>
	/// �G�̃^�[���ɐ؂�ւ�������̃��S�摜��\������
	/// </summary>
	public void ShowLogo_EnemyTurn()
	{
		// ���X�ɕ\������\�����s���A�j���[�V����(Tween)
		_enemyTurnImage
			.DOFade(1.0f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
                1.0f) // �A�j���[�V��������(�b)
            .SetEase(Ease.OutCubic) // �C�[�W���O(�ω��̓x��)��ݒ�
            .SetLoops(2, LoopType.Yoyo); // ���[�v�񐔁E�������w��
    }

	/// <summary>
	/// �ړ��L�����Z���{�^����\������
	/// </summary>
	public void ShowMoveCancelButton()
	{
		_moveCancelButton.SetActive(true);
	}
	/// <summary>
	/// �ړ��L�����Z���{�^�����\���ɂ���
	/// </summary>
	public void HideMoveCancelButton()
	{
		_moveCancelButton.SetActive(false);
	}

	/// <summary>
	/// �Q�[���N���A���̃��S�摜��\������
	/// </summary>
	public void ShowLogo_GameClear()
	{
		// ���X�ɕ\������A�j���[�V����
		_gameClearImage
			.DOFade(1.0f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
                1.0f) // �A�j���[�V��������(�b)
            .SetEase(Ease.OutCubic); // �C�[�W���O(�ω��̓x��)��ݒ�

        // �g�偨�k�����s���A�j���[�V����
        _gameClearImage.transform
			.DOScale(1.5f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
                1.0f) // �A�j���[�V��������(�b)
            .SetEase(Ease.OutCubic) // �C�[�W���O(�ω��̓x��)��ݒ�
            .SetLoops(2, LoopType.Yoyo); // ���[�v�񐔁E�������w��
    }
	/// <summary>
	/// �Q�[���I�[�o�[�̃��S�摜��\������
	/// </summary>
	public void ShowLogo_GameOver()
	{
        // ���X�ɕ\������A�j���[�V����
        _gameOverImage
			.DOFade(1.0f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
                1.0f) // �A�j���[�V��������(�b)
            .SetEase(Ease.OutCubic); // �C�[�W���O(�ω��̓x��)��ݒ�
    }

	/// <summary>
	/// �t�F�[�h�C�����J�n����
	/// </summary>
	public void StartFadeIn()
	{
		_fadeImage
            .DOFade(1.0f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
                5.5f) // �A�j���[�V��������(�b)
            .SetEase(Ease.Linear); // �C�[�W���O(�ω��̓x��)��ݒ�
    }

	/// <summary>
	/// �s������E�L�����Z���{�^����\������
	/// </summary>
	public void ShowDecideButtons()
	{
		_decideButtons.SetActive(true);
	}

	/// <summary>
	/// �s������E�L�����Z���{�^�����\���ɂ���
	/// </summary>
	public void HideDecideButtons()
	{
		_decideButtons.SetActive(false);
	}

}