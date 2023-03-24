using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectile;

    public float shootForce, upwardForce;
    public RaycastHit aimRay;

    [Header("Weapon Stats")]
    public float attackSpeed;
    public float spread;
    public float reloadTime;
    public float timeBetweenAttacks;
    public int magazineSize;
    public int bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft;
    int bulletsShot;

    [Header("Weapon Checks")]
    public bool shooting;
    public bool readyToShoot;
    public bool reloading;

    [Header("Extra Shit")]
    public Camera fpsCam;
    public Transform attackPoint;

    //graphicality
    public GameObject muzzleFlash;
    public int ammoDisplay; //math sample to show mag contents in a string: ammo = bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap

    //bug fix tool
    public bool allowInvoke = true;
    public PlayerHud hudComm;
    public PlayerSoundHandler sfxHandler;

    private void Awake()
    {
        bulletsLeft = magazineSize;

        readyToShoot = true;

        sfxHandler = GetComponent<PlayerSoundHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        //check if allowed to hold down button and take corresponding input
        if(allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0); // if allowbuttonhold is true, shooting is true so long as you hold down the mouse. Automatic fire
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0); // Semi Automatic
        }

        //Reload the weapon
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
        //reload automatically when firing empty weapon
        if(readyToShoot && shooting && reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0, a reset
            bulletsShot = 0;

            Shoot();
        }
    }

    public void Shoot()
    {
        sfxHandler.StartCoroutine(sfxHandler.PlaySFXwDelay(sfxHandler.magicShoot, 0, true));
        hudComm.PlayShootAnim(attackSpeed);
        readyToShoot = false;

        //find hit location to send projectile to
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75); //point far away from player, to avoid nullref
        }

        //calculate direction attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //add random spread to the previous direction

        //instantiate the projectile
        GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity); //spawned bullet is immediately stored for later use
        //rotate bullet toward shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //add force to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse); //for lobbed projectiles

        //instantiate muzzle flash if present
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already revoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", attackSpeed);
            allowInvoke = false; // Invoke calls function name after a set time, disable invoke so it only happens once
        }

        //if more bullets per tap make sure to repeat shoot function, for shotguns and such
        if(bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenAttacks);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true; //set back to true so it can loop
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadComplete", reloadTime);
    }
    private void ReloadComplete()
    {
        bulletsLeft = magazineSize;

        reloading = false;
    }
}
