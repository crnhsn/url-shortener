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

## Getting Started

To run this locally: 

1. Ensure Docker is running on your machine.

2. Clone the repository.

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

To tear down the Docker containers, run:


```
docker-compose down
```


## Project Structure

- `/server`: the .NET url shortening API, with its own Dockerfile
- `/client`: the React frontend, with its own Dockerfile
- `nginx.conf`: the routing configuration
- `docker-compose.yml`: builds and runs the necessary containers (backend, frontend, database, proxy)
