# runs the project locally
@run:
	just app/run

# runs the project locally in a container
@run-docker:
	just app/build
	docker compose up --detach

# runs the project locally in watch mode with hot reloading
@watch:
	just app/watch

# deploys the application as a webapp on Azure
@app-deploy:
	just app/build
	just app/deploy config-deployed

# starts a live log stream of the deployed application on Azure
@app-logs:
	just app/logs-deployed

@push-docker:
	just app/build
	docker buildx build \
		--platform linux/arm64 \
		--tag registry.pi.home:31000/heatwave:latest \
		--push \
		--file resources/dotnet/Dockerfile \
		.
