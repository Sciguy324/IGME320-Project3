using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public List<GameObject> bulletObjectMagazine;
    public int maxMagazineSize;
    public int currentBullet;
    public float shotSpread;
    public GameObject bulletPrefeb;
    public Transform firePoint;
    public float bulletSpeed = 20;
    public int bulletPiercing =1;
    // Start is called before the first frame update
    void Start()
    {
        bulletObjectMagazine = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    public void Shoot() {
        GameObject bullet;
        //If there are no bullets waiting to be shot, make a new one
        if (bulletObjectMagazine.Count == 0)
        {
            bullet = Instantiate(bulletPrefeb, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().sender = this;
        }
        //If there are unactive bullets ready to be shot, use them
        else
        { 
            bullet = bulletObjectMagazine[0];
            bullet.SetActive(true);
            bulletObjectMagazine.Remove(bullet);
        }
        bullet = Instantiate(bulletPrefeb, firePoint.position, firePoint.rotation);

        //fire the bullet by giving it velocity
        bullet.GetComponent<Bullet>().maxPierces = bulletPiercing;
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
