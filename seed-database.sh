#!/bin/bash
# Seed Database Script
# Run this script to populate your database with dummy data

cd "$(dirname "$0")"

echo "Running database seed..."
echo "========================"

cd CodeFirst && dotnet run

echo ""
echo "========================"
echo "Done!"
