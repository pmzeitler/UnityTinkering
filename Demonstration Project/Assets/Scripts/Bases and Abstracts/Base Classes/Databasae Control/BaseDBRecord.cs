using System;
using Mono.Data.Sqlite;
using System.Data.Common;
using System.Data;
using UnityEngine;

public abstract class BaseDBRecord
{
    protected abstract string TableName { get; }
    protected abstract string SaveSQL { get; }
    protected abstract string CreateSQL { get; }
    protected abstract string LoadSQL { get; }
    protected abstract string DeleteSQL { get; }

    protected int ID { get; private set; }

    protected abstract void _prepareSave(SqliteCommand commandIn);
    protected abstract void _prepareCreate(SqliteCommand commandIn);
    protected abstract void _prepareLoad(SqliteCommand commandIn);
    protected abstract void _prepareDelete(SqliteCommand commandIn);

    protected abstract void _readLoad(SqliteDataReader readerIn);

    protected bool IsNew { get; set; }

    private SqliteConnection Connection { get; set; }

    public BaseDBRecord(DbConnection connectionIn)
    {
        if (connectionIn is SqliteConnection)
        {
            Connection = (SqliteConnection)connectionIn;
            IsNew = true;
        }
        else
        {
            throw new Exception("SQLite connection expected, but instead got connection of type " + connectionIn.GetType().Name);
        }
    }

    public virtual void Save(SqliteTransaction transaction)
    {
        SqliteCommand command = Connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandType = CommandType.Text;

        if (IsNew)
        {
            command.CommandText = CreateSQL;
            _prepareCreate(command);
        }
        else
        {
            command.CommandText = SaveSQL;
            _prepareSave(command);
        }

        int changes = command.ExecuteNonQuery();
        Debug.Assert((changes == 1), "Irregular number of changes on save; IsNew: " + IsNew);

        IsNew = false;

    }

    public virtual void Load(int idIn)
    {
        ID = idIn;
        SqliteCommand command = Connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = LoadSQL;
        _prepareLoad(command);
        _readLoad(command.ExecuteReader());
    }

    public virtual void Delete(SqliteTransaction transaction)
    {
        if (IsNew)
        {
            return;
        }

        SqliteCommand command = Connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandType = CommandType.Text;
        command.CommandText = DeleteSQL;
        _prepareDelete(command);

        int changes = command.ExecuteNonQuery();
        Debug.Assert((changes == 1), "Irregular number of changes on delete; IsNew: " + IsNew);


    }

}

