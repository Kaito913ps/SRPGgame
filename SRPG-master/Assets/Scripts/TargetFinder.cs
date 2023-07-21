using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Static�ŃN���X���`����
public static class TargetFinder
{
	// �s���v�����N���X
	// (�s������G�L�����N�^�[�A�ړ���̈ʒu�A�U������̃L�����N�^�[�̂R�f�[�^���P�܂Ƃ߂Ɉ���)
	public class ActionPlan
	{
        // �s������G�L�����N�^�[
        public Character _charaData;
        // �ړ���̈ʒu
        public MapBlock _toMoveBlock;
        // �U������̃L�����N�^�[
        public Character _toAttackChara; 
	}

	/// <summary>
	/// �U���\�ȍs���v������S�Č������A���̓��̂P�������_���ɕԂ�����
	/// </summary>
	/// <param name="mapManager">�V�[������MapManager�̎Q��</param>
	/// <param name="charactersManager">�V�[������CharactersManager�̎Q��</param>
	/// <param name="enemyCharas">�G�L�����N�^�[�̃��X�g</param>
	/// <returns></returns>
	public static ActionPlan GetRandomActionPlan(MapManager mapManager, CharactersManager charactersManager, List<Character> enemyCharas)
	{
		// �S�s���v����(�U���\�ȑ��肪������x�ɒǉ������)
		var actionPlans = new List<ActionPlan>();
		// �ړ��͈̓��X�g
		var reachableBlocks = new List<MapBlock>();
		// �U���͈̓��X�g
		var attackableBlocks = new List<MapBlock>();

		// �S�s���v������������
		foreach (Character enemyData in enemyCharas)
		{
			// �ړ��\�ȏꏊ���X�g���擾����
			reachableBlocks = mapManager.SearchReachableBlocks(enemyData._xPos, enemyData._zPos);
			// ���ꂼ��̈ړ��\�ȏꏊ���Ƃ̏���
			foreach (MapBlock block in reachableBlocks)
			{
				// �U���\�ȏꏊ���X�g���擾����
				attackableBlocks = mapManager.SearchAttackableBlocks(block._xPos, block._zPos);
				// ���ꂼ��̍U���\�ȏꏊ���Ƃ̏���
				foreach (MapBlock attackBlock in attackableBlocks)
				{
					// �U���ł��鑊��L�����N�^�[(�v���C���[���̃L�����N�^�[)��T��
					Character targetChara =
						charactersManager.GetCharacterDataByPos(attackBlock._xPos, attackBlock._zPos);
					if (targetChara != null &&
						!targetChara._isEnemy)
					{
						var newPlan = new ActionPlan();
						newPlan._charaData = enemyData;
						newPlan._toMoveBlock = block;
						newPlan._toAttackChara = targetChara;

						// �S�s���v�������X�g�ɒǉ�
						actionPlans.Add(newPlan);
					}
				}
			}
		}

        // �����I����A�s���v�������P�ł�����Ȃ炻�̓��̂P�������_���ɕԂ�
        if (actionPlans.Count > 0)
			return actionPlans[Random.Range(0, actionPlans.Count)];
        // �s���v�����������Ȃ�null��Ԃ�
        else
            return null;
	}
}