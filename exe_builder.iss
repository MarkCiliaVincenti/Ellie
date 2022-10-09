#define sysfolder "system"
#define version GetEnv("ELLIE_INSTALL_VERSION")
#define target "win7-x64"
#define platform "net6.0"

[Setup]
AppName = {param:botname|Ellie}
AppVersion={#version}
AppPublisher=EllieBotDevs
DefaultDirName={param:installpath|{commonpf}\Ellie}
DefaultGroupName=Ellie
UninstallDisplayIcon={app}\{#sysfolder}\ellie.ico
Compression=lzma2
SolidCompression=yes
UsePreviousLanguage=no
UsePreviousSetupType=no
UsePreviousAppDir=no
OutputDir=ellie-installers/{#version}/
OutputBaseFilename=ellieo-setup-{#version}
AppReadmeFile=https://commands.elliebot.net
ArchitecturesInstallIn64BitMode=x64
DisableWelcomePage=yes
DisableDirPage=yes
DisableFinishedPage=yes
DisableReadyMemo=yes
DisableProgramGroupPage=yes
WizardStyle=modern
UpdateUninstallLogAppName=no
CreateUninstallRegKey=no
Uninstallable=no

[Files]
;install 
Source: "src\Ellie\bin\Release\{#platform}\{#target}\publish\*"; DestDir: "{app}\{#sysfolder}"; Permissions: users-full; Flags: recursesubdirs onlyifdoesntexist ignoreversion createallsubdirs; Excludes: "*.pdb, *.db"

;reinstall - i want to copy all files, but i don't want to overwrite any data files because users will lose their customization if they don't have a backup, 
;            and i don't want them to have to backup and then copy-merge into data folder themselves, or lose their currency images due to overwrite.
Source: "src\Ellie\bin\Release\{#platform}\{#target}\publish\*"; DestDir: "{app}\{#sysfolder}"; Permissions: users-full; Flags: recursesubdirs ignoreversion onlyifdestfileexists createallsubdirs; Excludes: "*.pdb, *.db, data\*, credentials.json, creds.yml";
Source: "src\Ellie\bin\Release\{#platform}\{#target}\publish\data\*"; DestDir: "{app}\{#sysfolder}\data"; Permissions: users-full; Flags: recursesubdirs onlyifdoesntexist createallsubdirs;
; overwrite strings and aliases
Source: "src\Ellie\bin\Release\{#platform}\{#target}\publish\data\aliases.yml"; DestDir: "{app}\{#sysfolder}\data\"; Permissions: users-full; Flags: recursesubdirs ignoreversion onlyifdestfileexists createallsubdirs;
Source: "src\Ellie\bin\Release\{#platform}\{#target}\publish\data\strings\*"; DestDir: "{app}\{#sysfolder}\data\strings"; Permissions: users-full; Flags: recursesubdirs ignoreversion onlyifdestfileexists createallsubdirs;

[Dirs]
Name:"{app}\{#sysfolder}\data"; Permissions: everyone-modify
Name:"{app}\{#sysfolder}\config"; Permissions: everyone-modify
Name:"{app}\{#sysfolder}"; Permissions: everyone-modify

; [Run]
; Filename: "https://docs.elliebot.net"; Flags: postinstall shellexec runasoriginaluser; Description: "Open setup guide"
; Filename: "{app}\{#sysfolder}\creds.yml"; Flags: postinstall shellexec runasoriginaluser; Description: "Open creds file"

[Icons]
; for pretty install directory
Name: "{app}\Ellie"; Filename: "{app}\{#sysfolder}\Ellie.exe"; IconFilename: "{app}\{#sysfolder}\ellie_icon.ico"
Name: "{app}\creds"; Filename: "{app}\{#sysfolder}\creds.yml" 
Name: "{app}\data"; Filename: "{app}\{#sysfolder}\data" 

; desktop shortcut 
Name: "{commondesktop}\{#SetupSetting("AppName")}"; Filename: "{app}\Ellie";

[Code]
function GetFileName(const AFileName: string): string;
begin
  Result := ExpandConstant('{app}\{#sysfolder}\' + AFileName);
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if (CurStep = ssPostInstall) then
  begin
      FileCopy(GetFileName('creds_example.yml'), GetFileName('creds.yml'), True);
  end;
end;