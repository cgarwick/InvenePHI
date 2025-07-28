import { useState, useRef } from 'react';
import './App.css';

function App() {
    const [isSubmitted, setIsSubmitted] = useState(false);
    const fileUploadRef = useRef([]);
    let errorMessage = "";

    const fileChange = (event) => {

        // Access the FileList object
        const fileList = event.target.files;

        // Check if one or more files were selected
        if (fileList.length > 0) {
            // Loop through files
            for (let i = 0; i < fileList.length; i++) {
                // Check File type
                if (fileList[i].type != "text/plain") {
                    errorMessage = "Please select only .txt file type.";
                    // Reset file input since non .txt file(s) were selected
                    event.target.value = "";
                    alert(errorMessage);
                    event.target.reset();
                }
            }

        } else {
            errorMessage = "Please select at least one .txt file.";
            alert(errorMessage);
        }
    };

    const submitClick = (event) => {
        event.preventDefault();

        // Access selected upload files
        const fileList = fileUploadRef.current.files;

        // Check if one or more files are selected
        if (fileList.length == 0) {
            errorMessage = "Please select at least one .txt file.";
            alert(errorMessage);
        } else {
            // Pass files to uploadPHIdocs
            uploadPHIdocs(fileList);
        }
    };

    return (
        <div id="container-phi-upload">
            <h3>PHI FILE UPLOAD</h3>
            {isSubmitted ? (
                <p>Your files have been processed. Please check the Uploads folder in the root directory of this project.</p>
            ) : (
                <>
                    <p>Upload one or more text(.txt) files.</p>
                    <form onSubmit={submitClick}>
                        <input id="phi-upload" type="file" placeholder="PHI Upload" accept=".txt" ref={fileUploadRef} onChange={fileChange} multiple></input>
                        <button id="phi-upload-submit" type="submit">Submit PHI</button>
                    </form>
                </>
            )}
            </div>
    );
    
    async function uploadPHIdocs(fileList) {

        const fd = new FormData();

        if (fileList.length > 0) {
            // Loop through files
            for (let i = 0; i < fileList.length; i++) {
                // Add files to formdata
                fd.append('files', fileList[i]);
            }
        } 

        // Send files to API for processing
        const response = await fetch('api/patient', {
            method: 'POST',
            body: fd
        });

        if (response.ok) {
            setIsSubmitted(true);
        } else {
            //errorMessage = "There seems to be a problem with your upload, please try again.";
            response.text().then(responseText => { alert(responseText) });
        }
    }
}

export default App;