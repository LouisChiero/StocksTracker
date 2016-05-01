@echo off
echo %DEPLOY_PROJ%

IF "%DEPLOY_PROJ%" == "Angular" (
	deploy.Angular.cmd
) ELSE (
	IF "%DEPLOY_PROJ%" == "API" (
		deploy.API.cmd
	) ELSE (
		echo You have to set DEPLOY_PROJ setting to either "Angular" or "API"
		exit /b 1
	)
)