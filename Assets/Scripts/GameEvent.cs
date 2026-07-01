using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameEventMonoBehaviour : MonoBehaviour
{
    public static GameEventMonoBehaviour Instance;

    [Header("CSV Settings")]
    public string fileName = "player_event_counter.csv";

    private string filePath;
    private bool csvCreated = false;

    private Dictionary<string, EventCounterData> eventCounters =
        new Dictionary<string, EventCounterData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            filePath = Path.Combine("C:/Users/Yun/OneDrive/Dokumen/GitHub/WhereMemoriesRemain/Report", fileName);

            Debug.Log("CSV created at the end");
            Debug.Log("CSV location: " + filePath);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LogEvent(string eventType, string objectName)
    {
        if (Instance == null)
        {
            Debug.LogWarning("GameEventCsv tidak di scene");
            return;
        }

        Instance.AddCounter(eventType, objectName);
    }

    private void AddCounter(string eventType, string objectName)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string key = eventType + "_" + objectName;

        if (!eventCounters.ContainsKey(key))
        {
            eventCounters[key] = new EventCounterData
            {
                eventType = eventType,
                objectName = objectName,
                count = 1,
                firstTimestamp = timestamp,
                lastTimestamp = timestamp
            };
        }
        else
        {
            eventCounters[key].count++;
            eventCounters[key].lastTimestamp = timestamp;
        }

        Debug.Log(eventType + " - " + objectName + " count: " + eventCounters[key].count);
    }

    public static void EndGameAndCreateCsv()
    {
        if (Instance == null)
        {
            Debug.LogWarning("GameEventCsv tidak di scene");
            return;
        }

        Instance.CreateCsvFile();
    }

    private void CreateCsvFile()
    {
        if (csvCreated)
        {
            Debug.Log("CSV created.");
            return;
        }

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("Event Type,Object Name,Count,First Timestamp,Last Timestamp");

            foreach (EventCounterData data in eventCounters.Values)
            {
                string line =
                    $"{EscapeCsv(data.eventType)}," +
                    $"{EscapeCsv(data.objectName)}," +
                    $"{data.count}," +
                    $"{EscapeCsv(data.firstTimestamp)}," +
                    $"{EscapeCsv(data.lastTimestamp)}";

                writer.WriteLine(line);
            }
        }

        csvCreated = true;

        Debug.Log("CSV created successfully at: " + filePath);
    }

    private string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "";
        }

        if (value.Contains(",") || value.Contains("\""))
        {
            value = value.Replace("\"", "\"\"");
            return $"\"{value}\"";
        }

        return value;
    }

    private void OnApplicationQuit()
    {
        CreateCsvFile();
    }
}

[Serializable]
public class EventCounterData
{
    public string eventType;
    public string objectName;
    public int count;
    public string firstTimestamp;
    public string lastTimestamp;
}