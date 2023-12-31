# BaseLib.Core.MySql

## Overview

Contains concrete implementations of the interfaces from **BaseLib.Core** for MySql

## Services

`JournalEntryWriter` is the implementation of `IJournalEntryWriter` to store the events in a relational JOURNAL table.
Check the source code for the dbinstall-v1.0.0.sql script to create the table

> As a best practice, for security and performance reasons, store only the JournalEntry in the relational database. Request and Responses should be store in an object store in a secure manner.

```sql
#JOURNAL TABLE
CREATE TABLE IF NOT EXISTS JOURNAL (
  ID int NOT NULL AUTO_INCREMENT,
  SERVICE_NAME VARCHAR(255) DEFAULT NULL,
  STARTED_ON TIMESTAMP(6) DEFAULT NULL,
  FINISHED_ON TIMESTAMP(6) DEFAULT NULL,
  OPERATION_ID VARCHAR(255) DEFAULT NULL,
  CORRELATION_ID VARCHAR(255) DEFAULT NULL,
  SUCCEEDED BOOLEAN DEFAULT NULL,
  REASON_CODE INT DEFAULT NULL,
  REASON VARCHAR(255),
  MESSAGES TEXT,
  PRIMARY KEY (ID),
  UNIQUE KEY IX_OPERATION (OPERATION_ID),
  KEY IX_CORRELATION (CORRELATION_ID),
  KEY IX_SERVICE_DATES (STARTED_ON,SERVICE_NAME)
)
```


##Extensions

`MySqlConnection::SetWaitTimeout()` allows to specify the session wait timeout