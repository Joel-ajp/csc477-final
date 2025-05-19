using UnityEngine;

public class FireballParticle : MonoBehaviour
{
    private ParticleSystem particle;
    private Transform fireball;
    private bool detached = false;

    void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        fireball = transform;
        particle.transform.parent = null;
    }

    void Update()
    {
        if (!detached)
        {
            particle.transform.position = fireball.position;
        }
    }

    void OnDisable()
    {
        if (!detached)
        {
            detached = true;
            fireball = null;

            particle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            float delay = particle.main.duration + particle.main.startLifetime.constantMax;
            Destroy(particle.gameObject, delay);
        }
    }
}
