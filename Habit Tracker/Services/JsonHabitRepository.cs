using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Habit_Tracker.Models;

namespace Habit_Tracker.Services
{
    /// <summary>
    /// <see cref="IHabitRepository"/> backed by a JSON file in the user's local
    /// application data folder (<c>%LocalAppData%/HabitTracker/habits.json</c>).
    /// Enums are written as readable strings rather than integers.
    /// </summary>
    public class JsonHabitRepository : IHabitRepository
    {
        private readonly string _filePath;

        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public JsonHabitRepository()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "HabitTracker");
            Directory.CreateDirectory(folder);
            _filePath = Path.Combine(folder, "habits.json");
        }

        public List<Habit> GetAll()
        {
            if (!File.Exists(_filePath))
                return new List<Habit>();

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Habit>>(json, Options) ?? new List<Habit>();
            }
            catch
            {
                // Corrupt or unreadable file: start fresh rather than crash on launch.
                return new List<Habit>();
            }
        }

        public void Save(IEnumerable<Habit> habits)
        {
            string json = JsonSerializer.Serialize(habits, Options);
            File.WriteAllText(_filePath, json);
        }
    }
}
