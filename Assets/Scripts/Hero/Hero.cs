using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using AzizStuff;

[RequireComponent(typeof(HeroStats))]
public class Hero : MonoBehaviour, IDamageable
{
    private int currentHealth;

    public HeroStats Stats { get; private set; }

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject DeathPanel;
    [SerializeField] private EnemySpawner enemySpawner;
    private bool isDead = false;

    [HideInInspector] public bool IsInvincible = false;

    [Header("Turn Audio")]
    [SerializeField] private AudioClip[] turnSounds;
    [Range(0f, 1f)]
    [SerializeField] private float turnSoundProbability = 0.25f;
    private AudioSource audioSource;

    private void Awake()
    {
        Stats = GetComponent<HeroStats>();
        Stats.OnMaxHealthChanged += HandleMaxHealthChanged;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (Stats != null)
            Stats.OnMaxHealthChanged -= HandleMaxHealthChanged;
    }

    private void Start()
    {
        currentHealth = Stats.MaxHealth;
        CheckHealth();
    }

    private void HandleMaxHealthChanged(int difference)
    {
        currentHealth += difference;
        CheckHealth();
    }

    public void TakeDamage(int amount)
    {
        if (IsInvincible) return;
        
        currentHealth -= amount;
        CheckHealth();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > Stats.MaxHealth)
        {
            currentHealth = Stats.MaxHealth;
        }
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{Stats.MaxHealth}";
        }

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            DeathPanel.SetActive(true);
            enemySpawner.enabled = false;
            foreach (var enemy in FindObjectsOfType<EnemyMover>())
            {
                enemy.enabled = false;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayTurnSound()
    {
        if (turnSounds == null || turnSounds.Length == 0 || audioSource == null) return;

        if (UnityEngine.Random.value <= turnSoundProbability)
        {
            AudioClip randomClip = turnSounds[UnityEngine.Random.Range(0, turnSounds.Length)];
            audioSource.clip = randomClip;
            audioSource.Play();
        }
    }
}
