#!/usr/bin/env bash
# ============================================
# init-lfs.sh
# Initializes Git LFS for a Unity 6000.4.0f1
# VR rhythm game project.
#
# Usage:
#   1. cd into your project root
#   2. chmod +x init-lfs.sh
#   3. ./init-lfs.sh
# ============================================

set -euo pipefail

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

log()   { echo -e "${GREEN}[OK]${NC} $1"; }
warn()  { echo -e "${YELLOW}[WARN]${NC} $1"; }
error() { echo -e "${RED}[ERROR]${NC} $1"; exit 1; }

# ---------------------------
# Preflight checks
# ---------------------------

if ! command -v git &> /dev/null; then
    error "git is not installed. Install it first."
fi

if ! command -v git-lfs &> /dev/null; then
    error "git-lfs is not installed.\n  macOS:   brew install git-lfs\n  Windows: https://git-lfs.com\n  Linux:   sudo apt install git-lfs"
fi

if [ ! -d ".git" ]; then
    warn "No .git directory found. Initializing a new repo."
    git init
    log "Git repo initialized."
fi

# ---------------------------
# Install LFS hooks
# ---------------------------

git lfs install
log "Git LFS hooks installed."

# ---------------------------
# Create .gitattributes
# ---------------------------

cat > .gitattributes << 'EOF'
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
EOF

log "Created .gitattributes with LFS tracking rules."

# ---------------------------
# Track existing files
# ---------------------------

git add .gitattributes
log "Staged .gitattributes."

# ---------------------------
# Summary
# ---------------------------

echo ""
echo "============================================"
echo " Git LFS initialized."
echo "============================================"
echo ""
echo " Tracked file types:"
echo "   Audio:    wav, mp3, ogg, aif, flac"
echo "   Models:   fbx, obj, blend, gltf, glb"
echo "   Textures: psd, tga, tif, png, jpg, exr, hdr"
echo "   Video:    mp4, mov, webm"
echo "   Fonts:    ttf, otf"
echo "   Unity:    unitypackage, asset, prefab, anim, controller"
echo "   Shaders:  sbsar, shadergraph"
echo "   Builds:   dll, so, apk, aab"
echo ""
echo " Next steps:"
echo "   1. Copy .gitignore into the project root (if not already there)"
echo "   2. git add -A"
echo "   3. git commit -m \"Initial commit with LFS\""
echo "   4. git remote add origin <your-repo-url>"
echo "   5. git push -u origin main"
echo ""
