# ============================================
# init-lfs.ps1
# Initializes Git LFS for a Unity 6000.4.0f1
# VR rhythm game project.
#
# Usage:
#   1. cd into your project root
#   2. powershell -ExecutionPolicy Bypass -File init-lfs.ps1
# ============================================

$ErrorActionPreference = "Stop"

function Log($msg)   { Write-Host "[OK]    $msg" -ForegroundColor Green }
function Warn($msg)  { Write-Host "[WARN]  $msg" -ForegroundColor Yellow }
function Fail($msg)  { Write-Host "[ERROR] $msg" -ForegroundColor Red; exit 1 }

# ---------------------------
# Preflight checks
# ---------------------------

if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Fail "git is not installed. Download from https://git-scm.com"
}

if (-not (Get-Command git-lfs -ErrorAction SilentlyContinue)) {
    Fail "git-lfs is not installed. Download from https://git-lfs.com"
}

if (-not (Test-Path ".git")) {
    Warn "No .git directory found. Initializing a new repo."
    git init
    Log "Git repo initialized."
}

# ---------------------------
# Install LFS hooks
# ---------------------------

git lfs install
Log "Git LFS hooks installed."

# ---------------------------
# Create .gitattributes
# ---------------------------

$gitattributes = @'
# ============================================
# Git LFS tracking rules
# Unity 6000.4.0f1 - Futuristic Cavemen VR
# ============================================

# --- Audio ---
*.wav filter=lfs diff=lfs merge=lfs -text
*.mp3 filter=lfs diff=lfs merge=lfs -text
*.ogg filter=lfs diff=lfs merge=lfs -text
*.aif filter=lfs diff=lfs merge=lfs -text
*.aiff filter=lfs diff=lfs merge=lfs -text
*.flac filter=lfs diff=lfs merge=lfs -text

# --- 3D Models ---
*.fbx filter=lfs diff=lfs merge=lfs -text
*.obj filter=lfs diff=lfs merge=lfs -text
*.blend filter=lfs diff=lfs merge=lfs -text
*.gltf filter=lfs diff=lfs merge=lfs -text
*.glb filter=lfs diff=lfs merge=lfs -text
*.dae filter=lfs diff=lfs merge=lfs -text
*.3ds filter=lfs diff=lfs merge=lfs -text
*.max filter=lfs diff=lfs merge=lfs -text
*.ma filter=lfs diff=lfs merge=lfs -text
*.mb filter=lfs diff=lfs merge=lfs -text

# --- Textures ---
*.psd filter=lfs diff=lfs merge=lfs -text
*.tga filter=lfs diff=lfs merge=lfs -text
*.tif filter=lfs diff=lfs merge=lfs -text
*.tiff filter=lfs diff=lfs merge=lfs -text
*.png filter=lfs diff=lfs merge=lfs -text
*.jpg filter=lfs diff=lfs merge=lfs -text
*.jpeg filter=lfs diff=lfs merge=lfs -text
*.exr filter=lfs diff=lfs merge=lfs -text
*.hdr filter=lfs diff=lfs merge=lfs -text
*.bmp filter=lfs diff=lfs merge=lfs -text
*.gif filter=lfs diff=lfs merge=lfs -text
*.svg filter=lfs diff=lfs merge=lfs -text

# --- Video ---
*.mp4 filter=lfs diff=lfs merge=lfs -text
*.mov filter=lfs diff=lfs merge=lfs -text
*.webm filter=lfs diff=lfs merge=lfs -text
*.avi filter=lfs diff=lfs merge=lfs -text

# --- Fonts ---
*.ttf filter=lfs diff=lfs merge=lfs -text
*.otf filter=lfs diff=lfs merge=lfs -text
*.woff filter=lfs diff=lfs merge=lfs -text
*.woff2 filter=lfs diff=lfs merge=lfs -text

# --- Unity-specific binaries ---
*.unitypackage filter=lfs diff=lfs merge=lfs -text
*.asset filter=lfs diff=lfs merge=lfs -text
*.cubemap filter=lfs diff=lfs merge=lfs -text
*.prefab filter=lfs diff=lfs merge=lfs -text
*.lighting filter=lfs diff=lfs merge=lfs -text
*.terrainlayer filter=lfs diff=lfs merge=lfs -text
*.physicMaterial filter=lfs diff=lfs merge=lfs -text
*.controller filter=lfs diff=lfs merge=lfs -text
*.overrideController filter=lfs diff=lfs merge=lfs -text
*.mask filter=lfs diff=lfs merge=lfs -text
*.anim filter=lfs diff=lfs merge=lfs -text

# --- Compiled / build artifacts ---
*.dll filter=lfs diff=lfs merge=lfs -text
*.so filter=lfs diff=lfs merge=lfs -text
*.a filter=lfs diff=lfs merge=lfs -text
*.dylib filter=lfs diff=lfs merge=lfs -text
*.apk filter=lfs diff=lfs merge=lfs -text
*.aab filter=lfs diff=lfs merge=lfs -text

# --- Archives ---
*.zip filter=lfs diff=lfs merge=lfs -text
*.7z filter=lfs diff=lfs merge=lfs -text
*.gz filter=lfs diff=lfs merge=lfs -text
*.tar filter=lfs diff=lfs merge=lfs -text
*.rar filter=lfs diff=lfs merge=lfs -text

# --- Substance / Shader graphs ---
*.sbsar filter=lfs diff=lfs merge=lfs -text
*.spp filter=lfs diff=lfs merge=lfs -text
*.shadergraph filter=lfs diff=lfs merge=lfs -text
*.shadersubgraph filter=lfs diff=lfs merge=lfs -text

# --- Lightmap data ---
*.exr filter=lfs diff=lfs merge=lfs -text

# --- Ensure text files stay text ---
*.cs text diff=csharp
*.shader text
*.cginc text
*.hlsl text
*.glsl text
*.json text
*.yaml text
*.yml text
*.xml text
*.md text
*.txt text
*.asmdef text
*.asmref text
*.inputactions text
'@

# Write with UTF-8 no BOM and LF line endings
$utf8NoBom = New-Object System.Text.UTF8Encoding $false
[System.IO.File]::WriteAllText(
    (Join-Path $PWD ".gitattributes"),
    $gitattributes.Replace("`r`n", "`n"),
    $utf8NoBom
)

Log "Created .gitattributes with LFS tracking rules."

# ---------------------------
# Stage .gitattributes
# ---------------------------

git add .gitattributes
Log "Staged .gitattributes."

# ---------------------------
# Summary
# ---------------------------

Write-Host ""
Write-Host "============================================"
Write-Host " Git LFS initialized."
Write-Host "============================================"
Write-Host ""
Write-Host " Tracked file types:"
Write-Host "   Audio:    wav, mp3, ogg, aif, flac"
Write-Host "   Models:   fbx, obj, blend, gltf, glb"
Write-Host "   Textures: psd, tga, tif, png, jpg, exr, hdr"
Write-Host "   Video:    mp4, mov, webm"
Write-Host "   Fonts:    ttf, otf"
Write-Host "   Unity:    unitypackage, asset, prefab, anim, controller"
Write-Host "   Shaders:  sbsar, shadergraph"
Write-Host "   Builds:   dll, so, apk, aab"
Write-Host ""
Write-Host " Next steps:"
Write-Host "   1. Copy .gitignore into the project root (if not already there)"
Write-Host "   2. git add -A"
Write-Host '   3. git commit -m "Initial commit with LFS"'
Write-Host "   4. git remote add origin <your-repo-url>"
Write-Host "   5. git push -u origin main"
Write-Host ""
