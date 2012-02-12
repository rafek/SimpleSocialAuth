Simple Social Auth
==================

## About ##

Simple Social Auth is a project that enables easy OAuth support addition to any ASP.NET MVC3 projects that uses Forms Authentication. It ships as a [NuGet package](https://nuget.org/packages/SimpleSocialAuth.MVC3). It's based on proven and well-known solution: DotNetOpenAuth.

## How to install ##

It's almost as easy as running the command:

`Install-Package SimpleSocialAuth.MVC3 -Pre`

The following actions are then performed:

1.  Required assemblies are added to the project
2.  SimpleAuthController is added to the controllers folder
3.  LogIn.cshtml view (Razor) is added to the Views/SimpleAuth folder
4.  Forms authentication `loginUrl` is changed to `~/SimpleAuth/LogIn`
5.  New `appSettings` keys are added to store appropriate keys and secrets

## Contributions ##

Any contribution is welcomed.