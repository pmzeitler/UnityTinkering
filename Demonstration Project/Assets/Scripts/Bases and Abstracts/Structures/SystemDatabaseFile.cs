//TODO fix this define for later implementations
#define WINDOWS
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
        private const string FILE_SEPARATOR = "\\";
#elif (LINUXLIKE)
        private const string FILE_SEPARATOR = "/";
#else 
        private const string FILE_SEPARATOR = "\\";
#endif


    public static string GetFilename(SystemDatabaseFile databaseFile, String locale)
    {

        string retval = null;
        switch (databaseFile)
        {
            case SystemDatabaseFile.GAME_DATA:
                retval = "GameData[FS]game_data.db";
                break;
            case SystemDatabaseFile.SYSTEM_PREFS:
                retval = "SaveData[FS]system_prefs.db";
                break;
            case SystemDatabaseFile.LOCALIZATION_BASE:
                retval = "GameData[FS]Localizations[FS][LOCALE][FS]Dialogues.db";
                break;
        }
        if (retval == null)
        {
            throw new NotImplementedException();
        }
        retval = retval.Replace("[FS]", FILE_SEPARATOR);

        if (retval.Contains("[LOCALE]"))
        {
            retval = retval.Replace("[LOCALE]", locale);
        }

        return retval;
    }


}
