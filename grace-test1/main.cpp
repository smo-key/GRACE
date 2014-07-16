#include "main.h"

#define PAINT new PaintData(&world, &view, &projection, &ortho, &win.viewport->GetViewport(), &win.d3d->GetRasterizer())
#define GETDEVICE win.d3d->GetDevice()

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance
	, PSTR pScmdline, int iCmdshow)
{
	Init();
	Run();
	Shutdown();
}

void Run()
{
	///*** DEFINE VARIABLES ***///
	D3DXMATRIX world;
	D3DXMATRIX view;
	D3DXMATRIX projection;
	D3DXMATRIX ortho;
	float deg = 0.0f; //rotation of Earth

	///*** RUN LOOP ***///
	while (true)
	{
		//a return aborts the app
		if (win.Run(true) == false) { return; }
		if (win.input->IsKeyPressed(DIK_ESCAPE) == true) { return; }
		if (!win.HasFocus())
		{
			while (win.HasFocus() == false)
			{
				if (win.Run(true) == false)	{ return; }
			}
		}

		///*** SET SETTINGS ***///
		win.d3d->TurnZBufferOn(); //allow 3D rendering
		win.d3d->SetRasterizer(D3DDesc::Rasterizer(true, D3D11_CULL_NONE, D3D11_FILL_SOLID)); //set raster settings
		win.d3d->BeginScene(0.0f, 0.0f, 0.0f, 0.0f); //clear the screen to black
		win.camera->SetPosition(0.0f, 0.0f, -30.0f); //set camera position (x,y,z)
		win.camera->SetRotation(0.0f, 0.0f, 0.0f); //set camera rotation (a,b,c)
		win.camera->Render(); //activate camera settings
		win.camera->GetViewMatrix(view); //retrieve view matrix
		win.d3d->GetWorldMatrix(world); //retrieve world matrix
		win.d3d->GetOrthoMatrix(ortho); //retrieve orthographic (2D) matrix
		win.d3d->GetProjectionMatrix(projection); //retrieve projection matrix
		win.frustum->ConstructFrustum(SCREEN_DEPTH, projection, view); //get bounds of screen in 3D (frustum)
		win.viewport->SetViewport(win.d3d->GetDeviceContext(), (float)win_width, (float)win_height, 0.0f, 1.0f, 0.0f, 0.0f); //prepare viewport

		/*** MATH ***/

		deg = deg + 5.0f;
		if (deg >= 360.0f) { deg -= 360.0f; }

		Movement();

		win.camera->SetPosition(movementX, rotationZ, movementZ - 15.0f);
		win.camera->SetRotation(rotationX, rotationY, 0.0f);
		win.camera->Render();
		win.camera->GetViewMatrix(view);

		/***** DRAWING *****/

		win.painter->ClearList(); //removes all items from queue

		//Earth
		shade_tex.SetParameters(earthtex.GetTexture());
		win.painter->AddToFront(ModelType(&sphere, &shade_tex,
			new Transform(D3DXVECTOR3(-deg, 0, 0), D3DXVECTOR3(3, 3, 3), D3DXVECTOR3(0, 0, 0),
			RotMode::Deg), new CullAuto(), PAINT));
		//GRACE Sattelite
		shade_tex.SetParameters(earthtex.GetTexture());
		win.painter->AddToFront(ModelType(&grace, &shade_tex,
			new Transform(D3DXVECTOR3(0, 0, 0), D3DXVECTOR3(0.1f, 0.1f, 0.1f), D3DXVECTOR3(0, 0, 0),
			RotMode::Deg), new CullAuto(), PAINT));

		win.painter->Render(win.d3d, win.frustum, win.viewport, world, view, projection, ortho);

		DrawInfo();
		win.d3d->EndScene();
	}
}

void Init()
{
	///*** INITIALIZE WINDOW ***///
	win = Window();
	win.Initialize(L"GRACE", true, false, false, 800, 600, true);
	win.GetWindowSize(win_width, win_height, win_full);
	
	///*** INITIALIZE MODELS ***///
	DrawStartupText(L"Initializing models...");
	sphere.Initialize(win.d3d->GetDevice(), "../d3dlib/assets/model/sphere.fbx", true);
	grace.Initialize(GETDEVICE, "assets/models/GRACE_v011.fbx", true);
	
	///*** INITIALIZE TEXTURES ***///
	DrawStartupText(L"Initializing textures...");
	gracetex.Initialize(GETDEVICE, L"assets/images/GRACE_Texture.tga");
	earthtex.Initialize(GETDEVICE, L"../d3dlib/assets/image/Earth_CloudyDiffuse.dds");

	///***INITIALIZE SHADERS***///
	DrawStartupText(L"Completing initialization...");
	shade_tex.Initialize(GETDEVICE, win.GetHWND());

	///*** SET DIRECT3D SETTINGS ***///
	win.d3d->TurnZBufferOn();
	win.d3d->SetRasterizer(D3DDesc::Rasterizer(true, D3D11_CULL_NONE, D3D11_FILL_SOLID));
}

void Shutdown()
{
	///*** MODELS ***///
	sphere.Shutdown();
	grace.Shutdown();

	///*** TEXTURES ***///
	gracetex.Shutdown();
	earthtex.Shutdown();

	///*** OTHERS ***///
	win.Shutdown();
}


void DrawInfo()
{
	WCHAR tempString[80];
	if (mouseEnabled)
	{
		swprintf_s(tempString, L"%d", mouseX);
		WCHAR mouseString[80];
		wcscpy_s(mouseString, L"Mouse X: ");
		wcscat_s(mouseString, tempString);

		win.text->Render(win.d3d->GetDeviceContext(), mouseString, L"Segoe UI", 10, 40, 12.0f, 0xffffffff,
			FW1_LEFT | FW1_TOP | FW1_RESTORESTATE);

		WCHAR mouseString2[80];
		swprintf_s(tempString, L"%d", mouseY);
		wcscpy_s(mouseString2, L"Mouse Y: ");
		wcscat_s(mouseString2, tempString);

		win.text->Render(win.d3d->GetDeviceContext(), mouseString2, L"Segoe UI", 10, 55, 12.0f, 0xffffffff,
			FW1_LEFT | FW1_TOP | FW1_RESTORESTATE);
	}
	else
	{
		win.text->Render(win.d3d->GetDeviceContext(), L"No mouse input.", L"Segoe UI", 10, 40, 12.0f, 0xbb00ff00,
			FW1_LEFT | FW1_TOP | FW1_RESTORESTATE);
	}

	WCHAR fpsString[80];
	swprintf_s(tempString, L"%d", win.GetFPS());
	wcscpy_s(fpsString, L"FPS: ");
	wcscat_s(fpsString, tempString);
	win.text->Render(win.d3d->GetDeviceContext(), fpsString, L"Segoe UI", 10, 10, 12.0f, 0xffffffff,
		FW1_LEFT | FW1_TOP | FW1_RESTORESTATE);

	WCHAR cpuString[80];
	swprintf_s(tempString, L"%d", win.GetCPU());
	wcscpy_s(cpuString, L"CPU: ");
	wcscat_s(cpuString, tempString);
	wcscat_s(cpuString, L"%");

	win.text->Render(win.d3d->GetDeviceContext(), cpuString, L"Segoe UI", 10, 25, 12.0f, 0xffffffff,
		FW1_LEFT | FW1_TOP | FW1_RESTORESTATE);
}

void Movement()
{
	frameTime = win.GetTime();
	mouseEnabled = win.input->GetMouseLocation(mouseX, mouseY);

	/*ScrollMove(DIK_LEFT, leftTurnSpeed, rotationY, frameTime, true, false);
	ScrollMove(DIK_RIGHT, rightTurnSpeed, rotationY, frameTime, false, false);
	ScrollMove(DIK_DOWN, downTurnSpeed, rotationX, frameTime, false, false);
	ScrollMove(DIK_UP, upTurnSpeed, rotationX, frameTime, true, false);*/
	ScrollMove(DIK_A, moveL, movementX, frameTime, true, true);
	ScrollMove(DIK_D, moveR, movementX, frameTime, false, true);
	ScrollMove(DIK_S, moveD, movementZ, frameTime, false, true);
	ScrollMove(DIK_W, moveUP, movementZ, frameTime, true, true);
	ScrollMove(DIK_PGUP, rotUSpeed, rotationZ, frameTime, true, true);
	ScrollMove(DIK_PGDN, rotDSpeed, rotationZ, frameTime, false, true);

	if (mouseEnabled)
	{
		if (win.input->IsMousePressed(Input::MouseButton::left))
		{
			if (mousePressed == false)
			{
				originalX = mouseX;
				originalY = mouseY;
				rotXo = -rotationY;
				rotYo = -rotationX;
				mousePressed = true;
			}
			else
			{
				float dX, dY;
				dX = (float)mouseX - (float)originalX;
				dY = (float)mouseY - (float)originalY;
				rotationY += -(dX / 5);
				rotationX += -(dY / 5);
				originalX = mouseX;
				originalY = mouseY;
			}
		}
		else
		{
			mousePressed = false;
		}
	}

	if (win.input->IsKeyPressed(DIK_R) == true)
	{
		rotationX = 0.0f;
		rotationY = 0.0f;
		movementX = 0.0f;
		movementZ = 0.0f;
		rotationZ = 0.0f;
	}
}

void ScrollMove(unsigned char DIK, float &directionSpeedVar, float &outputVar,
	float frameTime, bool positive, bool allowNegatives)
{
	if (win.input->IsKeyPressed(DIK) == true)
	{
		directionSpeedVar += frameTime * 0.01f;

		if (directionSpeedVar > (frameTime * 0.15f))
		{
			directionSpeedVar = frameTime * 0.15f;
		}
	}
	else
	{
		directionSpeedVar -= frameTime * 0.005f;

		if (directionSpeedVar < 0.0f)
		{
			directionSpeedVar = 0.0f;
		}
	}

	if (!positive) { outputVar -= directionSpeedVar; }
	else { outputVar += directionSpeedVar; }
	if ((outputVar < 0.0f) && (!allowNegatives))
	{
		outputVar += 360.0f;
	}
}

void DrawStartupText(const WCHAR* string)
{
	win.d3d->BeginScene(0.0f, 0.0f, 0.0f, 0.0f);
	win.text->Render(win.d3d->GetDeviceContext(), string, L"Segoe UI", 10, 10, 12.0f, 0xff00ff00,
		FW1_LEFT | FW1_TOP | FW1_RESTORESTATE);
	win.d3d->EndScene();
}