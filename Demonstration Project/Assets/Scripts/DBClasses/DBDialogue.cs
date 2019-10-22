using System;
using System.Data.Common;
using Mono.Data.Sqlite;
using UnityEngine;

public class DBDialogue : ReadOnlyDBRecord
{
    protected override string TableName
    {
        get
        {
            return "Dialogues";
        }
    }

    protected override string LoadSQL
    {
        get
        {
            return "SELECT TEXT_KEY, LOCAL_TEXT FROM " + TableName + " WHERE TEXT_KEY = @textkey ";
        }
    }

    public DBDialogue(DbConnection connectionIn) : base(connectionIn)
    {
        this.TextKey = "";
        //nothing special;
    }

    protected override void _prepareLoad(SqliteCommand commandIn, params object[] paramsIn)
    {
        string textKeyIn = paramsIn.Length >= 1 ? (string)paramsIn[0] : TextKey;
        if (textKeyIn == "")
        {
            Debug.Log("Bad params pasased in to load DBDialogue: " + paramsIn + " TextKey: " + TextKey);
        }
        else
        {
            commandIn.Parameters.AddWithValue("@textkey", textKeyIn);
        }

        //throw new NotImplementedException();
    }

    public string TextKey { get; set; }
    public string LocalizedText { get; private set; }

    protected override void _readLoad(SqliteDataReader readerIn)
    {
        if (readerIn.Read())
        {
            string TextKeyLoaded = readerIn["TEXT_KEY"].ToString();
            if (TextKey == "")
            {
                //Assume that this is the correct row
                TextKey = TextKeyLoaded;
            }
            else if (TextKeyLoaded != TextKey)
            {
                Debug.Log("Load expected key '" + TextKey + "' , got '" + TextKeyLoaded + "' instead");
            } else
            {
                //i guess everything's ok here
            }
            LocalizedText = readerIn["LOCAL_TEXT"].ToString();
        }
    }
}

