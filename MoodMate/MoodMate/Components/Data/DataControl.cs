using Google.Cloud.Firestore;
using MoodMate.Components.Data.Abstractions;
using SerializationTools;

namespace MoodMate.Components.Data;

public class DataControl<T> : DataLoading<T>, IDataControl<T>, IDataCloud
{
    public FirestoreDb Db { get; set; }
    public DataControl()
    {
        Db = FirestoreDb.Create(PrivateConstants.ProjectId);
    }

    public void Add(T item)
    {
        Data.Add(item);
    }

    public void Change(int index, T item)
    {
        Data[index] = item;
    }

    public void Delete(int index)
    {
        Data.RemoveAt(index);
    }

    public void ClearData()
    {
        Data.Clear();
    }

    public async Task UpdateFile(string path)
    {
        try
        {
            string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, path);
            using (FileStream fs = new FileStream(targetFile, FileMode.Create))
            {
                await DataSerializer.JsonSerializeAsync(fs, Data);
            }
        }
        catch { }
    }

    public string GenerateKey()
    {
        var random = new Random();
        return new string(Enumerable.Range(1, 16).
            Select(x => Constants.ForGenerateKey[random.Next(Constants.ForGenerateKey.Length)]).ToArray());
    }

    public async Task<DocumentReference> AddAsync(string uid, string name, Dictionary<string, object> dict)
    {
        return await Db.Collection("Users").Document(uid).Collection(name).AddAsync(dict);
    }

    public Task<QuerySnapshot> GetOrderByDateAsync(string uid, string name, int offset, int limit)
    {
        return Db.Collection("Users").Document(uid).Collection(name).
            OrderByDescending("Date").Offset(offset).Limit(limit).GetSnapshotAsync();
    }

    public async Task UpdateAsync(string uid, string name, string id, Dictionary<string, object> dict)
    {
        await Db.Collection("Users").Document(uid).Collection(name).Document(id).UpdateAsync(dict);
    }

    public async Task DeleteAsync(string uid, string name, string id)
    {
        await Db.Collection("Users").Document(uid).Collection(name).
            Document(id).DeleteAsync();
    }

    public async Task<QuerySnapshot> GetintervalAsync(string uid, string name, DateTime firstDay, DateTime lastDay)
    {
        return await Db.Collection("Users").Document(uid).Collection(name).
            WhereGreaterThanOrEqualTo("Date", firstDay).WhereLessThan("Date", lastDay).GetSnapshotAsync();
    }
}