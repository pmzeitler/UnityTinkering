using System;
using Mono.Data.Sqlite;
using System.Data.Common;
using System.Data;
using UnityEngine;

public abstract class MutableDBRecord : BaseDBRecord
{
    public MutableDBRecord(DbConnection connectionIn) : base(connectionIn)
    {
        // nothing special;
    }
}