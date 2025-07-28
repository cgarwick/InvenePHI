using Microsoft.AspNetCore.Mvc;
using InvenePHI.Server.Models;
using System.Text.RegularExpressions;

namespace InvenePHI.Server.Services
{
    public class FileManager : IFileManager
    {
        public (bool isSuccess, string message) FileUpload(List<IFormFile> files)
        {
            bool isSuccess = false;
            string message = "";

            // Check if files are null or empty
            if (files == null || files.Count == 0)
            {
                message = "No files selected for upload.";
                isSuccess = false;
            }
            else
            {
                // Add try/catch error handling
                try
                {   // Ensure the Uploads folder exists
                    if (!Directory.Exists("Uploads"))
                    {
                        Directory.CreateDirectory("Uploads");
                    }
                
                    // Loop through each file in the list
                    foreach (var file in files)
                    {
                        // Save each file to Uploads folder
                        var filePath = Path.Combine("Uploads", file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        FileRedact(file);
                    }

                }
                catch (Exception ex)
                {
                    message = $"There seems to be a problem with your upload: " + ex.Message;
                    isSuccess = false;
                }

                message = "Files Uploaded Successfully!";
                isSuccess = true;
            }

            return (isSuccess, message);
        }

        public string FileRedact(IFormFile file)
        {
            var redactedContent = "";

            // Read File
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                // Read the content of the file line by line
                string line;

                while ((line = stream.ReadLine()) != null)
                {
                    // Search file for PHI and redact it
                    redactedContent += IdentifyPHI(line);
                }
            }

            // Save the redacted content to a new file
            var redactedFilePath = Path.Combine("Uploads", $"{Path.GetFileNameWithoutExtension(file.FileName)}_sanitized.txt");
            File.WriteAllTextAsync(redactedFilePath, redactedContent);

            return "Success!";
        }

        public string IdentifyPHI(string line)
        {
            // PHI identification logic
            // This method analysizes the text line to identify PHI label and value

            // Create list of PHI Identifiers
            List<string> phiLabels = new List<string>
            {
                "Patient Name",
                "Date of Birth",
                "Social Security Number",
                "Address",
                "Phone Number",
                "Email",
                "Medical Record Number",
                "Order Details"
            };

            // Check if the line contains any PHI label
            foreach (var label in phiLabels)
            {
                // Use regular expression to identify the label
                // \b is a word boundary, ensuring we match the whole word not just a substring of a larger word
                // @ is used to disable escape sequences, so we don't need to escape backslashes
                // $ is used for string interpolation
                var phiLabel = new Regex($@"\b{label}\b", RegexOptions.IgnoreCase);

                // Check if line contains PHI
                //if (line.Contains(label, StringComparison.OrdinalIgnoreCase))
                if (phiLabel.IsMatch(line))
                {
                    // Get the PHI value
                    var phiValue = line.Substring(line.IndexOf(':') + 1).Trim();

                    // If the value is empty, return the label with a newline
                    if (phiValue.Length == 0)
                    {
                        return $"{label}: \n";
                    }
                    else
                    {
                        return $"{label}: [REDACTED] \n";
                    }
                }
                else
                {
                    // If line contains Order Details, redact the entire line after -
                    if (line.Contains('-') && !line.Contains(':'))
                    {
                        return "- [REDACTED] \n";
                    }
                }
            }

            return line;
        }
    }
}
