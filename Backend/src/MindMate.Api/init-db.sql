-- Initialize the MindMate database structure for SQLite
-- This script will be used on first deployment to Render

-- Users table
CREATE TABLE IF NOT EXISTS Users (
    Id TEXT PRIMARY KEY,
    Username TEXT NOT NULL UNIQUE,
    Email TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    FirstName TEXT NULL,
    LastName TEXT NULL,
    DateCreated TEXT NOT NULL,
    LastLogin TEXT NULL
);

-- JournalEntries table
CREATE TABLE IF NOT EXISTS JournalEntries (
    Id TEXT PRIMARY KEY,
    UserId TEXT NOT NULL,
    EntryText TEXT NOT NULL,
    MoodRating INTEGER NOT NULL,
    Sentiment INTEGER NOT NULL,
    IsPrivate INTEGER NOT NULL DEFAULT 0,
    DateCreated TEXT NOT NULL,
    DateModified TEXT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Tags table (stored as TEXT JSON in SQLite for simplicity)
CREATE TABLE IF NOT EXISTS Tags (
    JournalEntryId TEXT NOT NULL,
    TagName TEXT NOT NULL,
    PRIMARY KEY (JournalEntryId, TagName),
    FOREIGN KEY (JournalEntryId) REFERENCES JournalEntries(Id) ON DELETE CASCADE
);

-- Create indexes for performance
CREATE INDEX IF NOT EXISTS idx_journal_userid ON JournalEntries(UserId);
CREATE INDEX IF NOT EXISTS idx_journal_created ON JournalEntries(DateCreated);
CREATE INDEX IF NOT EXISTS idx_journal_mood ON JournalEntries(MoodRating);
CREATE INDEX IF NOT EXISTS idx_journal_sentiment ON JournalEntries(Sentiment);
