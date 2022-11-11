using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private List<Bullet> magazine;
    public int maxMagazineSize;
    public float shotSpread;
    public Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        magazine = new List<Bullet>();
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsertBullet(Bullet bullet) {
        magazine.Add(bullet);
    }

    public int BulletsRemaining() {
        // Returns the number of bullets remaining in the magazine
        return magazine.Count;
    }

    public void Shoot() {
        // Shoots the gun
        Debug.Log("Pew pew!");

        // Get firing angle and position
        Vector2 origin = _transform.position;
        float angleDegrees = _transform.eulerAngles.z;

        // Only fire if enough bullets are available
        if (BulletsRemaining() > 0) {
            // Add a small amount of spread to the angle
            angleDegrees += Random.Range(-shotSpread, shotSpread);

            // Fire a bullet
            magazine[0].Shoot(angleDegrees, origin);

            // Remove from magazine
            magazine.RemoveAt(0);
        }
    }
}
