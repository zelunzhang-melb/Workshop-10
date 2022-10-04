// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f; // Default speed sensitivity
    //[SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Vector3 _aimDirection;
    [SerializeField] private ProjectileController projectilePrefab;
    [SerializeField] private ParticleSystem deathEffect;

    private MeshRenderer _renderer;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector3.left * (this.speed * Time.deltaTime), Space.World);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Vector3.right * (this.speed * Time.deltaTime), Space.World);

        aim();
        
        // Use the "down" variant to avoid spamming projectiles. Will only get
        // triggered on the frame where the key is initially pressed.
        if (Input.GetMouseButtonDown(0))
        {
            var projectile = Instantiate(this.projectilePrefab);
            projectile.transform.position = gameObject.transform.position;
            projectile.Initiation(_aimDirection);
        }
    }

    void aim()
    {
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var gamePlane = new Plane(Vector3.up, Vector3.zero);

        // TODO:
        // save the distance variable
        if (gamePlane.Raycast(mouseRay, out var distance))
        {
            var hitPoint = mouseRay.GetPoint(distance);
            // TODO: aim towards point, store in _aimDirection
            _aimDirection = (hitPoint - transform.position).normalized;


            // player will look at the shoot direction
            transform.LookAt(hitPoint, Vector3.up);

            // alternative:
            //transform.rotation = Quaternion.LookRotation(_aimDirection, Vector3.up);
        }

    }


    private void Awake()
    {
        this._renderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void UpdateHealth(float frac)
    {
        this._renderer.material.color = Color.blue * frac;
    }

    // Same as above, but listens to onDeath events.
    public void Kill()
    {
        var particles = Instantiate(this.deathEffect);
        particles.transform.position = transform.position;
    }
}
