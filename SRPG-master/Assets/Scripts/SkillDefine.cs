using System.Collections;
using System.Collections.Generic;

public static class SkillDefine
{
	// ���Z�̎�ނ��`
	public enum Skill
	{
		_None, // �X�L������
		Critical,// ��S�̈ꌂ
		DefBreak,// �V�[���h�j��
		Heal, // �q�[��
		FireBall, // �t�@�C�A�{�[��
	}

	// Dictionary�œ��Z��`�Ɗe�f�[�^��R�Â���
	// ���Z��
	public static Dictionary<Skill, string> dic_SkillName = new Dictionary<Skill, string>()
	{
		{Skill._None, "�X�L������"},
		{Skill.Critical, "��S�̈ꌂ"},
		{Skill.DefBreak, "�V�[���h�j��"},
		{Skill.Heal, "�q�[��"},
		{Skill.FireBall, "�t�@�C�A�{�[��"},
	};
	// �\�����������
	public static Dictionary<Skill, string> dic_SkillInfo = new Dictionary<Skill, string>()
	{
		{Skill._None, "----"},
		{Skill.Critical, "�_���[�W�Q�{�̍U��\n(�P�����)"},
		{Skill.DefBreak, "�G�̖h��͂��O�ɂ��܂�\n(�_���[�W�͂O)"},
		{Skill.Heal, "������HP���񕜂��܂�"},
		{Skill.FireBall, "�ǂ̈ʒu�ɋ���G���U���ł��܂�\n(�_���[�W�͔���)"},
	};

}