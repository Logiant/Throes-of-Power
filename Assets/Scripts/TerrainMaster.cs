using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class TerrainMaster : MonoBehaviour {

	//data storage object for Tiles
	public struct Tile {
		public static float size;
		public float x, y, z;
		public float slope; //TODO this should be a grade or an angle?
		public bool occupied;
		public TILE_TYPES type;
	}

	public enum TILE_TYPES {
		GRASSLAND, HILLS, MOUNTAINS, WATER
	}
	//TODO maybe put a list of each type of tile up here?

	public Texture2D image;

	public bool debug = false;

	float maxHeight = 0.0001f;

	float[,] imageValues;

	HeightMap map;

//	Vertex[,] verts;
	Tile[,] tiles;

	int width = 10;   //height in units
	int height = 10;  //width in units

	static float tile_size = 0.25f; //height of the tile

//	struct Vertex {
//		public float x, y, z;
//	}

	public float Sample(float x, float y) {
		return map.sample (x, y);
	}

	/*getTileWithSpecifications(max_height, max_slope)
	 *	int max tries
	 *	Vector3 pos = 000.
	 *	//loop through
	 *		//check conditions
	 *		//set position
	 *		//set occupied
	 *	//return position
	 *	
	*/

	public Vector3 getTileOfType(TILE_TYPES type) {
		Vector3 pos = new Vector3 ();

		List<Tile> availableTiles = new List<Tile>();

		for (int i = 0; i < tiles.GetLength(0); i++) {
			for (int j = 0; j < tiles.GetLength(1); j++) {
				if (tiles [i, j].type == type && !tiles[i,j].occupied) {
					availableTiles.Add (tiles [i, j]);
				}
			}
		}

		if (availableTiles.Count > 0) {
			Tile selected = availableTiles [Random.Range (0, availableTiles.Count)];
			pos = new Vector3 (selected.x, selected.y, selected.z);
		}

		return pos;
	}


	public Tile getTile(float x, float z) {
		int i = (int)(x/Tile.size);
		int j = (int)(z/Tile.size);
		return tiles [i, j];
	}

	// Use this for initialization
	void Awake () {

		Tile.size = tile_size;

		//unpack the image
		imageValues = new float[image.width, image.height];

		Color[] pixels = image.GetPixels ();

		for (int i = 0; i < image.width; i++) {
			for (int j = 0; j < image.height; j++) {
				imageValues [i, j] = pixels[i + j*image.width].grayscale;
			}
		}
		map = new HeightMap(imageValues, width);



		//initialize heightmap
		width = (int) (width / tile_size);
		height = (int) (height / tile_size);



		tiles = new Tile[width, height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tiles [i, j] = new Tile ();
				tiles [i, j].occupied = false;

				tiles [i, j].x = i*tile_size;
				tiles [i, j].z = j*tile_size;

				tiles [i, j].y = Sample(i*tile_size, j*tile_size);
			}
		}

		Mesh mesh = new Mesh ();

		GetComponent<MeshFilter> ().mesh = mesh;

		mesh.Clear ();

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				addTile (i, j, new Vector3 (tiles [i, j].x, tiles [i, j].y, tiles [i, j].z), ref mesh);
				//calculate tile slope
				if (i == 0 || i == width - 1 || j == 0 || j == height - 1) {
					tiles [i, j].slope = 0f;
				} else {
					//2nd order center difference method
					float dydx = (tiles[i+1,j].y - tiles[i-1,j].y) / (2*Tile.size);
					float dydz = (tiles[i,j+1].y - tiles[i,j+1].y) / (2*Tile.size);
					tiles [i, j].slope = Mathf.Sqrt(dydx*dydx + dydz*dydz);
				}
				//Set tile types
				if (tiles [i, j].y <= 0.01) { // water tile
					tiles [i, j].type = TILE_TYPES.WATER;
				} else if (tiles [i, j].y > 0.675 * maxHeight) { // mountain peak tile
					tiles [i, j].type = TILE_TYPES.MOUNTAINS;
				} else if (tiles [i, j].slope < 0.5f) { // shallow slope
					tiles [i, j].type = TILE_TYPES.GRASSLAND;
				} else {
					tiles [i, j].type = TILE_TYPES.HILLS;
				}
			}
		}

		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();


		Debug.Log (mesh.triangles.Length + " triangles");

		//set UVs
		float dx = 2*mesh.bounds.extents.x;
		float dz = 2 * mesh.bounds.extents.z;

		Vector2[] uvs = new Vector2[mesh.vertices.Length];
		for (int i = 0; i < uvs.Length; i++) {
			uvs [i] = new Vector2(mesh.vertices[i].x/dx, mesh.vertices[i].z/dz);
		}
		// assign the array of colors to the Mesh.
		mesh.uv = uvs;
		//set max height var in shader
		this.GetComponent<Renderer>().material.SetFloat("_MaxHeight", maxHeight);

//		Debug.Log (maxHeight);

	}
		

	// Update is called once per frame
	void Update () {
		if (debug) {
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					drawOutline (new Vector3 (tiles [i, j].x, tiles [i, j].y, tiles [i, j].z));	
				}
			}
		}
	}


	void OnDrawGizmos() {
		//loop through all the spaces
		float y = 0;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				y = Sample (i * tile_size, j * tile_size);
				//default to red color
				Color gc = Color.red;
				//check for flatness and water?  -- these should match the shader, probably
				if (tiles[i,j].type == TILE_TYPES.WATER) { // water tile
					gc = Color.blue;
				} else if (tiles[i,j].type == TILE_TYPES.GRASSLAND) { // mountain peak tile
					gc = Color.green;
				} else if (tiles[i,j].type == TILE_TYPES.MOUNTAINS) { // steep slope
					gc = Color.gray;
				} else if (tiles[i,j].type == TILE_TYPES.HILLS) { // steep slope
					gc = Color.red;
				}


				gc.a = 0.5f;
				Gizmos.color = gc;

				Gizmos.DrawSphere(new Vector3(i*tile_size, y, j*tile_size), tile_size/2);
			}
		}
	}


	void addTile(int i, int j, Vector3 pos, ref Mesh mesh) {

		/*
		 *  2\------\3
		 *   \      \
		 *   \      \
		 *  1\------\4
		 */

		//four verts used to make a square
		Vector3 v1, v2, v3, v4;
		v1 = pos + new Vector3(0, 0, 0);
		v2 = pos + new Vector3 (0, 0, tile_size);
		v3 = pos + new Vector3 (tile_size, 0, tile_size);
		v4 = pos + new Vector3 (tile_size, 0, 0);
//		v1.y = map.sample (v1.x, v1.z);
//		v2.y = map.sample (v2.x, v2.z);
//		v3.y = map.sample (v3.x, v3.z);
//		v4.y = map.sample (v4.x, v4.z);

		float[] ys = { v1.y, v2.y, v3.y, v4.y, maxHeight };

		maxHeight = Mathf.Max (ys);

		int iOff = 0;
		//offset to apply to triangle index
		if (mesh.triangles.Length > 0) {
			for (int q = 0; q < 6; q++) { //get the max of the last 6 values
				iOff = Mathf.Max(mesh.triangles[mesh.triangles.Length - 1 - q], iOff);
			}
			iOff++;
		}

		Vector3[] vertices = new Vector3[] { v1, v2, v3, v4};
		int[] indices;
		int i_ll = 0 + iOff;
		int i_ul = 1 + iOff;
		int i_ur = 2 + iOff;
		int i_lr = 3 + iOff;

		if (i == 0 && j > 0) {
			if (j == 1) {
				i_ll = iOff - 3;
				i_lr = iOff - 2;
				i_ul = iOff;
				i_ur = iOff + 1;
			} else {
				i_ll = iOff - 2;
				i_lr = iOff - 1;
				i_ul = iOff;
				i_ur = iOff + 1;
			}
			vertices = new Vector3[] { v2, v3 };

		} else if (j == 0 && i > 0) {
			if (i == 1) {
				i_ll = 3;
				i_ul = 2;
				i_ur = iOff;
				i_lr = iOff + 1;

			} else {

				i_ll = iOff - height;
				i_ul = i_ll - 1;
				i_ur = iOff;
				i_lr = iOff + 1;

			}
			vertices = new Vector3[] { v3, v4 };
		} else if (i > 0 && j > 0) {

			i_ul = iOff - height - 1;
			i_ll = i_ul - 1;
			i_ur = iOff;
			i_lr = i_ur - 1;

			if (i == 1) {
				i_ul = iOff - (2*height - j);
				i_ll = i_ul - 2;
			}
			if (j == 1) {
				i_lr--;
				i_ll--;
			}

			vertices = new Vector3[] {v3};

			if (i == 2 && j == 1) {
				Debug.Log (height + ", " + iOff);
				Debug.Log (i_ll + ", " + i_ul + ", " + i_ur + ", " + i_lr);

			}
		}



		if ((v2 - v4).magnitude < (v3 - v1).magnitude) { //UL to LR edge is shortest
			
			indices = new int[] {i_ll, i_ul, i_lr,  i_ul, i_ur, i_lr};
		} else {
			
			indices = new int[] {i_ll, i_ul, i_ur, i_ll, i_ur, i_lr};
		}
	
		//concatenate arrays
		List<Vector3> newVerts = new List<Vector3> ();
		newVerts.AddRange (mesh.vertices);
		newVerts.AddRange (vertices);

		List<int> newIndices = new List<int> ();
		newIndices.AddRange (mesh.triangles);
		newIndices.AddRange (indices);
	
		mesh.vertices = newVerts.ToArray();
		mesh.triangles = newIndices.ToArray();
	}


	void drawOutline(Vector3 pos) {

		//seven verts used to make a hexagon
				Vector3 v1, v2, v3, v4;

		v1 = pos;
		v2 = pos + new Vector3 (0, 0, tile_size);
		v3 = pos + new Vector3 (tile_size, 0, tile_size);
		v4 = pos + new Vector3 (tile_size, 0, 0);

		v1.y = map.sample (v1.x, v1.z);
		v2.y = map.sample (v2.x, v2.z);
		v3.y = map.sample (v3.x, v3.z);
		v4.y = map.sample (v4.x, v4.z);


		//drawing
		Debug.DrawLine (v1, v2, Color.black);
		Debug.DrawLine (v2, v3, Color.black);
		Debug.DrawLine (v3, v4, Color.black);
		Debug.DrawLine (v4, v1, Color.black);
	}


}
