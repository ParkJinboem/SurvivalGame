using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    static public bool isActivated = true;

    //���ǵ� ���� ����
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    private float applySpeed;

    [SerializeField] private float jumpForce;
    //���º���
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    // ������ üũ ����
    private Vector3 lastPos;

    //�ɾ����� �󸶳� �������� �����ϴ� ���� 
    [SerializeField] private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;


    //�� ���� ����
    private CapsuleCollider capsuleCollider;

    //�ΰ���
    [SerializeField] private float lookSensitivity;

    //ī�޶� �Ѱ�
    [SerializeField] private float cameraRotationLimit;
    private float currentCameraRotationX = 0f;
    private bool pauseCameraRotation = false;

    //������Ʈ
    [SerializeField] Camera mainCam;
    private Rigidbody myRigid;
    private GunController gunController;
    private Crosshair crosshair;
    private StatusController statusController;
    
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        gunController = FindObjectOfType<GunController>();
        crosshair = FindObjectOfType<Crosshair>();
        statusController = FindObjectOfType<StatusController>();

        //�ʱ�ȭ
        applySpeed = walkSpeed;
        originPosY = mainCam.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(isActivated)
        {
            IsGround();
            TryJump();
            TryRun();
            TryCrouch();
            Move();
            MoveCheck();
            if (!Inventory.inventoryActivated)
            {
                cameraRotation();
                CharacterRotation();
            }
        }
    }

    //�ɱ� �õ�
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }    

    //�ɱ� ����
    private void Crouch()
    {
        isCrouch = !isCrouch;
        crosshair.CrouchingAnimation(isCrouch);
        if (isCrouch)
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

    //�ε巯�� �ɱ� ���� ����
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

    // ���� üũ
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        crosshair.JumpingAnimation(!isGround);
    }

    // ���� �õ�
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround && statusController.GetCurrentSp() > 0)
        {
            Jump();
        }
    }

    // ����
    private void Jump()
    {
        //���� ���¿��� ������ �������� ����
        if(isCrouch)
        {
            Crouch();
        }
        statusController.DecreaseStamina(100);
        myRigid.velocity = transform.up * jumpForce;
    }

    // �޸��� �õ�
    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift) && statusController.GetCurrentSp() > 0)
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) || statusController.GetCurrentSp() <= 0)
        {
            RunningCancel();
        }
    }

    // �޸��� ����
    private void Running()
    {
        // �޸���� ���� ���� ����
        if (isCrouch)
        {
            Crouch();
        }

        gunController.CancelFineSight();
        
        isRun = true;
        crosshair.RunningAnimation(isRun);
        statusController.DecreaseStamina(10);
        applySpeed = runSpeed;
    }

    //�޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        crosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    // ������ ����
    Vector3 velocity;
    private void Move()
    {
        Debug.Log("����");
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        //���� * �ð�
        velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    private void MoveCheck()
    {
        if(!isRun && !isCrouch && isGround)
        {
            //if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
            //{
            //    isWalk = true;
            //}
            if (velocity.magnitude >= 0.01f)
            {
                isWalk = true;
            }
            else
            {
                isWalk = false;
            }
            crosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    // �¿� ĳ���� ȸ��
    private void CharacterRotation()
    {
        float yRotation = Input.GetAxis("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));
    }

    // ���� ī�޶� ȸ��
    private void cameraRotation()
    {
        if(!pauseCameraRotation)
        {
            float xRotation = Input.GetAxis("Mouse Y");
            float cameraRotationX = xRotation * lookSensitivity;
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            mainCam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    public IEnumerator TreeLookCoroutine(Vector3 target)
    {
        pauseCameraRotation = true;

        Quaternion direction = Quaternion.LookRotation(target - mainCam.transform.position);
        Vector3 eulerValue = direction.eulerAngles;
        float destinationX = eulerValue.x;

        while(Mathf.Abs(destinationX - currentCameraRotationX) >= 0.5f)
        {
            eulerValue = Quaternion.Lerp(mainCam.transform.localRotation, direction, 0.3f).eulerAngles;
            mainCam.transform.localRotation = Quaternion.Euler(eulerValue.x, 0f, 0f);
            currentCameraRotationX = mainCam.transform.localEulerAngles.x;
            yield return null; 
        }

        pauseCameraRotation = false;
    }

    public bool GetRun()
    {
        return isRun;
    }
}