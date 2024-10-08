# URL Shortener

This is a full-stack URL shortener, with a .NET backend, React frontend, Redis for data storage, and nginx for routing. Docker and Docker Compose are used for containerization and orchestration. 

## Architecture

- **Backend**: .NET
- **Frontend**: React with TypeScript
- **Database**: Redis
- **Routing**: nginx
- **Containerization & Orchestration**: Docker and Docker Compose

## Prerequisites

Before you begin, please ensure you have the following installed on your system:

- Docker
- Docker Compose

Please ensure Docker is running. 

## Getting Started

To run this locally, please follow one of the below sets of instructions.

### Semi-Automated Approach
1. Open a bash shell and navigate to the desired directory.

2. Download the ```run-setup.sh``` bash script provided in this repo to your local directory, and run the script:

```
bash run-setup.sh
```
The ```run-setup.sh``` script is located in the root directory of this repo (in the same directory as this ```README```). 

3. In a browser, navigate to
```
http://localhost:3000/
```
to access the application, once setup is complete. 

If there are any issues, please try the manual approach detailed below.

### Manual Approach

1. Open a command-line interface / shell. 

2. Clone this repository.

3. Navigate to the project root:

   ```
   cd url-shortener/
   ```

4. Build and run the application using Docker Compose:

   ```
   docker-compose up --build
   ```

   This command will use the included docker-compose file to:
   - Build the .NET backend service
   - Build the React client
   - Set up the Redis instance
   - Configure nginx to route requests between frontend and backend

5. Once the build process is complete, access the application by navigating to:

   ```
   http://localhost:3000/
   ```

## Stopping the Application

Terminate the application process (e.g., via CTRL + C in the shell). 

To tear down the Docker containers, run:


```
docker-compose down
```

in the same directory as the docker-compose.yml. 

## Project Structure

- `/server`: the .NET url shortening API, with its own Dockerfile
- `/client`: the React frontend, with its own Dockerfile
- `nginx.conf`: the routing configuration
- `docker-compose.yml`: builds and runs the necessary containers (backend, frontend, database, proxy)
