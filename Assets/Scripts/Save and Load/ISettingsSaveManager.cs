public interface ISettingsSaveManager
{
    void LoadData(SettingsData _data);

    void SaveData(ref SettingsData _data);
}
