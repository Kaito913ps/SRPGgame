using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock : MonoBehaviour
{
	// �����\���I�u�W�F�N�g
	private GameObject _selectionBlockObj;

	// �����\���}�e���A��
	[Header("�����\���}�e���A���F�I����")]
	public Material _selMat_Select;
	[Header("�����\���}�e���A���F���B�\")]
	public Material _selMat_Reachable;
	[Header("�����\���}�e���A���F�U���\")]
	public Material _selMat_Attackable; 
	public enum Highlight
	{
		Off, // �I�t
		Select, // �I����
		Reachable, // �L�����N�^�[�����B�\
		Attackable, // �L�����N�^�[���U���\
	}

	// �u���b�N�f�[�^
	[HideInInspector,Tooltip("X�����̈ʒu")] 
	public int _xPos; 
	[HideInInspector,Tooltip("Z�����̈ʒu")]
	public int _zPos; 
	[Header("�ʍs�\�t���O(true�Ȃ�ʍs�\�ł���)")]
	public bool _passable;

	void Start()
	{
        // �����\���I�u�W�F�N�g���擾
        _selectionBlockObj = transform.GetChild(0).gameObject;

        // ������Ԃł͋����\�������Ȃ�
        SetSelectionMode(Highlight.Off);
	}

	/// <summary>
	/// �I����ԕ\���I�u�W�F�N�g�̕\���E��\����ݒ肷��
	/// </summary>
	/// <param name="mode">�����\�����[�h</param>
	public void SetSelectionMode(Highlight mode)
	{
		switch (mode)
		{
			// �����\���Ȃ�
			case Highlight.Off:
				_selectionBlockObj.SetActive(false);
				break;
			// �I����
			case Highlight.Select:
				_selectionBlockObj.GetComponent<Renderer>().material = _selMat_Select;
				_selectionBlockObj.SetActive(true);
				break;
			// �L�����N�^�[�����B�\
			case Highlight.Reachable:
				_selectionBlockObj.GetComponent<Renderer>().material = _selMat_Reachable;
				_selectionBlockObj.SetActive(true);
				break;
			// �L�����N�^�[���U���\
			case Highlight.Attackable:
				_selectionBlockObj.GetComponent<Renderer>().material = _selMat_Attackable;
				_selectionBlockObj.SetActive(true);
				break;
		}
	}

}