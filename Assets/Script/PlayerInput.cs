using UnityEngine;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
public class PlayerInput : MonoBehaviour
{
    public VariableJoystick joystick;
    public string moveAxisName = "Vertical"; // 앞뒤
    public string rotateAxisName = "Horizontal"; // 좌우
    public string fireButtonName = "Fire1"; // 발사
    public string reloadButtonName = "Reload"; // 재장전

    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }

    private void Update()
    {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않음
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