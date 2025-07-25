# PowerShell script to copy all .csproj, .cs, and .xaml files to a "data" folder
# Ignoring obj and bin folders and storing all files directly in the data folder
# Get the current directory where the script is running
$currentDir = Get-Location

# Create the "data" directory if it doesn't exist
$dataDir = Join-Path -Path $currentDir -ChildPath "data"
if (-not (Test-Path -Path $dataDir)) {
    New-Item -Path $dataDir -ItemType Directory | Out-Null
    Write-Host "Created directory: $dataDir"
} else {
    Write-Host "Directory already exists: $dataDir"
}

# Get all the .csproj, .cs, and .xaml files from the current directory and all subdirectories
# Exclude the obj and bin directories
$files = Get-ChildItem -Path $currentDir -Include "*.csproj", "*.cs", "*.xaml" -Recurse -File | 
         Where-Object { $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\' }

# Add files from the current directory directly
$currentDirFiles = Get-ChildItem -Path $currentDir -Include "*.csproj", "*.cs", "*.xaml" -File |
                  Where-Object { $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\' }
$files = $files + $currentDirFiles

# Remove duplicates (in case the -Recurse already found the files in the current directory)
$files = $files | Select-Object -Unique

# Display the number of files found
Write-Host "Found $($files.Count) files to copy (excluding obj and bin folders)"

# Copy each file to the data directory
foreach ($file in $files) {
    # Generate a unique filename using the parent folder name and original filename
    $relativePath = $file.FullName.Substring($currentDir.Path.Length).TrimStart('\')
    $newFileName = $file.Name
    
    # If file is from a subdirectory, consider using a different name to avoid conflicts
    if ($relativePath -ne $file.Name) {
        # You can customize this naming scheme as needed
        $parentFolderName = Split-Path -Path (Split-Path -Path $file.FullName -Parent) -Leaf
        $newFileName = "$parentFolderName-$($file.Name)"
    }
    
    # Copy the file directly to the data folder
    $destinationPath = Join-Path -Path $dataDir -ChildPath $newFileName
    
    # Copy the file
    Copy-Item -Path $file.FullName -Destination $destinationPath -Force
    Write-Host "Copied: $($file.FullName) to $newFileName"
}

Write-Host "Operation completed. All .csproj, .cs, and .xaml files have been copied to $dataDir (excluding obj and bin folders)"