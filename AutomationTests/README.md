# 🧪 DemoQA Automation Framework (.NET)
This project represents a hybrid (API + UI) test framework written in C# and .NET 8.

## 🚀 Tech Stack
Component	        Technology	      Description
Language	        C# (.NET 8)	      Core development language.
BDD Framework	    Reqnroll	      BDD tool (Cucumber for .NET), successor to SpecFlow.
UI Automation	    Playwright	      The fastest and most reliable tool for browser automation.
API Automation	    RestSharp	      Client for RESTful API requests.
Test Runner	        NUnit	          Test execution engine.
Data Generation	    Bogus	          Test data generator (equivalent to JavaFaker).
Assertions	        FluentAssertions  For readable and clear assertions.
DI Container	    Reqnroll (BoDi)	  Built-in Dependency Injection container.

## 📂 Project Structure
The project follows a strict Page Object Model (POM) and layer separation:
AutomationTests
├── Api
│   ├── Data            # API Models (Request/Response POCO classes)
│   ├── Steps           # API Step Definitions
│   └── TestData        # API Data Factory
├── Ui
│   ├── Data            # UI Models (PracticeFormData with Bogus)
│   ├── Pages           # Page Objects (Locators & Actions)
│   ├── Steps           # Step Definitions (Hooks & Steps)
│   └── UiHooks.cs      # Browser Management (Setup/Teardown)
├── Common
│   ├── ConfigFactory.cs # Configuration loading (Dev/Test/Browser)
│   └── ApiConstants.cs  # Constants and Paths
├── Resources
│   └── Features        # Gherkin (.feature) files
└── appsettings.json    # Configuration (URL, Browser, etc.)

## ⚙️ Prerequisites
To run the project, you must have the following installed:
* .NET SDK 8.0 - Download here
* Visual Studio Code (with the "C# Dev Kit" extension installed).

## 📥 SetupClone the repository:

```bash
git clone <repo-url>
cd DemoQA_WebAPI_.NET/AutomationTests
```

Restore packages:
```bash
dotnet restore
```

Install Playwright browsers:
```bash
~/.dotnet/tools/playwright install
```

## ▶️ Running Tests
You can control the Environment and Browser directly via the command line.

### 🟢 Default Execution (Default: Chrome / QA Env)
```bash
dotnet test
```

### 🎭 Run UI Tests (via Tag)
```bash
dotnet test --filter "Category=UI"
```

### 🌍 Change Browser (Cross-browser Testing)
Supported browsers: chrome, firefox, webkit (Safari).
#### Firefox:
```bash
BROWSER=firefox dotnet test --filter "Category=UI"
```
#### Safari (WebKit):
```bash
BROWSER=webkit dotnet test --filter "Category=UI"
```
### 🛠 Change Environment
Configurations are loaded from appsettings.json (Default) or appsettings.dev.json (Dev).

#### Run on DEV environment:
```bash
ENV=dev dotnet test --filter "Category=UI"
```
#### Combination (Dev + Firefox):
```bash
ENV=dev BROWSER=firefox dotnet test --filter "Category=UI"
```
### 📊 Reporting (Allure)
The project is configured to generate Allure results in bin/Debug/net8.0/allure-results.

Run the tests (as shown above).

Generate and Open the Report:

```bash
allure serve bin/Debug/net8.0/allure-results
```

This will automatically open your default browser with the dashboard.
Clean old results (Optional but recommended before run):

```bash
rm -rf bin/Debug/net8.0/allure-results
```