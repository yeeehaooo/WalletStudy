# WalletStudy
	是一個基於 .NET 8 的程式庫，提供與 Google Wallet 整合的功能，支援處理航班票券（Flight Passes）等操作。此程式庫包含多個模組化的服務，並使用依賴注入 (Dependency Injection) 進行管理，方便擴展與維護。  

## Apple 電子錢包
## Google 電子錢包  
#### 功能特性  
1. Google Wallet Flight 整合：  
    > 支援 Google Wallet Flight API 的操作，包括建立、更新、部分更新與訊息新增。 
    >```csharp
    >googleWalletService.FlightWallet.GetClassAsync(classId);
    >googleWalletService.FlightWallet.InsertClassAsync(flightClass);
    >googleWalletService.FlightWallet.UpdateClassAsync(flightClass);
    >googleWalletService.FlightWallet.PatchClassAsync(flightClass);
    >
    >googleWalletService.FlightWallet.GetObjectAsync(objecId);
    >googleWalletService.FlightWallet.InsertObjectAsync(flightObject);
    >googleWalletService.FlightWallet.UpdateObjectAsync(flightObject);
    >googleWalletService.FlightWallet.PatchObjectAsync(flightObject);
2. 航班票券管理：  
    > 提供 FlightClass 和 FlightObject 的操作功能。 
    >```csharp
    >ClassRepository.GetAsync(classId);
    >ClassRepository.InsertAsync(flightClass);
    >ClassRepository.UpdateAsync(flightClass);
    >ClassRepository.PatchAsync(flightClass);
    >
    >ObjectRepository.GetAsync(objectId);
    >ObjectRepository.InsertAsync(flightObject);
    >ObjectRepository.UpdateAsync(flightObject);
    >ObjectRepository.PatchAsync(flightObject); 
3. 多公司支援：  
    > 支援基於 CompanyCode 的多公司配置與服務隔離。  
    >```json
    >{
    >  "GoogleWalletSettings": {  
    >    "CompanySettings": {  
    >      "CompanyA": {  
    >        "ServiceAccountJsonPath": "path/to/service-account.json"  
    >      }  
    >    }  
    >  }  
    >}
4. 依賴注入：  
    > 使用 .NET 的 DI 容器進行服務註冊與管理，支援鍵值服務 (AddKeyedSingleton)。  
    >```csharp
    >services.AddGoogleWalletServices(builder.Configuration);