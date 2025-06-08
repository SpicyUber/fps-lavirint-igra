using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    public Weapon GunObject;
    public HudScript Hud;
    public Rigidbody PlayerRB;
    public CinemachineCamera Camera;
    public Transform CameraPositionTransform;
    public AudioSource Walking;
    public float MouseSensitivity;
    public float MoveSpeed;
    public GameObject PauseMenu;
    private Vector2 _moveDir;
    private Vector2 _mouseDir;
    private float _pitch, _yaw;
    private Transform _playerComponentsTransform;
    private Vector3 _startCameraPosition;
    private float _t = 0;
    private bool _dead = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         GunObject = GetComponentInChildren<Gun>();
        if (GunObject == null) throw new UnityException("Could not find Gun in the children gameobjects of the player. Please add Gun Prefab inside of PlayerComponents");
        _playerComponentsTransform = transform.GetChild(0);
        _startCameraPosition = CameraPositionTransform.localPosition;
        PlayerRB.maxLinearVelocity = 10;
        _playerComponentsTransform.rotation = Quaternion.identity;
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity") * 50f;
        if (MouseSensitivity < 1f) MouseSensitivity = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (PlayerPrefs.GetInt("Hint") > 0) { GetComponent<HealthComponent>().MaxHealth = GetComponent<HealthComponent>().MaxHealth + 75f * PlayerPrefs.GetInt("Hint"); GetComponent<HealthComponent>().CurrentHealth = GetComponent<HealthComponent>().MaxHealth;  }
        if (PlayerPrefs.GetInt("Checkpoint") == 2) { _pitch = -2.8f; _yaw = 67.2f; transform.position = new Vector3(-126.145065f, 2.77638674f, 27.2838383f); }
        else if (PlayerPrefs.GetInt("Checkpoint") == 1) { _pitch = 4.8f; _yaw = 90; transform.position = new Vector3(-282.71994f, -0.51327306f, 171.500427f); } else { _pitch = 4.8f; _yaw = 90; }

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

   

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void LateUpdate()
    {
        UpdateRotation();
        UpdateHeadBob();
        UpdateCamera();


    }
    private void UpdateHeadBob()
    { if (!Physics.Raycast(transform.position, Vector3.down, 0.5f)) { Walking.Pause(); return; }


        if (_moveDir.magnitude <= 0.01f) { _t = 0; if (Walking.isPlaying) { Walking.Pause(); } }
        CameraPositionTransform.localPosition = _startCameraPosition+new Vector3(0,Mathf.Sin(_t*8f)/32f,0);
        if (_moveDir.magnitude >0.01f ) { _t+=Time.deltaTime; if (!Walking.isPlaying) { Walking.Play(); } }
    }
    private void UpdateRotation()
    {
        _yaw += _mouseDir.x * MouseSensitivity * Time.deltaTime;
        _pitch -= _mouseDir.y * MouseSensitivity / 2 * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -50f, 50f);

       
        _playerComponentsTransform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

        

    }
    private void UpdateMovement() {
        Vector3 gravity = (!Physics.Raycast(transform.position,Vector3.down,0.5f))? new Vector3(0, -5, 0) : Vector3.zero; 
        Vector3 temp = _moveDir.y * _playerComponentsTransform.forward + _moveDir.x * _playerComponentsTransform.right;
        temp.y = 0;
        PlayerRB.AddForce((temp).normalized * MoveSpeed+gravity, ForceMode.Impulse);
    }
    private void UpdateCamera() {

       
        

        Camera.transform.position = CameraPositionTransform.position;
        Camera.transform.rotation = CameraPositionTransform.rotation;
    }
    public void OnMove(InputValue value) { _moveDir = value.Get<Vector2>(); }
    public void OnLook(InputValue value) {_mouseDir = value.Get<Vector2>(); 
         
    }
    public void OnAttack(InputValue value) { if (PauseMenu.activeInHierarchy) return; if (!_dead && GunObject.UseAttack()) { UpdateCamera(); GetComponentInChildren<Animator>().SetTrigger("Shoot"); RecoilCameraShake(); } }
    
    public void Death() { GetComponent<Collider>().enabled = false; _dead = true; GetComponent<AudioSource>().Play(); if (Hud == null) return; Hud.YouDied(); Cursor.visible = true; Cursor.lockState = CursorLockMode.None; }

    public void RecoilCameraShake() { if (Hud == null) return; Hud.RecoilCameraShake(1f); }

    public void PlayHurtSound() { GetComponent<AudioSource>().Play(); }

    public void OnPause()
    { if (PauseMenu == null || _dead || FindAnyObjectByType<TheTimer>().CurrentTime<=0) return;

        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);

        Time.timeScale = (!PauseMenu.activeInHierarchy) ? 1f:0f;

        if (!PauseMenu.activeInHierarchy && !_dead) { Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity") * 50f; } else { Cursor.visible = true; Cursor.lockState = CursorLockMode.None; }


    }
}
