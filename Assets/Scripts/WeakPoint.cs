using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeakPoint : MonoBehaviour
{

    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip HitWeakPointSound;

    private ParticleSystem damageParticlesInstance;


    Vector2 weakPointLocation;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        weakPointLocation = transform.localPosition;
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetFloat("Direction") >= 0)
        {
            transform.localPosition = weakPointLocation;
        }
        else
        {
            transform.localPosition = new Vector2(-weakPointLocation.x, weakPointLocation.y);
        }
    }

    private void OnDestroy()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.isLoaded)
            Instantiate(damageParticles, transform.position, Quaternion.identity);
        SoundManager.instance.PlaySound(HitWeakPointSound);

    }
}
