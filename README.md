# Sitecore.FederatedAuth.ADFS
Example of the implementation: Sitecore Identity plug-in for authentication through ADFS, appropriate configuration for CM instance and claims with ADFS groups configured to be mapped to corresponding Sitecore roles (names should match)

Following ADFS rules can be used on in ADFS configuration in order to map groups with domain name appropriately:

## First rule: mapping whole membership

```
c:[Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname", Issuer == "AD AUTHORITY"]
 => add(store = "Active Directory", types = ("http://schemas.xmlsoap.org/claims/TempGroup"), query = ";tokenGroups;{0}", param = c.Value);
```

## Second rule: adding domain name

**Note**: use your value instead of ```domainname```

```
c:[Type == "http://schemas.xmlsoap.org/claims/TempGroup", Value =~ "^prefix"]
 => issue(Type = "http://schemas.xmlsoap.org/claims/Group", Value = "domainname\" + c.Value);
```
