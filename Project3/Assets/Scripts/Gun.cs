using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private List<GameObject> objectMagazine;
    public int maxMagazineSize =6;
    private int currentBulletCount =6;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public int piercing = 1;
    private bool isReloading;
    public GameObject isRealodingSprite;
    // Start is called before the first frame update
    void Start()
    {
        objectMagazine = new List<GameObject>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnBullet(GameObject bullet)
    {
        objectMagazine.Add(bullet);
    }
  

    public void Shoot() {

        if (currentBulletCount > 0)
        {



            // Shoots the gun
            GameObject bullet;
            //if there are objects ready to be used, use them
            if (objectMagazine.Count != 0)
            {
                bullet = objectMagazine[0];
                bullet.transform.position = firePoint.position;
                bullet.SetActive(true);
                objectMagazine.Remove(bullet);
            }
            //if there are no object to be used, make a new bullet
            else
            {
                bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                bullet.GetComponent<Bullet>().sender = this;
            }
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
            currentBulletCount--;
        }
        else if(!isReloading)
        {
            StartCoroutine( Reload());
            return;
        }
    }

    IEnumerator Reload()
    {
        isRealodingSprite.SetActive(true);
        isReloading = true;
         yield return new WaitForSeconds(Player.Instance.reloadSpeed);
        currentBulletCount = maxMagazineSize;
        isReloading = false;
        isRealodingSprite.SetActive(false);

    }
}
