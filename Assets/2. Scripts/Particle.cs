using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    // Start is called before the first frame update
    private ParticleSystem[] particles;

    [SerializeField] private float duration;

    private void Awake()
    {
        particles = GetComponents<ParticleSystem>();
        
    }
    private void OnEnable()
    {
        StartCoroutine(ParticleOnEnable());
    }

    private void OnDisable()
    {
        
    }

    IEnumerator ParticleOnEnable()
    {
        for (int i = 0; i < particles.Length; ++i)
            particles[i].Play();

        yield return new WaitForSeconds(duration);
        ObjectPooling.Instance.PushObject(gameObject);
    }

}
