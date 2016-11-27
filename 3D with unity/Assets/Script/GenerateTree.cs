using UnityEngine;
using System.Collections;

public class GenerateTree : MonoBehaviour {
	
	public GameObject branch;
	public GameObject leaf;

	public int depth = 6;
	public int posX = 0;
	public int posY = 0;
	public int posZ = 0;
	public int minAngleSpread = 0;
	public int maxAngleSpread = 45;
	public int minBaseAngleX = 75;
	public int maxBaseAngleX = 105;
	public int minBaseAngleY = 75;
	public int maxBaseAngleY = 105;
	public int minLength = 6;
	public int maxLength = 20;
	public int minWidth = 8;
	public int maxWidth = 15;

	private float deg_to_rad = 3.14159265f / 180.0f;
	private LineRenderer line;
	private GameObject fractalTree;

	float clean_angle (float a) {
		if (a > 360) {
			return a - 360;
		} else if (a < 0) {
			return a + 360;
		} else {
			return a;
		}
	}


	void drawWithCaps(Vector3 start, Vector3 end, float size){	

		//Clone the base branch
		GameObject new_branch;
		new_branch = Instantiate(branch);
	
		//Make the branch child of the main gameobject
		new_branch.transform.parent = fractalTree.transform;

		//Place it between start and end position
		//Translation
		Vector3 p_milieu = new Vector3((start.x+end.x)/2,(start.y+end.y)/2,(start.z+end.z)/2);
		new_branch.transform.position = p_milieu; 

		//Rotation
		new_branch.transform.LookAt (end);
		new_branch.transform.eulerAngles += new Vector3(90.0f,0.0f,0.0f); 

		//Scale
		float l = Vector3.Distance(start,end);
		new_branch.transform.localScale = new Vector3(size/3, l/2.0f,size/3);
	}



	void drawLeaf(Vector3 start, Vector3 end,float size){

		//Clone the base leaf
		GameObject new_leaf;
		new_leaf = Instantiate(leaf);

		//Make the leaf child of the main gameobject
		new_leaf.transform.parent = fractalTree.transform;

		//Place it between start and end position
		//Translation
		Vector3 p_milieu = new Vector3(start.x+(start.x-end.x)/2,start.y+(start.y-end.y)/2,start.z+(start.z-end.z)/2);
		new_leaf.transform.position = start; 

		//Rotation
		new_leaf.transform.LookAt (p_milieu);

		//Scale
		new_leaf.transform.localScale = new Vector3(size*20,size*20,size*20);
	}


	//A recursive function used to draw the fractal tree
	void drawTree(Vector3 pos, float angle, float angle2,int nb, int depth,float length,float width){

		//Calculate next position
		Vector3 pos2 = new Vector3(
			pos.x + (Mathf.Cos(angle * deg_to_rad) * length),
		  ( pos.y + (Mathf.Sin(angle * deg_to_rad) * length) + pos.y + (Mathf.Sin(angle2 * deg_to_rad) * length) )/2,
			pos.z + (Mathf.Cos(angle2 * deg_to_rad) * length)
		);	

		if (depth <= 1) {
			//Draw leaf at the end of a branch
			drawLeaf(pos,pos2,length*1.25f);
			return;
		}

		//Draw the current branch	
		drawWithCaps (pos, pos2, width);

		//Avoid angle > 360 or < 0
		angle = clean_angle (angle);
		angle2 = clean_angle (angle2);

		//call the recursive function with the new parameter
		/* TODO remplace this ugly think by a loop depending for the nb parameter for choice the nomber of subdivision of earch branch */

		float angleSpread = Random.Range(minAngleSpread,maxAngleSpread);
		if (Random.value * depth > 1.0f) {
			drawTree (pos2, angle - angleSpread, angle2 - angleSpread, nb, depth - 1, length / (1 + Random.value),width / (1 + Random.value));
		} else {
			drawTree (pos2, angle - angleSpread, angle2 - angleSpread, nb, 1, length / (1 + Random.value),width/ (1 + Random.value));
		}

		angleSpread = Random.Range(minAngleSpread,maxAngleSpread);
		if (Random.value * depth > 1.0f) {
			drawTree (pos2, angle + angleSpread, angle2 + angleSpread, nb, depth - 1, length / (1 + Random.value),width/ (1 + Random.value));
		} else {
			drawTree (pos2, angle + angleSpread, angle2 + angleSpread, nb, 1, length / (1 + Random.value),width/ (1 + Random.value));
		}

		angleSpread = Random.Range(minAngleSpread,maxAngleSpread);
		if (Random.value * depth > 1.0f) {
			drawTree (pos2, angle + angleSpread, angle2 - angleSpread, nb, depth - 1, length / (1 + Random.value),width/ (1 + Random.value));
		} else {
			drawTree (pos2, angle + angleSpread, angle2 - angleSpread, nb, 1, length / (1 + Random.value),width/ (1 + Random.value));
		}

		angleSpread = Random.Range(minAngleSpread,maxAngleSpread);
		if (Random.value * depth > 1.0f) {
			drawTree (pos2, angle - angleSpread, angle2 + angleSpread, nb, depth - 1, length / (1 + Random.value),width/ (1 + Random.value));
		} else {
			drawTree (pos2, angle - angleSpread, angle2 + angleSpread, nb, 1, length / (1 + Random.value),width/ (1 + Random.value));
		}


	}

	// Use this for initialization
	void Start () {
		
		/* Make a tree */

		float angleX =  Random.Range(minBaseAngleX, maxBaseAngleX);
		float angleY = Random.Range(minBaseAngleY, maxBaseAngleY);
		float length = Random.Range(minLength, maxLength);
		float width = Random.Range(minWidth, maxWidth);	
	/*
		fractalTree = new GameObject ("FractalTree");
		drawTree (					
			new Vector3 (posX, posY, posZ),		//Satart position
			angleX, angleX, 					//start angle x y
			2, 								    //Nb subdivision per branch
			depth,								//Nb iteration
			length,								//Length
			width 								//Width
		);	
		*/


		/* For generate a forest */


		if (depth > 7) {
			return;
		}

		for (int i = 0; i < 30; i++) {

			angleX =  Random.Range(minBaseAngleX, maxBaseAngleX);
			angleY = Random.Range(minBaseAngleY, maxBaseAngleY);
			length =  Random.Range(minLength, maxLength);
			width = Random.Range(minWidth, maxWidth);	

			fractalTree = new GameObject ("FractalTree"+i);
			drawTree (	
				new Vector3 (50+Random.value*400.0f, 0.0f, 50+Random.value*400.0f), 			
				angleX, angleX, 					
				2, 					//Nb subdivision per branch
				depth,				//Nb iteration
				length,				//Length
				width 				//Width
			);	
		
		}
	


	}


	void Update () {		


		//Press space for make a new tree
		if (Input.GetKeyDown ("space")) {
			
			/* Make a tree */
			float angleX = Random.Range (minBaseAngleX, maxBaseAngleX);
			float angleY = Random.Range (minBaseAngleY, maxBaseAngleY);
			float length = Random.Range (minLength, maxLength);
			float width = Random.Range (minWidth, maxWidth);	

			Destroy (fractalTree);
			fractalTree = new GameObject ("FractalTree");
			drawTree (
				new Vector3 (posX, posY, posZ),		//Satart position
				angleX, angleX, 					//start angle x y
				2, 								    //Nb subdivision per branch
				depth,								//Nb iteration
				length,								//Length
				width 								//Width
			);	

		}

	}

}
