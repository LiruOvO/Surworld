using UnityEngine;

public class FootstepDust : MonoBehaviour
{
    private ParticleSystem dustParticles;
    private Rigidbody2D rb;

    public float minSpeedToEmit = 0.5f;
    public float stepInterval = 0.3f;
    private float stepTimer;

    private void Start()
    {
        dustParticles = GetComponent<ParticleSystem>();
        rb = GetComponentInParent<Rigidbody2D>();

        // Налаштувати fade через скрипт
        var colorModule = dustParticles.colorOverLifetime;
        colorModule.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(new Color(0.78f, 0.65f, 0.5f), 0f),
                new GradientColorKey(new Color(0.78f, 0.65f, 0.5f), 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),  // початок — видимі
                new GradientAlphaKey(0f, 1f)   // кінець — прозорі
            }
        );
        colorModule.color = new ParticleSystem.MinMaxGradient(gradient);

        // Зменшення розміру
        var sizeModule = dustParticles.sizeOverLifetime;
        sizeModule.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 1f);  // початок — повний розмір
        curve.AddKey(1f, 0f);  // кінець — зникає
        sizeModule.size = new ParticleSystem.MinMaxCurve(1f, curve);
    }

    private void Update()
    {
        if (rb == null) return;
        bool isMoving = rb.linearVelocity.magnitude > minSpeedToEmit;

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                dustParticles.Play();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}