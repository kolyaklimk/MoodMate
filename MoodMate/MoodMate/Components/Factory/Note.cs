namespace MoodMate.Components.Factory;

public class Note
{
    public dynamic note { get; }
    public Note(string kind)
    {
        switch (kind)
        {
            case "Simple":
                note = new SimpleNoteFactory().CreateNote();
                break;
            case "Mood":
                note = new MoodNoteFactory().CreateNote();
                break;
        }
    }
}
