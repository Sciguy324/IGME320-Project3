using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private List<Bullet> magazine;
    public int maxMagazineSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(float angleDegrees, Vector2 origin) {
        // Shoots the gun

        // Only fire if enough bullets are available
        if (magazine.Count > 0) {
            // Fire a bullet
            magazine[0].Shoot(angleDegrees, origin);

            // Remove from magazine
            magazine.RemoveAt(0);
        }
    }
}
