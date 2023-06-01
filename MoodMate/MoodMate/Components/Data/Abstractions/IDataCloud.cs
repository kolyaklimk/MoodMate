using Google.Cloud.Firestore;

namespace MoodMate.Components.Data.Abstractions;

public interface IDataCloud
{
    FirestoreDb Db { get; set; }
    Task<DocumentReference> AddAsync(string uid, string name, Dictionary<string, object> dict);
    Task<QuerySnapshot> GetOrderByDateAsync(string uid, string name, int offset, int limit);
    Task UpdateAsync(string uid, string name, string id, Dictionary<string, object> dict);
    Task DeleteAsync(string uid, string name, string id);
    Task<QuerySnapshot> GetintervalAsync(string uid, string name, DateTime firstDay, DateTime lastDay);
}
