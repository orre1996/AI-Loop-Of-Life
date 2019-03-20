using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	

	[SerializeField]
	private Vector2 TilePosition;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float GetTilePositionX()
	{
		return TilePosition.x;
	}
	public float GetTilePositionY()
	{
		return TilePosition.y;
	}

	public void SetTilePosition(int x, int y)
	{
		TilePosition.x = x;
		TilePosition.y = y;
	}
	
	public Vector3 GetTilePosition()
	{

		return TilePosition;
	}
}
