#!/bin/bash
# Script to initialize the SQLite database for MindMate on Render

set -e # Exit on error

DB_FILE=./mindmate.db

echo "Checking for database file..."

# Create database if it doesn't exist
if [ ! -f "$DB_FILE" ]; then
    echo "Database file not found. Creating new database..."
    touch "$DB_FILE"
    echo "Created empty database file: $DB_FILE"
    
    # Initialize database schema using the SQL script
    echo "Initializing database schema..."
    sqlite3 "$DB_FILE" < ./src/MindMate.Api/init-db.sql
    echo "Database schema initialized successfully."
else
    echo "Database file already exists at: $DB_FILE"
fi

# Set proper permissions
chmod 664 "$DB_FILE"
echo "File permissions set."

echo "Database initialization complete."
