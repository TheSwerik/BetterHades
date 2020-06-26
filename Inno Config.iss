; Variables:
#define MyAppName "BetterHades"
#define MyAppVersion "1.0.2"
#define MyAppPublisher "Swerik"
#define MyAppURL "https://github.com/TheSwerik"   
#define MyAppExeName "BetterHades.exe"
#define MyAppIconName "BetterHades.ico"

[Setup]
AppId={{D952D89A-F88F-42A8-8E3C-639CED1EE150}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL} 
;AppComments=
; The Name displayed in the "add or remove programs" page (default is "{#MyAppName} version {#MyAppVersion}")
AppVerName={#MyAppName}
DefaultDirName={autopf}\{#MyAppName}   
DefaultGroupName={#MyAppName} 
Compression=lzma2   
SolidCompression=yes   
WizardStyle=modern
OutputBaseFilename={#MyAppName}-Installer
OutputDir=Installer
ArchitecturesInstallIn64BitMode=x64  
AllowNoIcons=yes  
ShowLanguageDialog=auto
CloseApplications=yes
CloseApplicationsFilter=*.*
; set the name of a License Agreement File (.txt or .rtf) which is displayed before the user selects the destination directory for the program
; gets ignored if Specified in [Language]
;LicenseFile=
; Makes Explorer Refresh File Associations at the End of the Un-/Installation
ChangesAssociations=yes      
SetupIconFile={#MyAppIconName}
UninstallDisplayIcon={app}\{#MyAppIconName}   

[Registry]
Root: HKCR; Subkey: ".bhds";                            ValueData: "{#MyAppName}";          Flags: uninsdeletevalue;  ValueType: string;  ValueName: ""
Root: HKCR; Subkey: "{#MyAppName}";                     ValueData: "Program {#MyAppName}";  Flags: uninsdeletekey;    ValueType: string;  ValueName: ""
Root: HKCR; Subkey: "{#MyAppName}\DefaultIcon";         ValueData: "{app}\{#MyAppExeName},0";                         ValueType: string;  ValueName: ""
Root: HKCR; Subkey: "{#MyAppName}\shell\open\command";  ValueData: """{app}\{#MyAppExeName}"" ""%1""";                ValueType: string;  ValueName: ""

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked   

[Files]
Source: "Publish\bin\64bit\*"; DestDir: "{app}\bin\64bit"; Excludes:"*.pdb"; Check: Is64BitInstallMode;     Flags: ignoreversion recursesubdirs
Source: "Publish\bin\32bit\*"; DestDir: "{app}\bin\32bit"; Excludes:"*.pdb"; Check: not Is64BitInstallMode; Flags: ignoreversion recursesubdirs solidbreak  

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\bin\64bit\{#MyAppExeName}"; Flags: createonlyiffileexists;
Name: "{group}\{#MyAppName}"; Filename: "{app}\bin\32bit\{#MyAppExeName}"; Flags: createonlyiffileexists; 
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\bin\64bit\{#MyAppExeName}"; Tasks: desktopicon; Flags: createonlyiffileexists;
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\bin\32bit\{#MyAppExeName}"; Tasks: desktopicon; Flags: createonlyiffileexists; 
Name: "{app}\{#MyAppName}"; Filename: "{app}\bin\64bit\{#MyAppExeName}"; Flags: createonlyiffileexists;
Name: "{app}\{#MyAppName}"; Filename: "{app}\bin\32bit\{#MyAppExeName}"; Flags: createonlyiffileexists; 

[Run] 
Filename: "{app}\bin\64bit\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Check: IsWin64; Flags: nowait postinstall skipifsilent
Filename: "{app}\bin\32bit\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Check: not IsWin64; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{autodocs}\{#MyAppName}"