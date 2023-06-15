![rfhnbyrf](https://github.com/kolyaklimk/MoodMate/assets/93304825/bb1bf0a6-c56c-423c-812f-91e8dbb78f62)

# MoodMate
This name means that the application will help users become a partner of their mood.

The app allows users to record their mood every day and analyze its changes over time, listen to soothing music and create notes.

# Security & Privacy
MoodMate is built on the principles of transparency and honesty. You can store your data either locally or on Firebase Firestore servers. No third parties have access to the data.

# Demonstration of the app
https://github.com/kolyaklimk/MoodMate/assets/93304825/f6efad1e-f0fc-4a97-823a-d547bd5dea8a

# Installation Instructions
1. Create a vendor email and register the project on Firebase.
2. Create Firestore Database and Authentication.
3. Change Firestore rules:
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
4. Create Email/Password provider in Authentication.
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

# Class Diagram
![MoodMateDiagram](https://github.com/kolyaklimk/MoodMate/assets/93304825/1c2288e7-717a-479a-849d-a37e614e2b00)

# Use-case
![UseCase](https://github.com/kolyaklimk/MoodMate/assets/93304825/a2324fa5-8ef6-4af9-9f62-0c0735ad6d6a)
