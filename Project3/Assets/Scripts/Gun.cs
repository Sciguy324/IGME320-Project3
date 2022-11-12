using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private List<Bullet> magazine;
    public int maxMagazineSize;
    public float shotSpread;

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void Shoot(float angleDegrees, Vector2 origin) {
        // Shoots the gun

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
