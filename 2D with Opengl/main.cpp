#include "stdafx.h"
#include <stdio.h>
#include <math.h> 
#include "glm\glm.hpp"
#include <GL/glut.h>

#define window_width  640
#define window_height 640
#define PI 3.14159265358979323846


void init_GL () {

	//Enable transparency
	glEnable (GL_BLEND); 
	glBlendFunc (GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	glClearColor (0.0f, 0.0f, 0.0f, 1.0f);
}

void init_Windows (char* title, int x, int y, int sx, int sy) {

	//Calcul window's initial width,height,x,y
	if (x == -1) {
		x = (glutGet (GLUT_SCREEN_WIDTH) - sx) / 2;
	}
	if (y == -1) {
		y = (glutGet (GLUT_SCREEN_HEIGHT) - sy) / 2;
	}
	
	//Set position
	glutInitWindowPosition (x, y);
	//Set size
	glutInitWindowSize (sx, sy);
	// Create window with the given title
	glutCreateWindow (title);

}

void reshape (GLsizei width, GLsizei height) {
	// Set the viewport to cover the new window
	//Comment for avoid redimention of the content
	glViewport (0, 0, width, height);
}

float clean_angle (float a) {
	if (a > 360) {
		return a - 360;
	} else if (a < 0) {
		return a + 360;
	} else {
		return a;
	}
}

void draw_tree_worker (glm::vec2 pos, float length, float pas_angle, float current_angle, int nb,int depth) {

	if (depth < 0) {
		return;
	}

	if (depth == 0) {
		//Draw leaf
		return;
	}

	//Calcul the ending point of the current segment depending of the angle and the length
	glm::vec2 new_pos (pos.x + length * cos (current_angle / 180 * PI), pos.y + length * sin (current_angle / 180 * PI));

	//Draw one line
	glBegin (GL_LINES);
	glColor4f (1.0f , 1.0f, 1.0f, 0.3f);
	glVertex2f (pos.x, pos.y);
	glVertex2f (new_pos.x, new_pos.y);
	glEnd ();

	//loop nb time for make the subdivision of the tree
	for (int i = 0; i <= nb; i++) {
		//Calcul the next angle by adding to the actual current_angle mutiple of the pas_angle
		float next_angle = clean_angle (current_angle + (pas_angle * (i - ((float)nb / 2))));
		//Recursive call
		draw_tree_worker (new_pos, length / 1.25, pas_angle, next_angle, nb, depth-1);
	}

}

void draw_tree (glm::vec2 pos, float length, float a, int nb) {
	draw_tree_worker (pos, length, a, 90, nb,8);
}



void display () {
	glClear (GL_COLOR_BUFFER_BIT); // Clear the color buffer with current clearing color
	draw_tree (glm::vec2 (0.0f, -0.5f), 0.3f, 30, 1);
	glFlush ();  // Render now
}



void specialKeys (int key, int x, int y) {

	switch (key) {
		case GLUT_KEY_RIGHT:
			//  Right arrow
			printf ("KEY_RIGHT Pressed\n");
			break;
		case GLUT_KEY_LEFT:
			//  Up arrow
			printf ("KEY_LEFT Pressed\n");
			break;
		case GLUT_KEY_UP:
			//  Right arrow
			printf ("KEY_UP Pressed\n");
			break;
		case GLUT_KEY_DOWN:
			//  Up arrow
			printf ("KEY_DOWN Pressed\n");
			break;
		case GLUT_KEY_F1:
			//  Right arrow
			printf ("KEY_F1 Pressed\n");
			break;
		case GLUT_KEY_F2:
			//  Up arrow
			printf ("KEY_F2 Pressed\n");
			break;
		default:
			// Unknow key
			printf ("Unknow Pressed\n");
	}

	glutPostRedisplay ();
}


void mouse_event (int button, int dir, int x, int y) {

	switch (button) {
		case GLUT_LEFT_BUTTON:
			//  Left Clik			
			if (dir == 0) {
				printf ("mouse LEFT_BUTTON Pressed\n");
			} else {
				printf ("mouse LEFT_BUTTON Released\n");
			}
			break;
		case GLUT_RIGHT_BUTTON:
			//  Rigth clik
			if (dir == 0) {
				printf ("mouse RIGHT_BUTTON Pressed\n");
			} else {
				printf ("mouse RIGHT_BUTTON Released\n");
			}
			break;
		case 3:
			//  3th button of the mouse clik
			printf ("mouse 3_BUTTON  Pressed\n");
			break;
		case 4:
			// 4th button of the mouse clik
			printf ("mouse 4_BUTTON  Pressed\n");
			break;
		default:
			// Unknow key
			printf ("mouse Unknow_BUTTON  Pressed\n");
	}

	glutPostRedisplay ();
}


void mouse_position (int x, int y) {
	//If a mouse event append, and a mouse motion, this function is call
	printf ("mouse position : x=%d; y=%d\n", x, y);
}


int main (int argc, char** argv) {

	// Initialize GLUT & OpenGL
	printf ("Initialize GLUT\n");
	glutInit (&argc, argv);
	init_GL ();

	// Initialize GLUT
	printf ("Initialize Windows\n");
	init_Windows ("OpenGL", -1, -1, window_width, window_height);

	//Initialise callBack
	printf ("Initialise callBack\n");
	// Register callback handler for window re-paint event
	glutDisplayFunc (display);

	// Register callback handler for window re-size event
	glutReshapeFunc (reshape);
	// Register callback handler for key event
	glutSpecialFunc (specialKeys);
	// Register callback handler for mouse event
	glutMouseFunc (mouse_event);
	// Register callback handler for mouse move event
	glutMotionFunc (mouse_position);

	// Enter the infinite event-processing loop
	printf ("Start Looping\n");
	glutMainLoop ();

	// Exit and clean
	printf ("End of the programme \n");
	return 0;
}

