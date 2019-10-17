using System;
using Mono.Data.Sqlite;
using System.Data.Common;
using System.Data;
using UnityEngine;

public abstract class ReadOnlyDBRecord : BaseDBRecord
{
    protected override sealed string SaveSQL
    {
        get
        {
            return "";
        }
    }

    protected override sealed string CreateSQL
    {
        get
        {
            return "";
        }
    }

    protected override sealed string DeleteSQL
    {
        get
        {
            return "";
        }
    }

    protected sealed override void _prepareCreate(SqliteCommand commandIn)
    {
        //throw new NotImplementedException();
    }

    protected sealed override void _prepareSave(SqliteCommand commandIn)
    {
        //throw new NotImplementedException();
    }

    public sealed override void Save(SqliteTransaction transaction)
    {
        //base.Save(transaction);
    }

    public sealed override void Delete(SqliteTransaction transaction)
    {
        //base.Delete(transaction);
    }

    public ReadOnlyDBRecord(DbConnection connectionIn) : base(connectionIn)
    {
        // nothing special;
    }


}

