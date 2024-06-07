using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : Car
{
    [SerializeField] CarModel carModel = null;
    [SerializeField] CarView carView = null;
    [SerializeField] GameObject car = null;
    [SerializeField] Rigidbody rigidbody = null;
    [SerializeField] bool isAIControlledCar = false;
    [SerializeField] AI agent = null;
    [SerializeField] SelfDrivingCar pathFollower = null;
    [SerializeField] GameObject racingCameraObject = null;
    [SerializeField] GameObject carHeadlightFlare1 = null;
    [SerializeField] GameObject carHeadlightFlare2 = null;
    [SerializeField] GameObject nitrous1 = null;
    [SerializeField] GameObject nitrous2 = null;
    [SerializeField] AudioSource gasAudio = null;
    [SerializeField] AudioSource brakeAudio = null;
    [SerializeField] AudioSource nitrousAudio = null;
    [SerializeField] GameObject brakeLight1 = null;
    [SerializeField] GameObject brakeLight2 = null;
    [SerializeField] AudioSource crashAudio = null;
    [SerializeField] GameObject nitrousUI = null;
    [SerializeField] GameObject interiorView = null;
    [SerializeField] List<MeshRenderer> bodyMeshRenderers = null;
    [SerializeField] List<GameObject> bodyGameObjects = null;
    [SerializeField] MeshCollider bodyCollider = null;
    [SerializeField] GameObject rearViewMirrorCamera = null;
    bool nitrousReady = false;
    [SerializeField] bool raceStarted;
    float nitrousTimer;

    [System.Serializable]

    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;


    }


    public override void Start()
    {
        base.Start();

        if (isAIControlledCar)
        {
            Events.CarSpawnedToTrack?.Invoke(this);
        }

        Events.RaceStarted += HandleRaceStarted;
        if (!isAIControlledCar)
        {
            carModel.SetCarLabel(GameManager.GetUsername());
        }

        if (nitrous1 != null)
        {
            nitrous1.SetActive(false);
        }

        if (nitrous2 != null)
        {
            nitrous2.SetActive(false);
        }
    }

    public override void Update()
    {
        base.Update();
        if (isAIControlledCar || !raceStarted)
        {
            return;
        }

        Quaternion tempRotation = transform.rotation;
        tempRotation.x = 0;
        tempRotation.z = 0;
        transform.rotation = tempRotation;
        if (Input.GetKeyDown(KeyCode.C))
        {
            interiorView.SetActive(!interiorView.activeInHierarchy);
        }

        if (nitrousReady && Input.GetKeyDown(KeyCode.N))
        {
            UseNitrous();
        }

        nitrousUI.SetActive(nitrousReady);

        foreach (MeshRenderer meshRenderer in bodyMeshRenderers)
        {
            meshRenderer.enabled = !interiorView.activeInHierarchy;
        }

        foreach (GameObject bodyGameObject in bodyGameObjects)
        {
            //bodyGameObject.SetActive(!interiorView.activeInHierarchy);
        }

        bodyCollider.enabled = !interiorView.activeInHierarchy;
        rearViewMirrorCamera.SetActive(interiorView.activeInHierarchy);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            HandleGasPedal();
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandleBrakePedal();
        }

        brakeLight1.SetActive(Input.GetKey(KeyCode.DownArrow));
        brakeLight2.SetActive(Input.GetKey(KeyCode.DownArrow));
        if (!nitrousReady && nitrousTimer >= 30f)
        {
            nitrousReady = true;
        }
        else if (!nitrousReady)
        {
            nitrousTimer += Time.deltaTime;
        }
    }



    private void OnDestroy()
    {
        Events.RaceStarted -= HandleRaceStarted;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (crashAudio != null && !crashAudio.isPlaying)
        {
            crashAudio.Play();
        }
    }

    private void UseNitrous()
    {
        nitrousReady = false;
        nitrousUI.SetActive(false);
        ToggleNitrous(true);
        IncreaseSpeed();
        Invoke("HideNitrousEffect", 5f);
        nitrousTimer = 0;
    }

    private void HideNitrousEffect()
    {
        DecreaseSpeed();
        ToggleNitrous(false);
    }

    public void DisplayCar(bool shouldShow, bool applyCustomization = false, bool showHeadlightFlares = false)
    {
        car.SetActive(shouldShow);
        if (applyCustomization)
        {
            SetCarColorAndRims();
        }

        ToggleHeadlightFlares(showHeadlightFlares);
    }

    public GameObject GetCar()
    {
        return car;
    }

    public string GetCarName()
    {
        return carModel.GetCarName();
    }

    public float GetCarPrice()
    {
        return carModel.GetCarPrice();
    }

    public void SetCarColorAndRims()
    {
       // carView.SetCarColorAndRims();
    }

    public void SetCarColor(Color color)
    {
        carView.SetCarColor(color);
    }

    public void SetRimMaterial(Material material)
    {
        carView.SetRimMaterial(material);
    }

    public Material[] GetCarBodyMaterials()
    {
        return carView.GetCarBodyMaterials();
    }

    public Material GetRimMaterial()
    {
        return carView.GetRimMaterial();
    }

    public float GetSpeed()
    {
        return rigidbody.velocity.magnitude;
    }

    public void HandleCheckpointWasHit()
    {
        carModel.CheckPointWasHit();
    }

    public void SetCarLabel(string label)
    {
        carModel.SetCarLabel(label);
    }

    public string GetCarLabel()
    {
        return carModel.GetCarLabel();
    }

    public void SetDistanceToNextCheckpoint(float distance)
    {
        carModel.SetDistanceToNextCheckpoint(distance);
    }

    public float GetDistanceToNextCheckpoint()
    {
        return carModel.GetDistanceToNextCheckpoint();
    }

    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float motor;


    public void FixedUpdate()
    {
        if (isAIControlledCar || !raceStarted)
        {
            return;
        }

        motor = maxMotorTorque * Input.GetAxis("Vertical");

        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        foreach (AxleInfo axleinfo in axleInfos)
        {
            if (axleinfo.steering)
            {
                axleinfo.leftWheel.steerAngle = steering;
                axleinfo.rightWheel.steerAngle = steering;

            }

            if (axleinfo.motor)
            {
                axleinfo.leftWheel.motorTorque = motor;
                axleinfo.rightWheel.motorTorque = motor;
            }


        }
    }


    public void EnableAICar()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        agent.enabled = true;
        pathFollower.enabled = true;
    }

    public GameObject CarRacingCameraObject()
    {
        return racingCameraObject;
    }

    public void ToggleNitrous(bool shouldShow)
    {
        nitrous1.SetActive(shouldShow);
        nitrous2.SetActive(shouldShow);
        if (shouldShow)
        {
            nitrousAudio.Play();
        }
        else
        {
            nitrousAudio.Stop();
        }
    }

    public void IncreaseSpeed()
    {
        maxMotorTorque *= 4;
    }

    public void DecreaseSpeed()
    {
        maxMotorTorque /= 4;
    }

    void HandleRaceStarted()
    {
        raceStarted = true;
    }

    public void ToggleHeadlightFlares(bool shouldshow)
    {
        carHeadlightFlare1.SetActive(shouldshow);
        carHeadlightFlare2.SetActive(shouldshow);


    }

    void HandleGasPedal()
    {
        if (gasAudio.isPlaying)
        {
            return;
        }

        gasAudio.Play();
    }

    void HandleBrakePedal()
    {
        if (brakeAudio.isPlaying)
        {
            return;
        }

        brakeAudio.Play();
    }


















}

