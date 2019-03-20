using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Config
{
	private GridGenerator grid;

	private Vector2 newPos;
	private Vector2 oldPos;
	private Vector2 currentSheepPos;
	[SerializeField]
	private GameObject blood;
	[SerializeField]
	private float hp;
	[SerializeField]
	private int DecideTimer = 0;
	[SerializeField]
	private int SenseTimer = 0;
	[SerializeField]
	private int ActSelect = 0;


	private enum _states { searching, eating, escaping, moving };
	_states sheepStates;

	private Color EatingColor = new Color(0.0f, 0.3f, 0.7f);
	private Color NormalColor = new Color(1.0f, 1.0f, 1.0f);
	private Color EscapingColor = new Color(1.0f, 0f, 0f);

	private bool run = false;
	private bool eating = false;

	// Use this for initialization
	void Start()
	{
		oldPos = currentSheepPos;
		hp = Random.Range(4, 6);
		grid = FindObjectOfType<GridGenerator>();
		Startimus();
	}

	// Update is called once per frame
	void Startimus()
	{
		currentSheepPos = grid.ClosestTile(this);

		if (grid.SheepSearching(this) == false)
		{
			currentSheepPos = grid.ClosestTile(this);
			newPos = grid.ClosestGrass(this);
		}
		else
		{
			newPos = new Vector2(Random.Range(0, 20), Random.Range(0, 20));
		}
		if (grid.grassTaken(newPos, this) == false)
		{
			currentSheepPos = grid.ClosestTile(this);
			oldPos = currentSheepPos;
			currentSheepPos = newPos;
		}
		else
		{
			currentSheepPos = grid.ClosestTile(this);
			newPos = new Vector2(Random.Range(0, 20), Random.Range(0, 20));
			oldPos = currentSheepPos;
			currentSheepPos = newPos;
		}
		sheepStates = _states.moving;
		run = false;

	}


	void FixedUpdate() // 30 times per second, sense 1 time per second, decide 2 times per second
	{

		hp -= 0.005f;

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


		if (grid.ClosestWolf(this) > 2)
		{
			this.GetComponent<SpriteRenderer>().color = NormalColor;
		}

		if (sheepStates == _states.eating)
		{
			if (grid.SheepEating(this) == true)
			{
				hp += 0.25f;
				this.GetComponent<SpriteRenderer>().color = EatingColor;
			}
			else
			{
				this.GetComponent<SpriteRenderer>().color = NormalColor;
				Startimus();
			}
		}

		if (hp > 20)
		{
			grid.AddSheep(currentSheepPos.x, currentSheepPos.y);
			hp = 5;
		}

		if (hp <= 0)
		{
			currentSheepPos = grid.ClosestTile(this);
			grid.AddSheepGrass(currentSheepPos.x, currentSheepPos.y, this);
			Destroy(gameObject);
		}

		if (sheepStates == _states.moving)
		{
			if (transform.position.x != newPos.x)
			{
				transform.position = Vector2.MoveTowards(transform.position, new Vector2(newPos.x, oldPos.y), 0.65f * Time.deltaTime);
			}
			if (transform.position.x == newPos.x)
			{
				transform.position = Vector2.MoveTowards(transform.position, new Vector2(newPos.x, newPos.y), 0.65f * Time.deltaTime);
			}

		}

		if (grid.SheepEaten(this) == true)
		{
			Instantiate(blood, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}

	}

	public void Decide()
	{
		if (ActSelect == 1)
		{
			//transform.position = newPos;
			sheepStates = _states.eating;
			eating = true;
			ActSelect = 0;
		}
		else if (ActSelect == 2)
		{
			this.GetComponent<SpriteRenderer>().color = EscapingColor;
			run = true;
			Escape();
			ActSelect = 0;
		}
	}

	public void Sense()
	{
		if (transform.position.x == newPos.x && transform.position.y == newPos.y)
		{
			
			ActSelect = 1;
		}
		else
		{
			eating = false;
		}
		if (grid.ClosestWolf(this) <= 2 && run == false)
		{
			
			ActSelect = 2;

		}
	}

	public void Escape()
	{
		if (Random.value < 0.80)
		{
			currentSheepPos = grid.ClosestTile(this);
			if (grid.GetWolfiePosition().x > transform.position.x && grid.GetWolfiePosition().y > transform.position.y)
			{
				newPos.x = 0;
				newPos.y = 0;
				oldPos = currentSheepPos;
				currentSheepPos = newPos;
			}
			if (grid.GetWolfiePosition().x > transform.position.x && grid.GetWolfiePosition().y < transform.position.y)
			{
				newPos.x = 0;
				newPos.y = 19;
				oldPos = currentSheepPos;
				currentSheepPos = newPos;
			}
			if (grid.GetWolfiePosition().x < transform.position.x && grid.GetWolfiePosition().y < transform.position.y)
			{
				newPos.x = 19;
				newPos.y = 19;
				oldPos = currentSheepPos;
				currentSheepPos = newPos;
			}
			if (grid.GetWolfiePosition().x < transform.position.x && grid.GetWolfiePosition().y > transform.position.y)
			{
				newPos.x = 19;
				newPos.y = 0;
				oldPos = currentSheepPos;
				currentSheepPos = newPos;
			}
		}
	}

	public Vector3 GetSheepPosition()
	{
		return currentSheepPos;
	}
	public void SetSheepPosition(float x, float y)
	{
		currentSheepPos.x = x;
		currentSheepPos.y = y;
	}
	public float GetSheepPositionX()
	{
		return currentSheepPos.x;
	}
	public float GetSheepPositionY()
	{
		return currentSheepPos.y;
	}

	public bool IsEating()
	{
		return eating;
	}

}
