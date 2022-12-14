using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private List<GameObject> objectMagazine;
    public int maxMagazineSize = 6;
    private int currentBulletCount = 0;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public int piercing = 1;
    private bool isReloading;
    public GameObject isRealodingSprite;
    public int damage = 1;
    // public int shotCount = 1;
    public Gui gui;
    // Start is called before the first frame update
    void Start()
    {
        objectMagazine = new List<GameObject>();
        currentBulletCount = maxMagazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnBullet(GameObject bullet)
    {
        objectMagazine.Add(bullet);
    }

    public void expandMagazine(int amount)
    {
        // Expands the gun's magazine size
        maxMagazineSize += amount;
        gui.SetAmmo(maxMagazineSize, currentBulletCount);
    }
  

    public void Shoot(string sourceName = "Player") {
        // Shoots the gun if any bullets are left
        if (currentBulletCount > 0)
        {
            GameObject bullet;
            // If there are objects ready to be used, use them
            if (objectMagazine.Count != 0)
            {
                bullet = objectMagazine[0];
                bullet.transform.position = firePoint.position;
                bullet.SetActive(true);
                objectMagazine.Remove(bullet);
            }
            // If there are no object to be used, make a new bullet
            else
            {
                bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                bullet.GetComponent<Bullet>().sender = this;
            }
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            // Configure bullet and send it on its way
            bulletScript.maxPierces = piercing;
            bulletScript.Restore();
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
            bulletScript.sourceTag = sourceName;
            bulletScript.damage = damage;
            currentBulletCount--;

            // Update GUI accordingly (if available)
            if (gui != null)
                gui.SetAmmo(maxMagazineSize, currentBulletCount);

        }
        else if(!isReloading)
        {
            StartCoroutine(Reload());
            return;
        }
    }

    IEnumerator Reload()
    {
        if (gui != null)
            gui.startReloading(Player.Instance.reloadSpeed);
        if (isRealodingSprite != null)
            isRealodingSprite.SetActive(true);
        isReloading = true;
        yield return new WaitForSeconds(Player.Instance.reloadSpeed);
        currentBulletCount = maxMagazineSize;
        isReloading = false;
        if (isRealodingSprite != null)
            isRealodingSprite.SetActive(false);
        gui.SetAmmo(maxMagazineSize, currentBulletCount);

    }
}
