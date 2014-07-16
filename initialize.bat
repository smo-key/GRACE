@echo Initializing environment...
git submodule init
git submodule update
cp d3dlib/assets/dll d3dlib-test/bin
@echo If any of these failed, open initialize.bat and run them manually!
@pause