using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharactersManager : MonoBehaviour
{
    // �S�L�����N�^�[�I�u�W�F�N�g�̐e�I�u�W�F�N�gTransform
    public Transform _charactersParent;

	// �S�L�����N�^�[�f�[�^
	[HideInInspector]
	public List<Character> _characters = new List<Character>();

	void Start()
	{
		// (charactersParent�ȉ��̑SCharacter�R���|�[�l���g�����������X�g�Ɋi�[)
		_charactersParent.GetComponentsInChildren(_characters);

		// �f�[�^�}�l�[�W������f�[�^�Ǘ��N���X���擾
		Data data = GameObject.Find("DataManager").GetComponent<Data>();

		// �X�e�[�^�X�㏸�ʃf�[�^��K�p����
		foreach (Character charaData in _characters)
		{
			// �G�L�����N�^�[�̏ꍇ�͋������Ȃ�
			if (charaData._isEnemy)
				continue;

            // �L�����N�^�[�̔\�͂��㏸������
            //�ő�HP
            charaData._nowHP += data._addHP;
            // ����HP
            charaData._maxHP += data._addHP;
            // �U����
            charaData._atk += data._addAtk;
            // �h���
            charaData._def += data._addDef; 
		}
	}

	/// <summary>
	/// �w�肵���ʒu�ɑ��݂���L�����N�^�[�f�[�^���������ĕԂ�
	/// </summary>
	/// <param name="xPos">X�ʒu</param>
	/// <param name="zPos">Z�ʒu</param>
	/// <returns>�Ώۂ̃L�����N�^�[�f�[�^</returns>
	public Character GetCharacterDataByPos(int xPos, int zPos)
	{
		// ��������(foreach�Ń}�b�v���̑S�L�����N�^�[�f�[�^�P�̂P�̂��ɓ����������s��)
		foreach (Character charaData in _characters)
		{
			// �L�����N�^�[�̈ʒu���w�肳�ꂽ�ʒu�ƈ�v���Ă��邩�`�F�b�N
			if ((charaData._xPos == xPos) &&(charaData._zPos == zPos)) 	
			{
                // �ʒu����v���Ă���
                return charaData; 
			}
		}

		// �f�[�^��������Ȃ����null��Ԃ�
		return null;
	}

	/// <summary>
	/// �w�肵���L�����N�^�[���폜����
	/// </summary>
	/// <param name="charaData">�ΏۃL�����f�[�^</param>
	public void DeleteCharaData(Character charaData)
	{
        // ���X�g����f�[�^���폜
        _characters.Remove(charaData);
        // �I�u�W�F�N�g�폜(�x�����s)
        DOVirtual.DelayedCall(
            0.5f, // �x������(�b)
            () =>
            {// �x�����s������e
                Destroy(charaData.gameObject);
            }
        );
		//�p�[�e�B�[�N��
        // �Q�[���I��������s��
        GetComponent<GameManager>().CheckGameSet();
	}
}