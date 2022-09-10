# Azure AD Login

Expose Azure AD Login as COM DLL accessible from VB 6 and VBA

Example:
```vb 6

    Dim login As New TVMAzureAd.AzureLogin
    Dim sucess as Boolean
    Dim token As String
    Dim userName As String
    Dim errorMessage As String

    sucess = login.login("974e60ee-7add-4f7e-a3b7-1f9dfd4e32b7", "common", "https://login.microsoftonline.com/")

    If success Then
        token = login.AccessToken
        userName = login.userName
    Else
        errorMessage = login.errorMessage
    End If
```