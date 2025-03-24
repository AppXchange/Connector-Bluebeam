# Bluebeam Connector

The Bluebeam connector for App Xchange enables integration with Bluebeam Studio's API to automate workflows around Studio Sessions and receive notifications about changes in Studio Projects and Sessions.

## Overview

This connector provides functionality to:

1. Create and manage Studio Sessions
2. Upload and manage files within Sessions
3. Manage Session users and permissions
4. Create snapshots of marked-up files
5. Subscribe to notifications for changes in Studio Projects and Sessions

## Modules

### 1. Studio Sessions Module

The Sessions module (`sessions-1`) provides capabilities for managing the complete lifecycle of Studio Sessions:

#### Session Management
- Initialize new Studio Sessions with customizable settings
- Configure session permissions and access controls
- Set session end dates and notification preferences
- Delete/close sessions when complete

#### File Management  
- Upload PDF files to sessions using a multi-step process:
  1. Create metadata block
  2. Upload file to AWS storage
  3. Confirm upload completion
- Create snapshots of marked-up files
- Download finalized files with markups

#### User Management
- Invite users to sessions via email
- Add existing Studio users directly
- Configure user permissions

### 2. Event Notifications Module

The Notifications module (`notifications-1`) enables subscription to various events in Studio:

#### Subscription Types
- Studio Project changes
- File/folder changes within Projects
- Session status changes
- Document changes within Sessions

#### Subscription Management
- List existing subscriptions
- Create new subscriptions
- Create comprehensive subscriptions that monitor all changes
- Delete/unsubscribe from notifications

## Authentication

The connector uses OAuth 2.0 Code Flow authentication to securely connect to the Bluebeam Studio API.

## Getting Started

1. Configure the connector with your Bluebeam Studio API credentials
2. Set up OAuth authentication
3. Create a Studio Session
4. Upload files and invite users
5. Set up any desired notifications

## Common Use Cases

1. **Automated Session Creation**
   - Create Sessions programmatically
   - Upload relevant files
   - Invite required participants

2. **Document Processing**
   - Upload documents to Sessions
   - Create snapshots of marked-up files
   - Download finalized documents

3. **Change Monitoring**
   - Subscribe to Project changes
   - Monitor Session status
   - Track document modifications

4. **User Management**
   - Automate user invitations
   - Manage permissions
   - Control access to Sessions

## API Endpoints

The connector interacts with the following Bluebeam API endpoints:

- Base URL: `https://studioapi.bluebeam.com/publicapi/v1`
- Sessions: `/sessions`
- Files: `/sessions/{sessionId}/files`
- Users: `/sessions/{sessionId}/users`
- Notifications: `/notifications/subscriptions`

## Additional Resources

- [Bluebeam Studio API Documentation](https://bbdn.bluebeam.com)
- [App Xchange Documentation](https://docs.appxchange.trimble.com)

## Support

For technical support or questions about this connector, please contact:
- App Xchange Support: xchange_build@trimble.com