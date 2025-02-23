-- Must be run as superuser
SET session_replication_role = 'replica';

-- Enable vector extension (requires superuser)
CREATE EXTENSION IF NOT EXISTS vector;

-- Create Documents table
CREATE TABLE IF NOT EXISTS "Documents" (
    "id" uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    "content" text NOT NULL,
    "title" text,
    "embedding" vector(1536),
    "metadata" jsonb,
    "created_at" timestamp DEFAULT CURRENT_TIMESTAMP
);

-- Create index for vector similarity search
CREATE INDEX IF NOT EXISTS documents_embedding_idx ON "Documents" 
USING ivfflat (embedding vector_cosine_ops)
WITH (lists = 100);

SET session_replication_role = 'origin'; 