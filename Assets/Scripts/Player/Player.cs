using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Basic,
    Shotgun,
    Nova,
    Volley,
    BacklogMissile
}

public class Player : MonoBehaviour
{
    public static Player playerInstance;

    [Header("Player Movement Stats")]
    public float thrustForce;
    public float slowPlayerMultiplier = .25f;
    private float defaultPlayerSpeed = 1000f;
    public float boostForce;
    public float boostCooldown = 7f;
    private float lastBoostTime;
    private float nextBoostTime;

    public int MicromanageCount { get; private set; }

    [Header("Player Weapon Bullet Options")]
    public Projectile basicProjectile;
    public Projectile volleyProjectile;
    public Projectile novaProjectile;
    public Projectile shottyProjectile;
    public Projectile backlogRocket;
    private Projectile currentSetProjectile;

    [Header("Player Components")]
    public Sprite defaultPlayerSprite;
    public Rigidbody2D shipRigidBody;
    public ParticleSystem thrustParticles;
    public GameObject deathVFX;
    public float rotationSpeed;
    public Animator playerAnimator;

    public Transform diegeticHealthBarFill;

    public float playerScore = 0f;
    private HUD playerHUD;
    public Health PlayerHealth { get; private set; }
    public int PlayerCurrency { get; private set; }

    [Header("Loot Vacuum Settings")]
    public float defaultLootVacuumRadius = 2.25f;
    public float defaultLootVacuumMagnitude = -20f;
    public float endOfWaveVacuumRadius = 30f;
    public float endOfWaveVacuumMagnitude = -50f;
    public CircleCollider2D vacuumCollider2D;
    public PointEffector2D vacuumController;

    [Header("Shield Powerup Features")]
    private bool isShielded = false;
    public GameObject shield;
    public float shieldDuration = 1f;

    [Header("Basic Weapon Features")]
    public bool basicWeapon = true;
    public bool shotGunSpread = false;
    public bool novaFire = false;
    public bool volleyFire = false;
    public WeaponType currentWeaponType = WeaponType.Basic;
    public bool punchThrough = false;
    public float currentWepProjLifetime = 1f;
    public float currentWepProjCount = 1f;
    public float currentWepProjCooldown = 1f;
    private float nextFireTime;
    private float lastFireTime;

    [Header("Backlog Rocket Features")]
    public bool specWepBacklogMissEnabled = false;
    public float specWepBacklogMissCooldown = 10f;
    public float specWepBacklogMissDamageRange = 1.5f;
    public float specWepBacklogMissPullRange = 3f;
    private float nextBacklogFireTime;
    private float lastBacklogFireTime;

    [Header("Volley Aim Helpers List")]
    public List<Transform> volleyAimHelpers = new List<Transform>();

    private void Start()
    {
        nextFireTime = Time.time + currentWepProjCooldown;
        nextBacklogFireTime = Time.time + specWepBacklogMissCooldown;
        nextBoostTime = Time.time + boostCooldown;
    }

    private void Awake()
    {
        if (playerInstance == null)
        {
            playerInstance = this;
        }

        defaultPlayerSpeed = thrustForce;

        if (diegeticHealthBarFill == null)
        {
            diegeticHealthBarFill = transform.Find("DiageticHealthbar").Find("HealthbarMask");
        }


        PlayerHealth = GetComponent<Health>();

        playerHUD = FindObjectOfType<HUD>();


        //Change diegetic healthbar to use a canvas so I only need one ratio
        //derivedDiegeticHealthValue = diegeticHPBarLength / playerHealth;
        //derivedRegHealthValue = 1 / playerMaxHealth;

        //Player starts with the boost ability by default so it needs to be added to the HUD immediately.
        CooldownUIManager.instance.CreateCooldownEntry(HUDIconType.Boost);
    }

    private void Update()
    {
        HandleParticles();

        if(PanelManager.IsPanelOpen<UpgradePanel>() == true)
        {
            return;
        }

        if (Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            Shoot();
            lastFireTime = Time.time;
            nextFireTime = Time.time + currentWepProjCooldown;
        }

        if (Input.GetButtonDown("Fire2") && Time.time > nextBacklogFireTime)
        {
            FireBacklogRocket();

            lastBacklogFireTime = Time.time;
            nextBacklogFireTime = Time.time + specWepBacklogMissCooldown;
        }

        if (Input.GetButtonDown("Jump") && Time.time > nextBoostTime)
        {
            BoostMovement();
            lastBoostTime = Time.time;
            nextBoostTime = Time.time + boostCooldown;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestEnableWeapon();
        }

    }
    private void FixedUpdate()
    {
        if (PanelManager.IsPanelOpen<UpgradePanel>() == true)
        {
            return;
        }
        RotateTowardMouse();
        AddThrust();
    }

    public float GetCurrentWeaponCooldownRatio()
    {
        //float cooldownRatio = (Time.time - lastFireTime) / currentWepProjCooldown;

        float cooldownRatio = (1 / currentWepProjCooldown) * (Time.time - lastFireTime);

        //Debug.LogWarning("Cooldown ratio for the primary weapon is: " + cooldownRatio);
        
        if(cooldownRatio > 1f)
        {
            return 1f;
        }

        return cooldownRatio;
    }

    public float GetBacklogWeaponCooldownRatio()
    {
        //float cooldownRatio = (Time.time - lastBacklogFireTime) / specWepBacklogMissCooldown;

        float cooldownRatio = (1 / specWepBacklogMissCooldown) * (Time.time - lastBacklogFireTime);

        //Debug.LogError("Cooldown ratio for the backlog rocket is: " + cooldownRatio);


        //11,365 - 11,358 = 7
        //1 / 10 = .1
        // .1 * 7 = .7

        if (cooldownRatio > 1f)
        {
            return 1f;
        }

        return cooldownRatio;
    }

    public float GetBoostCooldownRatio()
    {
        float cooldownRatio = (Time.time - lastBoostTime) / boostCooldown;

        if (cooldownRatio > 1f)
        {
            return 1f;
        }

        return cooldownRatio;
    }


    private void Rotate()
    {
        float currentHorizontalInput = Input.GetAxisRaw("Horizontal");
        float turningValue = -currentHorizontalInput * rotationSpeed * Time.fixedDeltaTime;

        shipRigidBody.AddTorque(turningValue);

    }

    private void RotateTowardMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TargetUtilities.RotateSmoothlyTowardTarget(mousePosition, transform, rotationSpeed);
    }

    private void AddThrust()
    {
        float currentVerticalInput = Input.GetAxisRaw("Vertical");
        Vector2 thrustValue = transform.up * currentVerticalInput * thrustForce * Time.fixedDeltaTime;

        shipRigidBody.AddForce(thrustValue);
    }

    public void MoreMicromanaged()
    {
        if(MicromanageCount >= 2)
        { 
            return;
        }

        MicromanageCount += 1;

        thrustForce = defaultPlayerSpeed * (1 - (MicromanageCount * slowPlayerMultiplier));
    }

    public void LessMicromanaged()
    {
        if(MicromanageCount == 0)
        {
            return;
        }

        MicromanageCount -= 1;

        thrustForce = defaultPlayerSpeed * (1 - (MicromanageCount * slowPlayerMultiplier));
    }


    public void TestEnableWeapon()
    {
        specWepBacklogMissEnabled = true;
    }

    private void Shoot()
    {
        switch (currentWeaponType)
        {
            case WeaponType.Basic:
                FireBasicShot();
                break;
            case WeaponType.Shotgun:
                FireShotgunShot();
                break;
            case WeaponType.Nova:
                FireNovaShot();
                break;
            case WeaponType.Volley:
                FireVolleyShot();
                break;
            default:
                break;
        }
    }

    private void BoostMovement()
    {
        AudioManager.PlaySound("Boost SFX", .75f);
        shipRigidBody.AddForce(transform.up * boostForce, ForceMode2D.Force);
    }

    public void ChangeWeaponProjectile()
    {
        switch (currentWeaponType)
        {
            case WeaponType.Basic:
                Player.playerInstance.currentSetProjectile = basicProjectile;
                break;
            case WeaponType.Shotgun:
                Player.playerInstance.currentSetProjectile = shottyProjectile;
                break;
            case WeaponType.Nova:
                Player.playerInstance.currentSetProjectile = novaProjectile;
                break;
            case WeaponType.Volley:
                Player.playerInstance.currentSetProjectile = volleyProjectile;
                break;
            default:
                break;
        }

    }

    private void FireBasicShot()
    {
        Projectile activeShot = Instantiate(currentSetProjectile, transform.position, transform.rotation);
        AudioManager.PlaySound("Basic Weapon Zap", .75f);
    }

    private void FireShotgunShot()
    {
        for (int i = 0; i < currentWepProjCount; i++)
        {
            Projectile activeShot = Instantiate(currentSetProjectile, transform.position, transform.rotation);
            activeShot.transform.eulerAngles += new Vector3(0f, 0f, Random.Range(-30f, 30f));
            activeShot.bulletLifetime = currentWepProjLifetime;
        }
        AudioManager.PlaySound("Shotgun Zap", .75f);
    }

    private void FireNovaShot()
    {
        float degree = 360f / currentWepProjCount;
        for (float i = -180f; i < 180f; i += degree)
        {
            
            Quaternion rotation = Quaternion.AngleAxis(i, transform.forward);
            Projectile activeShot = Instantiate(currentSetProjectile, transform.position, rotation * transform.rotation);
            activeShot.bulletLifetime = currentWepProjLifetime;
        }
        AudioManager.PlaySound("Nova Zap", .75f);
    }

    private void FireVolleyShot()
    {


        for (int i = 0; i < currentWepProjCount; i++)
        {
            Projectile activeShot = Instantiate(currentSetProjectile, volleyAimHelpers[i].position, volleyAimHelpers[i].rotation);
            activeShot.bulletLifetime = currentWepProjLifetime;

        }

        AudioManager.PlaySound("Volley Zap", .75f);
    }

    private void FireBacklogRocket()
    {
        if(specWepBacklogMissEnabled == true)
        {
            Projectile rocketShot = Instantiate(backlogRocket, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("The Backlog Rocket is not enabled or is on cooldown.");
        }
    }

    private void Die()
    {
        GameManager.gameManagerInstance.waveManager.ResetWaveCount();
        GameObject activeVFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(activeVFX, 2f);

        PanelManager.OpenPanel<EndGamePanel>().Setup(true);

        //Quaternion.identity refers to the default orientation of the object.
        Destroy(gameObject);


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (isShielded == true)
            {
                StartCoroutine(DischargeShields());
            }
            else
            {
                if (PlayerHealth == null)
                {
                    Debug.LogError("No health script is attached to: " + gameObject.name);
                }
                PlayerHealth.ReduceCurrentHealth(1f, true);
                bool amIDead = PlayerHealth.CheckIfDead();
                if (amIDead == true)
                {
                    Die();
                    //PlayDeathVFX
                }
                else
                {
                    //PlayTakenDamageVFX
                }
            }

            Rigidbody2D otherBody = collision.GetComponent<Rigidbody2D>();

            if (otherBody != null)
            {
                Vector2 dir = collision.transform.position - transform.position;

                otherBody.AddForce(dir.normalized * 4f, ForceMode2D.Impulse);
            }

        }

    }

    public void PlayDamageTakenEffects()
    {
        playerAnimator.SetTrigger("Hurt");
    }


    private void HandleParticles()
    {
        float currentVerticalInput = Input.GetAxisRaw("Vertical");

        if (currentVerticalInput > 0)
        {
            if (thrustParticles.isPlaying == false)
            {
                thrustParticles.Play();
            }
        }
        else
        {
            if (thrustParticles.isPlaying == true)
            {
                thrustParticles.Stop();
            }
        }

    }

    public void UpdateScore(float scoreToAdd)
    {
        playerScore += scoreToAdd;
        playerHUD.SetScoreText("Score: " + playerScore);
    }



    #region POWER UP METHODS


    public void ActivateShields()
    {
        isShielded = true;
        shield.SetActive(true);
    }

    public IEnumerator DischargeShields()
    {
        WaitForSeconds waiter = new WaitForSeconds(shieldDuration);

        yield return waiter; 

        isShielded = false;
        shield.SetActive(false);
    }


    public bool TrySpendCurrency(int currencyToSpend)
    {
        if (currencyToSpend > PlayerCurrency)
        {
            return false;
        }
        RemoveCurrency(currencyToSpend);
        return true;
    }

    public void AddCurrency(int currencyGained)
    {
        PlayerCurrency += currencyGained;
        PanelManager.GetPanel<HUD>().UpdateCurrencyDisplay(PlayerCurrency);
        PanelManager.GetPanel<UpgradePanel>().UpdateCurrencyDisplay(PlayerCurrency);
    }

    public void RemoveCurrency(int currencyLost)
    {
        PlayerCurrency -= currencyLost;
        PanelManager.GetPanel<HUD>().UpdateCurrencyDisplay(PlayerCurrency);
        PanelManager.GetPanel<UpgradePanel>().UpdateCurrencyDisplay(PlayerCurrency);
    }

    public void EnableEnhancedVacuum()
    {
        vacuumCollider2D.radius = endOfWaveVacuumRadius;
        vacuumController.forceMagnitude = endOfWaveVacuumMagnitude;
    }

    public void ResetDefaultVacuum()
    {
        if(vacuumCollider2D == null)
        {
            Debug.Log("Attempted to access player Vacuum Collider Radius, but the collider was null.");
            return;
        }
        vacuumCollider2D.radius = defaultLootVacuumRadius;
        vacuumController.forceMagnitude = defaultLootVacuumMagnitude;
    }

    public IEnumerator CycleVacuum()
    {
        EnableEnhancedVacuum();
        yield return new WaitForSeconds(4f);

        ResetDefaultVacuum();
    }

    #endregion
}
