OpenAI Integration Web API

This ASP.NET Core Web API project is designed to integrate with OpenAI's services, providing a simple interface to interact with AI models. 
It includes endpoints for submitting questions to the OpenAI API and receiving responses.

Features
Integration with OpenAI API using HigLabo.OpenAI.
Basic API endpoints for submitting questions and receiving AI-generated answers.
Configurable API and Assistant IDs for OpenAI interaction.
Getting Started
Prerequisites
.NET Core 3.1 SDK
Any IDE that supports .NET development (e.g., Visual Studio, Visual Studio Code)
An OpenAI API key (for accessing OpenAI services)
Installation
Clone the repository or download the source code.
Open the solution in your IDE.
Restore NuGet packages.
Ensure you have an OpenAI API key and set it in the appsettings.json file.
Configuration
Update the appsettings.json file with your OpenAI API key and Assistant ID:

json
Copy code
{
  "apiKey": "<your_openai_api_key>",
  "assistantId": "<your_openai_assistant_id>"
}
Running the Application
Build the solution.
Run the application. This can typically be done by running dotnet run in the project directory or using the IDE's run feature.
The API will be hosted locally, and you can use tools like Postman or Swagger (if enabled) to test the API endpoints.
Usage
The API currently supports the following endpoints:

GET /api/Home: Returns a test response.
POST /api/Home: Accepts a question in the request body and returns the AI-generated answer.
Example of a POST request body:

json
Copy code
{
  "question": "What is the capital of France?"
}
Contributing
Contributions to this project are welcome. Please fork the repository and submit a pull request with your changes.

Acknowledgments
HigLabo.OpenAI for providing the OpenAI API wrapper.
ASP.NET Core team for the Web API framework.
