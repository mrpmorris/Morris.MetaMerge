cls
@echo **** 0.1.0-Alpha1 : UPDATED THE VERSION NUMBER IN THE PROJECT *AND* BATCH FILE? ****
pause

cls
@call BuildAndTest.bat

@echo
@echo
@echo
@echo ======================

set /p ShouldPublish=Publish 0.1.0-Alpha1 [yes]?
@if "%ShouldPublish%" == "yes" (
	@echo PUBLISHING
	dotnet nuget push .\Source\nugets\MetaMerge.Fody.0.1.0-Alpha1.nupkg -k %MORRIS.NUGET.KEY% -s https://api.nuget.org/v3/index.json
)

