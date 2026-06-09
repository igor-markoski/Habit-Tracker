using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Habit_Tracker.Models;

namespace Habit_Tracker.Services
{
    public class StorageService
    {
        private readonly string _filePath;

        public StorageService()
        {
            // Save data in the user's local app data folder or current directory
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HabitTracker");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            _filePath = Path.Combine(folder, "habits.json");
        }

        public void SaveHabits(List<Habit> habits)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(habits, options);
            File.WriteAllText(_filePath, json);
        }

        public List<Habit> LoadHabits()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Habit>();
            }

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Habit>>(json) ?? new List<Habit>();
            }
            catch
            {
                return new List<Habit>();
            }
        }
    }
}