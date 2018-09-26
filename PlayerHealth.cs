//Jordan Black 2016

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour 
{

	public float maxHealth = 100.0f;
	public AudioClip[] painSounds;
	public AudioClip[] deathSounds;

	private GameObject healthBar;
	private Slider healthSlider;

	private float currentHealth;
	private bool isDead;

	void Start () 
  {
		InitializeHealth();
	}
	
	void Update () 
  {
		CheckHealth();

		UpdateHealthBar();
	}

	void InitializeHealth()
  {

		currentHealth = maxHealth;

		try
    {
			healthSlider = healthBar.GetComponentInChildren<Slider>();
		}
    catch
    {
			Debug.LogWarning("Health slider not initialized!");
		}
	}

	public void TakeDamage(float amount)
  {

		if(painSounds.Length > 0 && painSounds[0] != null)
    {
			try
      {
				GetComponentInChildren<AudioManager>().CreateAndAutoDestroySound(painSounds[Random.Range(0, painSounds.Length)]);
			}
      catch
      {
				Debug.LogWarning("Pain sound error.");
			}
		}

		if(currentHealth - amount >= 0)
    {
			currentHealth -= amount;
			//Debug.Log("Taking "+amount+" damage.");
		}
    else
    {
			currentHealth = 0;
		}
	}

	public IEnumerator AddHealth(float amount)
  {
		
		if(currentHealth + amount <= maxHealth)
    {
			currentHealth += amount;
			//Debug.Log("Adding "+amount+" health.");
		}
    else
    {
			currentHealth = maxHealth;
			//Debug.Log("Adding "+(maxHealth-currentHealth)+" health.");
		}

		//I'm not dead!
		if(isDead)
    {
			isDead = false;
		}

		yield return new WaitForFixedUpdate();
	}

	public float GetHealth()
	{
		return currentHealth;
	}

	public void IncreaseMaxHealth(float amount)
	{
		maxHealth += amount;
		UpdateHealthBar();
	}

	void CheckHealth()
	{
		if(currentHealth <= 0 && !isDead)
		{
			isDead = true;

			if(isDead)
			{

				TellAnimatorImDead();

				if(deathSounds.Length > 0 && deathSounds[0] != null)
				{
					try
					{
						GetComponentInChildren<AudioManager>().CreateAndAutoDestroySound(deathSounds[Random.Range(0, painSounds.Length)]);
					}
					catch
					{
						Debug.LogWarning("Death sound error.");
					}
				}

				GameController.ConfirmRestart();
			}
		}
	}

	void UpdateHealthBar()
	{
		if(healthSlider.maxValue != maxHealth)
		{
			healthSlider.maxValue = maxHealth;
		}

		if(healthSlider.value != currentHealth)
		{
			healthSlider.value = currentHealth;
		}
	}

	void TellAnimatorImDead()
	{
		GetComponent<PlayerMovement>().ImDead();
	}
}
