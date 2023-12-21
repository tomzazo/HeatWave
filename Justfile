# runs the project locally
@run:
	just app/run

# runs the project locally in watch mode with hot reloading
@watch:
	just app/watch

# deploys the application as a webapp on Azure
@app-deploy:
	just app/deploy config-deployed

# starts a live log stream of the deployed application on Azure
@app-logs:
	just app/logs-deployed
