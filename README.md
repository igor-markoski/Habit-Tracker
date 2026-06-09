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
This project was developed with the assistance of AI tools to accelerate the development process and ensure best practices:
- **ChatGPT**: Used for initial brainstorming and generating the precise system prompts and architecture requirements.
- **Gemini Pro (via Gemini CLI)**: Used as the primary coding assistant for generating the C# code, setting up the Avalonia MVVM structure, implementing logic (like streak calculations), and managing the Git Feature Branch workflow.

## Media (Screenshots & Videos)
*(Replace the placeholder links below with your actual images or videos before submitting)*

### Main View
![Main View Placeholder](link-to-your-main-view-image.png)
*Description: The main dashboard showing active habits and current streaks.*

### Dark Mode Toggle
![Dark Mode Placeholder](link-to-your-dark-mode-image.png)
*Description: The application running in Dark Mode for better nighttime visibility.*

### Adding a Habit
![Add Habit Placeholder](link-to-your-add-habit-image.png)
*Description: Demonstrating the process of creating a new habit.*
