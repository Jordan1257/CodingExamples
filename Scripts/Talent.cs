//Jordan Black 2016

using UnityEngine;
using System.Collections;

public class Talent 
{
	public string name;
	public bool available;
	public int minLevel;
	public Talent requisiteOne;
	public Talent requisiteTwo;
	public int currentPoints;
	public int maxPoints;
	public string tooltip;

	//base talent constructor
	public Talent ( string newName )
	{
		name = newName;
		available = true;
		maxPoints = 1;
		currentPoints = maxPoints;
		minLevel = 1;
		tooltip = "Base talent.";
	}

	//single requisite talent constructor
	public Talent( string newName, int levelRequirement, Talent newRequisite, int newMax )
	{
		name = newName;
		requisiteOne = newRequisite;

		minLevel = levelRequirement;

		UpdateTalentRequisites(1);

		if(newMax > 0)
		{
			maxPoints = newMax;
		}
		else
		{
			maxPoints = 1;
		}
	}

	//double OR requisite talent constructor
	public Talent( string newName, int levelRequirement, Talent newRequisiteOne, Talent newRequisiteTwo, int newMax)
	{
		name = newName;
		requisiteOne = newRequisiteOne;
		requisiteTwo = newRequisiteTwo;

		minLevel = levelRequirement;

		UpdateTalentRequisites(1);

		if(newMax > 0)
		{
			maxPoints = newMax;
		}
		else
		{
			maxPoints = 1;
		}
	}

	//single requisite talent constructor with tooltip
	public Talent( string newName, int levelRequirement, Talent newRequisite, int newMax, string description )
	{
		name = newName;
		requisiteOne = newRequisite;

		minLevel = levelRequirement;

		UpdateTalentRequisites(1);

		if(newMax > 0)
		{
			maxPoints = newMax;
		}
		else
		{
			maxPoints = 1;
		}

		tooltip = description;
	}

	//double OR requisite talent constructor with tooltip
	public Talent( string newName, int levelRequirement, Talent newRequisiteOne, Talent newRequisiteTwo, int newMax, string description )
	{
		name = newName;
		requisiteOne = newRequisiteOne;
		requisiteTwo = newRequisiteTwo;

		minLevel = levelRequirement;

		UpdateTalentRequisites(1);

		if(newMax > 0)
		{
			maxPoints = newMax;
		}
		else
		{
			maxPoints = 1;
		}

		tooltip = description;
	}


	public void AddPoint()
	{
		if(currentPoints < maxPoints)
		{
			currentPoints++;
			Debug.Log(name+": "+currentPoints+" / "+maxPoints);
		}
	}

	public void UpdateTalentRequisites(int currentLevel)
	{
		if(requisiteTwo != null)
		{
			if(currentLevel >= minLevel)
			{
				if( requisiteOne.currentPoints == requisiteOne.maxPoints || 
					requisiteTwo.currentPoints == requisiteTwo.maxPoints || 
					requisiteOne.currentPoints + requisiteTwo.currentPoints == Mathf.RoundToInt( (requisiteOne.maxPoints + requisiteTwo.maxPoints) / 2f))
				{
					this.available = true;
					//Debug.Log(this.name + " talent now available.");
				}
			}
		}
		else if(requisiteOne.currentPoints == requisiteOne.maxPoints && currentLevel >= minLevel)
		{
			this.available = true;
			//Debug.Log(this.name + " talent now available.");
		}
		else
		{
			this.available = false;
			//Debug.Log(this.name + " talent is not available.");
		}
	}

}
