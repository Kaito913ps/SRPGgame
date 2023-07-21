using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    // ���C���J����
    private Camera _mainCamera;

    // �萔��`
    // �Y�[�����x
    const float ZOOM_SPEED = 0.1f;
    // �J�����̍ŏ��̎���
    const float ZOOM_MIN = 40.0f;
    // �J�����̍ő�̎���
    const float ZOOM_MAX = 60.0f; 

	void Start()
	{
        // �J�����̎Q�Ƃ��擾
        _mainCamera = GetComponent<Camera>(); 
	}

	void Update()
	{
        // �}���`�^�b�`(�Q�_�����^�b�`)�łȂ��Ȃ�I��
        if (Input.touchCount != 2)
			return;

		// �Q�_�̃^�b�`�����擾����
		var touchData_0 = Input.GetTouch(0);
		var touchData_1 = Input.GetTouch(1);

        // �P�t���[���O�̂Q�_�Ԃ̋��������߂�
        float oldTouchDistance = Vector2.Distance( 
			touchData_0.position - touchData_0.deltaPosition, 
			touchData_1.position - touchData_1.deltaPosition
			);

		// ���݂̂Q�_�Ԃ̋��������߂�
		float currentTouchDistance = Vector2.Distance(touchData_0.position, touchData_1.position);

		// �Q�_�Ԃ̋����̕ω��ʂɉ����ăY�[������(�J�����̎���̍L����ύX����)
		float distanceMoved = oldTouchDistance - currentTouchDistance;
		_mainCamera.fieldOfView += distanceMoved * ZOOM_SPEED;

		// �J�����̎�����w��͈̔͂Ɏ��߂�
		_mainCamera.fieldOfView = Mathf.Clamp(_mainCamera.fieldOfView, ZOOM_MIN, ZOOM_MAX);
	}
}