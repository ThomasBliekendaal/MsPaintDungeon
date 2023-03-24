using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    [Header("Object References")]
    public Animator viewModel;
    public PlayerMovement playerMovementRef;
    public PlayerAttack playerAttackRef;

    [Header("Everything Else")]
    public float speedSum;
    public AnimationCurve layerWeightCurve;
    public float fallClamp;
    public PlayerSoundHandler sfxHandler;

    void Update()
    {
        viewModel.SetFloat("AnimSpeed", speedSum); //Speedsum controls the speed of animations

        //update viewmodel bob speed based on velocity
        if (playerMovementRef.rb.velocity.magnitude > 1)
        {
            viewModel.SetBool("IsMoving", true);

            speedSum = playerMovementRef.rb.velocity.magnitude / playerMovementRef.moveSpeed;

            if (playerMovementRef.rb.velocity.magnitude > playerMovementRef.moveSpeed + 0.5f)
            {
                speedSum *= 2;
            }
        }
        else if (playerMovementRef.grounded == true)
        {
            viewModel.SetBool("IsMoving", false);
            speedSum = 1;
        }

        //play jump anim when pressing the jump button
        if (Input.GetKeyDown(playerMovementRef.jumpKey))
        {
            viewModel.SetBool("IsJumping", true);
        }
        else
        {
            viewModel.SetBool("IsJumping", false);
        }
        
        //play land animation when player lands
        if(playerMovementRef.readyToLand)
        {
            viewModel.SetTrigger("IsLanded");
            sfxHandler.StartCoroutine(sfxHandler.PlaySFXwDelay(sfxHandler.groundHit, 0, true));
            playerMovementRef.readyToLand = false;
        }

        //move viewmodel up when falling
        if(playerMovementRef.rb.velocity.y < 0)
        {
            float curveWeight = playerMovementRef.rb.velocity.y;
            curveWeight = Mathf.Abs(curveWeight);
            curveWeight /= fallClamp;
            curveWeight = Mathf.Clamp(curveWeight, 0, 1);

            viewModel.SetLayerWeight(2, layerWeightCurve.Evaluate(curveWeight));
        }
        else
        {
            viewModel.SetLayerWeight(2, 0);
        }

        //check if player is shooting
        if (playerAttackRef.shooting)
        {
            viewModel.SetBool("StillShooting", true);
        }
        else
        {
            viewModel.SetBool("StillShooting", false);
        }
    }

    public void PlayShootAnim(float attackSpeed)
    {
        if (playerAttackRef.readyToShoot)
        {
            viewModel.SetFloat("AttackSpeed", 1f / attackSpeed);
            viewModel.SetTrigger("IsShooting");
        }
    }
}
