[Setup]
AppId={{9F5DD4BE-536C-4A56-AC59-779C164C6FAF}
AppName=Image Optimizer
AppVersion=1.3.2.0
AppVerName=Image Optimizer
AppPublisher=Dipeh
AppPublisherURL=https://Dipeh.ir
AppSupportURL=https://Dipeh.com
AppUpdatesURL=https://Dipeh.com
DefaultDirName={pf}\Image Optimizer
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir=Setup
OutputBaseFilename=ImageOptimizer-Setup
SetupIconFile=C:\Users\Ali\source\repos\ImageOptimizer\ImageOptimizer\Resource\ImageOptimizer.ico
AllowNoIcons=0
SolidCompression=yes
Compression=lzma2

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "C:\Users\Ali\source\repos\ImageOptimizer\ImageOptimizer\bin\Debug\ImageOptimizer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ali\source\repos\ImageOptimizer\ImageOptimizer\bin\Debug\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ali\source\repos\ImageOptimizer\ImageOptimizer\bin\Debug\*.xml"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{commonprograms}\Image Optimizer"; Filename: "{app}\ImageOptimizer.exe"
Name: "{commondesktop}\Image Optimizer"; Filename: "{app}\ImageOptimizer.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\Image Optimizer"; Filename: "{app}\ImageOptimizer.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\ImageOptimizer.exe"; Description: "{cm:LaunchProgram,Image Optimizer}"; Flags: nowait postinstall skipifsilent

