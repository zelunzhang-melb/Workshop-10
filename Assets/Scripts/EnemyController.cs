// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] ProjectileController projectilePrefeb;
    [SerializeField] float maxShootDelay = 10f;
    [SerializeField] float minShootDelay = 5f;

    private MeshRenderer _renderer;


    private void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));

            var player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                var aimDirection = (player.transform.position - transform.transform.position).normalized;

                var projectile = Instantiate(projectilePrefeb);
                projectile.transform.position = transform.position;
                projectile.Initiation(aimDirection);
            }

            


        }
    }

    private void Awake()
    {
        this._renderer = gameObject.GetComponent<MeshRenderer>();
    }

    // This method listens to HealthManager "onHealthChanged" events. The actual
    // event listening is set up within the editor interface. This is purely for
    // visuals currently, and takes a fractional value between 0 and 1.
    public void UpdateHealth(float frac)
    {
        this._renderer.material.color = Color.red * frac;
    }

    // Same as above, but listens to onDeath events.
    public void Kill()
    {
        var particles = Instantiate(this.deathEffect);
        particles.transform.position = transform.position;
    }
}
