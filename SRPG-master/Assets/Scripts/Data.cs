using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    // �V���O���g���Ǘ��p�ϐ�
    [HideInInspector]
	public static bool _instance = false;

    // �v���C���[�����f�[�^
    // �ő�HP�㏸��
    public int _addHP;
    // �U���͏㏸��
    public int _addAtk;
    // �h��͏㏸��
    public int _addDef;

	// �f�[�^�̃L�[��`
	public const string Key_AddHP = "Key_AddHP";
	public const string Key_AddAtk = "Key_AddAtk";
	public const string Key_AddDef = "Key_AddDef";

	private void Awake()
	{
		if (_instance)
		{
			Destroy(gameObject);
			return;
		}
		_instance = true; 

		DontDestroyOnLoad(gameObject);

		// �Z�[�u�f�[�^��PlayerPrefs����ǂݍ���
		_addHP = PlayerPrefs.GetInt(Key_AddHP, 0); 
		_addAtk = PlayerPrefs.GetInt(Key_AddAtk, 0);
		_addDef = PlayerPrefs.GetInt(Key_AddDef, 0);
	}

	/// <summary>
	/// ���݂̃v���C���[�����f�[�^��PlayerPrefs�ɕۑ�����
	/// </summary>
	public void WriteSaveData()
	{
		PlayerPrefs.SetInt(Key_AddHP, _addHP);
		PlayerPrefs.SetInt(Key_AddAtk, _addAtk);
		PlayerPrefs.SetInt(Key_AddDef, _addDef);
        // �ύX��ۑ�
        PlayerPrefs.Save(); 
	}
}