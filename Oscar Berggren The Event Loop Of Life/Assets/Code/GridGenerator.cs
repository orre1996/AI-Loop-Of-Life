using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : Config
{

	[Header("Tiles")]
	[SerializeField]
	private Tile grid;
	[SerializeField]
	private List<Tile> tiles = new List<Tile>();
	[SerializeField]
	private Vector2 mapsize;
	[SerializeField]
	private GameObject TileParent;

	[Header("Grass")]
	[SerializeField]
	private Grass grass;
	[SerializeField]
	private List<Grass> grasses = new List<Grass>();
	[SerializeField]
	private GameObject GrassParent;
	private int SheepSpread;
	private float SheepNumberX = 0;
	private float SheepNumberY = 0;

	[Header("Sheep")]
	[SerializeField]
	private Sheep sheep;
	[SerializeField]
	private List<Sheep> sheepies = new List<Sheep>();
	[SerializeField]
	private GameObject SheepParent;
	private int GrassSpread;
	private int GrassNumberX = 0;
	private int GrassNumberY = 0;
	[SerializeField]
	private int sheepAmount = 10;
	private int sheepCount = 0;

	[Header("Wolf")]
	[SerializeField]
	private Wolf wolf;
	[SerializeField]
	private List<Wolf> wolves = new List<Wolf>();
	[SerializeField]
	private GameObject WolfParent;
	[SerializeField]
	private int wolfAmount = 1;
	private int wolfCount = 0;
	private int WolfSpread;
	private float WolfNumberX = 0;
	private float WolfNumberY = 0;
	private bool eating = false;


	private float closestTarget = Mathf.Infinity;
	private Vector2 closestTar;

	private int DecideTimer = 0;
	private int SenseTimer = 0;
	
	



	// Use this for initialization
	void Start()
	{
		CreateGrid();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		DecideTimer++;
		SenseTimer++;
		if(DecideTimer > 30)
		{
			DecideTimer = 0;
		}
		if(SenseTimer > 15)
		{
			SenseTimer = 0;
		}
		RemoveGrass();
		RemoveSheep();
		RemoveWolf();
	}

	void CreateGrid()
	{
		for (int x = 0; x < mapsize.x; x++)
		{
			for (int y = 0; y < mapsize.y; y++)
			{
				Tile newTile = Instantiate<Tile>(grid, new Vector3(x, y, +10), Quaternion.identity);
				newTile.transform.parent = TileParent.transform;
				newTile.name = "X=" + x + "Y=" + y;
				newTile.SetTilePosition(x, y);
				tiles.Add(newTile);

				if (Random.value < 0.15f)
				{
					Grass newGrass = Instantiate<Grass>(grass, new Vector3(x, y, +1), Quaternion.identity);
					newGrass.transform.parent = GrassParent.transform;
					newGrass.name = "X=" + x + "Y=" + y;
					newGrass.SetGrassPosition(x, y);
					grasses.Add(newGrass);

				}
				if (Random.value < 0.05f && sheepCount < sheepAmount)
				{
					Sheep newSheep = Instantiate<Sheep>(sheep, new Vector3(x, y), Quaternion.identity);
					newSheep.transform.parent = SheepParent.transform;
					newSheep.name = "X=" + x + "Y=" + y;
					newSheep.SetSheepPosition(x, y);
					sheepies.Add(newSheep);
					sheepCount++;
				}
				if (x == 19 && y == 0 && wolfCount < wolfAmount)
				{
					Wolf newWolf = Instantiate<Wolf>(wolf, new Vector3(x, y), Quaternion.identity);
					newWolf.transform.parent = WolfParent.transform;
					newWolf.name = "X=" + x + "Y=" + y;
					newWolf.SetWolfPosition(x, y);
					wolves.Add(newWolf);
					wolfCount++;
				}
			}
		}
	}

	public void AddGrass(int x, int y)
	{

		GrassSpread = Random.Range(1, 8);

		if (GrassSpread == 1 && x < mapsize.x - 1)
		{
			GrassNumberX = 1;
		}
		else if (GrassSpread == 2 && x > 0)
		{
			GrassNumberX = -1;
		}
		else if (GrassSpread == 3 && y < mapsize.y - 1)
		{
			GrassNumberY = 1;
		}
		else if (GrassSpread == 4 && y > 0)
		{
			GrassNumberY = -1;
		}
		else if (GrassSpread == 5 && x < (mapsize.x - 1) && (y < mapsize.y - 1))
		{
			GrassNumberX = 1;
			GrassNumberY = 1;
		}
		else if (GrassSpread == 6 && x < (mapsize.x - 1) && y > 0)
		{
			GrassNumberX = 1;
			GrassNumberY = -1;
		}
		else if (GrassSpread == 7 && y < (mapsize.y - 1) && x > 0)
		{
			GrassNumberX = -1;
			GrassNumberY = 1;
		}
		else if (GrassSpread == 8 && x > 0 && y > 0)
		{
			GrassNumberX = -1;
			GrassNumberY = -1;
		}
		else
		{
			Breaking();
		}

		if ((GrassNumberX + GrassNumberY) != 0)
		{
			Grass newGrass = Instantiate<Grass>(grass, new Vector3(x + GrassNumberX, y + GrassNumberY, +1), Quaternion.identity);
			newGrass.transform.parent = GrassParent.transform;
			newGrass.name = "X=" + (x + GrassNumberX) + "Y=" + (y + GrassNumberY);
			newGrass.SetGrassPosition(x + GrassNumberX, y + GrassNumberY);
			for (int i = 0; i < grasses.Count; i++)
			{
				if (newGrass.GetGrassPositionX() == grasses[i].GetGrassPositionX() && newGrass.GetGrassPositionY() == grasses[i].GetGrassPositionY())
				{
					Destroy(newGrass.gameObject);
				}
			}
			if (newGrass != null)
			{
				grasses.Add(newGrass);
			}
		}
		GrassNumberX = 0;
		GrassNumberY = 0;
	}

	public void Breaking()
	{
	}

	public void RemoveGrass()
	{
		for (int i = grasses.Count - 1; i > -1; i--)
		{
			if (grasses[i] == null)
				grasses.RemoveAt(i);
		}
	}

	public void AddSheep(float x, float y)
	{
		SheepSpread = Random.Range(1, 8);

		if (SheepSpread == 1 && x < mapsize.x - 1)
		{
			SheepNumberX = 1;
		}

		else if (SheepSpread == 2 && x > 0)
		{
			SheepNumberX = -1;
		}
		else if (SheepSpread == 3 && y < mapsize.y - 1)
		{
			SheepNumberY = 1;
		}
		else if (SheepSpread == 4 && y > 0)
		{
			SheepNumberY = -1;
		}
		else if (SheepSpread == 5 && x < (mapsize.x - 1) && (y < mapsize.y - 1))
		{
			SheepNumberX = 1;
			SheepNumberY = 1;
		}
		else if (SheepSpread == 6 && x < (mapsize.x - 1) && y > 0)
		{
			SheepNumberX = 1;
			SheepNumberY = -1;
		}
		else if (SheepSpread == 7 && y < (mapsize.y - 1) && x > 0)
		{
			SheepNumberX = -1;
			SheepNumberY = 1;
		}
		else if (SheepSpread == 8 && x > 0 && y > 0)
		{
			SheepNumberX = -1;
			SheepNumberY = -1;
		}
		else
		{
			Breaking();
		}

		if ((SheepNumberX + SheepNumberY) != 0)
		{
			Sheep newSheep = Instantiate<Sheep>(sheep, new Vector3(x + SheepNumberX, y + SheepNumberY), Quaternion.identity);
			newSheep.transform.parent = GrassParent.transform;
			newSheep.name = "X=" + (x + SheepNumberX) + "Y=" + (y + SheepNumberY);
			newSheep.SetSheepPosition(x + SheepNumberX, y + GrassNumberY);
			for (int i = 0; i < sheepies.Count; i++)
			{
				if (newSheep.GetSheepPositionX() == sheepies[i].GetSheepPositionX() && newSheep.GetSheepPositionY() == sheepies[i].GetSheepPositionY())
				{
					Destroy(newSheep.gameObject);
				}
			}
			if (newSheep != null)
			{
				sheepies.Add(newSheep);
			}
		}
		SheepNumberX = 0;
		SheepNumberY = 0;
	}

	public void RemoveSheep()
	{
		for (int i = sheepies.Count - 1; i > -1; i--)
		{
			if (sheepies[i] == null)
				sheepies.RemoveAt(i);
		}
	}

	public bool SheepSearching(Sheep sheep)
	{
		closestTarget = Mathf.Infinity;
		for (int i = 0; i < grasses.Count; i++)
		{
			float distance = Vector3.Distance(sheep.GetSheepPosition(), grasses[i].GetGrassPosition());

			if (distance < closestTarget)
			{
				closestTarget = distance;
				if (grasses[i] != null)
				{
					closestTar = grasses[i].GetGrassPosition();
				}
			}
		}
		if (closestTarget > 10)
		{
			return true;
		}
		return false;
	}

	public Vector2 ClosestGrass(Sheep sheep)
	{
		closestTarget = Mathf.Infinity;
		for (int i = 0; i < grasses.Count; i++)
		{
			float distance = Vector2.Distance(sheep.GetSheepPosition(), grasses[i].GetGrassPosition());

			if (distance < closestTarget)
			{
				closestTarget = distance;
				if(grasses[i] != null) { 
				closestTar = grasses[i].GetGrassPosition();
					}
			}
		}
			return closestTar;
	}

	public bool GrassTrampled(Grass grass)
	{
		float minX = grass.transform.position.x - 0.2f;
		float maxX = grass.transform.position.x + 0.2f;
		float minY = grass.transform.position.y - 0.2f;
		float maxY = grass.transform.position.y + 0.2f;
		for (int i = 0; i < sheepies.Count; i++)
		{
			if (sheepies[i] != null)
			{
				if (minX < sheepies[i].transform.position.x && maxX > sheepies[i].transform.position.x)
				{
					if (minY < sheepies[i].transform.position.y && maxY > sheepies[i].transform.position.y)
						return true;
				}
			}
		}
		for (int y = 0; y < wolves.Count; y++)
		{
			if (wolves[y] != null)
			{
				if (minX < wolves[y].transform.position.x && maxX > wolves[y].transform.position.x)
				{
					if (minY < wolves[y].transform.position.y && maxY > wolves[y].transform.position.y)
						return true;
				}
			}
		}
		return false;
	}

	public bool GrassEaten(Grass grass)
	{
		for (int i = 0; i < sheepies.Count; i++)
		{
			if (sheepies[i] != null)
			{
				if (grass.GetGrassPosition() == sheepies[i].transform.position)
				{
					return true;
				}
			}

		}
		return false;
	}

	public float ClosestWolf(Sheep sheep)
	{
		closestTarget = Mathf.Infinity;
		for (int i = 0; i < wolves.Count; i++)
		{
			float distance = Vector2.Distance(sheep.transform.position, wolves[i].transform.position);

			if (distance < closestTarget)
			{
				closestTarget = distance;
				closestTar = wolves[i].GetWolfPosition();
			}
		}
			return closestTarget;
	}

	public bool SheepEaten(Sheep sheep)
	{
		for (int i = 0; i < wolves.Count; i++)
		{
			float distance = Vector3.Distance(wolves[i].transform.position, sheep.transform.position);
			if (distance < 0.19f)
			{
				eating = true;
				return true;
			}
		}
		return false;
	}

	public bool WolfEating(Wolf wolf)
	{
			if (eating == true)
			{

			eating = false;
			return true;
			}
		return false;
	}

	public bool SheepEating(Sheep sheep)
	{
		for (int i = 0; i < grasses.Count; i++)
		{
			if (sheep.transform.position == grasses[i].GetGrassPosition())
			{
				return true;
			}
		}
		return false;
	}
	
	public bool grassTaken(Vector3 SheepPosition, Sheep sheep)
	{
		for(int i = 0; i < sheepies.Count; i++)
		{
				if (SheepPosition == sheepies[i].GetSheepPosition())
				{
				if (sheep != sheepies[i])
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool SheepTargeted(Vector3 WolfPosition, Wolf wolf)
	{
		for (int i = 0; i < wolves.Count; i++)
		{

			if (WolfPosition == wolves[i].GetWolfPosition())
			{
				if (wolf != wolves[i])
				{
					return true;
				}
			}
		}
		return false;
	}

	public Vector2 ClosestSheep(Wolf wolf)
	{
		closestTarget = Mathf.Infinity;
		for (int i = 0; i < sheepies.Count; i++)
		{
			float distance = Vector3.Distance(wolf.GetWolfPosition(), sheepies[i].transform.position);

			if (distance < closestTarget)
			{
				closestTarget = distance;
				closestTar = sheepies[i].transform.position;
			}
		}
		if (closestTarget != Mathf.Infinity)
		{
			return closestTar;
		}
		return wolf.GetWolfPosition();
	}

	public bool WolfSearching(Wolf wolf)
	{
		closestTarget = Mathf.Infinity;
		for (int i = 0; i < sheepies.Count; i++)
		{

			float distance = Vector3.Distance(wolf.transform.position, sheepies[i].transform.position);

			if (distance < closestTarget)
			{
				closestTarget = distance;
				closestTar = sheepies[i].GetSheepPosition();
			}
		}
		if (closestTarget > 10)
		{
			return true;
		}
		return false;
	}

	public void AddWolf(float x, float y)
	{
		WolfSpread = Random.Range(1, 8);

		if (WolfSpread == 1 && x < (mapsize.x - 1))
		{
			WolfNumberX = 1;
		}

		else if (WolfSpread == 2 && x > 0)
		{
			WolfNumberX = -1;
		}
		else if (WolfSpread == 3 && y < (mapsize.y - 1))
		{
			WolfNumberY = 1;
		}
		else if (WolfSpread == 4 && y > 0)
		{
			WolfNumberY = -1;
		}
		else if (WolfSpread == 5 && x < (mapsize.x - 1) && (y < mapsize.y - 1))
		{
			WolfNumberX = 1;
			WolfNumberY = 1;
		}
		else if (WolfSpread == 6 && x < (mapsize.x - 1) && y > 0)
		{
			WolfNumberX = 1;
			WolfNumberY = -1;
		}
		else if (WolfSpread == 7 && y < (mapsize.y - 1) && x > 0)
		{
			WolfNumberX = -1;
			WolfNumberY = 1;
		}
		else if (WolfSpread == 8 && x > 0 && y > 0)
		{
			WolfNumberX = -1;
			WolfNumberY = -1;
		}
		else
		{
			Breaking();
		}

		if ((WolfNumberX + WolfNumberY) != 0)
		{
			Wolf newWolf = Instantiate<Wolf>(wolf, new Vector3(x + WolfNumberX, y + WolfNumberY), Quaternion.identity);
			newWolf.transform.parent = GrassParent.transform;
			newWolf.name = "X=" + (x + WolfNumberX) + "Y=" + (y + WolfNumberY);
			newWolf.SetWolfPosition(x + WolfNumberX, y + WolfNumberY);
			for (int i = 0; i < wolves.Count; i++)
			{
				//if (newWolf.GetWolfPositionX() == wolves[i].GetWolfPositionY() && newWolf.GetWolfPositionY() == wolves[i].GetWolfPositionY())
				//{
				//	Destroy(newWolf.gameObject);
				//}
			}
			if (newWolf != null)
			{
				wolves.Add(newWolf);
			}
		}
		WolfNumberX = 0;
		SheepNumberY = 0;

	}
	public void AddWolfGrass(float x, float y, Wolf wolf)
	{
		Grass newGrass = Instantiate<Grass>(grass, new Vector3(x ,y), Quaternion.identity);
		newGrass.transform.parent = GrassParent.transform;
		newGrass.name = "X=" + x  + "Y=" + y;
		newGrass.SetGrassPosition(x , y );
		for (int i = 0; i < grasses.Count; i++)
		{

			if (newGrass.GetGrassPositionX() == grasses[i].GetGrassPositionX() && newGrass.GetGrassPositionY() == grasses[i].GetGrassPositionY())
			{
					Destroy(newGrass.gameObject);	
			}
		}
		if (newGrass != null)
		{
			grasses.Add(newGrass);
		}
	}

	public void AddSheepGrass(float x, float y, Sheep sheep)
	{
		Grass newGrass = Instantiate<Grass>(grass, new Vector3(x, y), Quaternion.identity);
		newGrass.transform.parent = GrassParent.transform;
		newGrass.name = "X=" + x + "Y=" + y;
		newGrass.SetGrassPosition(x, y);
		for (int i = 0; i < grasses.Count; i++)
		{
			if (newGrass.GetGrassPositionX() == grasses[i].GetGrassPositionX() && newGrass.GetGrassPositionY() == grasses[i].GetGrassPositionY())
			{
					Destroy(newGrass.gameObject);
				
			}
		}
		if (newGrass != null)
		{
			grasses.Add(newGrass);
		}
	}

	public void RemoveWolf()
	{
		for (int i = wolves.Count - 1; i > -1; i--)
		{
			if (wolves[i] == null)
				wolves.RemoveAt(i);
		}
	}

	public Vector2 GetWolfiePosition()
	{
		for (int i = 0; i < wolves.Count; i++)
		{
			return wolves[i].transform.position;
		}
		return new Vector2(100, 100);
	}

	public Vector2 ClosestTile(Sheep sheep)
	{
		closestTarget = Mathf.Infinity;
		for (int i = 0; i < tiles.Count; i++)
		{
			float distance = Vector2.Distance(sheep.transform.position, tiles[i].GetTilePosition());

			if (distance < closestTarget)
			{
				closestTarget = distance;
				closestTar = tiles[i].GetTilePosition();
			}
		}
		return closestTar;
	}
	public Vector2 ClosestTile(Wolf wolf)
	{
		closestTarget = Mathf.Infinity;
		for (int i = 0; i < tiles.Count; i++)
		{
			float distance = Vector2.Distance(sheep.transform.position, tiles[i].GetTilePosition());

			if (distance < closestTarget)
			{
				closestTarget = distance;
				closestTar = tiles[i].GetTilePosition();
			}
		}
		return closestTar;
	}

	public int Sense()
	{
		return SenseTimer;
	}

	public int Decide()
	{
		return DecideTimer;
	}

	public bool SheepEating(Grass grass, Vector3 position)
	{
		for (int i = 0; i < sheepies.Count; i++)	
		{
			if(sheepies[i].IsEating() == true && grass.GetGrassPosition() == sheepies[i].GetSheepPosition())
			{
				return true;
			}
		}
		
		return false;
	}
}