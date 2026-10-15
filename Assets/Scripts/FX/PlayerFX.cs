using System.Collections;
using UnityEngine;

public class PlayerFX : EntityFX
{
    private bool canScreenShake = true;

    [Header("Screen Shake FX")]
    [SerializeField] private Transform cineCameraTransform;
    public float mediunumShakeStrength;
    public float heavyShakeStrength;
    public float lightShakeStrength;
    public float lightShakeDuration;
    public float mediunumShakeDuration;
    public float heavyShakeDuration;

    [Header("Afterimage FX")]
    [SerializeField] private GameObject afterimagePrefab;
    [SerializeField] private float afterimageColorLosingSpeed;
    [SerializeField] private float afterimageCooldown;
    private float afterimageCooldownTimer;

    [Space]
    [SerializeField] private ParticleSystem dustFX;
    [SerializeField] private ParticleSystem downStrikeDustFX;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        afterimageCooldownTimer = 0;
    }

    protected override void Update()
    {
        base.Update();

        afterimageCooldownTimer -= Time.deltaTime;
    }
    public void ScreenShake(float duration, float strength)
    {
        if(canScreenShake){
            StartCoroutine(Shake(duration, strength));
        }
    }

    IEnumerator Shake(float duration, float strength)
    {
        //to prevent gigantic screenshake when hitting multiple enemies at the same time
        if (canScreenShake)
        {
            Transform cameraTransform = cineCameraTransform;
            Vector3 startPosition = cameraTransform.position;

            while(duration > 0){
                cameraTransform.position = Random.insideUnitSphere * strength + startPosition;
                duration -= Time.deltaTime;
                yield return null;
            }
            cineCameraTransform.position = startPosition;
            canScreenShake = true;
        }
    }
    public void AttackPause(int duration){
        StartCoroutine(Pause(duration));
    }
    IEnumerator Pause(int duration){
        float pauseTime = duration / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }

    public void CreateAfterimage()
    {
        if (afterimageCooldownTimer < 0)
        {
            //need to pass transform.rotation here to make afterimage flip together with player when dashing to the left
            GameObject newAfterimage = Instantiate(afterimagePrefab, transform.position, transform.rotation);
            newAfterimage.GetComponent<AfterimageFX>()?.SetupAfterImage(sr.sprite, afterimageColorLosingSpeed);

            afterimageCooldownTimer = afterimageCooldown;
        }
    }

    public void PlayDustFX()
    {
        if (dustFX != null)
        {
            dustFX.Play();
        }
    }

    public void PlayDownStrikeDustFX()
    {
        if (downStrikeDustFX != null)
        {
            downStrikeDustFX.Play();
        }
    }
}
