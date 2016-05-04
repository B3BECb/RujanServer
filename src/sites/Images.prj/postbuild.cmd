@echo Copy site
@set configuration=%1
@set pluginfilename=%2
@set pluginname=%3
@set plugins=..\..\..\..\RujanService.prj\bin\Debug\sites
@echo Copy site %pluginfilename% to %plugins%...
@mkdir %plugins%
@copy /Y %pluginfilename% %plugins%\
@set plugins=..\..\..\..\RujanService.prj\bin\Debug\sites\%pluginname%
@mkdir %plugins%
@echo Copy site folder from ..\..\Images to %plugins%...
@xcopy /Y /S ..\..\%pluginname% %plugins%\
@exit 0
