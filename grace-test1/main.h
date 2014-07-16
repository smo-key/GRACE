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


#endif