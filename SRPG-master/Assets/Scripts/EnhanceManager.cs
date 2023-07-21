using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnhanceManager : MonoBehaviour
{
	// �f�[�^�N���X
	private Data _data;

	// UI�{�^��
	[SerializeField,Tooltip("�X�e�[�^�X�㏸�{�^��")]
    private List<Button> _enhanceButtons;
	[SerializeField,Tooltip("������x�v���C�{�^��")]
	private Button _goGameButton; 

	void Start()
	{
        // �f�[�^�}�l�[�W������f�[�^�Ǘ��N���X���擾
        _data = GameObject.Find("DataManager").GetComponent<Data>();

        // �����ꂩ����������܂ł́u������x�v���C�v�{�^���������Ȃ��悤�ɂ���
        _goGameButton.interactable = false;
	}

	/// <summary>
	/// (�X�e�[�^�X�㏸�{�^��)
	/// �ő�HP���㏸����
	/// </summary>
	public void Enhance_AddHP()
	{
		// ��������
		_data._addHP += 2;
		//sound
        // ��������������
        EnhanceComplete(); 
	}
	/// <summary>
	/// (�X�e�[�^�X�㏸�{�^��)
	/// �U���͂��㏸����
	/// </summary>
	public void Enhance_AddAtk()
	{
		// ��������
		_data._addAtk += 1;
		//sound
        // ��������������
        EnhanceComplete(); 
	}
	/// <summary>
	/// (�X�e�[�^�X�㏸�{�^��)
	/// �h��͂��㏸����
	/// </summary>
	public void Enhance_AddDef()
	{
		// ��������
		_data._addDef += 1;
        //sound
        // ��������������
        EnhanceComplete();
	}
	/// <summary>
	/// �v���C���[�����������̋��ʏ���
	/// </summary>
	private void EnhanceComplete()
	{
		// �����{�^���������s�ɂ���
		foreach (Button button in _enhanceButtons)
		{
			button.interactable = false;
		}
		// �u������x�v���C�v�{�^���������\�ɂ���
		_goGameButton.interactable = true;

		// �ύX���f�[�^�ɕۑ�
		_data.WriteSaveData();
	}

	/// <summary>
	/// �Q�[���V�[���ɐ؂�ւ���
	/// </summary>
	public void GoGameScene()
	{
        //sound
        SceneManager.LoadScene("Game");
	}
}