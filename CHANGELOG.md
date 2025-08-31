# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]

### Changed
- **BREAKING**: Deployment now uses version tags instead of pushing to main branch
- GitHub Actions workflows now trigger on version tags (v*.*.*, frontend-v*.*.*, backend-v*.*.*)
- Added interactive deployment script (`deploy.sh`) for easy version management

### Added
- New `deploy.sh` script for tag-based deployments
- Support for separate frontend and backend deployments using specific tags
- Semantic versioning support (v1.0.0, v2.1.3, etc.)
- Automated deployment validation and tag conflict detection

### Removed
- Automatic deployment on main branch pushes
- Path-based deployment triggers
- Old setup-deployment.sh script

## Migration Guide

If you were using the old deployment method:

1. **Before**: Changes deployed automatically when pushing to main
   ```bash
   git push origin main  # ❌ No longer triggers deployment
   ```

2. **After**: Create version tags to deploy
   ```bash
   git push origin main    # ✅ Just pushes code changes
   ./deploy.sh            # ✅ Interactive deployment with versioning
   # OR
   git tag -a v1.0.0 -m "Release v1.0.0"
   git push origin v1.0.0  # ✅ Triggers deployment
   ```

## Benefits of Tag-Based Deployment

- ✅ **Controlled Releases**: Deploy only when ready, not on every commit
- ✅ **Version Tracking**: Clear version history with semantic versioning
- ✅ **Selective Deployment**: Deploy frontend, backend, or both independently
- ✅ **Rollback Support**: Easy to identify and revert to previous versions
- ✅ **Production Safety**: No accidental deployments from development commits
