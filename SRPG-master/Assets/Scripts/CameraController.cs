using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // �J�����ړ��p�ϐ�
    // �J������]���t���O
    private bool _isCameraRotate;
    // ��]�������]�t���O
    private bool _isMirror;

    // ��]���x
    const float SPEED = 30.0f;

	void Update()
	{
        // �J������]����
        if (_isCameraRotate)
		{
            // ��]���x���v�Z����
            float speed = SPEED * Time.deltaTime;

            // ��]�������]�t���O�������Ă���Ȃ瑬�x���]
            if (_isMirror)
				speed *= -1.0f;

            // ��_�̈ʒu�𒆐S�ɃJ��������]�ړ�������
            transform.RotateAround( Vector3.zero,Vector3.up,speed);
		}
	}

	/// <summary>
	/// �J�����ړ��{�^���������n�߂�ꂽ���ɌĂяo����鏈��
	/// </summary>
	/// <param name="rightMode">�E�����t���O(�E�ړ��{�^������Ă΂ꂽ��true�ɂȂ��Ă���)</param>
	public void CameraRotate_Start(bool rightMode)
	{
        // �J������]���t���O��ON
        _isCameraRotate = true;
        // ��]�������]�t���O��K�p����
        _isMirror = rightMode;
	}
	/// <summary>
	/// �J�����ړ��{�^����������Ȃ��Ȃ������ɌĂяo����鏈��
	/// </summary>
	public void CameraRotate_End()
	{
        // �J������]���t���O��OFF
        _isCameraRotate = false;
	}
}