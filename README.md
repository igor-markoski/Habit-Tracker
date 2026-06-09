# Habit Tracker - .NET 9 Avalonia UI

## Problem Description
Maintaining daily habits can be challenging without visual feedback and progress tracking. This application solves the problem by providing a simple, local-first platform to manage habits, track daily completions, and visualize progress through streaks and statistics.

## Solution Overview
The Habit Tracker is a desktop application built with C# and Avalonia UI. It allows users to:
- Create and delete habits.
- Mark habits as completed for the current day.
- View real-time statistics including current streaks, best streaks, and total completions.
- Persist data locally using JSON storage.

## Architecture (MVVM)
The project follows the **Model-ViewModel-View (MVVM)** architectural pattern:
- **Models**: Represent the data (e.g., `Habit`).
- **ViewModels**: Contain the UI logic and state (`MainWindowViewModel`). They act as a bridge between Models and Views.
- **Views**: Define the UI layout using XAML (`MainWindow.axaml`).
- **Services**: Handle business logic and data persistence (`HabitService`, `StorageService`).

## Class Descriptions
- `Habit`: The core data model storing habit name, description, and completion history. Includes logic for streak calculation.
- `StorageService`: Manages saving and loading the habit list to `habits.json` in the local application data folder.
- `HabitService`: Orchestrates habit management (Add, Delete, Toggle) and interfaces with the `StorageService`.
- `MainWindowViewModel`: Manages the collection of habits and provides commands for UI interactions using the CommunityToolkit.Mvvm.
- `HabitConverters`: XAML value converters for dynamic UI updates (e.g., button colors and labels).

## How JSON Storage Works
Data is serialized using `System.Text.Json` and stored in a file named `habits.json`. The application automatically loads this file on startup and saves changes whenever a habit is added, deleted, or updated. The file is located in the `%AppData%/Local/HabitTracker` directory.

## AI Usage in Development
This project was developed with the assistance of **Gemini CLI**, an AI-powered coding agent. The AI helped in:
- Setting up the initial project structure and MVVM scaffolding.
- Implementing complex logic such as streak calculations using LINQ.
- Designing the UI layout and XAML bindings.
- Managing the **Feature Branch Workflow** to simulate professional collaboration.

## Screenshots Suggestions
- **Main View**: Show the list of habits with their streaks.
- **Dark Mode**: Toggle the theme to show the application in dark mode.
- **Add Habit**: Show the input fields at the bottom for adding a new habit.
