using UnityEngine;

// �÷��̾� ĳ���͸� �����ϱ� ���� ����� �Է��� ����
// ������ �Է°��� �ٸ� ������Ʈ���� ����� �� �ֵ��� ����
public class PlayerInput : MonoBehaviour
{
    public VariableJoystick joystick;
    public string moveAxisName = "Vertical"; // �յ�
    public string rotateAxisName = "Horizontal"; // �¿�
    public string fireButtonName = "Fire1"; // �߻�
    public string reloadButtonName = "Reload"; // ������

    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }

    private void Update()
    {
        // ���ӿ��� ���¿����� ����� �Է��� �������� ����
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        //move = Input.GetAxis(moveAxisName);
        //rotate = Input.GetAxis(rotateAxisName);
        //fire = Input.GetButton(fireButtonName);
        //reload = Input.GetButtonDown(reloadButtonName);
        move = joystick.Vertical;
        rotate = joystick.Horizontal; 
    }
}