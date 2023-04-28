namespace MoodMate.Components.Entities.Abstractions;

internal interface IFileService
{
    string Name { get; set; }
    string Source { get; set; }
    string Description { get; set; }
    Task Load(string path); 
}
