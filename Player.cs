using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Vector3 dir;
    Vector3 roDir;

    Camera cam;
    RaycastHit hit;
    CharacterController characterController;

    [SerializeField] Option option;

    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isStop;

    [SerializeField] CameraCtrl CCTV;
    [SerializeField] Slider slider;

    public float stamina;
    [SerializeField] float speed;

    public float mouseSensitive;

    float isRun = 1;

    bool isBreath;
    bool isHardRun;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        stamina = 3;
    }

    void Update()
    {
        DeadCheck();
        if(option.isActive == false && cam.enabled == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void DeadCheck()
    {
        if (GameMng.Instance.isClearStage == false)
        {
            if(isDead == false)
            {
                if (isStop == false)
                    Move();
                Interacion();
            }
        }
    }
    [SerializeField] GameObject interactionUI;
    float range = 0.5f;
    void Interacion()
    {
        if(GameMng.Instance.energy > 0)
        {
            if (cam.transform.rotation.x > 0.2f || cam.transform.rotation.x < -0.12f)
                range = 100;
            else
                range = 0.5f;
            if(Physics.Raycast(transform.position, transform.forward + transform.up * range, out hit, 2f) && cam.enabled == true)
            {
                if (hit.transform.gameObject.tag == "Door")
                {
                    interactionUI.SetActive(true);
                    Button button = hit.transform.gameObject.GetComponent<Button>();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (button.isOpen == false)
                        {
                            button.isOpen = true;
                            AudioMng.Instance.BgmOn("Door");
                            button.animator.SetTrigger("Open");
                        }
                        else
                        {
                            button.isOpen = false;
                            AudioMng.Instance.BgmOn("Door");
                            button.animator.SetTrigger("Close");
                        }
                    }
                }
                else if (hit.transform.gameObject.tag == "CCTV")
                {
                    interactionUI.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (CCTV.isOpen == false)
                            CCTV.isOpen = true;
                    }
                }
                else if (hit.transform.gameObject.tag == "Storage")
                {
                    interactionUI.SetActive(true);
                    Door door = hit.transform.gameObject.GetComponent<Door>();
                    if (Input.GetKey(KeyCode.E))
                        door.isCharge = true;
                    else
                        door.isCharge = false;
                }
            }
            else
                interactionUI.SetActive(false);
        }
    }

    void Breath()
    {
        if (stamina >= 0.25f && isBreath == true)
            StartCoroutine(Breathing());
    }

    void Move()
    {
        float gravity = -9.81f;
        float diagonal = 1;

        dir.x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed * isRun;
        dir.z = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed * isRun;

        if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(dir.x) + Mathf.Abs(dir.z) > 0 && isHardRun == false)
        {
            if (stamina > 0)
            {
                isRun = 1.5f;
                if (stamina <= 0)
                    stamina = 0;
                else
                    stamina -= Time.deltaTime;
                slider.value = stamina / 3;
            }
            else
            {
                isHardRun = true;
                isBreath = true;
                isRun = 1;
            }
        }
        else
        {
            Breath();
            isRun = 1;
            if (stamina >= 3f)
            {
                isHardRun = false;
                stamina = 3;
            }
            else
                stamina += Time.deltaTime;
            slider.value = stamina / 3;
        }    


        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) == 2)
            diagonal = 0.75f;

        roDir.x += Input.GetAxisRaw("Mouse Y") * -1 * mouseSensitive * Time.deltaTime;
        roDir.y += Input.GetAxisRaw("Mouse X") * mouseSensitive * Time.deltaTime;

        roDir.x = Mathf.Clamp(roDir.x, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(roDir.x, roDir.y, 0f);

        transform.rotation = Quaternion.Euler(0f, roDir.y, 0f);
        characterController.Move(transform.rotation * new Vector3(dir.x, 0f, dir.z) * diagonal);

        dir.y += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0f, dir.y, 0f) * Time.deltaTime);
    }

    IEnumerator Breathing()
    {
        isBreath = false;
        AudioMng.Instance.BgmOn("Breath1");
        yield return new WaitForSeconds(1.2f);
        AudioMng.Instance.BgmOn("Breath2");
    }
}
