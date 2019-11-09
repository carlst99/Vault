# Vault
Vault is a proof-of-concept application for my [StreamEncryptor](https://github.com/carlst99/StreamEncryptor) library. As such, I place no guarantees on the security of your encrypted data; I'm no expert! :smile:
Furthermore, I haven't properly tested the application yet, so use with a pinch of salt (or a pinch of IV, geddit? :joy:).

With that out of the way, Vault is designed to be application for hiding away all those files you'd rather other people don't see.
Imports are encrypted with `AES-256` and authenticated using `HMACSHA256`.
It currently has a built-in image viewer. I plan on creating a video viewer, generic file importer and notes editor. All of the above for your imported, encrypted files of course.
Also, at this stage you can't change your password once you've first set it; I'm working on it!

### Roadmap
- Implement password changing
- Implement file exporting
- Allow zooming in the image viewer
- Create video viewer
- Create generic file importer
- Create notes editor
- Port to Android, UWP?, iOS using Xamarin

Any issues/pull requests are welcome. Please respect my code styling.

### Libraries Used
- MvvmCross
- MaterialDesignInXamlToolkit
- StreamEncryptor
- BCrypt.Net-Next
- Realm.Database
- MaterialMessageBox
- MaterialDesignExtensions
- Serilog
- ImageSharp
