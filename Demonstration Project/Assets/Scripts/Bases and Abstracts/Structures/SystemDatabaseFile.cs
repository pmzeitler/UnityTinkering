//TODO fix this define for later implementations
#define LINUXLIKE
using System;

public enum SystemDatabaseFile
{
    GAME_DATA,
    SYSTEM_PREFS,
    LOCALIZATION_BASE

}

public static class SystemDatabaseFileName
{

#if (WINDOWS)
        public const string FILE_SEPARATOR = "\\";
#elif (LINUXLIKE)
        public const string FILE_SEPARATOR = "/";
#else 
        public const string FILE_SEPARATOR = "\\";
#endif


    public static string GetFilename(SystemDatabaseFile databaseFile, String locale)
    {

        string retval = GetFilename(databaseFile);


        if (retval.Contains("[LOCALE]"))
        {
            retval = retval.Replace("[LOCALE]", locale);
        }

        return retval;
    }

    public static string GetFilename(SystemDatabaseFile databaseFile)
    {
        string retval = null;

        switch (databaseFile)
        {
            case SystemDatabaseFile.GAME_DATA:
                retval = "[FS]GameData[FS]game_data.db";
                break;
            case SystemDatabaseFile.SYSTEM_PREFS:
                retval = "[FS]DefaultSaveData[FS]system_prefs.db";
                break;
            case SystemDatabaseFile.LOCALIZATION_BASE:
                retval = "[FS]GameData[FS]Localizations[FS][LOCALE][FS]Dialogues.db";
                break;
        }
        if (retval == null)
        {
            throw new NotImplementedException();
        }
        retval = retval.Replace("[FS]", FILE_SEPARATOR);

        return retval;
    }


}
