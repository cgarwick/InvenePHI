Dependencies:

- Visual Studio 2022 - React.js with ASP.NET Core 8.0
- Node.js
- Run 'npm install' on the invenephi.client folder - This should take care of esline, vite, and react.

Instructions:

- Clone Git repository, Open in Visual Studio (VS 2022 recommended).
- In VS, open terminal, navigate to invenephi.client directory and run 'npm install'.
- In VS, click Start to run both the front end and back end applications together (this will automatically build and run the application).

Approach to Identifying and redacting PHI:

- I decided to create a list of PHI identifiers that I would use to scan each line of the .txt file to locate a match between the text and the PHI identifiers.
- Once matched, I replace the PHI value with [REDACTED] per the requirements.
- I accomplished this by first uploading each file. After a successful upload, I would read each line of the file and invoke the FileRedact method to identify and redact the PHI.

Assumptions and improvements:

- One improvement would be to create a patient object with the values extracted from the .txt file and store the patient in a database. (I included an example patient class in the project).
- One limitation was time, I felt this was the best approach given only 4 hours, and I feel like I was able to meet the requirements in the time allotted.
- The assumptions I made were that the .txt files would all be the same template/structure (ex: Label: Value) and that each file would represent a single patient.
