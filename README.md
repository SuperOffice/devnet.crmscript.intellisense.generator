# DevNet CRMScript Intellisense Generator

Demonstrates how to generate CRMScript intellisense file from CRMScript reference documentation.

## Getting started

1. Fork or clone this repo

    ```git
    git clone https://github.com/SuperOffice/devnet.crmscript.intellisense.generator.git
    ```

1. Fork or clone the [superoffice-docs](https://github.com/SuperOfficeDocs/superoffice-docs) repo

    ```git
    git clone https://github.com/SuperOfficeDocs/superoffice-docs.git
    ```

1. Enter the intellisense generator directory and run `dotnet build`

1. Navigate into the build output directory and execute the application.

    ```
    .\devnet.crmscript.intellisense.generator.exe "destination file path" "source folder path"
    ```

    Example:

    ```
    .\devnet.crmscript.intellisense.generator "C:\\temp\\ejscriptIntellisense.js" "C:\git\superoffice-docs\docs\automation\crmscript\reference"
    ```
