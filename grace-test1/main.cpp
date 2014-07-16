#include "main.h"

#define PAINT new PaintData(&world, &view, &projection, &ortho, &win.viewport->GetViewport(), &win.d3d->GetRasterizer())

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance
	, PSTR pScmdline, int iCmdshow)
{
	win = new Window();
	win->Initialize(L"GRACE", true, false, false, 800, 600, true);
}

void Run()
{

}

void Init()
{

}