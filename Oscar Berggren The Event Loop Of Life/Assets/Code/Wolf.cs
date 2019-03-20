using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Config
{

	private GridGenerator grid;

	[SerializeField]
	private Vector2 currentWolfPos;
	private Vector2 WolfnewPos;
	private Vector2 WolfoldPos;

	[SerializeField]
	private float hp;
	

	private enum _states { hunting, eating, searching }
	_states wolfStates;

	private Color EatingColor = new Color(1.0f, 0.0f, 0.0f);
	private Color NormalColor = new Color(0.0f, 0.0f, 0.0f);


	// Use this for initialization
	void Start()
	{
		grid = FindObjectOfType<GridGenerator>();
		WolfoldPos = currentWolfPos;
		hp = Random.Range(5, 8);
		Startimus();
	}

	void Startimus()
	{
		if (grid.WolfSearching(this) == false)
		{
				WolfnewPos = grid.ClosestSheep(this);
		}
		else
		{
			WolfnewPos.x = Random.Range(0, 20);
			WolfnewPos.y = Random.Range(0, 20);
		}
		if (grid.SheepTargeted(WolfnewPos, this) == false)
		{
			WolfoldPos = currentWolfPos;
			currentWolfPos = WolfnewPos;
		}
		else
		{
			WolfnewPos = new Vector2(Random.Range(0, 20), Random.Range(0, 20));

			WolfoldPos = currentWolfPos;
			currentWolfPos = WolfnewPos;
		}
		wolfStates = _states.hunting;
	}


	// Update is called once per frame
	void FixedUpdate() {

		hp -= 0.003f;


		if (wolfStates == _states.hunting)
		{
			currentWolfPos = WolfnewPos;
			if (transform.position.x != WolfnewPos.x)
			{
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(WolfnewPos.x, WolfoldPos.y), 1f * Time.deltaTime);
			}
			else if (transform.position.x == WolfnewPos.x)
			{
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(WolfnewPos.x, WolfnewPos.y), 1f * Time.deltaTime);
				if (transform.position.x == WolfnewPos.x && transform.position.y == WolfnewPos.y)
				{
					if (grid.WolfEating(this) == false)
					{
						this.GetComponent<SpriteRenderer>().color = NormalColor;
						Startimus();
					}
					else 
					{
							hp += 5;
							this.GetComponent<SpriteRenderer>().color = EatingColor;
					}

				}
			}
		}

		if (hp >= 30)
		{
			grid.AddWolf(currentWolfPos.x, currentWolfPos.y);
			hp = 5;
		}

		if (hp <= 0)
		{
			currentWolfPos = grid.ClosestTile(this);
			grid.AddWolfGrass(currentWolfPos.x,currentWolfPos.y, this);
			Destroy(gameObject);
		}
	}

	
	
	public Vector3 GetWolfPosition()
	{
		return currentWolfPos;

	}
	public void SetWolfPosition(float x, float y)
	{
		currentWolfPos.x = x;
		currentWolfPos.y = y;
	}
	public float GetWolfPositionX()
	{
		return currentWolfPos.x;
	}
	public float GetWolfPositionY()
	{
		return currentWolfPos.y;
	}
}
