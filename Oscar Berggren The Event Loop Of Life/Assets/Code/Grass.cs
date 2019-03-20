using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Config
{

	private GridGenerator grid;
	private Sheep sheep;

	private int GrassXposition;
	private int GrassYposition;
	private float hp;
	[SerializeField]
	private int SenseTimer = 0;
	[SerializeField]
	private int DecideTimer = 0;

	private Vector2 currentGrassPos;
	public bool trampled = false;

	private Color GrassColor = new Color(0.5f, 0.4f, 0.05f);

	private enum _states { growing, shrinking, eaten, spreading};
	_states grassStates;
	

	// Use this for initialization
	void Start () {
		grassStates = _states.growing;
		grid = FindObjectOfType<GridGenerator>();
		sheep = FindObjectOfType<Sheep>();
		 hp = Random.Range(2,4);
	}

	// Update is called once per fram
	private void Update()
	{
		
	}
	void FixedUpdate() //Runs 30 times per second
	{
		DecideTimer = grid.Decide();
		SenseTimer = grid.Sense();
		if (DecideTimer == 30)
		{
			Decide();
		}
		if (SenseTimer == 15)
		{
			Sense();
		}
		
		
		 if (grassStates == _states.shrinking && trampled == false)
		{
			if (Random.value < 0.01)
			{
				hp -= 0.1f;
				ReduceGrassSize();
			}
		}

		 if (grassStates == _states.eaten)
		{
			hp -= 0.15f;
			ReduceGrassSize();
			this.GetComponent<SpriteRenderer>().color = GrassColor;
		}


		if (grassStates == _states.spreading && trampled == false)
		{
			if (Random.value < 0.15)
			{
				hp -= 0.3f;
				ReduceGrassSize();
				this.GetComponent<SpriteRenderer>().color = GrassColor;
			}
			if (Random.value < 0.06)
			{
				grid.AddGrass(Mathf.RoundToInt(currentGrassPos.x), Mathf.RoundToInt(currentGrassPos.y));
			}
		}
		if (grassStates == _states.growing && trampled == false)
		{
			if (Random.value < 0.2)
			{
				if (hp <= 5)
				{
					hp += 0.15f;
					GrassSize();
				}
			}
		}

		
		if (hp <= 0.0f)
		{
			Destroy(gameObject);
		}

	}
	
	void Sense()
	{
		if (grid.GrassTrampled(this) == true)
		{
			trampled = true;
			Invoke("Normal", 1);
		}
		if (grid.SheepEating(this,currentGrassPos) == true)
		{
			grassStates = _states.eaten;
		}
	}

	void Decide()
	{
		if (hp >= 5)
		{
			if (Random.value < 0.1)
			{
				grassStates = _states.spreading;
			}
			
		}
		
	}

	void GrassSize() {

		for (float i = 0; i <= hp; i ++)
		{
			if (transform.localScale.x < 0.16 && transform.localScale.y < 0.16)
			transform.localScale = transform.localScale * 1.02f;

		}
	}

	void Normal()
	{
		trampled = false;

	}
	void ReduceGrassSize()
	{
		for (float i = hp; i >= 0; i--)
		{
				transform.localScale = transform.localScale * 0.99f;
		}
	}

	public float GetGrassPositionX()
	{
		return currentGrassPos.x;
	}
	public float GetGrassPositionY()
	{
		return currentGrassPos.y;
	}

	public void SetGrassPosition(float x, float y)
	{
		currentGrassPos.x = x;
		currentGrassPos.y = y;
	}
	

	public Vector3 GetGrassPosition()
	{
			return currentGrassPos;
	}
}
