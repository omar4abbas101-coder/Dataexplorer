using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public SpriteRenderer sprite;    

    [Header("shooting type")]
    [HideInInspector] public int bulletCount = 1;
    [SerializeField] float bulletAngle;

    [Header("Fire Rate")]
    public float baseFireCooldown = 0.25f; // normal cooldown
    public float fireCooldown = 0.25f;     // current cooldown (powerups modify this)

    [Header("Audio")]
    public AudioClip shootSfx;
    AudioSource audioSource;

    [Header("Overheat")]
    float currentOverheat;
    [HideInInspector] public bool onCoolDown;
    float currentCooldownSpeed = 0;
    public float slowdownCoof;
    [SerializeField] float cooldownSpeed;
    [SerializeField] float slowCooldownSpeed;
    [SerializeField] float overheatPerShot;
    [SerializeField] Image overheatBar;
    [SerializeField] GameObject overheatObj;
    [SerializeField] GameObject overheatIndicator;
    [SerializeField] Color barColor;
    [SerializeField] Color barOverheatColor;
    [SerializeField] GameObject overheatTextIndicator;
    [SerializeField] Color overheatPlayerColor;
    Color defaultPlayerColor;

    float fireTimer;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        fireCooldown = baseFireCooldown; // start normal
        Overheat(false);
    }

    void Start()
    {
        defaultPlayerColor = sprite.color;
    }

    void Update()
    {
        // shooting
        fireTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireCooldown;
        }

        // cooldown
        PassiveCooldown();
    }

    void Shoot()
    {
        // doesn't shoot while overheated
        if (onCoolDown) return;

        // shooting bullet(s)
        for (int i = 0; i < bulletCount; i++)
        {
            // calculating bullet angle based on how many bullets are shooting at once
            float angle = (bulletAngle / -2 * (bulletCount - 1)) + bulletAngle * i;

            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            bullet.transform.Rotate(0, 0, angle);
        }

        // playing shooting soundeffect
        if (shootSfx != null && audioSource != null) audioSource.PlayOneShot(shootSfx);

        // add overheat
        currentOverheat += overheatPerShot;
        if (currentOverheat > 1) Overheat(true);
    }

    void Overheat(bool isOverheat)
    {
        onCoolDown = isOverheat;
        currentOverheat = (isOverheat) ? 1 : 0;
        currentCooldownSpeed = (isOverheat) ? slowCooldownSpeed : cooldownSpeed;
        //sprite.color = (isOverheat) ? overheatPlayerColor : defaultPlayerColor; // for whatever reason the player just turns invisible when I try to change the color here
        overheatTextIndicator.SetActive(isOverheat);

        // slowing down the ship
        if (onCoolDown) GetComponent<PlayerController2D>().ModifySpeed(slowdownCoof);
        else GetComponent<PlayerController2D>().ModifySpeed();

            // changing bar colors
            overheatBar.color = (isOverheat) ? barOverheatColor : barColor;
    }

    void PassiveCooldown()
    {
        if (currentOverheat > 0) currentOverheat -= currentCooldownSpeed * Time.deltaTime;
        else Overheat(false);

        // updating bar visuals
        overheatObj.SetActive(currentOverheat > 0);
        overheatBar.fillAmount = currentOverheat;

        // indicator
        overheatIndicator.SetActive(currentOverheat > 0.7f || onCoolDown);
    }
}
