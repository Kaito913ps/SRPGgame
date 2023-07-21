using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BattleWindowUI : MonoBehaviour
{
	// �o�g�����ʕ\���E�B���h�EUI
	[SerializeField,Tooltip("���OText")]
	private Text _nameText;
	[SerializeField, Tooltip("HP�Q�[�WImage")]
    private Image _hpGageImage;
    [SerializeField, Tooltip("HPText")]
    private Text _hpText;
    [SerializeField, Tooltip("�_���[�W��Text")]
    private Text _damageText;

	void Start()
	{
		//���������ɃE�B���h�E���B��
		HideWindow();
	}

	/// <summary>
	/// �o�g�����ʃE�B���h�E��\������
	/// </summary>
	/// <param name="charaData">�U�����ꂽ�L�����N�^�[�̃f�[�^</param>
	/// <param name="damageValue">�_���[�W��</param>
	public void ShowWindow(Character charaData, int damageValue)
	{
		//�I�u�W�F�N�g�A�N�e�B�u��
		gameObject.SetActive(true);

		//���OText�\��
		_nameText.text = charaData._charaName;

		//�_���[�W�v�Z��̎c��HP���擾����
		int nowHP = charaData._nowHP - damageValue;
        // HP��0�`�ő�l�͈̔͂Ɏ��܂�悤�␳
        nowHP = Mathf.Clamp(nowHP, 0, charaData._maxHP);

        // HP�Q�[�W�\��
        // �\������FillAmount
        float amount = (float)charaData._nowHP / charaData._maxHP;
        // �A�j���[�V�������FillAmount
        float endAmount = (float)nowHP / charaData._maxHP;
        // HP�Q�[�W�����X�Ɍ���������A�j���[�V����
        // �ϐ������Ԃ������ĕω�������,�ω�������ϐ����w��
        DOTween.To(// �ϐ������Ԃ������ĕω�������
                 () => amount, (n) => amount = n, // �ω�������ϐ����w��
                 endAmount, // �ω���̐��l
                 1.0f) // �A�j���[�V��������(�b)
             .OnUpdate(() =>// �A�j���[�V���������t���[�����s����鏈�����w��
             {
                 // �ő�l�ɑ΂��錻��HP�̊������Q�[�WImage��fillAmount�ɃZ�b�g����
                 _hpGageImage.fillAmount = amount;
             });

        // HPText�\��(���ݒl�ƍő�l������\��)
        _hpText.text = nowHP + "/" + charaData._maxHP;
        // �_���[�W��Text�\��
        if (damageValue >= 0)
            // �_���[�W������
            _damageText.text = damageValue + "�_���[�W�I";
		else
            // HP�񕜎�
            _damageText.text = -damageValue + "�񕜁I";
	}
	/// <summary>
	/// �o�g�����ʃE�B���h�E���B��
	/// </summary>
	public void HideWindow()
	{
        // �I�u�W�F�N�g��A�N�e�B�u��
        gameObject.SetActive(false);
	}
}