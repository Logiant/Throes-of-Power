﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class TerrainRender : MonoBehaviour {

	public Texture2D image;

	public bool debug = false;

	float maxHeight = 0.0001f;


	float[,] imageValues;

	HeightMap map;

	Vertex[,] verts;

	int width = 10;   //height in units
	int height = 10;  //width in units

	static float tile_size = 0.25f; //height of the tile

	struct Vertex {
		public float x, y, z;
	}

	public float Sample(float x, float y) {
		return map.sample (x, y);
	}

	// Use this for initialization
	void Awake () {

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



		verts = new Vertex[width, height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				verts [i, j] = new Vertex ();

				verts [i, j].x = i*tile_size;
				verts [i, j].z = j*tile_size;

				verts [i, j].y = 0;
			}
		}

		Mesh mesh = new Mesh ();

		GetComponent<MeshFilter> ().mesh = mesh;

		mesh.Clear ();

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				addTile (i, j, new Vector3 (verts [i, j].x, verts [i, j].y, verts [i, j].z), ref mesh);
			}
		}

		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		mesh.Optimize ();

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
					drawOutline (new Vector3 (verts [i, j].x, verts [i, j].y, verts [i, j].z));	
				}
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
		v1.y = map.sample (v1.x, v1.z);
		v2.y = map.sample (v2.x, v2.z);
		v3.y = map.sample (v3.x, v3.z);
		v4.y = map.sample (v4.x, v4.z);

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