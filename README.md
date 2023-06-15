![rfhnbyrf](https://github.com/kolyaklimk/MoodMate/assets/93304825/bb1bf0a6-c56c-423c-812f-91e8dbb78f62)

# MoodMate
MoodMate is an app that helps you control and improve your mood. It is designed for those who experience high levels of stress, anxiety or depression.

## Key Features
- Track Your Mood: Choose from a variety of emotions to mark your current emotional state and analyze it.
- Listen to soothing music: Choose specially selected music to help you relax, de-stress, and improve your mood.
- Create notes: Write down your thoughts and observations so you always have them at your fingertips.

## Technical Details
- Programming Language: C#
- Framework: .NET MAUI
- Database: Firebase Firestore
- Authentication: Firebase Authentication

## Design Patterns
The following design patterns were used in developing the MoodTracker application:
- MVVM (Model-View-ViewModel): This pattern helps to separate the application logic (ViewModel) from the user interface (View) and data model (Model). This allows you to manage the state and interaction of the application components more effectively.
- Abstract Factory: This pattern is used to create families of related objects without explicitly specifying their specific classes. 
- Singleton: The Singleton model ensures that a class has only one instance and provides a global access point to that instance.

## Security & Privacy
MoodMate is designed with transparency and honesty in mind. You can save your data both locally and on Firebase Firestore servers. No third parties have access to your data.

## Demonstration of the app
https://github.com/kolyaklimk/MoodMate/assets/93304825/f6efad1e-f0fc-4a97-823a-d547bd5dea8a

## Installation Instructions
1. Create a vendor email and register the project on Firebase.
2. Create Firestore Database and Authentication.
3. Modify the Firestore rules:
```python
rules_version = '2';
service cloud.firestore {
  match /databases/{database}/documents {
    match /{document=**} {
      allow read, write: if true;
    }
  }
}
```
4. Create an Email/Password provider in the Authentication section.
5. Fill in all the data in the PrivateConstants class - "MoodMate\Components\PrivateConstants.cs":
```csharp
namespace MoodMate.Components;

static class PrivateConstants
{
    public const string Email = "Supplier mail";
    public const string EmailPassword = "Supplier email password";
    public const string ApiKey = "Firebase Web API Key";
    public const string AuthDomain = "Firebase AuthDomain";
    public const string ProjectId = "Firebase ProjectId";
    public const string host = "Mail host";
    public const int port = 000;
    public const bool useSsl = true;
}
```
6. Generate a new private key in the "Firebase Admin SDK" and copy all its contents to the file admin.json located at "MoodMate\Resources\Raw\admin.json".

## Class Diagram
![MoodMateDiagram](https://github.com/kolyaklimk/MoodMate/assets/93304825/1c2288e7-717a-479a-849d-a37e614e2b00)

## Use-case
![UseCase](https://github.com/kolyaklimk/MoodMate/assets/93304825/a2324fa5-8ef6-4af9-9f62-0c0735ad6d6a)
