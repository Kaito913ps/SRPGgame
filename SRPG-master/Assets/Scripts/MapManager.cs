using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    // �I�u�W�F�N�g�E�v���n�u(�C���X�y�N�^����w��)
    public Transform _blockParent;
    public GameObject _blockPrefab_Grass; // ���u���b�N
    public GameObject _blockPrefab_Water; // ����u���b�N

	// �}�b�v�f�[�^
	public MapBlock[,] _mapBlocks;


	// �萔��`
	public const int MAP_WIDTH = 9;
    public const int MAP_HEIGHT = 9; 
    private const int GENERATE_RATIO_GRASS = 90;

    void Start()
    {
		// �}�b�v�f�[�^��������
		_mapBlocks = new MapBlock[MAP_WIDTH, MAP_HEIGHT];

		// �u���b�N�����ʒu�̊�_�ƂȂ���W��ݒ�
		Vector3 defaultPos = new Vector3(0.0f, 0.0f, 0.0f); 
        defaultPos.x = -(MAP_WIDTH / 2); 
        defaultPos.z = -(MAP_HEIGHT / 2);

		// �u���b�N��������
		for (int i = 0; i < MAP_WIDTH; i++)
		{// �}�b�v�̉������J��Ԃ�����
			for (int j = 0; j < MAP_HEIGHT; j++)
			{
				Vector3 pos = defaultPos; 
				pos.x += i; // 1�ڂ�for���̌J��Ԃ��񐔕�x���W�����炷
				pos.z += j; // 2�ڂ�for���̌J��Ԃ��񐔕�z���W�����炷

				// �u���b�N�̎�ނ�����
				int rand = Random.Range(0, 100); 
				bool isGrass = false; 
				if (rand < GENERATE_RATIO_GRASS)
					isGrass = true;

				// �I�u�W�F�N�g�𐶐�
				GameObject obj; 
				if (isGrass)
				{// ���u���b�N�����t���O�FON
					obj = Instantiate(_blockPrefab_Grass, _blockParent);
				}
				else
				{// ���u���b�N�����t���O�FOFF
					obj = Instantiate(_blockPrefab_Water, _blockParent);
				}
				// �I�u�W�F�N�g�̍��W��K�p
				obj.transform.position = pos;

				// �z��mapBlocks�Ƀu���b�N�f�[�^���i�[
				var mapBlock = obj.GetComponent<MapBlock>();
				_mapBlocks[i, j] = mapBlock;
				// �u���b�N�f�[�^�ݒ�
				mapBlock._xPos = (int)pos.x; 
				mapBlock._zPos = (int)pos.z; 			}
		}
	}

	/// <summary>
	/// �S�Ẵu���b�N�̑I����Ԃ���������
	/// </summary>
	public void AllSelectionModeClear()
	{
		for (int i = 0; i < MAP_WIDTH; i++)
			for (int j = 0; j < MAP_HEIGHT; j++)
				_mapBlocks[i, j].SetSelectionMode(MapBlock.Highlight.Off);
	}

	/// <summary>
	/// �n���ꂽ�ʒu����L�����N�^�[�����B�ł���ꏊ�̃u���b�N�����X�g�ɂ��ĕԂ�
	/// </summary>
	/// <param name="xPos">��_x�ʒu</param>
	/// <param name="zPos">��_z�ʒu</param>
	/// <returns>�����𖞂����u���b�N�̃��X�g</returns>
	public List<MapBlock> SearchReachableBlocks(int xPos, int zPos)
	{
		// �����𖞂����u���b�N�̃��X�g
		var results = new List<MapBlock>();

		// ��_�ƂȂ�u���b�N�̔z����ԍ�(index)������
		int baseX = -1, baseZ = -1; 
		for (int i = 0; i < MAP_WIDTH; i++)
		{
			for (int j = 0; j < MAP_HEIGHT; j++)
			{
				if ((_mapBlocks[i, j]._xPos == xPos) &&
					(_mapBlocks[i, j]._zPos == zPos))
				{
					baseX = i;
					baseZ = j;
					break;
				}
			}
			if (baseX != -1)
				break;
		}

		// �ړ�����L�����N�^�[�̈ړ����@���擾
		var moveType = Character.MoveType.Rook; 
		var moveChara = GetComponent<CharactersManager>().GetCharacterDataByPos(xPos, zPos); 
		if (moveChara != null)
			moveType = moveChara._moveType; 

		// �L�����N�^�[�̈ړ����@�ɍ��킹�ĈقȂ�����̃u���b�N�f�[�^���擾���Ă���
		// �c�E��
		if (moveType == Character.MoveType.Rook ||
			moveType == Character.MoveType.Queen)
		{
			// X+����
			for (int i = baseX + 1; i < MAP_WIDTH; i++)
				if (AddReachableList(results, _mapBlocks[i, baseZ]))
					break;
			// X-����
			for (int i = baseX - 1; i >= 0; i--)
				if (AddReachableList(results, _mapBlocks[i, baseZ]))
					break;
			// Z+����
			for (int j = baseZ + 1; j < MAP_HEIGHT; j++)
				if (AddReachableList(results, _mapBlocks[baseX, j]))
					break;
			// Z-����
			for (int j = baseZ - 1; j >= 0; j--)
				if (AddReachableList(results, _mapBlocks[baseX, j]))
					break;
		}
		// �΂߂ւ̈ړ�
		if (moveType == Character.MoveType.Bishop ||
			moveType == Character.MoveType.Queen)
		{
			// X+Z+����
			for (int i = baseX + 1, j = baseZ + 1;
				i < MAP_WIDTH && j < MAP_HEIGHT;
				i++, j++)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
			// X-Z+����
			for (int i = baseX - 1, j = baseZ + 1;
				i >= 0 && j < MAP_HEIGHT;
				i--, j++)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
			// X+Z-����
			for (int i = baseX + 1, j = baseZ - 1;
				i < MAP_WIDTH && j >= 0;
				i++, j--)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
			// X-Z-����
			for (int i = baseX - 1, j = baseZ - 1;
				i >= 0 && j >= 0;
				i--, j--)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
		}
		// �����̃u���b�N
		results.Add(_mapBlocks[baseX, baseZ]);

		return results;
	}

	/// <summary>
	/// (�L�����N�^�[���B�u���b�N���������p)
	/// �w�肵���u���b�N�𓞒B�\�u���b�N���X�g�ɒǉ�����
	/// </summary>
	/// <param name="reachableList">���B�\�u���b�N���X�g</param>
	/// <param name="targetBlock">�Ώۃu���b�N</param>
	/// <returns>�s���~�܂�t���O(�s���~�܂�Ȃ�true���Ԃ�)</returns>
	private bool AddReachableList(List<MapBlock> reachableList, MapBlock targetBlock)
	{
		// �Ώۂ̃u���b�N���ʍs�s�Ȃ炻�����s���~�܂�Ƃ��ďI��
		if (!targetBlock._passable)
			return true;

		// �Ώۂ̈ʒu�ɑ��̃L���������ɂ���Ȃ瓞�B�s�ɂ��ďI��(�s���~�܂�ɂ͂��Ȃ�)
		var charaData =	GetComponent<CharactersManager>().GetCharacterDataByPos(targetBlock._xPos, targetBlock._zPos);
		if (charaData != null)
			return false;

		reachableList.Add(targetBlock);
		return false;
	}

	/// <summary>
	/// �n���ꂽ�ʒu����L�����N�^�[���U���ł���ꏊ�̃}�b�v�u���b�N�����X�g�ɂ��ĕԂ�
	/// </summary>
	/// <param name="xPos">��_x�ʒu</param>
	/// <param name="zPos">��_z�ʒu</param>
	/// <returns>�����𖞂����}�b�v�u���b�N�̃��X�g</returns>
	public List<MapBlock> SearchAttackableBlocks(int xPos, int zPos)
	{
		// �����𖞂����}�b�v�u���b�N�̃��X�g
		var results = new List<MapBlock>();

		// ��_�ƂȂ�u���b�N�̔z����ԍ�(index)������
		int baseX = -1, baseZ = -1;
		for (int i = 0; i < MAP_WIDTH; i++)
		{
			for (int j = 0; j < MAP_HEIGHT; j++)
			{
				if ((_mapBlocks[i, j]._xPos == xPos) &&
					(_mapBlocks[i, j]._zPos == zPos))
				{
					baseX = i;
					baseZ = j;
					break; 
				}
			}
			// ���ɔ����ς݂Ȃ�1�ڂ̃��[�v�𔲂���
			if (baseX != -1)
				break;
		}

		// 4������1�}�X�i�񂾈ʒu�̃u���b�N�����ꂼ��Z�b�g
		// (�c�E��4�}�X)
		// X+����
		AddAttackableList(results, baseX + 1, baseZ);
		// X-����
		AddAttackableList(results, baseX - 1, baseZ);
		// Z+����
		AddAttackableList(results, baseX, baseZ + 1);
		// Z-����
		AddAttackableList(results, baseX, baseZ - 1);
		// (�΂�4�}�X)
		// X+Z+����
		AddAttackableList(results, baseX + 1, baseZ + 1);
		// X-Z+����
		AddAttackableList(results, baseX - 1, baseZ + 1);
		// X+Z-����
		AddAttackableList(results, baseX + 1, baseZ - 1);
		// X-Z-����
		AddAttackableList(results, baseX - 1, baseZ - 1);

		return results;
	}

	/// <summary>
	/// (�L�����N�^�[�U���\�u���b�N���������p)
	/// �}�b�v�f�[�^�̎w�肳�ꂽ�z����ԍ��ɑΉ�����u���b�N���U���\�u���b�N���X�g�ɒǉ�����
	/// </summary>
	/// <param name="attackableList">�U���\�u���b�N���X�g</param>
	/// <param name="indexX">X�����̔z����ԍ�</param>
	/// <param name="indexZ">Z�����̔z����ԍ�</param>
	private void AddAttackableList(List<MapBlock> attackableList, int indexX, int indexZ)
	{
		// �w�肳�ꂽ�ԍ����z��̊O�ɏo�Ă�����ǉ������I��
		if (indexX < 0 || indexX >= MAP_WIDTH ||
			indexZ < 0 || indexZ >= MAP_HEIGHT)
			return;

		attackableList.Add(_mapBlocks[indexX, indexZ]);
	}

	/// <summary>
	/// �}�b�v�f�[�^�z������X�g�ɂ��ĕԂ�
	/// </summary>
	/// <returns>�}�b�v�f�[�^�̃��X�g</returns>
	public List<MapBlock> MapBlocksToList()
	{
		// ���ʗp���X�g
		var results = new List<MapBlock>();

		for (int i = 0; i < MAP_WIDTH; i++)
		{
			for (int j = 0; j < MAP_HEIGHT; j++)
			{
				results.Add(_mapBlocks[i, j]);
			}
		}

		return results;
	}

}
