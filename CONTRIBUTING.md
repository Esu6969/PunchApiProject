# Contributing to PunchApiProject

Thank you for your interest in contributing to the Employee Punch API Project! This document provides guidelines for contributing to this project.

## How to Contribute

### Reporting Bugs

If you find a bug, please create an issue on GitHub with:
- A clear, descriptive title
- Steps to reproduce the issue
- Expected behavior vs actual behavior
- Screenshots (if applicable)
- Your environment (OS, .NET version, PostgreSQL version)

### Suggesting Enhancements

We welcome feature suggestions! Please create an issue with:
- A clear description of the feature
- Why this feature would be useful
- Possible implementation approach (optional)

### Pull Request Process

1. **Fork the Repository**
   ```bash
   git clone https://github.com/Esu6969/PunchApiProject.git
   cd PunchApiProject
   ```

2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/YourFeatureName
   # or
   git checkout -b fix/YourBugFix
   ```

3. **Make Your Changes**
   - Write clean, readable code
   - Follow the existing code style
   - Add comments for complex logic
   - Test your changes thoroughly

4. **Commit Your Changes**
   ```bash
   git add .
   git commit -m "Add: Brief description of your changes"
   ```
   
   Use clear commit messages:
   - `Add:` for new features
   - `Fix:` for bug fixes
   - `Update:` for improvements
   - `Refactor:` for code restructuring

5. **Push to Your Fork**
   ```bash
   git push origin feature/YourFeatureName
   ```

6. **Open a Pull Request**
   - Go to the original repository
   - Click "New Pull Request"
   - Describe your changes clearly
   - Reference any related issues

## Code Style Guidelines

### C# Backend
- Follow standard C# naming conventions
- Use PascalCase for classes, methods, and properties
- Use camelCase for local variables and parameters
- Add XML comments for public methods
- Keep methods focused and concise

### React Frontend
- Use functional components with hooks
- Follow consistent naming for components (PascalCase)
- Use meaningful variable names
- Keep components small and reusable

### General
- Write self-documenting code
- Add comments only when necessary to explain "why", not "what"
- Ensure proper error handling
- Don't commit sensitive data (passwords, API keys, etc.)

## Database Changes

If your contribution involves database changes:
- Create appropriate Entity Framework migrations
- Test migrations both up and down
- Document schema changes in the PR description

## Testing

Before submitting a PR:
- Test all affected functionality
- Ensure the application builds without errors
- Test both frontend and backend if both are affected
- Check that no sensitive data is exposed

## Configuration

- Never commit real configuration files (`appsettings.json`)
- Update `appsettings.example.json` if you add new configuration options
- Document any new environment variables or settings

## Questions?

If you have questions about contributing:
- Check existing issues and pull requests
- Create a new issue with the "question" label
- Be respectful and patient

## Code of Conduct

- Be respectful and inclusive
- Welcome newcomers and help them learn
- Focus on constructive feedback
- Keep discussions professional

## Recognition

Contributors will be recognized in:
- The project's README
- Release notes (for significant contributions)

---

Thank you for contributing to PunchApiProject! 🚀