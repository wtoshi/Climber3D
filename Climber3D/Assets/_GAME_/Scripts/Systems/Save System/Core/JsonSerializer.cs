using System.IO;
using Newtonsoft.Json;

public class JsonSerializer<T> where T : class, new()
{
    private string Path { get; }
    public JsonSerializer(string path)
    {
        Path = path;
        Data = new T();
        LoadData();
    }

    public T Data { get; set; }

    public void SaveData()
    {
        var saveData = JsonConvert.SerializeObject(Data);
        SaveUtils.SaveToDisk(saveData, Path);
    }

    public void LoadData()
    {
        if (!File.Exists(Path))
        {
            SaveData();
        }
        string data = SaveUtils.LoadFromDisk(Path);
        Data = JsonConvert.DeserializeObject<T>(data);
    }

    public void ClearData()
    {
        Data = new T();
        SaveData();
    }
}