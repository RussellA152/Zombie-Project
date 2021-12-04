
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public float health;
    public float maxHealthAmount;

    public float minRandomSpeed;
    public float maxRandomSpeed;
    [SerializeField] private float maxPossibleSpeed;

    public static float difficultySpeedIncrease = 0f;

    public float pointsForDeath;

    private NavMeshAgent navMesh;
    private Rigidbody target_rigidbody;
    private EnemyAttacks enemyAttacks_access;

    public GameObject enemy;

    public bool is_dead;

    public GameObject attackHitbox;
    [SerializeField] private GameObject headHitBox;

    [SerializeField] private GameObject instaKillPowerUp;
    [SerializeField] private GameObject maxAmmoPowerUp;
    [SerializeField] private GameObject infiniteAmmoPowerUp;
    [SerializeField] private GameObject doublePointsPowerUp;
    [SerializeField] private GameObject freePointsPowerUp;

    private GameObject powerUpObject;

    private bool callDieFunction;

    //[SerializeField] private AudioSource zombieAudioSource;
    [SerializeField] private AudioClip zombieDamageSound;
    [SerializeField] private AudioClip zombieDeathSound;
    private AudioSource zombieAudio { get; set; }
    private void OnEnable()
    {
        //enemy Audio Source is a static singleton object audio source for zombie attack and death sounds
        //zombieAudio = EnemyAudio.current.enemyAudioSourceGameObject.GetComponent<AudioSource>();
    }
    private void Start()
    {
        callDieFunction = true;
        //gameObject.GetComponent<NavMeshAgent>().speed = Random.Range(minSpeed, maxSpeed);
        navMesh = gameObject.GetComponent<NavMeshAgent>();
        target_rigidbody = GetComponent<Rigidbody>();
        enemyAttacks_access = GetComponent<EnemyAttacks>();
        zombieAudio = EnemyAudio.current.enemyAudioSourceGameObject.GetComponent<AudioSource>();
        //zombieDamagedAudio = EnemyDamagedSound.current.enemyDamagedAudioSourceGameObject.GetComponent<AudioSource>();
        is_dead = false;
        //zombieAudioSource.GetComponent<AudioSource>();


        //callDieFunction = true;
    }
    private void Update()
    {
        //Debug.Log(zombieAudio);
    }
    public void TakeDamage(float amount)
    {
        //amount is passed as an argument inside the GunScript.cs
        health -= amount;
        zombieAudio.PlayOneShot(zombieDamageSound, 0.5f);

        //when the zombie's health is 0 or lower, initate the Die() function
        //the Die() function should only be called once
        if (health <= 0f && callDieFunction == true)
        {
            Die();
        }
    }

    public void RoundDifficultyIncrease()
    {
        //AS OF NOW, every 2 rounds, zombie health and movement speed increases
        if(RoundController.round % 2 == 0 && RoundController.round != 0)
        {
            Debug.Log("Round Difficulty Increased");

            //Caps enemies' health amount so it doesn't go to infinite
            if(health < maxHealthAmount)
            {
                health += 10f;
                Debug.Log("health increased"+ this.gameObject);
            }
                
            
        }
        
    }
   
    public void IncreaseSpeed()
    {
        //Caps enemies' speed amount so it doesn't go to infinite (we can just call this once since speed is statically, we don't want to call this multiple times otherwise the speed will increase exponentially)
        if (maxRandomSpeed < maxPossibleSpeed)
            difficultySpeedIncrease += .15f;
    }
    public void Die()
    {
        //destroys the zombie and its AI (everything)
        if(gameObject.CompareTag("Enemy"))
        {
            callDieFunction = false;
            is_dead = true;

            //plays a death sound
            if (zombieAudio.isPlaying)
            {
                zombieAudio.Stop();
                zombieAudio.PlayOneShot(zombieDeathSound, 0.5f);
            }
            else
            {
                zombieAudio.PlayOneShot(zombieDeathSound, 0.5f);
            }
            

            //changes enemy's layer to "Ragdoll" to prevent player and enemy collision with corpse
            gameObject.layer = 11;

            //when enemy dies, give player score
            if (PowerUpEvent.current.hasDoublePoints)
            {
                PlayerScore.pScore += pointsForDeath * 2f;
            }
            else
            {
                PlayerScore.pScore += pointsForDeath;
            }
                

            //Disables enemy's attacking script to prevent attacks when dead
            enemyAttacks_access.enabled = false;

            //disable the dead zombie's attack hitbox (SO THEY DO NOT ATTACK PLAYER WHEN DEAD)
            //disables enemies headhitbox so player cannot collide with it 
            attackHitbox.SetActive(false);
            headHitBox.SetActive(false);

            //turn off zombie's navmesh, then turn off its constraints to make "ragdoll" effects
            navMesh.enabled = false;
            target_rigidbody.constraints = RigidbodyConstraints.None;


            //when enemy dies, they have a chance to drop a power-up
            SetPowerUpProbability();

            //after a few seconds, destroy zombie's body
            Destroy(gameObject, 2.5f);
            
            //killing a zombie will decrement the zombie counter (MIGHT BE A DEPEDENCY ISSUE LATER)
            RoundController.zombieCounter -= 1;
            //Debug.Log(RoundController.zombieCounter);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void SetPowerUpProbability()
    {
        //There is a small chance for a power up to spawn when a zombie dies

        //random.value returns a random number from 0 - 1.0
        var PowerUpDropChance = Random.value;

        //5% chance for zombie to drop a random power up (1.0 - 0.95 = 0.08)
        if(PowerUpDropChance > 0.95)
        {
            //IF a power-up spawns, it has a 1 in 5 chance to be a different power ability (I.E. Max Ammo, Insta-kill)
            int PowerUpAbilityObjectChance = Random.Range(1, 6);

            switch (PowerUpAbilityObjectChance)
            {
                case 1:
                    powerUpObject = Instantiate(instaKillPowerUp, transform.position, transform.rotation);
                    break;
                case 2:
                    powerUpObject = Instantiate(doublePointsPowerUp, transform.position, transform.rotation);
                    break;
                case 3:
                    powerUpObject = Instantiate(maxAmmoPowerUp, transform.position, transform.rotation);
                    break;
                case 4:
                    powerUpObject = Instantiate(infiniteAmmoPowerUp, transform.position, transform.rotation);
                    break;
                case 5:
                    powerUpObject = Instantiate(freePointsPowerUp, transform.position, transform.rotation);
                    break;
            }
        }

    }
    //IEnumerator DeathAnimationDelay()
    //{
        //yield return new WaitForSeconds(2.5f);
        //Destroy(gameObject);
    //}

    //public AudioSource returnZombieAudio()
    //{
        //return zombieAudio;
    //}
}
