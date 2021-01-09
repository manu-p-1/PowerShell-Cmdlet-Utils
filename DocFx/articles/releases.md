# GitHub Releases 

The following releases can be found on the [PowerPlug GitHub releases page](https://github.com/manu-p-1/PowerPlug/wiki/Cmdlets)

---

## 0.2.1-alpha Release of PowerPlug
The second and patch release of PowerPlug is now available on the PowerShell gallery as a prerelease available here: [PowerPlug](https://www.powershellgallery.com/packages/PowerPlug/0.2.1-alpha)

Install PowerPlug to PowerShell with:
```powershell
Install-Module -Name PowerPlug -AllowPrerelease
```

### Features
- **New-Byname** - for creating a new alias to your Profile across various sessions
  - ```powershell
    New-Byname [-Name] <string> [-Value] <string> [-Description <string>] [-Option {None | ReadOnly | Constant | Private | AllScope
    | Unspecified}] [-PassThru] [-WhatIf] [-Confirm] [-Scope {Global | Local | Private | Numbered scopes | Script}] [-Force]
    [<CommonParameters>]
    ``` 
  - The underlying structure of the command is the same as New-Alias, however, `New-Byname` writes to the user's `$PROFILE`

- **Set-Byname** - for setting an alias to your Profile across various sessions
  - ```powershell
    Set-Byname [-Name] <string> [-Value] <string> [-Description <string>] [-Option {None | ReadOnly | Constant | Private | AllScope
    | Unspecified}] [-PassThru] [-WhatIf] [-Confirm] [-Scope {Global | Local | Private | Numbered scopes | Script}] [-Force]
    [<CommonParameters>]
    ``` 
  - The underlying structure of the command is the same as Set-Alias, however, `Set-Byname` writes to the user's `$PROFILE`

- **Remove-Byname** - for removing an alias to your Profile across various sessions
  - ```powershell
    Remove-Byname [-Name] <string> [-Scope {Global | Local | Private | Numbered scopes | Script}] [-Force] [<CommonParameters>]
    ``` 
  - The underlying structure of the command is the same as Remove-Alias, however, `Remove-Byname` removes and "bynames" from the  user's `$PROFILE`

---

## 0.1.0-alpha Release of PowerPlug
The initial release of PowerPlug is now available.

Import the PowerPlug dll to PowerShell with:
```powershell
ipmo "path/to/PowerPlug.dll"
```

### Features
- **Move-Trash** - Moves a file or directory to the Recycle Bin instead of erasing it off the system.
  - ```powershell
    Move-Trash [-Path] <string> [[-List]] [<CommonParameters>]
    ```

- **Compare-Hash** - Compares a file hash with a known signature and displays whether the signature was a match
  - ```powershell
    Compare-Hash [-Hash] {SHA256 | SHA512 | MD5} [-Path] <string> [-Signature] <string> [<CommonParameters>]
    ```