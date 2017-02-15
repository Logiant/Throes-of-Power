using System.Collections;
using System;

using System.Diagnostics;

public class HeightMap {

	int nx, ny;
	float delta;

	float[,] heights;


	public HeightMap(float[,] heightmap, float width) {
		heights = heightmap;
		nx = heightmap.GetLength (0);
		ny = heightmap.GetLength (1);
		delta = width / (nx-1);
	}

	public void generate(float[,] heightmap) {

		//TODO write a sampling algorithm for this and the "sample" function

		int w0 = heightmap.GetLength (0);
		int h0 = heightmap.GetLength (1);

		for (int i = 0; i < nx; i++) {
			//get i as % of the total map
			float i_p = (float)i / nx;
			int i_new = (int)(i_p * w0);


			for (int j = 0; j < ny; j++) {
				//get j as % of the total map
				float j_p = (float)j / ny;
				int j_new = (int)(j_p * h0);

				heights [i, j] = heightmap[i_new,j_new] * 1.0f;
			}
		}
	}

	public void generate() {
		for (int i = 0; i < nx; i++) {
			for (int j = 0; j < ny; j++) {
				heights [i, j] = (i * j) / (float)(nx * ny) * 5;
			}
		}
	}


	public float sample(float x, float y) {

		//x, y rounded to index space
		int x_i = (int)(x / delta);
		int y_i = (int)(y / delta);

		return heights[x_i,y_i];
	}
	}
