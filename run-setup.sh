#!/bin/bash

# Check if the directory 'url-shortener' already exists
if [ -d "url-shortener" ]; then
    echo "'url-shortener' directory already exists. Please remove or rename it before running this script."
    exit 1
fi

# Clone the repo
echo "Cloning into $(pwd)/url-shortener..."
git clone https://github.com/crnhsn/url-shortener.git

# Navigate to the project root
cd url-shortener/ || { echo "Directory 'url-shortener' not found."; exit 1; }

# Build and run the application using Docker Compose
echo "Building and running the application with Docker Compose..."
docker-compose up --build
