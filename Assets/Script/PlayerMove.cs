using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMove : MonoBehaviour
{
    public float Speed = 10f;
    //public float moveSpeed = 5f;
    //public float rotateSpeed = 180f;


    private PlayerInput playerInput; 
    private Rigidbody playerRigidbody; 
    private Animator playerAnimator; 
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //Rotate();
        //Move();
        float moveSpeed = playerInput.move * Speed;
        float rotateSpeed = playerInput.rotate * Speed;
        float SpeedMove = moveSpeed * rotateSpeed;

        Vector3 newVel = new Vector3(rotateSpeed, 0f, moveSpeed);
        if (moveSpeed != 0f || rotateSpeed != 0f)
        {
            playerRigidbody.transform.rotation = Quaternion.LookRotation(newVel);
            playerRigidbody.MovePosition(playerRigidbody.position + transform.forward * Speed * Time.deltaTime);
        }
        playerAnimator.SetFloat("Move", SpeedMove);
    }

    //private void Move()
    //{
    //    Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;

    //    playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    //}

    //private void Rotate()
    //{
    //    float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;

    //    playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    //}
}