using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingMaster : MonoBehaviour {


	struct Node {
		public float x, y;
		public Node(float x, float y) {
			this.x = x;
			this.y = y;
		}
	}


	struct Path {
		public List<Node> path;
		public Path(List<Node> p) {
			path = p;
		}
	}


	float[,] grid;
	TerrainMaster terrain;

	Vector2 extents;
	float delta = 1f;



	// Use this for initialization
	void Start () {
		terrain = GetComponent<TerrainMaster> ();
		extents = terrain.getExtents ();

		int w = (int) (extents.x / delta + 0.5f);
		int h = (int) (extents.y / delta + 0.5f);

		grid = new float[w, h];

	}

	void UpdateGrid() {
		//updates the grid cost values
		for (int i = 0; i < grid.GetLength (0); i++) {
			for (int j = 0; j < grid.GetLength (1); j++) {
				//sample terrain information to rebuild the grid. Include city/resource info to path around it??
				grid [i, j] = 0.1f;
			}
		}
	}

	Path GenPath(Vector2 start, Vector2 end) {
		//generates a Path struct full of ndoes
		Node first = new Node(start.x, start.y);
		Node goal = new Node(end.x, end.y);

		List<Node> pl = new List<Node> ();
		pl.Add (first);
		//add all the middle parts...


		pl.Add (goal);
		Path p = new Path(pl);
		return p;
	}
}
