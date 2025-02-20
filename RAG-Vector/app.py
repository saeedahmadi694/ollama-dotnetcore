from fastapi import FastAPI, UploadFile, File
from fastapi.middleware.cors import CORSMiddleware
import uvicorn
from typing import List
import pandas as pd
from PyPDF2 import PdfReader
import docx
from sqlalchemy import create_engine
from io import StringIO
import os
from dotenv import load_dotenv

app = FastAPI(
    title="Document Processing API",
    description="API for processing multiple document types and storing in PostgreSQL",
    version="1.0.0"
)

# Add CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Database connection
load_dotenv()
DATABASE_URL = os.getenv("DATABASE_URL")
engine = create_engine(DATABASE_URL)

def process_csv(file):
    content = pd.read_csv(StringIO(str(file.file.read(), 'utf-8')))
    return content.to_dict(orient='records')

def process_excel(file):
    content = pd.read_excel(file.file)
    return content.to_dict(orient='records')

def process_pdf(file):
    pdf_reader = PdfReader(file.file)
    text = ""
    for page in pdf_reader.pages:
        text += page.extract_text()
    return {"content": text}

def process_doc(file):
    doc = docx.Document(file.file)
    text = ""
    for paragraph in doc.paragraphs:
        text += paragraph.text + "\n"
    return {"content": text}

@app.post("/upload/", tags=["Documents"])
async def upload_files(files: List[UploadFile] = File(...)):
    """
    Upload multiple files (CSV, Excel, PDF, DOC) and store their content in PostgreSQL
    """
    results = []
    
    for file in files:
        file_extension = file.filename.split(".")[-1].lower()
        
        try:
            if file_extension == "csv":
                content = process_csv(file)
            elif file_extension in ["xlsx", "xls"]:
                content = process_excel(file)
            elif file_extension == "pdf":
                content = process_pdf(file)
            elif file_extension in ["doc", "docx"]:
                content = process_doc(file)
            else:
                continue

            # Store in PostgreSQL
            df = pd.DataFrame([{
                "filename": file.filename,
                "content": str(content),
                "file_type": file_extension
            }])
            
            df.to_sql("documents", engine, if_exists="append", index=False)
            results.append({"filename": file.filename, "status": "success"})
            
        except Exception as e:
            results.append({"filename": file.filename, "status": "error", "message": str(e)})
    
    return results

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000) 