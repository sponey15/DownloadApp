# DownloadApp

The purpose of this app is to download xml file from server wich have oauth authentication and next select resources paths informations.

## Installation

```bash
In API folder:
You can use IIS server provided with VisualStudio 2022

Or you can choose to use commands:
dotnet restore -> to install dependencies
dotnet watch run -> run api server
```
Please remember to double check address of localhost and make changes in client [download.service.ts](https://github.com/sponey15/DownloadApp/blob/main/client/src/app/_services/download.service.ts#L10) file
```bash
In Client folder:
npm update -> installs all dependencies used at project
ng serve -> run angular
```

## Built With

* [.NET 6.0](https://dotnet.microsoft.com/) - The web framework used for backend 
* [Angular 15](https://angular.io/) - Web developer platform for frontend

## Presentation

![1](https://raw.githubusercontent.com/sponey15/DownloadApp/master/client/src/assets/readme1.png)

![1](https://raw.githubusercontent.com/sponey15/DownloadApp/master/client/src/assets/readme2.png)
