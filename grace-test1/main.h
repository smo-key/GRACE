#ifndef GRACE_TEST_MAIN
#define GRACE_TEST_MAIN

#include "window.h"
#include "shaders/shader_TEXTURE.h"
#include "shaders/shader_LIGHT.h"
#include "models/model_FBX.h"
#include "models/model_OBJ.h"
#pragma comment (lib, "D3DLib.lib")
using namespace D3DLIB;

/*** METHODS ***/
void Run();
void Init();
void Shutdown();

void DrawInfo();
void Movement();
void ScrollMove(unsigned char DIK, float &directionSpeedVar, float &outputVar,
	float frameTime, bool positive, bool allowNegatives);
void DrawStartupText(const WCHAR* string);

/***VARIABLES***/
Window win;

Model_FBX grace;
Model_FBX sphere;

Shader_TEXTURE shade_tex = 
	Shader_TEXTURE(L"../d3dlib/assets/shader/texture.vs", L"../d3dlib/assets/shader/texture.ps");

Texture earthtex;
Texture gracetex;

int win_width, win_height;
bool win_full;


/*** MOVEMENT CONTROL ***/

float frameTime = 0.0f;
float rotationX = 0.0f;
float rotationY = 0.0f;
float rotationZ = 0.0f;
float movementX = 0.0f;
float movementZ = 0.0f;

float rotUSpeed = 0.0f;
float rotDSpeed = 0.0f;
float moveL = 0.0f;
float moveR = 0.0f;
float moveUP = 0.0f;
float moveD = 0.0f;

bool mousePressed = false;
bool mouseEnabled = false;
int originalX = 0;
int originalY = 0;
float rotXo = 0.0f;
float rotYo = 0.0f;
int mouseX = 0;
int mouseY = 0;


#endif