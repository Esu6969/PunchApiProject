# Security Policy

## Reporting a Vulnerability

The security of PunchApiProject is taken seriously. If you discover a security vulnerability, please follow responsible disclosure practices.

### How to Report

**Please DO NOT create a public GitHub issue for security vulnerabilities.**

Instead, please report security issues by:
- **Email**: Create a private security advisory on GitHub (preferred)
- Or contact the repository owner directly through GitHub

### What to Include

When reporting a vulnerability, please include:
- **Description**: Clear description of the vulnerability
- **Impact**: Potential security impact and severity
- **Steps to Reproduce**: Detailed steps to reproduce the issue
- **Affected Versions**: Which versions are affected
- **Suggested Fix**: If you have suggestions for fixing it (optional)
- **Your Contact**: How we can reach you for follow-up

### What to Expect

- **Acknowledgment**: We aim to acknowledge receipt within 48 hours
- **Updates**: Regular updates on the progress of fixing the issue
- **Credit**: Credit in security advisories (if desired)
- **Timeline**: We aim to patch critical vulnerabilities within 7 days

## Supported Versions

We currently support the following versions with security updates:

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |
| < 1.0   | :x:                |

## Security Best Practices

When using this application:

### For Developers
- Never commit sensitive data (passwords, API keys, connection strings)
- Use strong, unique passwords for all accounts
- Keep dependencies up to date
- Review code for security issues before deployment
- Use environment variables for sensitive configuration

### For Deployment
- Use HTTPS in production
- Keep PostgreSQL credentials secure
- Rotate JWT secret keys regularly
- Use strong JWT secret keys (minimum 32 characters)
- Enable CORS only for trusted origins
- Keep database access restricted
- Regular security audits of dependencies
- Use prepared statements to prevent SQL injection

### For Users
- Use strong passwords for employee accounts
- Change default credentials immediately
- Keep your deployment environment updated
- Monitor access logs regularly

## Known Security Considerations

This project includes:
- ✅ Password hashing using secure algorithms
- ✅ JWT-based authentication
- ✅ Input validation
- ✅ CORS configuration
- ✅ Parameterized database queries (Entity Framework)

Areas for improvement:
- ⚠️ Rate limiting not yet implemented
- ⚠️ Advanced JWT features (refresh tokens) not fully implemented
- ⚠️ No two-factor authentication (2FA) yet

## Security Updates

Security updates will be:
- Released as soon as possible
- Documented in release notes
- Announced in the repository

## Disclosure Policy

- Security vulnerabilities will be disclosed responsibly
- Fixes will be released before public disclosure
- Credit will be given to researchers who report vulnerabilities responsibly

## Questions?

If you have questions about security that don't involve reporting a vulnerability:
- Create a GitHub issue with the "security question" label
- Check existing documentation first

---

Thank you for helping keep PunchApiProject secure! 🔒