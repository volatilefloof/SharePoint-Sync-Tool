# TAMU-CLBA SharePoint Sync Tool

## Synopsis
This is a Windows-based application written in C# to assist end-users in seamlessly synchronizing SharePoint document libraries to their computer

This tool was created as part of a full-scale digital transformation initiative, in which I led the architecture and execution of a complete migration to Microsoft 365 cloud services for my unit.

## Outcome:
To allow a seamless end-user experience mapping document libraries to their local computer.

## Technologies used:

1. C# (.NET Windows Form)
2. SharePoint Client Side Object Model (CSOM)
3. Webview 2 Runtime Engine for Sign-In Functinality

## Further insight:
The program uses the following Entra API roles to function:
1. Sites.Read.All (delegated)

The role itself is acquired through a static token using https://graph.microsoft.com/.default

As noted above, the role itself acts under the context of the currently signed-in user. 
This aligns with the principle of least privilege by prohibiting access to view or add folders under a site collection the signed-in user does not explicitiy have access too.

The program takes advantage of the fact that SharePoint Online uses odopen:// reference URLs when syncing document libraries to a computer.

In essence, the program's approach is implemented in the SharePointHelper class that centralizes all relevant SharePoint operations using CSOM. 

The ODOPEN link is explicitly constructed in the BuildOdopenUrl method under the class. The method itself takes several input parameters and uses string interpolation to construct a valid URL.

An example URL is of the form: odopen://sync/?siteId=escaped-guid&webId=escaped-guid&listId=escaped-guid&userEmail=escaped-email&webUrl=escaped-url&listTitle=escaped-title&webTitle=escaped-title&tenantName=Texas%20A%26M%20University.

The required input parameters are fetched asynchronously and listed here for clarity: 

1. siteID: From SharePointHelper.GetSiteMetadataAsync(context) -> site.Id (loaded via CSOM context.Load(site, s => s.Id))
2. webID: From same -> web.Id (loaded via context.Load(web, w => w.Id))
3. listId: From selected FolderItem.Id (populated in GetDocumentLibrariesAsync via l => l.Id)
4. webUrl: From same -> web.Url (loaded via context.Load(web, w => w.Url))
5. userEmail: From authentication (_userEmail, derived from _authResult.Account.Username)
6. listTitle: From selected FolderItem.Title (populated via l => l.Title in GetDocumentLibrariesAsync)
7. webTitle: From same -> web.Title (loaded via context.Load(web, w => w.Title))

## Final Note:

Onedrive Shortcuts were not accounted for, purposely, in an effort to not confuse the end-user. 
The goal in mind was a simplistic interface that would save both the end-users and admins time when it came to adding folders to a local computer.

Any questions should be redirected to cnotzon98@tamu.edu

