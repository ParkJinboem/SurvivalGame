using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;
    [SerializeField] private float crouchSpeed;

    [SerializeField] private float jumpForce;
    //상태변수
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    //앉았을때 얼마나 ㅇ늦을지 결정하는 변수 
    [SerializeField] private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;


    //땅 착지 여부
    private CapsuleCollider capsuleCollider;

    //민감도
    [SerializeField] private float lookSensitivity;

    //카메라 한계
    [SerializeField] private float cameraRotationLimit;
    private float currentCameraRotationX = 0f;

    //컴포넌트
    [SerializeField] Camera mainCam;
    private Rigidbody myRigid;
    
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        applySpeed = walkSpeed;

        //초기화
        originPosY = mainCam.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }


    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        cameraRotation();
        CharacterRotation();
    }

    //앉기 시도
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }    

    //앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;
        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }

    //부드러운 앉기 동작 실행
    IEnumerator CrouchCoroutine()
    {
        float posY = mainCam.transform.localPosition.y;
        int count = 0;
        while (posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.3f);
            mainCam.transform.localPosition = new Vector3(0, posY, 0);
            if(count > 15)
            {
                break;
            }
            yield return null;
        }
        mainCam.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    // 지면 체크
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    // 점프 시도
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    // 점프
    private void Jump()
    {
        //앉은 상태에서 점프시 앉은상태 해제
        if(isCrouch)
        {
            Crouch();
        }
        myRigid.velocity = transform.up * jumpForce;
    }

    // 달리기 시도
    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    // 달리기 실행
    private void Running()
    {
        // 달리기시 앉은 상태 해제
        if (isCrouch)
        {
            Crouch();
        }
        isRun = true;
        applySpeed = runSpeed;
    }

    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    // 움직임 실행
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        //방향 * 시간
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    // 좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float yRotation = Input.GetAxis("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));
    }

    // 상하 카메라 회전
    private void cameraRotation()
    {
        float xRotation = Input.GetAxis("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        mainCam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
