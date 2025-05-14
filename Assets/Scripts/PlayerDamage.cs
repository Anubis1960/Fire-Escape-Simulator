using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public int maxOxygen = 100;
    public int currentOxygen;

    public HealthBar healthBar;
    public OxygenBar oxygenBar;
       
    private Coroutine oxygenCoroutine;
    private Movement movementScript;

    // Start is called before the first frame update
    void Start()
    {
        movementScript = GetComponent<Movement>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentOxygen = maxOxygen;
        oxygenBar.SetMaxOxygen(maxOxygen);

        oxygenCoroutine = StartCoroutine(Breathe());
    }

    // Update is called once per frame
    void Update()
    {
           if (Input.GetKeyDown(KeyCode.X))
           {
               TakeDamage(20);
           }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead.");
            // TODO
        }
    }

    public void ReduceOxygen(int oxygenLoss)
    {
        currentOxygen -= oxygenLoss;
        oxygenBar.SetOxygen(currentOxygen);
    }

    IEnumerator Breathe()
    {
        while (currentOxygen > 0)
        {
            float waitTime = 1f;
            int oxygenLoss = 1;

            // Two times more oxygen loss while sprinting
            if (movementScript.IsSprinting)
            {
                waitTime = 0.5f;
                oxygenLoss = 2;
            }

            yield return new WaitForSeconds(waitTime);
            currentOxygen -= oxygenLoss;
            oxygenBar.SetOxygen(currentOxygen);
        }

        Debug.Log("Out of oxygen!");
        // TODO
    }
}
