#include "main.h"

#define PAINT new PaintData(&world, &view, &projection, &ortho, &win.viewport->GetViewport(), &win.d3d->GetRasterizer())
#define GETDEVICE win.d3d->GetDevice()

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance
	, PSTR pScmdline, int iCmdshow)
{
	Init();
	Run();
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

		/***** DRAWING *****/

		win.painter->ClearList(); //removes all items from queue

		//earth
		//shade_tex.SetParameters(earthtex.GetTexture());
		//win.painter->AddToFront(ModelType(&sphere, &shade_tex,
		//	new Transform(D3DXVECTOR3(-deg, 0, 0), D3DXVECTOR3(3, 3, 3), D3DXVECTOR3(0, 0, 0),
		//	RotMode::Deg), new CullAuto(), PAINT));
		//GRACE Sattelite
		shade_tex.SetParameters(earthtex.GetTexture());
		win.painter->AddToFront(ModelType(&grace, &shade_tex,
			new Transform(D3DXVECTOR3(0, 0, 0), D3DXVECTOR3(0.1, 0.1, 0.1), D3DXVECTOR3(0, 0, 0),
			RotMode::Deg), new CullAuto(), PAINT));

		win.painter->Render(win.d3d, win.frustum, win.viewport, world, view, projection, ortho);
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
	sphere.Initialize(win.d3d->GetDevice(), "../d3dlib/assets/model/sphere.fbx", true);
	grace.Initialize(GETDEVICE, "assets/models/GRACE_v011.fbx", true);
	
	///*** INITIALIZE TEXTURES ***///
	gracetex.Initialize(GETDEVICE, L"assets/images/GRACE_Texture.tga");
	earthtex.Initialize(GETDEVICE, L"../d3dlib/assets/image/Earth_CloudyDiffuse.dds");

	///***INITIALIZE SHADERS***///
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